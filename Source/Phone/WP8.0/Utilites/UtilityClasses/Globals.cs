using System.Globalization;
using Microsoft.Devices;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using SOS.Phone.MembershipServiceRef;
using SOS.Phone.ServiceWrapper;
using SOS.Phone.Utilites.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Windows.Devices.Geolocation;

namespace SOS.Phone
{
    public static class Globals
    {
        public static Geolocator LocationServiceGeolocator = null;
        public static List<GeoLocation> TagList = new List<GeoLocation>();

        //Below variables are used for processing optimizations
        public static GeoTagPlus RecentLocation = new GeoTagPlus(); // To be used for faster responses
        static bool _toastFlag = false;
        public static int PostedLocationIndex = 0;
        public static GeoCoordinate RouteDirectionsPushPinGeoCoordinate = null;
        public static string TrackMeDestinationName = string.Empty;
        public static bool IsSOSJustStopped = false;
        public static List<CountryCodes> AllCountryCodes { get; set; }
        public static bool IsSyncedFirstTime { get; set; }
        public static bool RetainSOSState { get; set; }
        public static bool IsDeactivateSOSSession { get; set; }

        public static bool IsAutoUpgradeFailed = false;
        public static string AutoUpgradeLiveAuthID;
        public static int AcceptanceAccuracy = Constants.IdealAccuray;

        public static UserTableEntity User
        {
            get
            {
                return App.CurrentUser.User;
            }
        }

        public static ProfileTableEntity CurrentProfile
        {
            get
            {
                return App.MyProfiles.CurrentProfile;
            }
        }

        public static bool IsRegisteredUser
        {
            get
            {
                if (User.UserId.GetValue().Trim() == string.Empty)
                    return false;

                return true;
            }
        }

        public static bool IsDataNetworkAvailable
        {
            get
            {
                if (Microsoft.Devices.Environment.DeviceType == DeviceType.Emulator) return true;
                //BL: Either Wifi or Cellular Data should be available. 
                bool isNetworkAvailable = true;
#if(!DEBUG)
                //Moved to DEBUG block as it is taking too much time in debug mode.
                isNetworkAvailable = isNetworkAvailable && (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType != Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None);
#endif
                isNetworkAvailable = isNetworkAvailable && (DeviceNetworkInformation.IsWiFiEnabled || (DeviceNetworkInformation.IsNetworkAvailable && (DeviceNetworkInformation.IsCellularDataEnabled || DeviceNetworkInformation.IsCellularDataRoamingEnabled)));

                return isNetworkAvailable;
            }
        }

        public static bool IsLocationConsentEnabled
        {
            get
            {
                if (!CurrentProfile.LocationConsent)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// To check both Consent and Phone's Location Services status
        /// </summary>
        public static bool IsLocationServiceEnabled
        {
            get
            {
                if (!IsLocationConsentEnabled)
                    return false;

                return IsPhoneLocationServiceEnabled;
            }
        }

        public static bool IsPhoneLocationServiceEnabled { get; set; }

        public static void DisposeLocator()
        {
            if (Config.UseGeoLocator)
            {
                App.Geolocator.PositionChanged -= Geolocator_PositionChanged;
                App.Geolocator = null;
            }
            else
            {
                App.geoCoordinateWatcher.PositionChanged -= Watcher_PositionChanged;
                App.geoCoordinateWatcher = null;
            }
        }

        public static void InitiateTracking(bool isNewSession = true, bool isFromAppActivation = false)
        {
            if (!CurrentProfile.IsTrackingOn || isFromAppActivation)
            {
                Debug.WriteLine(DateTime.Now.ToLongTimeString() + " - Executing Global Tracking method");

                PostedLocationIndex = 0;
                TagList = new List<GeoLocation>();

                if (isNewSession && string.IsNullOrEmpty(CurrentProfile.SessionToken))// Continue with SOS session, if session has already started.
                {
                    App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.SosStatusSynced, "true");
                    App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.TrackingStatusSynced, "true");

                    App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.SessionToken, Guid.NewGuid().ToString());
                }

                if (IsLocationServiceEnabled)
                {
                    App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.TrackingStatus, "true");
                    if (Config.UseGeoLocator)
                    {
                        App.Geolocator = new Geolocator();

                        App.Geolocator.DesiredAccuracy = PositionAccuracy.Default;
                        App.Geolocator.DesiredAccuracyInMeters = 101;
                        App.Geolocator.ReportInterval = Constants.GeolocatorInterval;//7.5 secs - 8 positions/ minute
                        //App.Geolocator.MovementThreshold = 25; 
                        //KeepLive() will keep the Status live by sending recent location to server every 5 minutes, if idle 

                        App.Geolocator.PositionChanged += Geolocator_PositionChanged;
                    }
                    else
                    {
                        //This is fallback option. We had observed GeoLocator is not working sometimes
                        App.geoCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
                        App.geoCoordinateWatcher.MovementThreshold = 25;
                        App.geoCoordinateWatcher.PositionChanged += Watcher_PositionChanged;
                        App.geoCoordinateWatcher.Start();
                    }
                }
            }
        }

        public static async void StopTracking(bool isForceStop = false)
        {
            if (!isForceStop && CurrentProfile.IsSOSOn) return;

            App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.TrackingStatus, "false");

            if (Config.UseGeoLocator)
            {
                App.Geolocator.PositionChanged -= Geolocator_PositionChanged;
                App.Geolocator = null;
            }
            else
            {
                App.geoCoordinateWatcher.PositionChanged -= Watcher_PositionChanged;
                App.geoCoordinateWatcher = null;
            }

            if (!CurrentProfile.IsSOSOn)
            {
                var retrier = new Retrier<Task<bool>>();
                bool result = await retrier.TryWithDelay(
                                () => LocationServiceWrapper.StopPostingsAsync(CurrentProfile.SessionToken), Constants.RetryMaxCount, 0);
                CurrentProfile.IsTrackingStatusSynced = result;
                App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.TrackingStatusSynced, result.ToString());
                CurrentProfile.IsSOSStatusSynced = result;
                App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.SosStatusSynced, result.ToString());

                TagList.Clear();
                PostedLocationIndex = 0;
                CurrentProfile.SessionToken = string.Empty;
            }
        }

        private static async void StopSOS(int retryCount)
        {
            if (IsRegisteredUser)
            {
                bool result = false;
                var retrier = new Retrier<Task<bool>>();
                if (CurrentProfile.IsTrackingOn)
                {
                    result = await retrier.TryWithDelay(
                                () => LocationServiceWrapper.StopSOSOnlyAsync(CurrentProfile.SessionToken), retryCount, 0);
                    if (result)
                        IsSOSJustStopped = true;
                }
                else
                {
                    result = await retrier.TryWithDelay(
                                () => LocationServiceWrapper.StopPostingsAsync(CurrentProfile.SessionToken), retryCount, 0);
                }
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    App.SosViewModel.UpdateErrorStatus(result ? false : true);
                    if (!result)
                    {
                        DisplayToast(CustomMessage.StopSOSFailText, "basicWrap", "Server connection failed");
                    }
                    else
                    {
                        DisplayToast(CustomMessage.StopSOSSuccessText, "basicWrap", "Guardian Tracking is active");
                    }
                });

                if (!CurrentProfile.IsTrackingOn)
                {
                    CurrentProfile.SessionToken = string.Empty;
                    CurrentProfile.IsTrackingStatusSynced = result;
                    App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.TrackingStatusSynced, result.ToString());
                }
                CurrentProfile.IsSOSStatusSynced = result;
                App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.SosStatusSynced, result.ToString());
            }
        }

        public static async Task InitiateStopSOSEventsAsync(bool appExit = false)
        {
            if (!appExit)
            {
                StopSOS(Constants.RetryMaxCount);

                if (CurrentProfile.CanSMS)
                {
                    if (MessageBox.Show(IsRegisteredUser ? CustomMessage.RegisteredUserSafeMessageConfirmationText : CustomMessage.SafeMessageConfirmationText, "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        SMSMessage smsMgs = new SMSMessage();
                        smsMgs.PhoneNumbers = Utility.GetBuddyNumbers();
                        smsMgs.Message = Constants.SafeMessageText;

                        Utility.SendSMS(smsMgs);
                    }
                }
            }
            else
                StopSOS(Constants.RetryMaxCountForAppExit);
        }

        public static void Geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            GeolocatorPositionChanged(sender, args);
        }

        public static void Watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> args)
        {
            WatcherPositionChanged(sender, args);
        }

        /// <summary>
        /// Removes noise from location. If the location is high noisy/ same location, returns null.
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static GeoCoordinate RefineLocation(GeoCoordinate loc)
        {
            try
            {
                Debug.WriteLine("     Started - Refining Location");
                loc.HorizontalAccuracy = Math.Round(loc.HorizontalAccuracy);

                var locCount = TagList.Count;
                // First location or If the last location captured is greater than 5 min
                if (locCount <= 3 || IsSOSJustStopped || RecentLocation.CapturedTime < DateTime.Now.AddMinutes(-4))
                    //if (loc.HorizontalAccuracy > 150)//TODO: Exception: The thread 0x1104 has exited with code 259 (0x103).
                    //{
                    //    var refinedLoc = Utility.GetCurrentLocationAsync().Result;
                    //    return (refinedLoc != null && refinedLoc.HorizontalAccuracy < loc.HorizontalAccuracy) ? refinedLoc : loc;
                    //}
                    //else
                    return loc;

                //If the there is more noise in the Location, ignore capturing
                if (loc.HorizontalAccuracy > AcceptanceAccuracy)
                {
                    AcceptanceAccuracy += Constants.IdealAccuray;
                    return null;//|| loc.VerticalAccuracy > 150
                }
                AcceptanceAccuracy = Constants.IdealAccuray;

                //If previous location and new location are same/ very near, ignore capturing
                if (RecentLocation.Coordinate != null)
                {
                    double distance = loc.GetDistanceTo(RecentLocation.Coordinate);
                    if (distance < Constants.GeolocatorMovementThreshold) return null;
                }

                //if (locCount > 2 &&
                //    !Algorithms.GPSFilter.IsValidLocation(TagList[locCount - 2], TagList[locCount - 1], loc.ToGeoLocation(), distance))
                //    return null;
            }
            catch { }

            Debug.WriteLine("     Completed - Refining Location");

            return loc;
        }

        public static bool WatcherPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> args, bool isGlobalInitialization = true)
        {
            Debug.WriteLine(DateTime.Now.ToLongTimeString() + " - Executing Global PositionChanged event");

            GeoCoordinate pos = RefineLocation(new GeoCoordinate(args.Position.Location.Latitude,
                                                                 args.Position.Location.Longitude,
                                                                 args.Position.Location.Altitude,
                                                                 Math.Round(args.Position.Location.HorizontalAccuracy),
                                                                 args.Position.Location.VerticalAccuracy,
                                                                 args.Position.Location.Speed,
                                                                 args.Position.Location.Course));

            if (pos == null) return false;

            //if (IsGlobalInitialization)
            //{
            //    //TODO: Commented as WP8 is not allowing to modify ReportInterval on the fly
            //    App.geolocator.PositionChanged -= Geolocator_PositionChanged;
            //    App.geolocator.ReportInterval = Algorithms.ReportIntervalCalculator.NextReportInterval(pos.Speed);
            //    App.geolocator.PositionChanged += Geolocator_PositionChanged;
            //}

            var currentLocation = pos.ToGeoLocation();

            if (CurrentProfile.IsTrackingOn || CurrentProfile.IsSOSOn)
            {
                RecentLocation.Coordinate = pos;
                RecentLocation.CapturedTime = DateTime.Now;

                //Capture Locally
                TagList.Add(currentLocation);

                //Update Server
                if (IsDataNetworkAvailable && IsRegisteredUser)
                {
                    //Note last updated time posted to server. If there are multiple previous posts, send them to server when connection is available
                    PostMyLocationWrapperAsync(pos);
                }

                ActiveToastNotifications();
            }
            return true;
        }

        public static void GeolocatorPositionChanged(Geolocator sender, PositionChangedEventArgs args, bool isGlobalInitialization = true)
        {
            Debug.WriteLine(DateTime.Now.ToLongTimeString() + " - Executing Global PositionChanged event");

            GeoCoordinate pos = RefineLocation(args.Position.Coordinate.ToGeoCoordinate());

            Debug.WriteLine("    " + DateTime.Now.ToLongTimeString() + " - Refining Location completed");
#if DEBUG
            if (pos == null) Debug.WriteLine("    " + DateTime.Now.ToLongTimeString() + " - Location Rejected");
#endif
            if (pos == null) return;

            //if (IsGlobalInitialization)
            //{
            //    //TODO: Commented as WP8 is not allowing to modify ReportInterval on the fly
            //    App.geolocator.PositionChanged -= Geolocator_PositionChanged;
            //    App.geolocator.ReportInterval = Algorithms.ReportIntervalCalculator.NextReportInterval(pos.Speed);
            //    App.geolocator.PositionChanged += Geolocator_PositionChanged;
            //}

            var currentLocation = pos.ToGeoLocation();

            if (CurrentProfile.IsTrackingOn || CurrentProfile.IsSOSOn)
            {
                RecentLocation.Coordinate = pos;
                RecentLocation.CapturedTime = DateTime.Now;

                //Capture Locally
                TagList.Add(currentLocation);

                //Update Server
                if (IsRegisteredUser)
                {
                    //Note last updated time posted to server. If there are multiple previous posts, send them to server when connection is available
                    PostMyLocationWrapperAsync(pos);
                }

                ActiveToastNotifications();
            }
        }

        private static void ActiveToastNotifications()
        {
            if (StateUtility.IsRunningInBackground && IsRegisteredUser)
            {
                Debug.WriteLine(DateTime.Now.ToLongTimeString() + " - Executing Global Position Changed event in background");

                ShellToast toast = new ShellToast();

                var currentMinute = DateTime.Now.Minute;
                if (CurrentProfile.IsSOSOn && currentMinute % 10 == 0 && !_toastFlag)
                {
                    _toastFlag = true;
                    toast.Title = "Guardian SOS is active!";
                    toast.Content += "If you are safe, please turn off";
                    toast.NavigationUri = new Uri("/Pages/SOS.xaml", UriKind.Relative);
                    toast.Show();

                }
                else if (CurrentProfile.IsTrackingOn && currentMinute % 15 == 0 && !_toastFlag)
                {
                    _toastFlag = true;
                    toast.Title = "Guardian Tracking is active!";
                    toast.Content = "Turn off, when not needed";
                    toast.NavigationUri = new Uri("/Pages/TrackMe.xaml", UriKind.Relative);
                    toast.Show();
                }
                else if (currentMinute % 10 == 0 || currentMinute % 15 == 0)
                    return;
                else if (currentMinute % 10 != 0 || currentMinute % 15 != 0)
                {
                    _toastFlag = false;
                }
            }
        }

        public static async void PostMyLocationWrapperAsync(GeoCoordinate pos)
        {
            var retrier = new Retrier<Task<bool>>();
            bool result = await retrier.TryWithDelay(
                            () => LocationServiceWrapper.PostMyLocationArrayAsync(), Constants.RetryMaxCount, 0);
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    App.SosViewModel.UpdateErrorStatus(result ? false : true);

                });
            if (result)
                if (!IsSyncedFirstTime && CurrentProfile.IsSOSOn) IsSyncedFirstTime = true;
        }

        public static void InitiateLocationServiceGeolocator()
        {
            if (LocationServiceGeolocator == null)
            {
                LocationServiceGeolocator = new Geolocator();

                LocationServiceGeolocator.DesiredAccuracy = PositionAccuracy.Default;
                LocationServiceGeolocator.ReportInterval = (uint)TimeSpan.FromMinutes(5).TotalMilliseconds;
                LocationServiceGeolocator.StatusChanged += LocationServiceGeolocator_StatusChanged;
            }
        }

        static void LocationServiceGeolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            switch (args.Status)
            {
                case PositionStatus.Disabled:
                    IsPhoneLocationServiceEnabled = false;
                    break;
                case PositionStatus.NotAvailable:
                case PositionStatus.NotInitialized:
                case PositionStatus.Initializing:
                case PositionStatus.NoData:
                case PositionStatus.Ready:
                    IsPhoneLocationServiceEnabled = true;
                    break;
            }
        }

        public static void Load2CurrentProfile(string profileId)
        {
            App.MyProfiles.LoadCurrentProfile(profileId);
            App.MyBuddies.LoadBuddies(profileId);
            App.MyGroups.LoadGroups(profileId);
        }

        /// <summary>
        /// If user doesnt move, but app is open, then keep his session Live with the last available position
        /// </summary>
        public static void KeepLive()
        {
            if ((CurrentProfile.IsSOSOn || CurrentProfile.IsTrackingOn) && RecentLocation.CapturedTime.AddMinutes(5) < DateTime.Now)
            {
                //RecentLocation.CapturedTime = DateTime.Now;
                LocationServiceWrapper.PostMyLocationAsync(RecentLocation.Coordinate, null);
            }
        }

        public static async void StopEvents()
        {
            if (IsRegisteredUser && !(CurrentProfile.IsTrackingStatusSynced && CurrentProfile.IsSOSStatusSynced))
            {
                bool result = false;
                if (!(CurrentProfile.IsSOSOn || CurrentProfile.IsTrackingOn))
                {
                    result = await LocationServiceWrapper.StopPostingsAsync("0");//Deletes all previous posts for the Profile
                }
                else if (!CurrentProfile.IsSOSStatusSynced && CurrentProfile.IsTrackingOn)
                {
                    var retrier = new Retrier<Task<bool>>();
                    result = await retrier.TryWithDelay(
                                () => LocationServiceWrapper.StopSOSOnlyAsync("0"), Constants.RetryMaxCount, 0);
                }
                else if (!CurrentProfile.IsTrackingStatusSynced && CurrentProfile.IsSOSOn)
                {
                    //Do nothing
                    result = true;
                }

                if (result)
                {
                    if (!CurrentProfile.IsSOSStatusSynced)
                    {
                        CurrentProfile.IsSOSStatusSynced = true;
                        App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.SosStatusSynced, "true");
                    }
                    if (!CurrentProfile.IsTrackingStatusSynced)
                    {
                        CurrentProfile.IsTrackingStatusSynced = true;
                        App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.TrackingStatusSynced, "true");
                    }
                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        DisplayToast(CustomMessage.StopSOSFailText, "basicWrap", "Server connection failed");
                    });
                }

            }
        }

        public static async Task<HealthUpdate> GetSystemHealthAsync()
        {
            HealthUpdate objHealth = null;
            if (IsDataNetworkAvailable && IsRegisteredUser)
            {
                objHealth = await MembershipServiceWrapper.CheckPendingUpdates(CurrentProfile.ProfileId, CurrentProfile.LastSynced.HasValue ? CurrentProfile.LastSynced.Value : DateTime.MinValue);
            }
            return objHealth;
        }

        public static void ShowPrivacy()
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri(Config.GuardianPortalUrl + "privacy.htm", UriKind.Absolute);
            webBrowserTask.Show();
        }


        public static void GetAllCountryCodes()
        {
            if (AllCountryCodes == null)
            {
                XDocument loaded = XDocument.Load("Content/CountryISDCodes.xml");
                AllCountryCodes = loaded.Descendants("Country").Select(x => new CountryCodes { CountryName = (string)x.Attribute("Name"), IsdCode = (string)x.Attribute("IsdCode"), MaxPhoneDigits = (string)x.Element("Properties").Attribute("MaxPhoneDigits"), Police = (string)x.Element("Properties").Attribute("Police"), Fire = (string)x.Element("Properties").Attribute("Fire"), Ambulance = (string)x.Element("Properties").Attribute("Ambulance") }).ToList();
                //if (countriesList != null)
                //{
                //    // int[] indexes = new int[] { 0, 36, 42, 90, 155, 180, 190 };
                //    int[] indexes = new int[] { 0 };
                //    //for china
                //    //int[] indexes = new int[] { 0, 42 };
                //    AllCountryCodes = indexes.Select(index => countriesList[index]).ToList();
                //}
            }

        }

        // Helper method for adding or updating a key/value pair in isolated storage
        public static bool AddOrUpdateValue(string key, Object value)
        {
            bool valueChanged = false;
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            // If the key exists
            if (settings.Contains(key))
            {
                // If the value has changed
                if (settings[key] != value)
                {
                    // Store the new value
                    settings[key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        public static void SetEmergencyNumbers(CountryCodes countryDetails)
        {
            Constants.CountryCode = countryDetails.IsdCode;
            Constants.CountryName = countryDetails.CountryName;
            Constants.AmbulanceContact = countryDetails.Ambulance;
            Constants.PoliceContact = countryDetails.Police;
            Constants.FireBrigadeContact = countryDetails.Fire;
            Constants.MaxPhonedigits = int.Parse(countryDetails.MaxPhoneDigits);
        }

        public static void DisplayToast(string msg, string type, string title)
        {
            Toast toast = new Toast(msg, title);

            toast.Show(type);
        }

        #region Auto Data Migration From V1.3 to V2.0 - Not required for New instances

        public static async Task GetMigratedData()
        {
            //Get the new Data and store locally. 
            ProfileList profiles = null;
            //Globals.IsAutoUpgradeFailed = false;
            try
            {
                profiles = await MembershipServiceWrapper.GetProfilesForLiveId(AutoUpgradeLiveAuthID);
            }
            catch (Exception ex)
            {
                Globals.IsAutoUpgradeFailed = true;
                //App.CurrentUser.UnregisterLocally();
                return;
            }
            if (profiles != null && profiles.List.Count > 0)
            {
                //App.CurrentUser.UnregisterLocally();
                App.MyProfiles.DeleteOfflineProfile();

                //Load all new profiles to Local Storage + Assign first profile as CurrentProfile  
                Globals.User.Name = profiles.List[0].Name;
                Globals.User.LiveEmail = profiles.List[0].Email;
                Globals.User.LiveAuthId = AutoUpgradeLiveAuthID;
                App.CurrentUser.UpdateUser(Globals.User);

                UserViewModel.RestoreAllProfiles(profiles.List);
                Globals.Load2CurrentProfile(profiles.List[0].ProfileID.ToString(CultureInfo.InvariantCulture));

                if (Globals.CurrentProfile.CountryCode != null) Globals.SetEmergencyNumbers(Globals.AllCountryCodes.First(c => c.IsdCode == Globals.CurrentProfile.CountryCode));

                Globals.IsAutoUpgradeFailed = false;
            }
            else
            {
                Globals.IsAutoUpgradeFailed = true;
                //App.CurrentUser.UnregisterLocally();
            }
        }
        #endregion
    }
}
