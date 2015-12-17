using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Tasks;
using System.Reflection;

namespace SOS.Phone
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private SOSDataContext _dataContext;

        public ProfileViewModel()
        {
            _dataContext = new SOSDataContext(SOSDataContext.DBConnectionString);
        }

        private ProfileTableEntity _currentProfile = null;
        public ProfileTableEntity CurrentProfile
        {
            get
            {
                return _currentProfile;
            }
            private set
            {
                _currentProfile = value;
                NotifyPropertyChanged("CurrentProfile");
            }
        }

        //private ObservableCollection<ProfileTableEntity> Profiles = null;
        /// <summary>
        /// Purpose: Use this to show all profiles in ListPicker, for user to select one.
        /// </summary>
        public ObservableCollection<ProfileTableEntity> AllProfiles
        {
            get
            {
                var profilesInLocalStore = from ProfileTableEntity profile in _dataContext.MyProfilesTable
                                           select profile;

                var profiles = new ObservableCollection<ProfileTableEntity>(profilesInLocalStore);

                return profiles;
            }
        }

        private MembershipServiceRef.HealthUpdate _pendingUpdates = null;
        public MembershipServiceRef.HealthUpdate PendingUpdates
        {
            get
            {
                return _pendingUpdates;
            }
            private set
            {
                _pendingUpdates = value;
            }
        }

        private DateTime _lastSyncDateTime = DateTime.Now;
        public DateTime LastSyncDateTime
        {
            get
            {
                return _lastSyncDateTime;
            }
            private set
            {
                _lastSyncDateTime = value;
            }
        }

        public void LoadCurrentProfile(string profileId)
        {
#if DEBUG
            var allProfiles = from ProfileTableEntity profile in _dataContext.MyProfilesTable
                              select profile;
            foreach (var p in allProfiles)
                Debug.WriteLine("Profile: AId= " + p.AutoProfileId + "  Id= " + p.ProfileId + "  Number= " + p.MobileNumber);
#endif

            this.Message = string.Empty;
            this.IsInProgress = true;

            this.CurrentProfile = (from ProfileTableEntity profile in _dataContext.MyProfilesTable
                                   where profile.ProfileId == profileId
                                   select profile).FirstOrDefault();
            if (this.CurrentProfile == null)
            {
                CreateDefaultProfile();
                App.CurrentUser.User.CurrentProfileId = this.CurrentProfile.ProfileId;//Set the default profile ID to the User Entity
                App.CurrentUser.UpdateUser(App.CurrentUser.User);
            }

            this.IsInProgress = false;
            this.IsDataLoaded = true;
        }

        public void CreateDefaultProfile()
        {
            ProfileTableEntity profile = new ProfileTableEntity();
            profile.ProfileId = "0";
            profile.MobileNumber = "+000000000000";
            profile.CanEmail = true;
            profile.CanSMS = true;
            profile.CanFBPost = false;
            profile.LocationConsent = true;
            profile.PostLocationConsent = true;
            profile.MapView = MapCartographicMode.Road;
            profile.IsSOSStatusSynced = true;
            profile.IsTrackingStatusSynced = true;
            profile.MessageTemplate = Constants.MessageTemplateText;
            profile.IsDataSynced = true;
            profile.CountryCode = "+91";
            profile.PoliceContact = "100";
            profile.AmbulanceContact = "108";
            profile.FireContact = "101";
            AddProfile(profile);
        }

        public void DeleteOfflineProfile()
        {
            DeleteProfile("0");
        }

        public void AddProfile(MembershipServiceRef.Profile newProfile)
        {
            AddProfile(ConvertProfile(newProfile));
        }

        public void AddProfile(ProfileTableEntity newProfile)
        {
            _dataContext.MyProfilesTable.InsertOnSubmit(newProfile);
            _dataContext.SubmitChanges();

            this.CurrentProfile = newProfile;
        }

        public void AddProfiles(ObservableCollection<MembershipServiceRef.Profile> profiles)
        {
            foreach (var profile in profiles)
                _dataContext.MyProfilesTable.InsertOnSubmit(ConvertProfile(profile));

            _dataContext.SubmitChanges();
        }

        public MembershipServiceRef.Profile ConvertProfile(ProfileTableEntity profile)
        {
            var outProfile = new MembershipServiceRef.Profile();
            outProfile.ProfileID = Convert.ToInt64(profile.ProfileId);

            outProfile.FBGroupName = profile.FBGroupName;
            outProfile.FBGroupID = profile.FBGroupId;
            outProfile.RegionCode = profile.CountryCode;
            outProfile.MobileNumber = profile.MobileNumber;

            outProfile.CanMail = profile.CanEmail;
            outProfile.CanSMS = profile.CanSMS;
            outProfile.CanPost = profile.CanFBPost;

            outProfile.NotificationUri = profile.NotificationUri;

            //outProfile.CanArchive = profile.CanArchiveEvidence; //TODO String -> bool
            //outProfile.ArchiveFolder = profile.ArchivalFolder;

            outProfile.LocationConsent = profile.LocationConsent;
            

            return outProfile;
        }

        public ProfileTableEntity ConvertProfile(MembershipServiceRef.Profile profile)
        {
            var outProfile = new ProfileTableEntity();

            outProfile.ProfileId = profile.ProfileID.ToString();
            outProfile.CountryCode = profile.RegionCode;
            outProfile.MobileNumber = profile.MobileNumber;

            outProfile.FBGroupName = profile.FBGroupName;
            outProfile.FBGroupId = profile.FBGroupID;

            outProfile.CanEmail = profile.CanMail;
            outProfile.CanSMS = profile.CanSMS;
            outProfile.CanFBPost = profile.CanPost;
            //newProfile.CanArchiveEvidence = profile.CanArchive; //TODO
            //outProfile.ArchiveFolder = profile.LiveDetails != null ? profile.LiveDetails.ArchivalFolder : string.Empty;

            outProfile.CountryCode = profile.RegionCode;

            outProfile.LocationConsent = profile.LocationConsent;
            outProfile.PostLocationConsent = Globals.CurrentProfile.PostLocationConsent;
            outProfile.IsTrackingStatusSynced = Globals.CurrentProfile.IsTrackingStatusSynced;
            outProfile.IsSOSStatusSynced = Globals.CurrentProfile.IsSOSStatusSynced;
            outProfile.MapView = Globals.CurrentProfile.MapView;
            outProfile.LastSynced = DateTime.Now;
            outProfile.IsDataSynced = true;//TODO - Check

            return outProfile;
        }

        public void UpdateProfile(string profileId, ProfileTableEntity profile)
        {
            var localProfile = (from ProfileTableEntity p in _dataContext.MyProfilesTable
                                where p.ProfileId == profileId
                                select p).FirstOrDefault<ProfileTableEntity>();

            if (localProfile != null)
            {
                localProfile.ProfileId = profile.ProfileId;
                localProfile.CountryCode = profile.CountryCode;
                localProfile.MobileNumber = profile.MobileNumber;
                //localProfile.TinyUri = profile.TinyUri;

                localProfile.FBGroupName = profile.FBGroupName;
                localProfile.FBGroupId = profile.FBGroupId;

                localProfile.CanEmail = profile.CanEmail;
                localProfile.CanSMS = profile.CanSMS;
                localProfile.CanFBPost = profile.CanFBPost;
                localProfile.CanArchiveEvidence = profile.CanArchiveEvidence;
                localProfile.ArchiveFolder = profile.ArchiveFolder;

                localProfile.MessageTemplate = profile.MessageTemplate;

                localProfile.LocationConsent = profile.LocationConsent;
                if (!Globals.IsRegisteredUser)
                    localProfile.IsDataSynced = true;
                else
                    localProfile.IsDataSynced = profile.IsDataSynced;
                localProfile.PostLocationConsent = profile.PostLocationConsent;
                localProfile.IsSOSStatusSynced = profile.IsSOSStatusSynced;
                localProfile.IsTrackingStatusSynced = profile.IsTrackingStatusSynced;
                localProfile.MapView = profile.MapView;
                localProfile.CountryCode = profile.CountryCode;
                localProfile.CountryName = profile.CountryName;
                localProfile.AmbulanceContact = profile.AmbulanceContact;
                localProfile.PoliceContact = profile.PoliceContact;
                localProfile.FireContact = profile.FireContact;
                localProfile.MaxPhonedigits = profile.MaxPhonedigits;
                //localProfile.NotificationUri = profile.NotificationUri;

                _dataContext.SubmitChanges();

                App.MyProfiles.CurrentProfile = localProfile;
            }
        }

        public void DeleteProfile(string profileId)
        {
            var profiles = from ProfileTableEntity b in _dataContext.MyProfilesTable
                           where b.ProfileId == profileId
                           select b;

            _dataContext.MyProfilesTable.DeleteAllOnSubmit(profiles);
            _dataContext.SubmitChanges();

            AssignCurrentProfile();
        }

        public void DeleteProfile(ProfileTableEntity profile)
        {
            _dataContext.MyProfilesTable.DeleteOnSubmit(profile);
            _dataContext.SubmitChanges();

            AssignCurrentProfile();
        }

        public MembershipServiceRef.Profile GetNewProfileObject(string userName, string liveId, string countryCode, string mobileNumber, string securityCode, string enterpriseEmail, string enterpriseSecurityToken, string AccessToken, string RefreshToken)
        {
            var profile = new MembershipServiceRef.Profile();
            //profile.LiveDetails = new MembershipServiceRef.LiveCred() { LiveAccessToken = AccessToken, LiveRefreshToken = RefreshToken };
            profile.Email = liveId;
            profile.Name = userName;
            profile.RegionCode = countryCode;
            profile.MobileNumber = countryCode + mobileNumber;
            profile.SecurityToken = securityCode;
            if (Config.IsEnterpriseBuild)
            {
                profile.EnterpriseEmailID = enterpriseEmail;
                profile.EnterpriseSecurityToken = enterpriseSecurityToken;
            }
            profile.CanMail = true;
            profile.CanSMS = true;
            profile.CanPost = false;
            profile.LocationConsent = true;
            profile.NotificationUri = Globals.CurrentProfile.NotificationUri;

            profile.MyBuddies = App.MyBuddies.GetAllCurrentProfileBuddies();

            return profile;
        }

        private void AssignCurrentProfile()
        {
            //If CurrentProfile is deleted, assign first Profile as CurrentProfile 
            var nextProfile = (from ProfileTableEntity b in _dataContext.MyProfilesTable
                               select b).FirstOrDefault();
            if (nextProfile != null)
                App.CurrentUser.SaveUserSetting(UserViewModel.UserSetting.CurrentProfileID, nextProfile.ProfileId);
            else
                CreateDefaultProfile();
        }

        public void RestoreProfiles(ObservableCollection<SOS.Phone.MembershipServiceRef.Profile> profilesFromServer)
        {
            try
            {
                this.Message = string.Empty;
                this.IsInProgress = true;

                //TODO Case
                // Delete all existing profiles. Case: If user has un synced data, alert user that he will lose data
                var allProfiles = from p in _dataContext.MyProfilesTable
                                  select p;
                _dataContext.MyProfilesTable.DeleteAllOnSubmit(allProfiles);
                _dataContext.SubmitChanges();

                var Profiles = new ObservableCollection<ProfileTableEntity>();
                foreach (var profile in profilesFromServer)
                    Profiles.Add(ConvertProfile(profile));

                _dataContext.MyProfilesTable.InsertAllOnSubmit(Profiles);
                _dataContext.SubmitChanges();

                this.IsInProgress = false;
                this.IsDataLoaded = true;
            }
            catch
            {
                CallErrorHandler();
            }
        }

        #region Service Calls

        public void SyncProfileServer2Local(string profileId, MembershipServiceRef.Profile serverProfile)
        {
            UpdateProfile(profileId, ConvertProfile(serverProfile));
            var localProfile = (from ProfileTableEntity p in _dataContext.MyProfilesTable
                                where p.ProfileId == serverProfile.ProfileID.ToString()
                                select p).FirstOrDefault<ProfileTableEntity>();

            if (localProfile != null)
            {
                localProfile.LastSynced = DateTime.Now;
                _dataContext.SubmitChanges();
            }
        }

        public async void CheckPendingUpdatesFromServer()
        {
            try
            {
                if (Globals.IsRegisteredUser && Globals.IsDataNetworkAvailable)
                {
                    this.PendingUpdates = null;
                    this.PendingUpdates = await MembershipServiceWrapper.CheckPendingUpdates(Globals.User.CurrentProfileId, this.LastSyncDateTime);

                    if (this.PendingUpdates != null)
                    {
                        if (!this.PendingUpdates.IsProfileActive)
                        {
                            SaveCurrentProfileSetting(ProfileSetting.MobileNumber, "+000000000000");
                            Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.ProfileDeactivatedText, "basicWrap", "Oops!"));
                            this.LastSyncDateTime = DateTime.Now;
                        }
                        if (this.PendingUpdates.IsGroupModified)
                        {
                            //TODO
                        }
                        if (!string.IsNullOrEmpty(this.PendingUpdates.ServerVersion))
                        {
                            var nameHelper = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
                            Version serverVersion = new Version(PendingUpdates.ServerVersion);
                            var currentVersion = nameHelper.Version;
                            if (serverVersion > currentVersion)
                            {
                                if (MessageBox.Show(CustomMessage.AppUpdateNotifyText, "Confirm", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                                {
                                    MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();


                                    marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;

                                    marketplaceDetailTask.Show();
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }


        public void SaveCurrentProfileSetting(ProfileSetting setting, string value, bool skipIsDataSync = false)
        {
            try
            {
                ProfileTableEntity profile = this.CurrentProfile;
                string oldProfileId = profile.ProfileId;
                switch (setting)
                {
                    case ProfileSetting.ProfileId:
                        profile.ProfileId = value;
                        break;
                    case ProfileSetting.MobileNumber:
                        profile.MobileNumber = value;
                        break;
                    case ProfileSetting.LocationServicePref:
                        profile.LocationConsent = Convert.ToBoolean(value);
                        break;
                    case ProfileSetting.PostLocationServicePref:
                        profile.PostLocationConsent = Convert.ToBoolean(value);
                        break;
                    case ProfileSetting.SosStatusSynced:
                        profile.IsSOSStatusSynced = Convert.ToBoolean(value);
                        break;
                    case ProfileSetting.TrackingStatusSynced:
                        profile.IsTrackingStatusSynced = Convert.ToBoolean(value);
                        break;
                    case ProfileSetting.MapView:
                        profile.MapView = (MapCartographicMode)Enum.Parse(typeof(MapCartographicMode), value, true);
                        break;
                    case ProfileSetting.MessageTemplatePref:
                        profile.MessageTemplate = value;
                        break;
                    case ProfileSetting.CanSendSMS:
                        profile.CanSMS = Convert.ToBoolean(value);
                        break;
                    case ProfileSetting.CanSendEmail:
                        profile.CanEmail = Convert.ToBoolean(value);
                        break;
                    case ProfileSetting.CanSendFBPost:
                        profile.CanFBPost = Convert.ToBoolean(value);
                        break;
                    case ProfileSetting.FBGroupId:
                        profile.FBGroupId = value;
                        break;
                    case ProfileSetting.FBGroupName:
                        profile.FBGroupName = value;
                        break;
                    case ProfileSetting.SessionToken:
                        profile.SessionToken = value;
                        break;
                    case ProfileSetting.SetPrimeBuddy:
                        App.MyBuddies.SetPrimeBuddy(profile.ProfileId, value);
                        break;
                    case ProfileSetting.TrackingStatus:
                        if (!Convert.ToBoolean(value) && !this.CurrentProfile.IsSOSOn)
                            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
                        else
                            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;

                        profile.IsTrackingOn = Convert.ToBoolean(value);
                        break;
                    case ProfileSetting.SOSStatus:
                        if (!Convert.ToBoolean(value) && !this.CurrentProfile.IsTrackingOn)
                            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
                        else
                            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
                        profile.IsSOSOn = Convert.ToBoolean(value);
                        break;
                    case ProfileSetting.CountryCode:
                        profile.CountryCode = value;
                        break;
                    case ProfileSetting.PoliceContact:
                        profile.PoliceContact = value;
                        break;
                    case ProfileSetting.AmbulanceContact:
                        profile.AmbulanceContact = value;
                        break;
                    case ProfileSetting.FireContact:
                        profile.FireContact = value;
                        break;
                    case ProfileSetting.CountryName:
                        profile.CountryName = value;
                        break;
                    case ProfileSetting.MaxPhonedigits:
                        profile.MaxPhonedigits = value;
                        break;
                    case ProfileSetting.NotificationUri:
                        profile.NotificationUri = value;
                        break;
                    default:
                        break;
                }

                // Update the local storage with the new setting value
                this.UpdateProfile(oldProfileId, profile);

                // Set DataSyncedToServer status to false, if the key is not in the below list
                if (Globals.IsRegisteredUser &&
                    !(setting == ProfileSetting.ProfileId || setting == ProfileSetting.PostLocationServicePref ||
                        setting == ProfileSetting.MapView || setting == ProfileSetting.SessionToken ||
                        setting == ProfileSetting.TrackingStatus || setting == ProfileSetting.SOSStatus ||
                        setting == ProfileSetting.MessageTemplatePref || //TODO: Remove, if message is configurable by user.
                        setting == ProfileSetting.TrackingStatusSynced || setting == ProfileSetting.SosStatusSynced
                        || skipIsDataSync))
                    UpdateIsDataSynced(false);
            }
            catch (Exception)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.UnableToSaveSettingText, "basicWrap", "Oops!"));
            }
        }


        public void SaveEmergencyNumbers(CountryCodes countryDetails)
        {
            try
            {
                ProfileTableEntity profile = this.CurrentProfile;
                string oldProfileId = profile.ProfileId;
                profile.CountryCode = countryDetails.IsdCode;
                profile.CountryName = countryDetails.CountryName;
                profile.AmbulanceContact = countryDetails.Ambulance;
                profile.PoliceContact = countryDetails.Police;
                profile.FireContact = countryDetails.Fire;
                profile.MaxPhonedigits = countryDetails.MaxPhoneDigits;

                //Update the local storage with the new setting value
                this.UpdateProfile(oldProfileId, profile);

            }
            catch (Exception)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.UnableToSaveSettingText, "basicWrap", "Oops!"));
            }
        }

        public void UpdateIsDataSynced(bool value)
        {
            try
            {
                ProfileTableEntity profile = this.CurrentProfile;
                if (profile != null && value != profile.IsDataSynced)
                {
                    profile.IsDataSynced = value;
                    this.UpdateProfile(CurrentProfile.ProfileId, profile);
                }
            }
            catch (Exception)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.UnableToSaveSettingText, "basicWrap", "Oops!"));
            }
        }

        public enum ProfileSetting
        {
            ProfileId,
            MobileNumber,
            LocationServicePref,
            PostLocationServicePref,
            SosStatusSynced,
            TrackingStatusSynced,
            MapView,
            MessageTemplatePref,
            CanSendSMS,
            CanSendEmail,
            CanSendFBPost,
            FBGroupId,
            FBGroupName,
            SOSStatus,
            TrackingStatus,
            SessionToken,
            SetPrimeBuddy,
            CountryCode,
            PoliceContact,
            AmbulanceContact,
            FireContact,
            CountryName,
            MaxPhonedigits,
            NotificationUri
        }
        #endregion


        public void DeleteAllProfiles()
        {
            var allProfiles = from p in _dataContext.MyProfilesTable
                              select p;
            _dataContext.MyProfilesTable.DeleteAllOnSubmit(allProfiles);
            _dataContext.SubmitChanges();
        }


        #region Common Properties and Methods

        private bool _IsDataLoaded;
        public bool IsDataLoaded
        {
            get
            {
                return _IsDataLoaded;
            }
            set
            {
                if (value != _IsDataLoaded)
                {
                    _IsDataLoaded = value;
                    NotifyPropertyChanged("IsDataLoaded");
                }
            }
        }

        private bool _IsInProgress = false;
        public bool IsInProgress
        {
            get
            {
                return _IsInProgress;
            }
            set
            {
                if (value != _IsInProgress)
                {
                    _IsInProgress = value;
                    NotifyPropertyChanged("IsInProgress");
                }
            }
        }

        private string _message = string.Empty;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (value != _message)
                {
                    _message = value;
                    NotifyPropertyChanged("Message");
                }
            }
        }

        private void CallErrorHandler()
        {
            IsInProgress = false;
            Message = "Unable to retrieve the latest information...";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}