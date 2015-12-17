using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SOS.Phone.ServiceWrapper;

namespace SOS.Phone
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private SOSDataContext _dataContext;

        public UserViewModel()
        {
            _dataContext = new SOSDataContext(SOSDataContext.DBConnectionString);
        }

        //View model for non-collections
        private UserTableEntity _user = null;
        public UserTableEntity User
        {
            get
            {
                return _user;
            }
            private set
            {
                _user = value;
                NotifyPropertyChanged("User");
            }
        }

        public void LoadUserData()
        {
            this.User = (from UserTableEntity u in _dataContext.UserTable
                         select u).FirstOrDefault();

            if (this.User == null)
                CreateDefaultUser();

            this.IsDataLoaded = true;
        }

        public void CreateDefaultUser()
        {
            UserTableEntity user = new UserTableEntity();

            //user.IsDataSynced = true;
            user.CurrentProfileId = "0";

            _dataContext.UserTable.InsertOnSubmit(user);
            _dataContext.SubmitChanges();

            this.User = user;
        }

        public void UpdateUser(UserTableEntity user)
        {
            UserTableEntity localUser = (from UserTableEntity u in _dataContext.UserTable
                                         select u).FirstOrDefault();
            localUser.UserId = user.UserId;
            localUser.Name = user.Name;
            localUser.LiveEmail = user.LiveEmail;
            localUser.LiveAuthId = Globals.User.LiveAuthId;
            if (user.CurrentProfileId != null)
                localUser.CurrentProfileId = user.CurrentProfileId;
            localUser.FBAuthId = user.FBAuthId;

            _dataContext.SubmitChanges();

            this.User = localUser;
        }

        public UserTableEntity ConvertUser(MembershipServiceRef.Profile profile)
        {
            UserTableEntity user = new UserTableEntity();
            user.UserId = profile.UserID.ToString();
            user.Name = profile.Name;
            user.LiveEmail = profile.Email;
            user.LiveAuthId = Globals.User.LiveAuthId;
            user.FBAuthId = profile.FBAuthID;
            user.CurrentProfileId = profile.ProfileID.ToString();

            return user;
        }

        public MembershipServiceRef.Profile ConvertUser(UserTableEntity user)
        {
            MembershipServiceRef.Profile profile = new MembershipServiceRef.Profile();
            profile.UserID = Convert.ToInt64(user.UserId);
            profile.Name = user.Name;
            profile.Email = user.LiveEmail;
            profile.LiveDetails = new MembershipServiceRef.LiveCred();
            profile.FBAuthID = user.FBAuthId;

            return profile;
        }
        #region Service Calls

        private void SyncUserServer2Local(MembershipServiceRef.Profile profile)
        {
            UpdateUser(ConvertUser(profile));
        }

        public async Task<string> UpdateUserLocal2Server(bool IsMobileNumberEdit = false, string NewMobileNumber = "", string NewSecurityToken = "")
        {
            IsSuccess = true;
            Message = string.Empty;
            this.IsInProgress = true;

            string result = "SUCCESS";
            try
            {
                MembershipServiceRef.Profile profObjectToBeSynced = App.MyProfiles.ConvertProfile(App.MyProfiles.CurrentProfile);

                profObjectToBeSynced.Name = Globals.User.Name;
                profObjectToBeSynced.UserID = Convert.ToInt64(Globals.User.UserId);
                profObjectToBeSynced.Email = Globals.User.LiveEmail;
                profObjectToBeSynced.FBAuthID = Globals.User.FBAuthId;
                profObjectToBeSynced.LiveDetails = new MembershipServiceRef.LiveCred();

                profObjectToBeSynced.MyBuddies = App.MyBuddies.GetAllCurrentProfileBuddies();
                profObjectToBeSynced.AscGroups = App.MyGroups.GetAllCurrentProfileGroups();

                MembershipServiceRef.Profile serverProfile = null;
                if (IsMobileNumberEdit && NewMobileNumber != string.Empty && NewSecurityToken != String.Empty)
                {
                    profObjectToBeSynced.MobileNumber = NewMobileNumber;
                    profObjectToBeSynced.SecurityToken = NewSecurityToken;
                    profObjectToBeSynced.RegionCode = Constants.CountryCode;
                    serverProfile = await MembershipServiceWrapper.UpdateProfilePhone(profObjectToBeSynced);
                }
                else
                {
                    serverProfile = await MembershipServiceWrapper.UpdateProfile(profObjectToBeSynced);
                }
                if (!ResultInterpreter.IsError(serverProfile.DataInfo))
                {
                    this.SyncFullProfileServer2Local(serverProfile);
                }
                else
                {
                    result = "ERROR";
                    if (ResultInterpreter.IsError(serverProfile.DataInfo, "PROFILENOTFOUND"))
                        result = "PROFILENOTFOUND";
                    else if (ResultInterpreter.IsError(serverProfile.DataInfo, "The Profile is invalid."))
                        result = "INVALIDPROFILE";
                    else if (ResultInterpreter.IsError(serverProfile.DataInfo, "Invalid Security Token"))
                        result = "INCORRECTSECURITYCODE";
                    
                }
                this.IsInProgress = false;
                this.IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                result = "ERROR";
                IsSuccess = false;
                Message = ex.Message;
            }
            return result;
        }

        public void SyncFullProfileServer2Local(MembershipServiceRef.Profile serverProfile)
        {
            string currentProfileId = Globals.User.CurrentProfileId; // For new Profile, Globals.User.CurrentProfileId will be 0

            this.SyncUserServer2Local(serverProfile);
            App.MyProfiles.SyncProfileServer2Local(currentProfileId, serverProfile);
            App.MyBuddies.SyncBuddiesServer2Local(currentProfileId, serverProfile.MyBuddies);
            App.MyGroups.SyncGroupsServer2Local(currentProfileId, serverProfile.AscGroups);
            App.MyProfiles.UpdateIsDataSynced(true);

            App.MyBuddies.CleanBuddies();
            App.MyGroups.CleanGroups();
        }

        public void UnregisterLocally()
        {
            App.CurrentUser.SaveUserSetting(UserViewModel.UserSetting.UserId, string.Empty);
            App.CurrentUser.SaveUserSetting(UserViewModel.UserSetting.CurrentProfileID, "0");
            App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.ProfileId, "0");
            App.MyBuddies.CleanBuddyIds("0");
            App.MyGroups.DeleteAllGroups();
            App.MyProfiles.UpdateIsDataSynced(true);
        }
        #endregion

        public static void RestoreAllProfiles(ObservableCollection<SOS.Phone.MembershipServiceRef.Profile> profiles)
        {
            if (profiles != null && profiles.Count > 0)
            {
                UserTableEntity user = new UserTableEntity();
                user.UserId = profiles[0].UserID.ToString();
                user.Name = profiles[0].Name;
                user.LiveEmail = profiles[0].Email;
                user.LiveAuthId = Globals.User.LiveAuthId;
                user.CurrentProfileId = profiles[0].ProfileID.ToString();
                user.FBAuthId = profiles[0].FBAuthID;

                App.CurrentUser.UpdateUser(user);
                App.MyProfiles.RestoreProfiles(profiles);

                App.MyBuddies.DeleteAllBuddies();
                App.MyGroups.DeleteAllGroups();

                var allBuddies = new ObservableCollection<MembershipServiceRef.Buddy>();
                var allGroups = new ObservableCollection<MembershipServiceRef.Group>();

                foreach (var profile in profiles)
                {
                    if (profile.MyBuddies != null)
                        App.MyBuddies.RestoreProfileBuddies(profile.ProfileID.ToString(), profile.MyBuddies);

                    if (profile.AscGroups != null)
                        App.MyGroups.RestoreProfileGroups(profile.ProfileID.ToString(), profile.AscGroups);
                }

            }
        }

        public void SaveUserSetting(UserSetting setting, string value)
        {
            try
            {
                UserTableEntity user = this.User;
                switch (setting)
                {
                    case UserSetting.UserId:
                        user.UserId = value;
                        break;
                    case UserSetting.Name:
                        user.Name = value;
                        break;
                    case UserSetting.Email:
                        user.LiveEmail = value;
                        break;
                    case UserSetting.CurrentProfileID:
                        user.CurrentProfileId = value;
                        break;
                    case UserSetting.FBAuthID:
                        user.FBAuthId = value;
                        break;
                    default:
                        break;
                }
                //Update the local storage with the new setting value
                this.UpdateUser(user);
                if (Globals.IsRegisteredUser && !(setting == UserSetting.CurrentProfileID || setting == UserSetting.UserId))
                    App.MyProfiles.UpdateIsDataSynced(false);
            }
            catch (Exception)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.UnableToSaveSettingText, "basicWrap", "Oops!"));
            }
        }

        public enum UserSetting
        {
            UserId,
            Name,
            Email,
            CurrentProfileID,
            IsDataSynced,
            FBAuthID
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

        private bool _IsSuccess = false;
        public bool IsSuccess
        {
            get
            {
                return _IsSuccess;
            }
            set
            {
                if (value != _IsSuccess)
                {
                    _IsSuccess = value;
                    NotifyPropertyChanged("IsSuccess");
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