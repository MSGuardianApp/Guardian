using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SOS.Phone.Resources;
using Windows.Devices.Geolocation;
using System.Device.Location;
using System.Windows.Threading;
using SOS.Phone.MVVM.Model;
using SOS.Phone.Utilites.UtilityClasses;
using Microsoft.Phone.Data.Linq;
using System.Collections;
using System.Windows.Media;
using System.Reflection;
using System.IO.IsolatedStorage;
using SOS.Phone.MembershipServiceRef;
using System.Linq;

namespace SOS.Phone
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        //Properties for Location Background task
        public static Geolocator Geolocator { get; set; }
        public static GeoCoordinateWatcher geoCoordinateWatcher { get; set; }

        public static WPClogger logger = new WPClogger(LogLevel.debug);

        #region Fast resume

        enum SessionType
        {
            None,
            Home,
            DeepLink
        }

        // Set to Home when the app is launched from Primary tile.
        // Set to DeepLink when the app is launched from Deep Link.
        private SessionType sessionType = SessionType.None;

        // Set to true when the page navigation is being reset 
        bool wasRelaunched = false;

        // set to true when 5 min passed since the app was relaunched
        bool mustClearPagestack = false;

        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;


        #endregion

        #region User Properties

        private static UserViewModel user = null;
        public static UserViewModel CurrentUser
        {
            get
            {
                if (user == null)
                    user = new UserViewModel();

                return user;
            }
        }

        private static ProfileViewModel myProfiles = null;
        public static ProfileViewModel MyProfiles
        {
            get
            {
                if (myProfiles == null)
                    myProfiles = new ProfileViewModel();

                return myProfiles;
            }
        }

        private static BuddyViewModel myBuddies = null;
        public static BuddyViewModel MyBuddies
        {
            get
            {
                if (myBuddies == null)
                    myBuddies = new BuddyViewModel();

                return myBuddies;
            }
        }

        private static LocateBuddyViewModel locateBuddies = null;
        public static LocateBuddyViewModel LocateBuddies
        {
            get
            {
                if (locateBuddies == null)
                    locateBuddies = new LocateBuddyViewModel();

                return locateBuddies;
            }
        }

        private static GroupsViewModel myGroups = null;
        public static GroupsViewModel MyGroups
        {
            get
            {
                if (myGroups == null)
                    myGroups = new GroupsViewModel();

                return myGroups;
            }
        }

        private static HealthViewModel healthModel = null;
        public static HealthViewModel HealthModel
        {
            get
            {
                if (healthModel == null)
                    healthModel = new HealthViewModel();

                return healthModel;
            }
        }

        private static SOSViewModel sosViewModel = null;
        public static SOSViewModel SosViewModel
        {
            get
            {
                if (sosViewModel == null)
                {
                    sosViewModel = new SOSViewModel();

                }
                return sosViewModel;
            }
        }
        #endregion

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions.
            UnhandledException += Application_UnhandledException;

            // Standard XAML initialization
            InitializeComponent();
            this.MergeCustomColors();
            // Phone-specific initialization
            InitializePhoneApplication();

            // Language display initialization
            InitializeLanguage();

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                //PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            using (SOSDataContext db = new SOSDataContext(SOSDataContext.DBConnectionString))
            {
                int APP_VERSION = 1;    //sometimes Constants.APP_VERSION was not initiating to default value and thus was causing problems. Vijay will look for that. For now, have declared it here.
                var nameHelper = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
                Version currentVersion = nameHelper.Version;
                APP_VERSION = int.Parse(currentVersion.Major.ToString("00") + currentVersion.Minor.ToString("00") + currentVersion.Build.ToString("00") + currentVersion.Revision.ToString("00"));
                if (db.DatabaseExists() == false)
                {
                    db.CreateDatabase();
                    DatabaseSchemaUpdater dbUpdater = db.CreateDatabaseSchemaUpdater();
                    dbUpdater.DatabaseSchemaVersion = APP_VERSION;
                    dbUpdater.Execute();
                }
                else
                {
                    DatabaseSchemaUpdater dbUpdater = db.CreateDatabaseSchemaUpdater();

                    if (dbUpdater.DatabaseSchemaVersion < APP_VERSION)
                    {
                        if (dbUpdater.DatabaseSchemaVersion == 1)
                        {
                            dbUpdater.AddColumn<GroupTableEntity>("LastLocation");
                            dbUpdater.AddColumn<ProfileTableEntity>("PoliceContact");
                            dbUpdater.AddColumn<ProfileTableEntity>("AmbulanceContact");
                            dbUpdater.AddColumn<ProfileTableEntity>("FireContact");
                            dbUpdater.AddColumn<ProfileTableEntity>("MaxPhonedigits");
                            dbUpdater.AddColumn<ProfileTableEntity>("CountryName");

                            dbUpdater.AddColumn<ProfileTableEntity>("NotificationUri");
                        }
                        else if (dbUpdater.DatabaseSchemaVersion == 1000003)
                            //update columns for next DatabaseSchemaVersion 
                        {
                            dbUpdater.AddColumn<ProfileTableEntity>("NotificationUri");
                        }
                        else if (dbUpdater.DatabaseSchemaVersion == 2000000) //update columns for next DatabaseSchemaVersion 
                        {
                            dbUpdater.AddColumn<ProfileTableEntity>("NotificationUri");
                        }
                        // Add the new database version.
                        dbUpdater.DatabaseSchemaVersion = APP_VERSION;

                        // Perform the database update in a single transaction.
                        dbUpdater.Execute();
                    }
                }
            }

        }


        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            StateUtility.IsLaunching = true;

            if (!App.CurrentUser.IsDataLoaded)
            {
                LoadDataFromLocalStore();
                InitializeStaticVariables();
            }

            Globals.InitiateLocationServiceGeolocator();

            InitiateStartEvents();
            SetSOSStatus();
            //Fast resume
            // When a new instance of the app is launched, clear all deactivation settings
            RemoveCurrentDeactivationSettings();
        }

        private void MergeCustomColors()
        {
            //var dictionaries = new ResourceDictionary();
            string source = String.Format("/SOS.Phone;component/CustomTheme/ThemeResources.xaml");
            ResourceDictionary themeStyles = new ResourceDictionary { Source = new Uri(source, UriKind.Relative) };
            //dictionaries.MergedDictionaries.Add(themeStyles);


            ResourceDictionary appResources = App.Current.Resources;
            foreach (DictionaryEntry entry in themeStyles)
            {
                SolidColorBrush colorBrush = entry.Value as SolidColorBrush;
                SolidColorBrush existingBrush = appResources[entry.Key] as SolidColorBrush;
                if (existingBrush != null && colorBrush != null)
                {
                    existingBrush.Color = colorBrush.Color;
                }
            }
        }
        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            StateUtility.IsLaunching = false;
            StateUtility.IsRunningInBackground = false;

            //Fast resume
            // If some interval has passed since the app was deactivated (30 seconds in this example),
            // then remember to clear the back stack of pages
            mustClearPagestack = CheckDeactivationTimeStamp(0);


            // If IsApplicationInstancePreserved is not true, then set the session type to the value
            // saved in isolated storage. This will make sure the session type is correct for an
            // app that is being resumed after being tombstoned.
            if (!e.IsApplicationInstancePreserved)
            {
                RestoreSessionType();
            }

            if (!App.CurrentUser.IsDataLoaded)
            {
                LoadDataFromLocalStore();
                InitializeStaticVariables();
            }
            Globals.InitiateLocationServiceGeolocator();// To be verified ... called if Geolocator object is null
            SetSOSStatus();
        }

        private void SetSOSStatus()
        {
            if (CheckDeactivationTimeStamp(14400))
                Globals.IsDeactivateSOSSession = true;
            else
                Globals.IsDeactivateSOSSession = false;

        }
        private void LoadDataFromLocalStore()
        {
            long pid;

            App.CurrentUser.LoadUserData();
            Globals.Load2CurrentProfile(CurrentUser.User.CurrentProfileId);
            App.LocateBuddies.LoadLocateBuddies();

            #region V1.3 to V2.0 Data Migration code - Not required for new instances

            if (!Int64.TryParse(CurrentUser.User.CurrentProfileId, out pid))
            {
                Globals.IsAutoUpgradeFailed = true;
                Globals.AutoUpgradeLiveAuthID = CurrentUser.User.LiveAuthId;
                
                App.CurrentUser.UnregisterLocally();
            }
            #endregion

        }

        private async void InitiateStartEvents()
        {
            if (Globals.CurrentProfile != null)
            {
                if (Globals.CurrentProfile.IsTrackingOn)
                    Globals.InitiateTracking(false, true);
                if (!(Globals.CurrentProfile.IsTrackingOn || Globals.CurrentProfile.IsSOSOn))
                    App.MyProfiles.SaveCurrentProfileSetting(SOS.Phone.ProfileViewModel.ProfileSetting.SessionToken, string.Empty);
            }
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {

        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            //BUG: This is not being called when user doesnt press any button for exit app confirmation
            //MessageBox.Show("vr");
            // When the application closes, delete any deactivation settings from isolated storage
            RemoveCurrentDeactivationSettings();//for fast resume
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            //if (Debugger.IsAttached)
            //{
            //    // An unhandled exception has occurred; break into the debugger
            //    Debugger.Break();
            //}

            logger.log(LogLevel.critical, e.ExceptionObject);

        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Assign the URI-mapper class to the application frame.
            RootFrame.UriMapper = new UriAssociationMapper();

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += CheckForResetNavigation;

            // Monitor deep link launching 
            //  RootFrame.Navigating += RootFrame_Navigating;//Fast resume

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Event handler for the Navigating event of the root frame. Use this handler to modify
        // the default navigation behavior.
        void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {

            // If the session type is None or New, check the navigation Uri to determine if the
            // navigation is a deep link or if it points to the app's main page.
            if (sessionType == SessionType.None && e.NavigationMode == NavigationMode.New)
            {
                // This block will run if the current navigation is part of the app's intial launch


                // Keep track of Session Type 
                if (e.Uri.ToString().Contains("DeepLink=true"))
                {
                    sessionType = SessionType.DeepLink;
                }
                else if (e.Uri.ToString().Contains("/MainPage.xaml"))
                {
                    sessionType = SessionType.Home;
                }
            }


            if (e.NavigationMode == NavigationMode.Reset)
            {
                // This block will execute if the current navigation is a relaunch.
                // If so, another navigation will be coming, so this records that a relaunch just happened
                // so that the next navigation can use this info.
                wasRelaunched = true;
            }
            else if (e.NavigationMode == NavigationMode.New && wasRelaunched)
            {
                // This block will run if the previous navigation was a relaunch
                wasRelaunched = false;

                if (e.Uri.ToString().Contains("DeepLink=true"))
                {
                    // This block will run if the launch Uri contains "DeepLink=true" which
                    // was specified when the secondary tile was created in MainPage.xaml.cs

                    sessionType = SessionType.DeepLink;
                    // The app was relaunched via a Deep Link.
                    // The page stack will be cleared.
                }
                else if (e.Uri.ToString().Contains("/MainPage.xaml"))
                {
                    // This block will run if the navigation Uri is the main page
                    if (sessionType == SessionType.DeepLink)
                    {
                        // When the app was previously launched via Deep Link and relaunched via Main Tile, we need to clear the page stack. 
                        sessionType = SessionType.Home;
                    }
                    else
                    {
                        if (!mustClearPagestack)
                        {
                            //The app was previously launched via Main Tile and relaunched via Main Tile. Cancel the navigation to resume.
                            e.Cancel = true;
                            RootFrame.Navigated -= ClearBackStackAfterReset;
                        }
                    }
                }

                mustClearPagestack = false;
            }
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            //if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
            //    return;
            if (e.NavigationMode != NavigationMode.New)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

        private void PhoneApplicationService_RunningInBackground(object sender, RunningInBackgroundEventArgs e)
        {
            StateUtility.IsRunningInBackground = true;
            // When the applicaiton is deactivated, save the current deactivation settings to isolated storage
            SaveCurrentDeactivationSettings();//for fast resume
        }


        #endregion

        // Initialize the app's font and flow direction as defined in its localized resource strings.
        //
        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        //
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }

        private void InitializeStaticVariables()
        {
            if (Globals.AllCountryCodes == null)
                Globals.GetAllCountryCodes();
            LoadSavedEmergencyNumbers();
        }

        void LoadSavedEmergencyNumbers()
        {
            if (Globals.CurrentProfile != null)
            {
                Constants.CountryCode = Globals.CurrentProfile.CountryCode == null ? "+91" : Globals.CurrentProfile.CountryCode;
                Constants.CountryName = Globals.CurrentProfile.CountryName == null ? "India" : Globals.CurrentProfile.CountryName;
                Constants.PoliceContact = Globals.CurrentProfile.PoliceContact == null ? "100" : Globals.CurrentProfile.PoliceContact;
                Constants.AmbulanceContact = Globals.CurrentProfile.AmbulanceContact == null ? "108" : Globals.CurrentProfile.AmbulanceContact;
                Constants.FireBrigadeContact = Globals.CurrentProfile.FireContact == null ? "101" : Globals.CurrentProfile.FireContact;
                Constants.MaxPhonedigits = int.Parse(Globals.CurrentProfile.MaxPhonedigits == null ? "10" : Globals.CurrentProfile.MaxPhonedigits);
            }

        }

        /// <summary>
        /// This background thread is across the pages. But this runs only when app is opened but not in suspend state.
        /// </summary>
        /// <param name="waitTime"></param>
        public static void BackgroundThread(int waitTime)
        {
            DispatcherTimer timer = new DispatcherTimer();
            int counter = 0;
            timer.Tick +=
                delegate(object s, EventArgs args)
                {
                    Globals.StopEvents();
                    Globals.KeepLive();
                };
            timer.Start();

            timer.Interval = new TimeSpan(0, 0, waitTime);
        }

        #region Fast resume

        // Helper method for removing a key/value pair from isolated storage
        public void RemoveValue(string Key)
        {
            // If the key exists
            if (settings.Contains(Key))
            {
                settings.Remove(Key);
            }
        }

        // Called when the app is deactivating. Saves the time of the deactivation and the 
        // session type of the app instance to isolated storage.
        public void SaveCurrentDeactivationSettings()
        {
            if (Globals.AddOrUpdateValue("DeactivateTime", DateTimeOffset.Now))
            {
                settings.Save();
            }

            if (Globals.AddOrUpdateValue("SessionType", sessionType))
            {
                settings.Save();
            }

        }

        // Called when the app is launched or closed. Removes all deactivation settings from
        // isolated storage
        public void RemoveCurrentDeactivationSettings()
        {
            RemoveValue("DeactivateTime");
            RemoveValue("SessionType");
            settings.Save();
        }

        // Helper method to determine if the interval since the app was deactivated is
        // greater than 30 seconds
        public bool CheckDeactivationTimeStamp(double time)
        {
            DateTimeOffset lastDeactivated;
            if (settings.Contains("DeactivateTime"))
            {
                lastDeactivated = (DateTimeOffset)settings["DeactivateTime"];
                var currentDuration = DateTimeOffset.Now.Subtract(lastDeactivated);

                return TimeSpan.FromSeconds(currentDuration.TotalSeconds) > TimeSpan.FromSeconds(time);

            }
            else
            {
                lastDeactivated = new DateTimeOffset();

                var currentDuration = DateTimeOffset.Now.Subtract(lastDeactivated);

                return time == 0 ? true : false;
            }
        }

        // Helper method to restore the session type from isolated storage.
        void RestoreSessionType()
        {
            if (settings.Contains("SessionType"))
            {
                sessionType = (SessionType)settings["SessionType"];
            }
        }


        #endregion
    }
}