using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json.Linq;
using SOS.Phone.ServiceWrapper;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Controls;
namespace SOS.Phone.Pages
{
    public partial class SOSPage : PhoneApplicationPage
    {
        bool IsPageLoadCall = false;
        List<NearByPlaceDetails> nearbyPlaceList = null;
        bool IsLocalHelpLoaded = false;
        public SOSPage()
        {
            InitializeComponent();
            DataContext = App.SosViewModel;

            SOSPageOnLoad();
        }

        private void SOSPageOnLoad()
        {
            if (!App.MyBuddies.IsDataLoaded)
            {
                App.MyBuddies.LoadBuddies(Globals.User.CurrentProfileId);
                App.MyGroups.LoadGroups(Globals.User.CurrentProfileId);
            }

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);
                string IsFromHelp = null;
                //Check if the navigation is not from back for ex. sms task, email task etc.
                if (NavigationContext != null)
                    NavigationContext.QueryString.TryGetValue("parameter", out IsFromHelp);
                if (!Globals.CurrentProfile.IsSOSOn && !Globals.RetainSOSState && (!(NavigationContext != null && IsFromHelp != null && (IsFromHelp == "fromLocalHelp"))))
                {
                    App.MyProfiles.SaveCurrentProfileSetting(SOS.Phone.ProfileViewModel.ProfileSetting.SOSStatus, "true");
                    IsPageLoadCall = true;
                }
                if (Globals.CurrentProfile.IsSOSOn && !Globals.CurrentProfile.IsTrackingOn)
                    Globals.InitiateTracking(IsPageLoadCall);

                if (Globals.IsLocationServiceEnabled && Globals.IsDataNetworkAvailable)
                    GetLocalHelp();

                //If Location service is disabled by user in the middle of tracking, turn off tracking.
                if (Globals.CurrentProfile.IsTrackingOn && !Globals.IsLocationServiceEnabled)
                    Globals.StopTracking(true);

                RenderUIBasedOnStatus();

                if (IsPageLoadCall)//Bug: OnPageLoad, Task will not work.
                    InitiateSosEventsAsync();
                IsPageLoadCall = false;

                //if (NavigationService != null && NavigationService.BackStack != null && NavigationService.BackStack.Count() > 0)
                //    NavigationService.RemoveBackEntry();

                if ((App.SosViewModel.Helplines == null || App.SosViewModel.Helplines.Count <= 0) && PagePivot.Items.Count >= 4)
                    PagePivot.Items.RemoveAt(3);
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }


        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            try
            {
                base.OnNavigatingFrom(e);
                Globals.RetainSOSState = !Globals.CurrentProfile.IsSOSOn;

            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }
        }

        private bool isBackMsgBoxDisplayed = false;
        private void SosApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void BuddyUC_TrackNavigationHandler(object sender, EventArgs e)
        {
            NavigationService.Navigate(((NavigationEventArgs)e).Uri);
        }

        private void GroupUC_TrackNavigationHandler(object sender, EventArgs e)
        {
            NavigationService.Navigate(((NavigationEventArgs)e).Uri);
        }

        private void ToggleSosAppBarItem_OnClick(object sender, EventArgs e)
        {
            ToggleSos();
        }

        private void ToggleTrackingAppBarItem_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!Globals.CurrentProfile.IsTrackingOn)
                {
                    if (!Globals.IsLocationServiceEnabled)
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
                            return;
                        }
                    }

                    Globals.InitiateTracking();
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.TurnOffTrackingText, "basicWrap", "Guardian tracking is active!"));
                }
                else
                {
                    Globals.StopTracking();

                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(Globals.CurrentProfile.IsSOSOn ? CustomMessage.TrackingCannotBeOffText : CustomMessage.TrackingIsOffText, "basicWrap", "Info"));
                }
                RenderUIBasedOnStatus();
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }
        #region Private Methods

        private void ToggleSos()
        {
            try
            {
                if (Globals.CurrentProfile.IsSOSOn)
                {
                    App.MyProfiles.SaveCurrentProfileSetting(SOS.Phone.ProfileViewModel.ProfileSetting.SOSStatus, "false");
                    Globals.InitiateStopSOSEventsAsync(false);
                }
                else
                {
                    //BL: Countdown timer is not required here as the user have already been thru the counter to land here.
                    App.MyProfiles.SaveCurrentProfileSetting(SOS.Phone.ProfileViewModel.ProfileSetting.SOSStatus, "true");
                    Globals.InitiateTracking();
                    InitiateSosEventsAsync();//TODO: Implement task
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.TurnOffIfSafeText, "basicWrap", "Guardian SOS is active!"));
                }
                RenderUIBasedOnStatus();
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        private async Task SendOfflineMessagesAsync()
        {
            try
            {
                string message = await Utility.GetDistressMessage();
                if (Globals.CurrentProfile.CanSMS) //TODO
                {
                    //TODO: Issue: When SMS is shown, SendEmail and InitiateCall are executed without waiting for user action and with ignore behavior.
                    SMSMessage smsMgs = new SMSMessage();
                    smsMgs.PhoneNumbers = Utility.GetBuddyNumbers();
                    smsMgs.Message = message;

                    Utility.SendSMS(smsMgs);
                }
                if (Globals.IsDataNetworkAvailable && Globals.CurrentProfile.CanFBPost && Globals.User.FBAuthId.GetValue() != string.Empty && Globals.CurrentProfile.FBGroupId.GetValue() != string.Empty)
                {
                    Utility.PostMessagetoFacebook(Globals.CurrentProfile.FBGroupId, Globals.User.FBAuthId, message);
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        protected async void InitiateSosEventsAsync()
        {
            bool initiateSMS = false;
            if (!Globals.IsRegisteredUser || !Globals.IsDataNetworkAvailable)
            {
                initiateSMS = true;
            }
            else
            {
                try
                {
                    LocationServiceWrapper.PostMyLocationAsync(await Utility.GetLocationQuick(), null);
                }
                catch
                {
                    initiateSMS = true;
                }
            }
            if (initiateSMS) await SendOfflineMessagesAsync();

            try
            {
                if (App.MyBuddies.Buddies != null && App.MyBuddies.Buddies.Count > 0)
                {
                    Callee defaultCallee = new Callee();
                    BuddyTableEntity buddyEntity = App.MyBuddies.GetPrimeBuddy(Globals.CurrentProfile.ProfileId);
                    if (buddyEntity != null)
                    {
                        defaultCallee.PhoneNumber = buddyEntity.PhoneNumber;
                        defaultCallee.DisplayName = buddyEntity.Name;
                        Utility.InitiateCall(defaultCallee);
                    }
                }
            }
            catch
            {
                //Ignore if exception raised while calling the default caller
            }
        }

        #endregion

        private void PhotoButton_OnClick(object sender, EventArgs e)
        {
            //TODO
            //1. Async capture Geo Loc. 2. Capture Cam 3. Await for 1st - Send loc and pic to service async
            try
            {
                CameraCaptureTask cameraCaptureTask = new CameraCaptureTask();
                cameraCaptureTask.Completed += new EventHandler<PhotoResult>(EvidenceCameraCaptureTask_Completed);
                cameraCaptureTask.Show();
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }
        }

        private void VideoButton_OnMouseEnter(object sender, EventArgs e)
        {
            //Bug: To fix few navigation issues
            NavigationService.Navigate(new Uri("/Pages/video.xaml", UriKind.Relative));
        }

        private async void EvidenceCameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                try
                {
                    if (Globals.CurrentProfile.IsSOSOn || Globals.CurrentProfile.IsTrackingOn)
                    {
                        GeoCoordinate gc = await Utility.GetLocationQuick();
                        await LocationServiceWrapper.PostMyLocationAsync(gc, e.ChosenPhoto);
                    }
                    else
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.InvalidPhotoCapture, "basicWrap", "Evidence upload failed!"));
                    }
                }
                catch (Exception ex)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.EvidenceUploadFailText, "basicWrap", "Evidence upload failed!"));
                }

            }
        }

        private void RenderUIBasedOnStatus()
        {
            if (Globals.CurrentProfile.IsSOSOn)
            {
                var appBarButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                appBarButton.IconUri = new Uri("/Assets/Images/stopsos.png", UriKind.Relative);
                appBarButton.Text = "stop sos";
            }
            else
            {
                var appBarButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                appBarButton.IconUri = new Uri("/Assets/Images/sos.png", UriKind.Relative);
                appBarButton.Text = "start sos";
            }

            if (Globals.CurrentProfile.IsTrackingOn)
            {
                var appBarButton = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
                appBarButton.IconUri = new Uri("/Assets/Images/stoptrack.png", UriKind.Relative);
                appBarButton.Text = "stop tracking";
            }
            else
            {
                var appBarButton = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
                appBarButton.IconUri = new Uri("/Assets/Images/track.png", UriKind.Relative);
                appBarButton.Text = "start tracking";
            }

            if (!Globals.IsRegisteredUser)
            {
                GroupRegisterPanel.Visibility = Visibility.Visible;
                GroupListPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (App.SosViewModel.Groups != null)
                    JoinGroupPanel.Visibility = App.SosViewModel.Groups.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            SwitchOnSosButton.Visibility = Globals.IsLocationServiceEnabled ? Visibility.Collapsed : Visibility.Visible;

        }

        private async void GetLocalHelp()
        {
            try
            {
                GeoCoordinate geoCoOrdinates = await Utility.GetCurrentLocationAsync();
                GetNearByHelp();
            }
            catch (Exception)
            {

            }

        }

        public async void GetNearByHelp()
        {
            string type;
            try
            {
                ShowProgressBar(true);
                if (Globals.IsDataNetworkAvailable)
                {
                    type = "police station";
                    string requestUrl = Utility.BingSearchUrl(type, "", Constants.MaxGetLocalHelpResults);
                    Uri uri = new Uri(requestUrl);
                    WebClient webClient = new WebClient();
                    webClient.DownloadStringAsync(uri);
                    WebClient webClient1 = new WebClient();

                    type = "hospital";
                    requestUrl = Utility.BingSearchUrl(type, "", Constants.MaxGetLocalHelpResults);
                    uri = new Uri(requestUrl);
                    webClient1.DownloadStringAsync(uri);

                    webClient.DownloadStringCompleted += webClient_DownloadStringCompleted1;
                    webClient1.DownloadStringCompleted += webClient_DownloadStringCompleted2;
                    nearbyPlaceList = null;
                    IsLocalHelpLoaded = false;
                }
            }
            catch (Exception e)
            {
            }
        }

        private void webClient_DownloadStringCompleted1(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                parseLocalHelpData("police", e.Result);
                IsLocalHelpLoaded = true;
            }
            catch
            {

                ShowProgressBar(false);
                NoDataMsgText.Visibility = Visibility.Visible;
                localhelpLongList.Visibility = Visibility.Collapsed;

            }
        }

        private void webClient_DownloadStringCompleted2(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                parseLocalHelpData("hospital", e.Result);
                IsLocalHelpLoaded = true;
            }
            catch
            {
                ShowProgressBar(false);
                NoDataMsgText.Visibility = Visibility.Visible;
                localhelpLongList.Visibility = Visibility.Collapsed;

            }
        }
        void parseLocalHelpData(string type, string jsonString)
        {
            try
            {

                JObject parsedData = JObject.Parse(jsonString.Replace("microsoftMapsNetworkCallback(", "").Replace(".d},'r229');", "}"));
                foreach (var data in parsedData)
                {
                    if (data.Key.Equals("response"))
                    {
                        if (nearbyPlaceList == null)
                            nearbyPlaceList = new List<NearByPlaceDetails>();
                        NearByPlaceDetails nearByPlaceDetails;
                        foreach (var result in data.Value)
                        {
                            foreach (var child in result.First["SearchResults"])
                            {
                                nearByPlaceDetails = new NearByPlaceDetails();
                                nearByPlaceDetails.Name = child["Name"].ToString();
                                nearByPlaceDetails.Vicinity = child["Address"].ToString() + (!string.IsNullOrEmpty(child["City"].ToString()) ? "," : "") + child["City"].ToString() + (!string.IsNullOrEmpty(child["State"].ToString()) ? "," : "") + child["State"].ToString() + (!string.IsNullOrEmpty(child["PostalCode"].ToString()) ? "," : "") + child["PostalCode"].ToString() + (!string.IsNullOrEmpty(child["Country"].ToString()) ? "," : "") + child["Country"].ToString() + (!string.IsNullOrEmpty(child["Phone"].ToString()) ? "," : "") + child["Phone"].ToString();
                                nearByPlaceDetails.Latitude = child["Location"]["Latitude"].ToString();
                                nearByPlaceDetails.Longitude = child["Location"]["Longitude"].ToString();
                                nearByPlaceDetails.Category = type;
                                nearByPlaceDetails.PhoneNumber = child["Phone"].ToString();
                                nearbyPlaceList.Add(nearByPlaceDetails);
                            }
                        }
                    }
                }

                if (nearbyPlaceList != null && nearbyPlaceList.Count > 0)
                {
                    NoDataMsgText.Visibility = Visibility.Collapsed;
                    localhelpLongList.Visibility = Visibility.Visible;
                    localhelpLongList.ItemsSource = nearbyPlaceList.OrderByDescending(a => a.Category).ToList();
                }
                else
                {
                    NoDataMsgText.Visibility = Visibility.Visible;
                    localhelpLongList.Visibility = Visibility.Collapsed;
                }
                if (IsLocalHelpLoaded)
                    ShowProgressBar(false);
            }
            catch (Exception)
            {
                NoDataMsgText.Visibility = Visibility.Visible;
                localhelpLongList.Visibility = Visibility.Collapsed;

                ShowProgressBar(false);
            }
        }

        public void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            List<NearByPlaceDetails> nearbyPlaceList = null;
            try
            {
                JObject parsedData = JObject.Parse(e.Result);

                foreach (var data in parsedData)
                {
                    if (data.Key.Equals("results"))
                    {
                        nearbyPlaceList = new List<NearByPlaceDetails>();
                        NearByPlaceDetails nearByPlaceDetails;
                        foreach (var result in data.Value)
                        {
                            nearByPlaceDetails = new NearByPlaceDetails();
                            nearByPlaceDetails.Name = result["name"].ToString();
                            nearByPlaceDetails.Vicinity = result["vicinity"].ToString();
                            nearByPlaceDetails.Latitude = result["geometry"]["location"]["lat"].ToString();
                            nearByPlaceDetails.Longitude = result["geometry"]["location"]["lng"].ToString();
                            nearByPlaceDetails.Category = result["types"][0].ToString();
                            nearbyPlaceList.Add(nearByPlaceDetails);
                        }
                    }
                }

                if (nearbyPlaceList != null && nearbyPlaceList.Count > 0)
                {
                    NoDataMsgText.Visibility = Visibility.Collapsed;
                    localhelpLongList.Visibility = Visibility.Visible;
                    localhelpLongList.ItemsSource = nearbyPlaceList.OrderByDescending(a => a.Category).ToList();
                }
                else
                {
                    NoDataMsgText.Visibility = Visibility.Visible;
                    localhelpLongList.Visibility = Visibility.Collapsed;
                }

                ShowProgressBar(false);
            }
            catch (Exception)
            {
                NoDataMsgText.Visibility = Visibility.Visible;
                localhelpLongList.Visibility = Visibility.Collapsed;

                ShowProgressBar(false);
            }
        }

        private void localhelpLongList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                FrameworkElement fe = sender as FrameworkElement;
                dynamic selectedDetails = fe.DataContext as dynamic;
                NavigationService.Navigate(new Uri("/Pages/ShowRouteMap.xaml?lat=" + selectedDetails.Latitude + "&lng=" + selectedDetails.Longitude, UriKind.Relative));
                return;
            }
            catch (Exception)
            {

            }


        }

        private void ShowProgressBar(bool enable)
        {
            if (enable)
            {
                getlocalhelpProgressBar.Visibility = Visibility.Visible;
                getlocalhelpProgressBar.IsEnabled = true;
                getlocalhelpProgressMsgTxt.Visibility = Visibility.Visible;
            }
            else
            {
                getlocalhelpProgressBar.Visibility = Visibility.Collapsed;
                getlocalhelpProgressBar.IsEnabled = false;
                getlocalhelpProgressMsgTxt.Visibility = Visibility.Collapsed;
            }

        }

        private void SwitchOnGPS_OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!Globals.IsLocationServiceEnabled)
            {
                //to check if it is due to App's location setting
                if (!Globals.IsLocationConsentEnabled)
                {
                    Globals.CurrentProfile.LocationConsent = true;
                    App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.LocationServicePref, "true");
                    SwitchOnSosButton.Visibility = Globals.IsLocationServiceEnabled ? Visibility.Collapsed : Visibility.Visible;
                }
                //to check if it is from phone's location setting
                if (!Globals.IsPhoneLocationServiceEnabled)
                {
                    Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
                }
            }
        }

        private void imgPolice_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Callee defaultCallee = new Callee();
            defaultCallee.PhoneNumber = Constants.PoliceContact;
            defaultCallee.DisplayName = "Police";
            Utility.InitiateCall(defaultCallee);
        }

        private void imgAmbulance_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Callee defaultCallee = new Callee();
            defaultCallee.PhoneNumber = Constants.AmbulanceContact;
            defaultCallee.DisplayName = "Ambulance";
            Utility.InitiateCall(defaultCallee);
        }

        private void imgFire_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Callee defaultCallee = new Callee();
            defaultCallee.PhoneNumber = Constants.FireBrigadeContact;
            defaultCallee.DisplayName = "Fire Brigade";
            Utility.InitiateCall(defaultCallee);
        }

        private void LocalHelpCall_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var dataContext = (sender as Border).DataContext as NearByPlaceDetails;
            string phoneNumber = dataContext.PhoneNumber.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Replace(".", ""); ;
            Utility.InitiateCall(new Callee() { DisplayName = dataContext.Name, PhoneNumber = phoneNumber });
        }


        private void JoinGroupsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Settings.xaml?FromPage=SOS&index=2", UriKind.Relative));
        }

        #region Helplines

        private void btnCall_click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                //BL: This event will be fired only for IsLocateBuddy
                StackPanel oSPName = (StackPanel)sender;
                var oData = (Helpline)oSPName.DataContext;
                Utility.InitiateCall(new Callee() { DisplayName = oData.Name, PhoneNumber = oData.PhoneNumber });
            }
            catch (Exception ex)
            {
                CallErrorHandler(ex);
            }
        }

        private async void btnSMS_click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                StackPanel oSPName = (StackPanel)sender;
                string message = Constants.MessageTemplateText;
                Helpline oData = (Helpline)oSPName.DataContext;
                Utility.SendSMS(new SMSMessage() { PhoneNumbers = oData.PhoneNumber, Message = message });

            }
            catch (Exception ex)
            {
                CallErrorHandler(ex);
            }
        }

        private void CallErrorHandler(Exception ex)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(ex.Message, "basicWrap", "Info"));
        }
        #endregion
    }


    public class NearByPlaceDetails
    {
        const string VISIBLE = "Visible";
        const string COLLAPSED = "Collapsed";
        public string Name { get; set; }
        public string Vicinity { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Category { get; set; }
        public string PhoneNumber { get; set; }
        public string DialEnabled
        {
            get
            {
                return (!string.IsNullOrEmpty(PhoneNumber)) ? VISIBLE : COLLAPSED;
            }
        }
        public string IsPolice
        {
            get
            {
                return (Category.ToUpper().Equals("POLICE")) ? VISIBLE : COLLAPSED;
            }
        }
        public string IsHospital
        {
            get
            {
                return (Category.ToUpper().Equals("HOSPITAL")) ? VISIBLE : COLLAPSED;
            }

        }

    }


}