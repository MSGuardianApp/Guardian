using Microsoft.Phone.Controls;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using SOS.Phone.Resources;
using SOS.Phone.ServiceWrapper;
using SOS.Phone.Utilites.UtilityClasses;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Windows.Phone.Speech.VoiceCommands;
using Windows.System.Threading;

namespace SOS.Phone
{
    using System.Globalization;
    using System.Text;

    using Microsoft.Phone.Notification;

    public partial class MainPage : PhoneApplicationPage
    {
        #region Page Events
        private bool IsNavigatedFromMainPageTracktoLocationSetting = false;
        private bool IsNavigatedFromMainPageReporttoLocationSetting = false;
        private bool InitiateTrackingafterEnablingLocation = false;
        private static string SOSCountTextBlockData = "SOS: NA";
        private static string TrackingCountTextBlockData = "Tracking: NA";
        DispatcherTimer timer;

        public MainPage()
        {
            InitializeComponent();

            App.BackgroundThread(30);

            PanicStackPanel.DataContext = App.SosViewModel;

            InitiateNotificationChannel();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (timer != null) timer.Stop();
            timer = null;
        }

        protected override async void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            timer = new DispatcherTimer();
            this.BackgroundThread(30);

            #region Manage Page Navigation

            string IsFromToast = string.Empty;
            if (Globals.IsLocationServiceEnabled)
            {
                if (IsNavigatedFromMainPageTracktoLocationSetting)
                {
                    IsNavigatedFromMainPageTracktoLocationSetting = false;
                    NavigationService.Navigate(new Uri("/Pages/TrackMe.xaml", UriKind.Relative));
                }
                if (InitiateTrackingafterEnablingLocation)
                {
                    InitiateTrackingafterEnablingLocation = false;
                    Globals.InitiateTracking();
                }
                if (IsNavigatedFromMainPageReporttoLocationSetting)
                {
                    IsNavigatedFromMainPageReporttoLocationSetting = false;
                    NavigationService.Navigate(new Uri("/Pages/ReportIncident.xaml", UriKind.Relative));
                }
            }
            if (NavigationContext.QueryString.TryGetValue("ToastTitle", out IsFromToast) && (IsFromToast == "SOSToast"))
            {
                NavigationService.Navigate(new Uri("/Pages/SOS.xaml", UriKind.Relative));
                NavigationContext.QueryString.Clear();
            }

            if (NavigationContext.QueryString.TryGetValue("ToastTitle", out IsFromToast) && (IsFromToast == "TrackMeToast"))
            {
                NavigationService.Navigate(new Uri("/Pages/TrackMe.xaml", UriKind.Relative));
                NavigationContext.QueryString.Clear();
            }

            //Function Activation based on previous page call
            //When the page is navigated from settings
            string ToPage = "Unknown";
            if (NavigationContext.QueryString.TryGetValue("ToPage", out ToPage))
            {
                NavigationContext.QueryString.Remove("ToPage");
                switch (ToPage)
                {
                    case "ReportTease": // To trigger Reporting
                        NavigationService.Navigate(new Uri("/Pages/ReportIncident.xaml", UriKind.Relative));
                        return;
                    case "Tracking": // To trigger Tracking
                        NavigationService.Navigate(new Uri("/Pages/TrackMe.xaml", UriKind.Relative));
                        return;
                }
            }
            #endregion

            #region Handle Tracking and SOS activations

            if (Globals.IsDeactivateSOSSession && Globals.CurrentProfile.IsSOSOn)
            {
                Globals.CurrentProfile.IsSOSOn = false;
                App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.SOSStatus, "false");
                Globals.IsDeactivateSOSSession = false;
            }

            if (Globals.CurrentProfile.IsSOSOn && !Globals.CurrentProfile.IsTrackingOn)
                if (!Globals.IsLocationServiceEnabled)
                {
                    if (MessageBox.Show(Constants.LocationDisabledMessageText, "Information", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        //to check if it is due to App's location setting
                        if (!Globals.IsLocationConsentEnabled)
                        {
                            Globals.CurrentProfile.LocationConsent = true;
                            App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.LocationServicePref, "true");
                        }
                        //to check if it is from phone's location setting
                        if (!Globals.IsPhoneLocationServiceEnabled)
                        {
                            Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
                            InitiateTrackingafterEnablingLocation = true;
                            return;
                        }
                        else
                        {
                            Globals.InitiateTracking();
                        }
                    }
                }
                else
                {
                    Globals.InitiateTracking();
                }
            if (Globals.CurrentProfile.IsTrackingOn && !Globals.IsLocationServiceEnabled)
            {
                Globals.StopTracking(true);
            }
            //UpdatePrimaryTile(6, "Guardian");
            #endregion

            RenderUIBasedOnStatusAsync();

            #region V1.3 to V2.0 Migration Code - Not required for new instances

            if (Globals.IsAutoUpgradeFailed)//Auto upgrade data from V1.3 to V2.0
            {
                //Globals.GetMigratedData().Wait();
                await Globals.GetMigratedData();

                if (Globals.IsAutoUpgradeFailed)
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.UpgradedDataLoadFailText, "basicWrap", "Info"));
                else
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.ProfileLoadedSuccessfullyText, "basicWrap", "Info"));
            }
            #endregion

            #region Voice Commands
            // Is this a new activation or a resurrection from tombstone?
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                string voiceCommandName;
                if (NavigationContext.QueryString.TryGetValue("voiceCommandName", out voiceCommandName))
                {
                    // Was the app launched using a voice command?
                    HandleVoiceCommand(voiceCommandName);
                }
                else
                {
                    // If we just freshly launched this app without a Voice Command, asynchronously try to install the Voice Commands.
                    // If the commands are already installed, no action will be taken--there's no need to check.
                    Task.Run(() => InstallVoiceCommands());
                }
            }
            #endregion
        }

        #region Voice Commands Installation and Processing

        /// <summary>
        /// Installs the Voice Command Definition (VCD) file associated with the application.
        /// Based on OS version, installs a separate document based on version 1.0 of the schema or version 1.1.
        /// </summary>
        private async void InstallVoiceCommands()
        {
            const string wp80vcdPath = "ms-appx:///SOSVoiceCommandDefinition-8.0.xml";
            const string wp81vcdPath = "ms-appx:///SOSVoiceCommandDefinition-8.1.xml";

            try
            {
                bool using81orAbove = ((Environment.OSVersion.Version.Major >= 8)
                    && (Environment.OSVersion.Version.Minor >= 10));

                Uri vcdUri = new Uri(using81orAbove ? wp81vcdPath : wp80vcdPath);

                await VoiceCommandService.InstallCommandSetsFromFileAsync(vcdUri);
            }
            catch (Exception vcdEx)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show(String.Format(
                        AppResources.VoiceCommandInstallErrorTemplate, vcdEx.HResult, vcdEx.Message));
                });
            }
        }

        /// <summary>
        /// Takes specific action for a retrieved VoiceCommand name.
        /// </summary>
        /// <param name="voiceCommandName"> the command name triggered to activate the application </param>
        private void HandleVoiceCommand(string voiceCommandName)
        {
            // Voice Commands can be typed into Cortana; when this happens, "voiceCommandMode" is populated with the
            // "textInput" value. In these cases, we'll want to behave a little differently by not speaking back.
            bool typedVoiceCommand = (NavigationContext.QueryString.ContainsKey("commandMode")
                && (NavigationContext.QueryString["commandMode"] == "text"));

            string phraseTopicContents = null;
            bool doSearch = false;

            switch (voiceCommandName)
            {
                case "SOS":
                    NavigationService.Navigate(new Uri("/Pages/StartSOS.xaml", UriKind.Relative));
                    break;
                case "TrackMe":
                    NavigationService.Navigate(new Uri("/Pages/TrackMe.xaml", UriKind.Relative));
                    break;
                case "StopSOS":
                    if (Globals.CurrentProfile.IsSOSOn)
                    {
                        App.MyProfiles.SaveCurrentProfileSetting(SOS.Phone.ProfileViewModel.ProfileSetting.SOSStatus, "false");

                        PanicStatusText.Text = "OFF";
                        PanicSubText.Text = "Tap this, if you are threatened";
                        PanicButton.Background = new SolidColorBrush(Color.FromArgb(255, 0xF9, 0x65, 0x11));

                        var appBarButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                        appBarButton.IconUri = new Uri("/Assets/Images/sos.png", UriKind.Relative);
                        appBarButton.Text = "start sos";

                        Globals.InitiateStopSOSEventsAsync(false);
                    }
                    else
                        Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.NoActiveSessionText, "basicWrap", "Info"));

                    break;
                case "StopTracking":
                    if (Globals.CurrentProfile.IsTrackingOn)
                    {
                        if (Globals.CurrentProfile.IsSOSOn) return;

                        App.MyProfiles.SaveCurrentProfileSetting(SOS.Phone.ProfileViewModel.ProfileSetting.TrackingStatus, "false");
                        TrackmeStatus.Text = "OFF";
                        TrackTileImage.Source = new BitmapImage(new Uri("./Assets/TrackMeOff.png", UriKind.Relative));
                        TrackMeButton.Background = new SolidColorBrush(Color.FromArgb(255, 0xF9, 0x65, 0x11));

                        var appBarButton = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
                        appBarButton.IconUri = new Uri("/Assets/Images/track.png", UriKind.Relative);
                        appBarButton.Text = "start tracking";

                        Globals.StopTracking();
                        Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.TrackingIsOffText, "basicWrap", "Info"));
                    }
                    else
                        Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.NoActiveSessionText, "basicWrap", "Info"));

                    break;
                default:
                    // There is no match for the voice command name.
                    break;
            }
        }

        #endregion

        #region Back button click

        private bool isBackMsgBoxDisplayed = false;
        async void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Globals.CurrentProfile.IsSOSOn || Globals.CurrentProfile.IsTrackingOn)
            {
                e.Cancel = true;
                if (!isBackMsgBoxDisplayed)
                {
                    this.isBackMsgBoxDisplayed = true;
                    ThreadPool.RunAsync(delegate
                    {
                        Dispatcher.BeginInvoke(async () =>
                        {
                            if (MessageBox.Show(CustomMessage.ExitApplication, "Are you sure to quit Guardian?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                            {
                                //Call the code you would normally call in the functions OnNavigatedFrom and Application_Closing 
                                //as none of them would be fired once App.Terminate is called
                                m_ProgressBar.Visibility = Visibility.Visible;
                                Globals.LocationServiceGeolocator = null;

                                if (Config.UseGeoLocator)
                                {
                                    App.Geolocator.PositionChanged -= Globals.Geolocator_PositionChanged;
                                    App.Geolocator = null;
                                }
                                else
                                {
                                    App.geoCoordinateWatcher.PositionChanged -= Globals.Watcher_PositionChanged;
                                    App.geoCoordinateWatcher = null;
                                }

                                bool result = await (new Retrier<Task<bool>>()).TryWithDelay(() => LocationServiceWrapper.StopPostingsAsync(Globals.CurrentProfile.SessionToken), 2, 0);
                                if (result)
                                {
                                    App.MyProfiles.SaveCurrentProfileSetting(SOS.Phone.ProfileViewModel.ProfileSetting.SOSStatus, "false");
                                    App.MyProfiles.SaveCurrentProfileSetting(SOS.Phone.ProfileViewModel.ProfileSetting.TrackingStatus, "false");
                                    App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.SosStatusSynced, "true");
                                    App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.TrackingStatusSynced, "true");
                                }
                                //if (Globals.CurrentProfile.IsSOSOn)
                                //{
                                //    App.MyProfiles.SaveCurrentProfileSetting(SOS.Phone.ProfileViewModel.ProfileSetting.SOSStatus, "false");
                                //    await Globals.InitiateStopSOSEventsAsync(true);
                                //}
                                //App.MyProfiles.SaveCurrentProfileSetting(SOS.Phone.ProfileViewModel.ProfileSetting.TrackingStatus, "false");
                                //await StopTrackingEventsAsync(true);

                                ClearBackEntries();

                                //This will also have a side effect that every time, your app will be termed as terminated instead of being called by user
                                App.Current.Terminate();
                            }

                            this.isBackMsgBoxDisplayed = false;
                        });
                    });
                }
            }
            else
            {
                bool hasRated = false;
                DispatcherTimer dt = new DispatcherTimer();
                dt.Interval = new TimeSpan(0, 0, 5);
                dt.Tick += dt_Tick;
                dt.Start();
                if (IsolatedStorageSettings.ApplicationSettings.Contains("count"))
                {
                    //Retrieve the value
                    hasRated = (bool)IsolatedStorageSettings.ApplicationSettings["count"];
                }
                if (hasRated == false)
                {
                    //Globals.hasRated = false;

                    // Store the value
                    IsolatedStorageSettings.ApplicationSettings["count"] = hasRated;
                    MessageBoxResult result = MessageBox.Show("If you liked the app, please rate and review it on the store to help us improve it", "Rate & Review the app", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        hasRated = true;
                        IsolatedStorageSettings.ApplicationSettings["count"] = hasRated;
                        MarketplaceReviewTask mrTask = new MarketplaceReviewTask();
                        mrTask.Show();

                    }
                    else if (result == MessageBoxResult.Cancel)
                        App.Current.Terminate();
                }
                else
                {
                    hasRated = false;
                    // Store the value
                    IsolatedStorageSettings.ApplicationSettings["count"] = hasRated;
                    App.Current.Terminate();
                }


            }
        }

        void dt_Tick(object sender, EventArgs e)
        {
            App.Current.Terminate();
            ((sender) as DispatcherTimer).Stop();
        }

        #endregion

        #endregion

        #region Event Handlers
        private void PanicButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (Globals.CurrentProfile.IsSOSOn)
                NavigationService.Navigate(new Uri("/Pages/SOS.xaml", UriKind.Relative));
            else
                NavigationService.Navigate(new Uri("/Pages/StartSOS.xaml", UriKind.Relative));
        }

        private void TrackMeButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (!Globals.IsLocationServiceEnabled)
            {
                if (MessageBox.Show(Constants.LocationDisabledMessageText, "Information", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    //to check if it is due to App's location setting
                    if (!Globals.IsLocationConsentEnabled)
                    {
                        Globals.CurrentProfile.LocationConsent = true;
                        App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.LocationServicePref, "true");
                    }
                    //to check if it is from phone's location setting
                    if (!Globals.IsPhoneLocationServiceEnabled)
                    {
                        Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
                        IsNavigatedFromMainPageTracktoLocationSetting = true;
                        return;
                    }
                    NavigationService.Navigate(new Uri("/Pages/TrackMe.xaml", UriKind.Relative));
                }

            }
            else
            {
                NavigationService.Navigate(new Uri("/Pages/TrackMe.xaml", UriKind.Relative));
            }
        }

        private void SettingsButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Settings.xaml?FromPage=MainPage", UriKind.Relative));
        }

        private void ToggleSosAppBarItem_OnClick(object sender, EventArgs e)
        {
            if (Globals.CurrentProfile.IsSOSOn)
            {
                App.MyProfiles.SaveCurrentProfileSetting(SOS.Phone.ProfileViewModel.ProfileSetting.SOSStatus, "false");

                PanicStatusText.Text = "OFF";
                PanicSubText.Text = "Tap this, if you are threatened";
                PanicButton.Background = new SolidColorBrush(Color.FromArgb(255, 0xF9, 0x65, 0x11));

                var appBarButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                appBarButton.IconUri = new Uri("/Assets/Images/sos.png", UriKind.Relative);
                appBarButton.Text = "start sos";

                Globals.InitiateStopSOSEventsAsync(false);
            }
            else
            {
                NavigationService.Navigate(new Uri("/Pages/StartSOS.xaml", UriKind.Relative));
            }
        }

        private void ToggleTrackingAppBarItem_OnClick(object sender, EventArgs e)
        {
            if (Globals.CurrentProfile.IsTrackingOn)
            {
                if (Globals.CurrentProfile.IsSOSOn) return;

                App.MyProfiles.SaveCurrentProfileSetting(SOS.Phone.ProfileViewModel.ProfileSetting.TrackingStatus, "false");
                TrackmeStatus.Text = "OFF";
                TrackTileImage.Source = new BitmapImage(new Uri("./Assets/TrackMeOff.png", UriKind.Relative));
                TrackMeButton.Background = new SolidColorBrush(Color.FromArgb(255, 0xF9, 0x65, 0x11));

                var appBarButton = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
                appBarButton.IconUri = new Uri("/Assets/Images/track.png", UriKind.Relative);
                appBarButton.Text = "start tracking";

                Globals.StopTracking();
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.TrackingIsOffText, "basicWrap", "Info"));
            }
            else
            {
                if (!Globals.IsLocationServiceEnabled)
                {
                    if (MessageBox.Show(Constants.LocationDisabledMessageText, "Information", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        //to check if it is due to App's location setting
                        if (!Globals.IsLocationConsentEnabled)
                        {
                            Globals.CurrentProfile.LocationConsent = true;
                            App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.LocationServicePref, "true");
                        }
                        //to check if it is from phone's location setting
                        if (!Globals.IsPhoneLocationServiceEnabled)
                        {
                            Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
                            IsNavigatedFromMainPageTracktoLocationSetting = true;
                            return;
                        }
                        NavigationService.Navigate(new Uri("/Pages/TrackMe.xaml", UriKind.Relative));
                    }
                }
                else
                {
                    NavigationService.Navigate(new Uri("/Pages/TrackMe.xaml", UriKind.Relative));
                }

            }
        }

        private void LocateButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Locate.xaml", UriKind.Relative));
        }

        private void ReportButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (Globals.IsRegisteredUser)
            {
                if (!Globals.IsLocationServiceEnabled)
                {
                    if (MessageBox.Show(Constants.LocationDisabledMessageText, "Information", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        //to check if it is due to App's location setting
                        if (!Globals.IsLocationConsentEnabled)
                        {
                            Globals.CurrentProfile.LocationConsent = true;
                            App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.LocationServicePref, "true");
                        }
                        //to check if it is from phone's location setting
                        if (!Globals.IsPhoneLocationServiceEnabled)
                        {
                            Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
                            IsNavigatedFromMainPageReporttoLocationSetting = true;
                            return;
                        }
                        NavigationService.Navigate(new Uri("/Pages/ReportIncident.xaml", UriKind.Relative));
                    }
                }
                else
                    NavigationService.Navigate(new Uri("/Pages/ReportIncident.xaml", UriKind.Relative));
            }
            else
            {
                NavigationService.Navigate(new Uri("/Pages/RegisterMessage.xaml", UriKind.Relative));
            }
        }

        private void CellularStatusButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-cellular:"));
        }

        private void WifiButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-wifi:"));
        }

        private void LocationStatusButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (Globals.IsLocationConsentEnabled && !Globals.IsPhoneLocationServiceEnabled)
                Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
            else
                NavigationService.Navigate(new Uri("/Pages/Settings.xaml?ToPage=Preferences", UriKind.Relative));
        }

        #endregion

        #region Private Methods

        //Code to update Iconic Tile
        //public static void UpdatePrimaryTile(int count, string content)
        //{
        //    IconicTileData primaryTileData = new IconicTileData();
        //    primaryTileData.Count = count;
        //    primaryTileData.Title = content;

        //    ShellTile primaryTile = ShellTile.ActiveTiles.First();
        //    primaryTile.Update(primaryTileData);
        //}

        private async void RenderUIBasedOnStatusAsync()
        {
            RenderTrackingControlsUIAsync();
            var IsWifiConnected = FindWIFISSID();//phani
            CellularStatusButton.Background = DeviceNetworkInformation.IsCellularDataEnabled ? Constants.GreenColor : Constants.SaffronColor;
            WifiButton.Background = DeviceNetworkInformation.IsWiFiEnabled && IsWifiConnected ? Constants.GreenColor : Constants.SaffronColor;

            await RenderAddressAsync();

        }

        /// <summary>
        /// Find WIFI SSID
        /// </summary>
        private bool FindWIFISSID()
        {
            bool IsNetworkConnected = false;
            foreach (var network in new NetworkInterfaceList())
            {
                if ((network.InterfaceType == NetworkInterfaceType.Wireless80211) && (network.InterfaceState == ConnectState.Connected))
                {
                    IsNetworkConnected = true;
                    break;
                }
                // mLocatoinInfo.Text = network.InterfaceName; //Get the SSID of the WIFI
                else
                    IsNetworkConnected = false;

                //mLocatoinInfo.Text = "fail";
            }
            return IsNetworkConnected;
        }

        private async void RenderTrackingControlsUIAsync()
        {
            if (Globals.CurrentProfile.IsSOSOn)
            {
                PanicStatusText.Text = "ON";
                PanicSubText.Text = Globals.IsRegisteredUser ? "Your buddies have been informed" : string.Empty;
                PanicButton.Background = new SolidColorBrush(Color.FromArgb(255, 0x10, 0xAA, 0x1E));

                var appBarButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                appBarButton.IconUri = new Uri("/Assets/Images/stopsos.png", UriKind.Relative);
                appBarButton.Text = "stop sos";
            }
            else
            {
                PanicStatusText.Text = "OFF";
                PanicSubText.Text = "Tap this, if you are threatened";
                PanicButton.Background = new SolidColorBrush(Color.FromArgb(255, 0xF9, 0x65, 0x11));

                var appBarButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                appBarButton.IconUri = new Uri("/Assets/Images/sos.png", UriKind.Relative);
                appBarButton.Text = "start sos";
            }

            if (Globals.CurrentProfile.IsTrackingOn)
            {
                TrackmeStatus.Text = "ON";
                TrackTileImage.Source = new BitmapImage(new Uri("./Assets/TrackMeOn.png", UriKind.Relative));
                TrackMeButton.Background = new SolidColorBrush(Color.FromArgb(255, 0x10, 0xAA, 0x1E));

                var appBarButton = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
                appBarButton.IconUri = new Uri("/Assets/Images/stoptrack.png", UriKind.Relative);
                appBarButton.Text = "stop tracking";
            }
            else
            {
                TrackmeStatus.Text = "OFF";
                TrackTileImage.Source = new BitmapImage(new Uri("./Assets/TrackMeOff.png", UriKind.Relative));
                TrackMeButton.Background = new SolidColorBrush(Color.FromArgb(255, 0xF9, 0x65, 0x11));

                var appBarButton = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
                appBarButton.IconUri = new Uri("/Assets/Images/track.png", UriKind.Relative);
                appBarButton.Text = "start tracking";
            }

            ShellTile TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("DefaultTitle=SOSTile"));

            //Disable the application bar item if tile exits
            //if (TileToFind != null)
            //{
            //    ((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).IsEnabled = false;    //Not able to access the control with Name. So have to go with this only! this is some known bug(http://www.learningwindowsphone.com/blog/?p=62)
            //}

            if (Globals.IsRegisteredUser)
            {
                //Assigning static soscount to Locate tile data
                SOSCountTextBlock.Text = SOSCountTextBlockData;
                TrackingCountTextBlock.Text = TrackingCountTextBlockData;
            }
        }

        private async Task RenderAddressAsync()
        {
            bool IsLocationServiceEnabled = Globals.IsLocationServiceEnabled;
            LocationStatusButton.Background = IsLocationServiceEnabled ? Constants.GreenColor : Constants.SaffronColor;

            try
            {
                GetLiveTileCountsAsync();

                if (IsLocationServiceEnabled)
                {
                    if (string.IsNullOrEmpty(Globals.RecentLocation.Address))
                    {
                        if (Globals.RecentLocation.Coordinate != null)
                        {
                            AddressTextBlock.Text = "Lat: " + Globals.RecentLocation.Coordinate.Latitude.ToString() + "; " + "Long: " + Globals.RecentLocation.Coordinate.Longitude.ToString(); ;
                        }
                        else
                        {
                            m_ProgressBar.Visibility = System.Windows.Visibility.Visible;
                            AddressTextBlock.Text = "Getting location details...";
                        }
                    }
                    else
                        AddressTextBlock.Text = Globals.RecentLocation.Address;

                    AddressTextBlock.Text = await Utility.GetRecentAddress();
                }
                else if (!Globals.IsLocationConsentEnabled)
                {
                    AddressTextBlock.Text = "Location Consent is disabled in Preferences...";
                }
                else
                {
                    AddressTextBlock.Text = "Location Services are disabled in phone settings...";
                }

            }
            catch (Exception)
            {
                //Absorb exception
            }
            finally
            {
                m_ProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            }

        }

        private async void GetLiveTileCountsAsync()
        {
            if (Globals.IsRegisteredUser && SOSCountTextBlockData.Contains("NA"))
            {
                SOSCountTextBlock.Text = "SOS: 00";
                TrackingCountTextBlock.Text = "Tracking: 00";
            }
            if (Globals.IsRegisteredUser && Globals.IsDataNetworkAvailable)
            {
                try
                {
                    ProgressBarLocate.Visibility = Visibility.Visible;
                    Dictionary<string, string> liveCounts = await LocationServiceWrapper.GetLocateLiveTileCountAsync();
                    string sosCount = "00", trackCount = "00";

                    if (liveCounts != null)
                    {
                        liveCounts.TryGetValue("SOSCount", out sosCount);
                        SOSCountTextBlock.Text = "SOS: " + (sosCount.Length == 1 ? "0" + sosCount : sosCount);

                        liveCounts.TryGetValue("TrackCount", out trackCount);
                        TrackingCountTextBlock.Text = "Tracking: " + (trackCount.Length == 1 ? "0" + trackCount : trackCount);
                        SOSCountTextBlockData = SOSCountTextBlock.Text;
                        TrackingCountTextBlockData = TrackingCountTextBlock.Text;
                    }
                    ProgressBarLocate.Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    ProgressBarLocate.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                SOSCountTextBlock.Text = "SOS: NA";
                TrackingCountTextBlock.Text = "Tracking: NA";
            }
        }

        private void BackgroundThread(int waitTime)
        {
            timer.Tick +=
                delegate(object s, EventArgs args)
                {
                    GetLiveTileCountsAsync();
                };
            timer.Start();

            timer.Interval = new TimeSpan(0, 0, waitTime);
        }

        //TODO - Validate this
        private void ClearBackEntries()
        {
            while (NavigationService.BackStack != null && NavigationService.BackStack.Count() > 0)
                NavigationService.RemoveBackEntry();
        }
        #endregion

        private void HelpAppBarItem_OnClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Help.xaml", UriKind.Relative));
        }

        private void AboutAppBarItem_OnClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/About.xaml", UriKind.Relative));
        }

        private void PrivacyPolicyAppBarItem_OnClick(object sender, EventArgs e)
        {
            Globals.ShowPrivacy();
        }

        private void RateAppBarItem_OnClick(object sender, EventArgs e)
        {
            MarketplaceReviewTask mrTask = new MarketplaceReviewTask();
            mrTask.Show();
        }

        private async void StopButtonAppBarItem_Click(object sender, EventArgs e)
        {
            if (Globals.CurrentProfile.IsSOSOn)
            {
                App.MyProfiles.SaveCurrentProfileSetting(SOS.Phone.ProfileViewModel.ProfileSetting.SOSStatus, "false");
                Globals.InitiateStopSOSEventsAsync(false);
                RenderTrackingControlsUIAsync();
            }
            else
            {
                //Deletes all previous posts for the Profile
                var retrier = new Retrier<Task<bool>>();
                bool result = await retrier.TryWithDelay(
                                () => LocationServiceWrapper.StopPostingsAsync("0"), Constants.RetryMaxCount, 0);
            }
        }
        private void InitiateNotificationChannel()
        {
            /// Holds the push channel that is created or found.
            HttpNotificationChannel pushChannel;

            // The name of our push channel.
            string channelName = "GuardianNotificationChannel";

            InitializeComponent();

            // Try to find the push channel.
            pushChannel = HttpNotificationChannel.Find(channelName);

            // If the channel was not found, then create a new connection to the push service.
            if (pushChannel == null)
            {
                pushChannel = new HttpNotificationChannel(channelName);

                // Register for all the events before attempting to open the channel.
                pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

                // Register for this notification only if you need to receive the notifications while your application is running.
                pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

                pushChannel.Open();

                // Bind this new channel for toast events.
                pushChannel.BindToShellToast();
                pushChannel.BindToShellTile();

            }
            else
            {
                // The channel was already open, so just register for all the events.
                pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

                // Register for this notification only if you need to receive the notifications while your application is running.
                pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

                // Display the URI for testing purposes. Normally, the URI would be passed back to your web service at this point.
                System.Diagnostics.Debug.WriteLine(pushChannel.ChannelUri.ToString());
            }
        }

        void PushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.ChannelUri.ToString());
            Globals.CurrentProfile.NotificationUri = e.ChannelUri.ToString();
            App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.NotificationUri, e.ChannelUri.ToString());
        }

        void PushChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            // Error handling logic for your particular application would be here.
            Dispatcher.BeginInvoke(() =>
                MessageBox.Show(String.Format("A push notification {0} error occurred.  {1} ({2}) {3}",
                    e.ErrorType, e.Message, e.ErrorCode, e.ErrorAdditionalData))
                    );
        }



        void PushChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            StringBuilder message = new StringBuilder();
            string relativeUri = string.Empty;

            message.AppendFormat("Guardian {0}:\n", DateTime.Now.ToShortTimeString());

            // Parse out the information that was part of the message.
            foreach (string key in e.Collection.Keys)
            {
                message.AppendFormat("{0}: {1}\n", key, e.Collection[key]);

                if (string.Compare(key, "wp:Param", CultureInfo.InvariantCulture, CompareOptions.IgnoreCase) == 0)
                {
                    relativeUri = e.Collection[key];
                }
            }

            // Display a dialog of all the fields in the toast.
            Dispatcher.BeginInvoke(() => MessageBox.Show(message.ToString()));

        }
    }
}