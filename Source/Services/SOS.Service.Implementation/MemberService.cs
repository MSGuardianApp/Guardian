using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using SOS.AzureSQLAccessLayer;
using SOS.AzureStorageAccessLayer;
using Guardian.Common;
using SOS.Mappers;
using SOS.Model;
using SOS.Service.Interfaces;
using SOS.Service.Interfaces.DataContracts;
using SOS.Service.Interfaces.DataContracts.InBound;
using SOS.Service.Interfaces.DataContracts.OutBound;
using SOS.Service.Security;
using Buddy = SOS.Service.Interfaces.DataContracts.Buddy;
using HealthUpdate = SOS.Service.Interfaces.DataContracts.HealthUpdate;
using Incident = SOS.AzureStorageAccessLayer.Entities.Incident;
using LiveUserStatus = SOS.AzureSQLAccessLayer.LiveUserStatus;
using Member = SOS.Service.Interfaces.DataContracts.OutBound.Member;
using Profile = SOS.Service.Interfaces.DataContracts.Profile;
using User = SOS.Service.Interfaces.DataContracts.User;
using utility = SOS.Service.Utility;

namespace SOS.Service.Implementation
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class MemberService : IMembership
    {
        private readonly MemberRepository _MemberRepository;
        private readonly Authorization _authService;
        private ConfigurationStorageAccess _ConfigurationStorageAccess;
        private GroupService _GroupService;
        private MemberStorageAccess _MemberStorageAccess;

        public MemberService()
        {
            _MemberStorageAccess = new MemberStorageAccess();
            _MemberRepository = new MemberRepository();
            _authService = new Authorization();
        }

        public PhoneValidation CreatePhoneValidator(PhoneValidation PhoneValidationIP)
        {
            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];
            string email = PhoneValidationIP.AuthenticatedLiveID;
            try
            {
                //Create an entry in Validator
                if (PhoneValidationIP.IsFilled)
                {
                    utility.ResultsManager.AddResultInfo(PhoneValidationIP, ResultTypeEnum.Error, "Data not filled");
                }

                var phValidator = new AzureStorageAccessLayer.Entities.PhoneValidation
                {
                    Name = PhoneValidationIP.Name,
                    AuthenticatedLiveID = liveUserID,
                    PhoneNumber = PhoneValidationIP.PhoneNumber,
                    Email = email,
                    RegionCode = PhoneValidationIP.RegionCode,
                    SecurityToken = RestingDataCryptoProvider.GenerateRandomDigit(),
                    EnterpriseSecurityToken = Config.IsEnterpriseBuild ? RestingDataCryptoProvider.GenerateRandomDigit() : null,
                    EnterpriseEmailID = PhoneValidationIP.EnterpriseEmailID,
                    IsValiated = false,
                    DeviceType = PhoneValidationIP.DeviceType
                };

                if (Config.IsEnterpriseBuild)
                {
                    if (!(phValidator.EnterpriseEmailID.EndsWith(Config.EnterpriseDomain)))
                    {
                        utility.ResultsManager.AddResultInfo(PhoneValidationIP, ResultTypeEnum.Error, "Provide valid Enterprise Email.");
                        return PhoneValidationIP;
                    }
                }
                //If reused or created send appropriate message as sms.
                bool bIsReused = false;

                _MemberStorageAccess = new MemberStorageAccess();
                AzureStorageAccessLayer.Entities.PhoneValidation phValidReturn =
                    _MemberStorageAccess.CreatePhoneValidatorEntry(phValidator, ref bIsReused);

                if (bIsReused)
                    utility.ResultsManager.AddResultInfo(PhoneValidationIP, ResultTypeEnum.Information,
                        "Record already existing.");
                else
                    utility.ResultsManager.AddResultInfo(PhoneValidationIP, ResultTypeEnum.Information,
                        "Validatoin Record Created.");

                if (Config.IsEnterpriseBuild)
                {
                    if (utility.Email.SendEmail(new List<string> { phValidator.EnterpriseEmailID },
                        string.Format(Common.Resources.Messages.EnterpriseValidationMsg, phValidReturn.EnterpriseSecurityToken),
                        "Guardian App - Enterprise Security Code for registration"))
                    {
                        utility.ResultsManager.AddResultInfo(PhoneValidationIP, ResultTypeEnum.Information, "Email Sent with Enterprise Security Code");
                    }
                    else
                        utility.ResultsManager.AddResultInfo(PhoneValidationIP, ResultTypeEnum.Exception,
                            "Unable to send Email for Enterprise Security Code");
                }

                if (phValidator.PhoneNumber.StartsWith("+91"))
                //If Indian mobiles, send SMS. For international, send email
                {
                    if (phValidReturn != null && Config.SendSms)
                    {
                        //Send SMS
                        if (utility.SMS.SendPhoneValidatorMessage(phValidator.PhoneNumber, phValidReturn.SecurityToken,
                            bIsReused))
                        {
                            utility.ResultsManager.AddResultInfo(PhoneValidationIP, ResultTypeEnum.Information,
                                "SMS Sent");
                        }
                        else
                            utility.ResultsManager.AddResultInfo(PhoneValidationIP, ResultTypeEnum.Exception,
                                "Couldnot send SMS");
                    }
                }
                else
                //For International mobiles, send SMS if IntlSMSServiceUserID in Config is not null, otherwise send email.
                {
                    if (phValidReturn != null && Config.SendSms && !string.IsNullOrEmpty(Config.IntlSMSServiceUserID))
                    {
                        //Send SMS
                        if (utility.SMS.SendPhoneValidatorMessage(phValidator.PhoneNumber, phValidReturn.SecurityToken,
                            bIsReused, true))
                        {
                            utility.ResultsManager.AddResultInfo(PhoneValidationIP, ResultTypeEnum.Information,
                                "SMS Sent");
                        }
                        else
                            utility.ResultsManager.AddResultInfo(PhoneValidationIP, ResultTypeEnum.Exception,
                                "Couldnot send SMS");
                    }
                    else
                    {
                        //Send Email
                        string Last4Dig = phValidator.PhoneNumber.Substring(phValidator.PhoneNumber.Length - 4, 4);
                        if (utility.Email.SendEmail(new List<string> { phValidator.Email },
                            string.Format(Common.Resources.Messages.PhoneValidationSMS, phValidReturn.SecurityToken, Last4Dig),
                            "Guardian App - Security Code for registration"))
                        {
                            utility.ResultsManager.AddResultInfo(PhoneValidationIP, ResultTypeEnum.Information, "Email Sent");
                        }
                        else
                            utility.ResultsManager.AddResultInfo(PhoneValidationIP, ResultTypeEnum.Exception,
                                "Couldnot send Email");
                    }
                }

                return PhoneValidationIP;
            }
            catch
            {
                //SOS.Exceptions.Handler.HandleServiceException(new ServiceException(ex));
            }
            return PhoneValidationIP;
        }

        public async Task<Profile> CreateProfile(Profile BareProfile)
        {
            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];

            if (BareProfile == null || string.IsNullOrEmpty(BareProfile.SecurityToken))
            {
                utility.ResultsManager.AddResultInfo(BareProfile, ResultTypeEnum.Error,
                    "Required Information not provided");
                return BareProfile;
            }

            BareProfile.LiveDetails = BareProfile.LiveDetails ?? new LiveCred();
            BareProfile.LiveDetails.LiveID = liveUserID;

            bool IsTokenValidated = ValidateToken(BareProfile);

            if (IsTokenValidated)
            {
                //CreateProfile
                await ValidateAndCreateProfile(BareProfile);
                utility.ResultsManager.AddResultInfo(BareProfile, ResultTypeEnum.Success, "Profile Created Successfully");
                //Delete Validation token
                DeleteValidationRecord(liveUserID, BareProfile.MobileNumber);
                utility.ResultsManager.AddResultInfo(BareProfile, ResultTypeEnum.Information,
                    "Sanitation completed Successfully");

                List<Buddy> buddies = BareProfile.MyBuddies;
                //If buddy List avaialable Call ManageBuddy
                if (BareProfile != null)
                {
                    bool isManageBuddyRelations = await ManageBuddyRelations(BareProfile);
                    if (isManageBuddyRelations)
                    {
                        var newBuddies = new List<Buddy>();
                        buddies.ForEach(buddy =>
                        {
                            if (buddy.BuddyID == 0)
                            {
                                Buddy bud =
                                    BareProfile.MyBuddies.Find(
                                        updatedBuddy => updatedBuddy.MobileNumber.Equals(buddy.MobileNumber));
                                newBuddies.Add(bud);
                            }
                        });
                        NotifyBuddies(BareProfile.ProfileID, BareProfile.Name, BareProfile.MobileNumber, newBuddies);
                    }
                }
                //If group list available Call manage Group     
                string defaultGroupID = Config.DefaultGroupID;
                if (Config.IsEnterpriseBuild && !string.IsNullOrEmpty(defaultGroupID))
                {
                    var GroupList = new List<Group>();

                    GroupList.Add(new Group
                    {
                        GroupID = defaultGroupID,
                        EnrollmentType = Enrollment.None,
                        ToRemove = false
                    });
                    if (BareProfile.AscGroups == null)
                        BareProfile.AscGroups = GroupList;
                }
                if (BareProfile.AscGroups != null && BareProfile.AscGroups.Count > 0)
                {
                    await ManageGroups(BareProfile);
                }
            }
            else
            {
                utility.ResultsManager.AddResultInfo(BareProfile, ResultTypeEnum.AuthError, "Invalid Security Token");
            }
            //Profile RetProfile = null;
            //if (!string.IsNullOrEmpty(BareProfile.ProfileID))
            //    RetProfile = this.GetProfileByProfileID(BareProfile.ProfileID);

            BareProfile.SecurityToken = string.Empty;

            //  RetProfile.DataInfo = BareProfile.DataInfo;

            return BareProfile;
        }

        public async Task<Profile> UpdateProfilePhone(Profile Profile2Update)
        {
            if (Profile2Update == null || string.IsNullOrEmpty(Profile2Update.SecurityToken))
            {
                utility.ResultsManager.AddResultInfo(Profile2Update, ResultTypeEnum.Error,
                    "Required Information not provided");
                return Profile2Update;
            }

            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];

            bool isAuthorized = await _authService.SelfAccess(liveUserID, Profile2Update.ProfileID);
            if (isAuthorized)
            {
                Profile2Update.LiveDetails = Profile2Update.LiveDetails ?? new LiveCred();
                Profile2Update.LiveDetails.LiveID = liveUserID;

                bool IsTokenValidated = ValidateToken(Profile2Update);

                if (IsTokenValidated)
                {
                    //Get Profile for the currently claimed fone number
                    Model.Profile ProfRec = null;
                    if (!string.IsNullOrEmpty(Profile2Update.MobileNumber))
                        ProfRec = await _MemberRepository.GetProfileByMobileAsync(Profile2Update.MobileNumber);

                    //If profile is existing invalidate(+000000000000) it first before updating this profile.
                    if (ProfRec != null)
                    {
                        await InvalidateProfile(ProfRec);
                        ProfRec = null;
                    }

                    Model.Profile eProfile = await _MemberRepository.GetProfileAsync(Profile2Update.ProfileID);

                    if (eProfile == null)
                    {
                        utility.ResultsManager.AddResultInfo(Profile2Update, ResultTypeEnum.Error, "PROFILENOTFOUND");
                        return Profile2Update;
                    }

                    Model.User eUser = await _MemberRepository.GetUserByUserIDAsync(Profile2Update.UserID);

                    if (eUser == null)
                    {
                        utility.ResultsManager.AddResultInfo(Profile2Update, ResultTypeEnum.Error,
                            "Invalid User ID Data Entry");
                        return Profile2Update;
                    }

                    if (eUser.LiveID != liveUserID)
                    {
                        utility.ResultsManager.AddResultInfo(Profile2Update, ResultTypeEnum.AuthError,
                            "You are not authorized to access this method");
                        return Profile2Update;
                    }

                    eUser.Name = Profile2Update.Name;
                    eUser.Email = Profile2Update.Email;
                    eUser.MobileNumber = utility.Security.Encrypt(Profile2Update.MobileNumber);

                    FillFBDetails(eUser, Profile2Update);
                    //Restrict Live ID Update

                    Profile2Update.LiveDetails = Profile2Update.LiveDetails ?? new LiveCred();
                    Profile2Update.LiveDetails.LiveID = liveUserID;

                    FillLiveDetails(eUser, Profile2Update, true);

                    await _MemberRepository.SaveUserAsync(eUser);
                    Profile2Update.UserID = eUser.UserID;

                    //Validate Profile, since just validated the fone number
                    Profile2Update.IsValid = true;

                    //Update ProfileWith no restriction.
                    await SaveProfile(eProfile, Profile2Update, false);

                    utility.ResultsManager.AddResultInfo(Profile2Update, ResultTypeEnum.Success,
                        "Phone Number Updated Successfully");

                    //Delete Validation token
                    DeleteValidationRecord(Profile2Update.LiveDetails.LiveID, Profile2Update.MobileNumber);
                    utility.ResultsManager.AddResultInfo(Profile2Update, ResultTypeEnum.Information,
                        "Sanitation completed Successfully");

                    List<Buddy> buddies = Profile2Update.MyBuddies;
                    //If buddy List avaialable Call ManageBuddy
                    if (buddies != null)
                    {
                        bool isManageBuddy = await ManageBuddyRelations(Profile2Update);
                        if (isManageBuddy)
                        {
                            var newBuddies = new List<Buddy>();
                            buddies.ForEach(buddy =>
                            {
                                if (buddy.BuddyID == 0)
                                {
                                    Buddy bud =
                                        Profile2Update.MyBuddies.Find(
                                            updatedBuddy => updatedBuddy.MobileNumber.Equals(buddy.MobileNumber));
                                    newBuddies.Add(bud);
                                }
                            });

                            NotifyBuddies(Profile2Update.ProfileID, Profile2Update.Name, Profile2Update.MobileNumber,
                                newBuddies);
                        }
                    }
                    //If group list available Call manage Group
                    if (Profile2Update.AscGroups != null && Profile2Update.AscGroups.Count > 0)
                    {
                        await ManageGroups(Profile2Update);
                    }

                    if (_GroupService == null)
                        _GroupService = new GroupService();

                    Profile2Update.AscGroups = await _GroupService.GetGroupsForProfileID(Profile2Update.ProfileID);
                }
                else
                {
                    utility.ResultsManager.AddResultInfo(Profile2Update, ResultTypeEnum.Error, "Invalid Security Token");
                }

                Profile2Update.SecurityToken = string.Empty;

                //Live Auth ID should not be returned back to client. Need to check if LiveDetails object is required.Discuss with Sharath
                if (Profile2Update.LiveDetails != null)
                {
                    Profile2Update.LiveDetails.LiveID = string.Empty;
                }

                //RetProfile.DataInfo = Profile2Update.DataInfo;
            }
            else
            {
                utility.ResultsManager.AddResultInfo(Profile2Update, ResultTypeEnum.AuthError,
                    "You are not authorized to update this profile");
            }
            return Profile2Update;
        }

        public async Task<Profile> UpdateProfile(Profile Profile2Update)
        {
            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];

            bool isAuthorized = await _authService.SelfAccess(liveUserID, Profile2Update.ProfileID);
            if (isAuthorized)
            {
                if (Profile2Update == null)
                    return null;

                Model.Profile eProfile = await _MemberRepository.GetProfileAsync(Profile2Update.ProfileID);

                if (eProfile == null)
                {
                    utility.ResultsManager.AddResultInfo(Profile2Update, ResultTypeEnum.Error, "PROFILENOTFOUND");
                    return Profile2Update;
                }

                if (!eProfile.IsValid)
                {
                    utility.ResultsManager.AddResultInfo(Profile2Update, ResultTypeEnum.Error, "The Profile is invalid.");
                    return Profile2Update;
                }
                Profile2Update.IsValid = true;

                Model.User eUser = await _MemberRepository.GetUserByUserIDAsync(eProfile.UserID);

                if (eUser.LiveID != liveUserID)
                {
                    utility.ResultsManager.AddResultInfo(Profile2Update, ResultTypeEnum.AuthError,
                        "You are not authorized to access this method");
                    return Profile2Update;
                }

                if (eProfile == null)
                {
                    utility.ResultsManager.AddResultInfo(Profile2Update, ResultTypeEnum.Error,
                        "Invalid User ID Data Entry");
                    return Profile2Update;
                }
                eUser.Name = Profile2Update.Name;
                eUser.Email = Profile2Update.Email;

                FillFBDetails(eUser, Profile2Update);
                //Restrict Live ID Update

                Profile2Update.LiveDetails = Profile2Update.LiveDetails ?? new LiveCred();
                Profile2Update.LiveDetails.LiveID = liveUserID;


                FillLiveDetails(eUser, Profile2Update, true);

                //Update Other User Details, except UID and LiveID
                await _MemberRepository.SaveUserAsync(eUser);
                Profile2Update.UserID = eUser.UserID;

                //Restrict Phone number Update.
                await SaveProfile(eProfile, Profile2Update, true);

                List<Buddy> buddies = Profile2Update.MyBuddies;
                //If buddy List avaialable Call ManageBuddy
                if (Profile2Update != null)
                {
                    bool isMangeBuddy = await ManageBuddyRelations(Profile2Update);
                    if (isMangeBuddy && buddies != null)
                    {
                        var newBuddies = new List<Buddy>();
                        buddies.ForEach(buddy =>
                        {
                            if (buddy.BuddyID == 0)
                            {
                                Buddy bud =
                                    Profile2Update.MyBuddies.Find(
                                        updatedBuddy => updatedBuddy.MobileNumber.Equals(buddy.MobileNumber));
                                newBuddies.Add(bud);
                            }
                        });
                        NotifyBuddies(Profile2Update.ProfileID, Profile2Update.Name, Profile2Update.MobileNumber,
                            newBuddies);
                    }
                }

                //If group list available Call manage Group
                if (Profile2Update.AscGroups != null && Profile2Update.AscGroups.Count > 0)
                {
                    await ManageGroups(Profile2Update);
                }

                if (_GroupService == null)
                    _GroupService = new GroupService();

                Profile2Update.AscGroups = await _GroupService.GetGroupsForProfileID(Profile2Update.ProfileID);
                utility.ResultsManager.AddResultInfo(Profile2Update, ResultTypeEnum.Success,
                    "Profile Updated Successfully");

                Profile2Update.SecurityToken = string.Empty;
            }
            else
            {
                utility.ResultsManager.AddResultInfo(Profile2Update, ResultTypeEnum.AuthError,
                    "You are not authorized to update this profile");
            }
            return Profile2Update;
        }

        public async Task UnBuddy(string profileID, string buddyUserID)
        {
            long ProfileID = Convert.ToInt64(profileID);
            long BuddyUserID = Convert.ToInt64(buddyUserID);

            await _MemberRepository.RemoveBuddyRelationAsync(ProfileID, BuddyUserID);
        }

        public async Task<BasicProfile> GetBasicProfile(string profileID, string sessionID)
        {
            long ProfileID = Convert.ToInt32(profileID);

            LocationRepository locRepository = new LocationRepository();
            bool isActiveSession = await locRepository.IsActiveSession(ProfileID, sessionID);

            if (!isActiveSession)
            {
                return null;
            }

            Model.Profile profileEntity = await _MemberRepository.GetProfileAsync(ProfileID);

            BasicProfile reqProfileMini = new BasicProfile();
            if (profileEntity == null)
            {
                utility.ResultsManager.AddResultInfo(reqProfileMini, ResultTypeEnum.Error,
                    "Profile Not availalbe for ProfileID id#" + ProfileID);
                return reqProfileMini;
            }

            if (profileEntity.User == null)
            {
                utility.ResultsManager.AddResultInfo(reqProfileMini, ResultTypeEnum.Exception,
                    "User Not availalbe for ProfileID id#" + ProfileID);
                return reqProfileMini;
            }

            if (profileEntity != null)
            {
                reqProfileMini = new BasicProfile
                {
                    ProfileID = profileEntity.ProfileID,
                    MobileNumber = utility.Security.Decrypt(profileEntity.MobileNumber ?? profileEntity.User.MobileNumber),
                    Name = profileEntity.User.Name,
                    UserID = profileEntity.UserID
                };
            }

            utility.ResultsManager.AddResultInfo(reqProfileMini, ResultTypeEnum.Success, "Profile Fetched.");

            return reqProfileMini;
        }

        public async Task<ProfileLite> GetProfileLiteByProfileID(string profileID, string GroupID = "0")
        {
            long ProfileID = Convert.ToInt64(profileID);

            string LiveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            ProfileLite reqProfile = null;
            bool isAuthorized = false;
            if (UType != null && Convert.ToChar(UType) == 'a')
                isAuthorized = await _authService.OwnGroupMembersAccess(LiveUserID, Convert.ToInt32(GroupID), ProfileID);
            else
            {
                bool isAuthorizedForSelf = await _authService.SelfAccess(LiveUserID, ProfileID);
                bool isAuthorizedForLocateBuddy = await _authService.LocateBuddyAccess(LiveUserID, ProfileID);
                if (isAuthorizedForSelf || isAuthorizedForLocateBuddy)
                    isAuthorized = true;
            }

            if (isAuthorized)
            {
                Model.Profile profileEntity = await _MemberRepository.GetProfileAsync(ProfileID);

                if (profileEntity == null)
                {
                    reqProfile = new ProfileLite();
                    utility.ResultsManager.AddResultInfo(reqProfile, ResultTypeEnum.Error,
                        "Profile Not availalbe for ProfileID id#" + ProfileID);
                    return reqProfile;
                }

                //var userEntity = await _MemberRepository.GetUserByUserIDAsync(profileEntity.UserID);//as user exists with profile, replaced userEntity with profileEntity.User

                if (profileEntity.User == null)
                {
                    reqProfile = new ProfileLite();
                    utility.ResultsManager.AddResultInfo(reqProfile, ResultTypeEnum.Exception,
                        "User Not availalbe for ProfileID id#" + ProfileID);
                    return reqProfile;
                }

                Caster.DiscardAuthDetails(profileEntity.User);

                if (profileEntity != null)
                {
                    reqProfile = Caster.MakeProfileLiteOnCombination(profileEntity, profileEntity.User);
                }

                utility.ResultsManager.AddResultInfo(reqProfile, ResultTypeEnum.Success, "Profile Fetched.");
            }
            else
            {
                reqProfile = new ProfileLite();
                utility.ResultsManager.AddResultInfo(reqProfile, ResultTypeEnum.AuthError,
                    "You are not authorized to view this profile.");
            }

            return reqProfile;
        }

        /// <summary>
        ///     Loads profile for given ProfileID, with Location details
        /// </summary>
        /// <param name="ProfileID"></param>
        /// <returns></returns>
        public async Task<Profile> GetProfileByProfileID(string profileID)
        {
            long ProfileID = Convert.ToInt64(profileID);
            Profile reqProfile = null;
            string LiveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];

            bool isAuthorized = await _authService.SelfAccess(LiveUserID, Convert.ToInt64(profileID));
            if (isAuthorized)
            {
                var MR = new MemberRepository(); //to use in parallel tasks

                Model.Profile profileEntity = null;
                //SOS.Model.User userEntity = null;
                List<Group> groups = null;
                List<Model.Buddy> eBud = null;
                List<ProfileLite> LBuds = null;

                if (_GroupService == null) _GroupService = new GroupService();

                var tasks = new List<Task>();

                Task<Model.Profile> profileTask = _MemberRepository.GetProfileAsync(ProfileID);

                //Task<Model.User> userTask = _MemberRepository.GetUserByUserIDAsync(profileEntity.UserID);//getting user from profile.user 

                Task<List<Group>> groupsTask = _GroupService.GetGroupsForProfileID(ProfileID);

                Task<List<Model.Buddy>> buddiesTask = MR.GetBuddiesForProfileIDAsync(ProfileID);

                Task<List<ProfileLite>> lBuddiesTask = GetLocateBuddiesByProfileIDAsync(profileID);

                tasks.Add(profileTask);
                //tasks.Add(userTask);            
                tasks.Add(groupsTask);
                tasks.Add(buddiesTask);
                tasks.Add(lBuddiesTask);
                Task.WaitAll(tasks.ToArray());

                profileEntity = profileTask.Result;
                //userEntity = userTask.Result;
                groups = groupsTask.Result;
                eBud = buddiesTask.Result;
                LBuds = lBuddiesTask.Result;
                //await Task.WhenAll(tasks);

                if (profileEntity == null)
                {
                    reqProfile = new Profile();
                    utility.ResultsManager.AddResultInfo(reqProfile, ResultTypeEnum.Error,
                        "No Profile for the given profile ID");
                    return reqProfile;
                }

                if (profileEntity.User == null)
                {
                    reqProfile = new Profile();
                    utility.ResultsManager.AddResultInfo(reqProfile, ResultTypeEnum.Exception,
                        "No User for the given profile ID- Possible data corruption.");
                    return reqProfile;
                }

                if (profileEntity != null)
                {
                    reqProfile = Caster.MakeContractProfile(profileEntity, profileEntity.User, groups,
                        (eBud != null && eBud.Count > 0) ? eBud : new List<Model.Buddy>(),
                    (LBuds != null && LBuds.Count > 0) ? LBuds : new List<ProfileLite>());
                }

                //Loading location details not required when fetching the profile
                //ProfileLite result = await LoadLocationDetails(reqProfile);//TODO: Load latest data from LiveSession
                //if (result != null)
                //{
                //    reqProfile.IsSOSOn = result.IsSOSOn;
                //    reqProfile.IsTrackingOn = result.IsTrackingOn;
                //    reqProfile.LastLocs = result.LastLocs;
                //}
                utility.ResultsManager.AddResultInfo(reqProfile, ResultTypeEnum.Success, "Profile Fetched.");
            }
            else
            {
                reqProfile = new Profile();
                utility.ResultsManager.AddResultInfo(reqProfile, ResultTypeEnum.AuthError,
                    "You are not authorized to view this profile.");
            }
            return reqProfile;
        }

        /// <summary>
        ///     Gets list of profiles for give live id. Live id should be authenticated before sending. No Location Data sent
        /// </summary>
        /// <param name="AuthenticatedLiveID">Authenticated Live ID. Authentication shold be taken care by the client.</param>
        /// <returns>List of profiles saved for this user.</returns>
        public async Task<ProfileList> GetProfilesForLiveID()
        {
            string LiveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];

            return await GetProfilesForLiveIDAsync(LiveUserID);
        }

        public async Task<HealthUpdate> CheckPendingUpdates(string ProfileID, string LastUpdateDate, string CurrentTime)
        {
            return await CheckPendingUpdates(ProfileID, LastUpdateDate);
        }

        private async Task<HealthUpdate> CheckPendingUpdates(string profileID, string LastUpdateDate)
        {
            long ProfileID = Convert.ToInt64(profileID);

            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];
            HealthUpdate Updates = null;
            bool isAuthorized = await _authService.SelfAccess(liveUserID, ProfileID);
            if (isAuthorized)
            {
                DateTime vTime;
                if (!DateTime.TryParse(LastUpdateDate, out vTime))
                    vTime = utility.Converter.GetSOSMinDateTime();

                AzureSQLAccessLayer.HealthUpdate pendingUpdate = null;
                string serverVersion = null;

                var tasks = new List<Task>();

                Task<AzureSQLAccessLayer.HealthUpdate> pendingUpdateTask =
                    _MemberRepository.GetPendingUpdatesAsync(ProfileID);
                _ConfigurationStorageAccess = new ConfigurationStorageAccess();
                Task serverVerTask =
                    Task.Run(() => { serverVersion = _ConfigurationStorageAccess.GetLatestAppVersion(); });
                tasks.Add(pendingUpdateTask);
                tasks.Add(serverVerTask);
                Task.WaitAll(tasks.ToArray());
                pendingUpdate = pendingUpdateTask.Result;

                if (pendingUpdate != null)
                {
                    Updates = new HealthUpdate
                    {
                        IsGroupModified = pendingUpdate.IsGroupModified,
                        IsProfileActive = pendingUpdate.IsProfileActive,
                        ServerVersion = serverVersion
                    };
                }
            }
            return Updates;
        }

        public async Task<User> SubscribeBuddyAction(string ProfileID, string UserID, string ActionType,
            string SubscribtionID)
        {
            var result = new ResultInfo();
            User user = null;
            try
            {
                user =
                    await SubscribeBuddyForProfileAction(ProfileID, UserID, Convert.ToInt32(ActionType), SubscribtionID);
                if (user == null)
                {
                    user = new User();
                    utility.ResultsManager.AddResultInfo(user, ResultTypeEnum.Error, "Failed");
                }
                else
                {
                    utility.ResultsManager.AddResultInfo(user, ResultTypeEnum.Success, "Success");
                }
            }
            catch (Exception e)
            {
                user = new User();
                utility.ResultsManager.AddResultInfo(user, ResultTypeEnum.Error, "Failed");
                return user;
            }
            return user;
        }

        public async Task<ResultInfo> UnRegisterUser()
        {
            var result = new ResultInfo();

            Model.User user = null;
            try
            {
                string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];
                if (String.IsNullOrEmpty(liveUserID))
                {
                    result.ResultType = ResultTypeEnum.AuthError;
                    result.Message = "You are not authorized to access this method";
                    return result;
                }

                user = await _MemberRepository.ValidateAndGetUserAsync(liveUserID);
                if (user == null)
                {
                    result.ResultType = ResultTypeEnum.Error;
                    result.Message = "User doesnot exist!";
                    return result;
                }

                //Get all the profiles associated with the user
                List<Model.Profile> profileList = await _MemberRepository.GetProfilesForUserIDAsync(user.UserID);

                if (profileList == null || profileList.Count == 0)
                {
                    result.ResultType = ResultTypeEnum.Information;
                    result.Message = "No Profiles associated with the user.";
                    return result;
                }

                foreach (Model.Profile profile in profileList)
                {
                    await _MemberRepository.DeleteWhileUnregisterUserAsync(profile.ProfileID);

                    //Get all history for the profiles and remove
                    _MemberStorageAccess.DeleteHistoryForProfile(profile.ProfileID.ToString());

                    var _incidentAccess = new IncidentStorageAccess();
                    //Remove Images from Blob
                    List<Incident> _TeaseReport = _incidentAccess.GetAllIncidentsByProfile(profile.ProfileID.ToString());
                    if (_TeaseReport != null)
                    {
                        foreach (Incident record in _TeaseReport)
                            if (record.MediaUri != null)
                                new BlobAccess().RemoveImage(record.MediaUri.Substring(record.MediaUri.Length - 36, 36));
                    }
                    //Get all TeaseReports and remove
                    _incidentAccess.RemoveIncident(profile.ProfileID.ToString());
                }

                utility.Email.SendUnRegisterEmailNotification(user.Email);

                result.ResultType = ResultTypeEnum.Success;
                result.Message = "Profile(s) Unregistered successfully!!";
                return result;
            }
            catch (Exception ex)
            {
                result.ResultType = ResultTypeEnum.Error;
                result.Message = "Failed to unregister profile(s) for the user";
                return result;
            }
        }

        public async Task<ResultInfo> UnAssignMarshalFromList(string targetUserProfileID, string buddyUserID)
        {
            long TargetUserProfileID = Convert.ToInt64(targetUserProfileID);
            long BuddyUserID = Convert.ToInt64(buddyUserID);

            var result = new ResultInfo();
            try
            {
                await _MemberRepository.RemoveBuddyRelationAsync(TargetUserProfileID, BuddyUserID);

                result.ResultType = ResultTypeEnum.Success;
                result.Message = "Marshal UnAssigned Successfully";
            }
            catch (Exception e)
            {
                result.ResultType = ResultTypeEnum.Exception;
                result.Message = "Failed";
            }
            return result;
        }

        public async Task<string> GetMembersCountForGroup(string GroupID)
        {
            int groupID = 0;
            if (!string.IsNullOrEmpty(GroupID))
                groupID = Convert.ToInt32(GroupID);
            if (groupID > 0)
            {
                int groupMembersCount = await _MemberRepository.GetMembersCountForGroup(groupID);
                return groupMembersCount.ToString();
            }
            return null;
        }

        public async Task<string> GetLocateBuddiesCountForUser(string UserID)
        {
            long userID = 0;
            if (!string.IsNullOrEmpty(UserID))
                userID = Convert.ToInt64(UserID);


            if (userID > 0)
            {
                int groupMembersCount = await _MemberRepository.GetLocateBuddiesCountForUser(userID);
                return groupMembersCount.ToString();
            }
            return null;
        }

        public async Task<LiveUserStatusList> GetFilteredGroupMembers(string GroupID, string searchName)
        {
            int groupID = Convert.ToInt32(GroupID);
            if (groupID > 0)
            {
                List<LiveUserStatus> membersByGroupAndName =
                    await _MemberRepository.GetFilteredGroupMembers(groupID, searchName);

                List<Interfaces.DataContracts.OutBound.LiveUserStatus> groupMembersLst =
                    membersByGroupAndName.ConvertToLiveMemberStatusList();

                var groupMembers = new LiveUserStatusList { List = groupMembersLst };

                return groupMembers;
            }
            return null;
        }

        public async Task<LiveUserStatusList> GetFilteredLocateBuddies(string UserID, string searchName)
        {
            long userID = Convert.ToInt64(UserID);

            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];

            bool isAuthorized = await _authService.ValidUserAccess(liveUserID, userID);

            if ((userID > 0) && isAuthorized)
            {
                List<LiveUserStatus> membersByNameForUser =
                    await _MemberRepository.GetFilteredLocateBuddies(userID, searchName);

                List<Interfaces.DataContracts.OutBound.LiveUserStatus> locateBuddiesMembersLst =
                    membersByNameForUser.ConvertToLiveMemberStatusList();

                var locateBuddyMembers = new LiveUserStatusList { List = locateBuddiesMembersLst };

                return locateBuddyMembers;
            }
            return null;
        }

        public async Task<List<Member>> GetMembersByGroupID(string groupID, string searchName, string startDate, string endDate)
        {
            int GroupID = Convert.ToInt32(groupID);
            List<AzureSQLAccessLayer.Member> groupMembersData = await _MemberRepository.GetMembersByGroupID(GroupID, searchName, startDate, endDate);

            List<Member> groupMembersLst = groupMembersData.ConvertToMembersList();

            return groupMembersLst;
        }

        public async Task<ResultInfo> SaveDispatchInfo(DispatchInfo userAssignedTo)
        {
            long ProfileID = Convert.ToInt64(userAssignedTo.ProfileID);

            string LiveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];
            var result = new ResultInfo();

            bool isAuthorized =
                await _authService.OwnGroupMembersAccess(LiveUserID, Convert.ToInt32(userAssignedTo.GroupID), ProfileID);
            if (isAuthorized)
            {
                bool isAssigned =
                    await
                        _MemberRepository.SaveDispatchInfo(ProfileID, userAssignedTo.SessionID,
                            userAssignedTo.AssignedTo);

                if (isAssigned)
                {
                    result.ResultType = ResultTypeEnum.Success;
                    result.Message = "Assigned successfully.";
                }
                else
                {
                    result.ResultType = ResultTypeEnum.Error;
                    result.Message = "Failed assigning.";
                }
            }
            else
            {
                result.ResultType = ResultTypeEnum.AuthError;
                result.Message = "You are not authorized to view this profile.";
            }
            return result;
        }

        private bool NotifyBuddies(long profileId, string profileUserName, string profileMobileNumber,
            List<Buddy> buddies)
        {
            string subscribeURI;
            string unsubscribeURI;
            bool isSMSEnabled = Config.SendSms;

            try
            {
                foreach (Buddy buddy in buddies)
                {
                    subscribeURI = utility.Shortify.CreateSubscribeBuddyActionURI(profileId.ToString(),
                        buddy.UserID.ToString(), buddy.SubscribtionID.ToString(), "s");
                    unsubscribeURI = utility.Shortify.CreateSubscribeBuddyActionURI(profileId.ToString(),
                        buddy.UserID.ToString(), buddy.SubscribtionID.ToString(), "u");

                    utility.Email.SendEmailBuddyNotification(buddy.Email, profileUserName, profileMobileNumber,
                        subscribeURI, unsubscribeURI);

                    if (isSMSEnabled)
                        utility.SMS.SendSMSBuddyNotification(buddy.MobileNumber, profileUserName, profileMobileNumber,
                            subscribeURI, unsubscribeURI);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private async Task<Profile> ManageGroups(Profile ProfileInst)
        {
            if (ProfileInst == null)
                return null;

            if (ProfileInst.AscGroups == null && ProfileInst.AscGroups.Count == 0)
            {
                utility.ResultsManager.AddResultInfo(ProfileInst, ResultTypeEnum.Information, "No Groups to add");
                return ProfileInst;
            }
            //TODO: Load group from Group service data not from the user provided..
            var GS = new GroupService();
            foreach (Group grp in ProfileInst.AscGroups)
            {
                if (grp.ToRemove)
                {
                    //Remove Group Relation
                    await GS.DisEnroll(grp, ProfileInst.ProfileID);
                }
                else
                {
                    await GS.ExecuteGroupEnrollWF(ProfileInst, grp);
                }
            }
            return ProfileInst;
        }

        private bool ValidateToken(Profile BareProfile)
        {
            if (_MemberStorageAccess == null)
                _MemberStorageAccess = new MemberStorageAccess();

            AzureStorageAccessLayer.Entities.PhoneValidation PhValidRec =
                _MemberStorageAccess.LoadPhoneValidation(BareProfile.LiveDetails.LiveID, BareProfile.MobileNumber);

            if (PhValidRec == null)
            {
                utility.ResultsManager.AddResultInfo(BareProfile, ResultTypeEnum.Error,
                    "No Records Validation request for phone number");
                return false;
            }

            if (Config.IsEnterpriseBuild)
            {
                if (!(PhValidRec.EnterpriseSecurityToken == BareProfile.EnterpriseSecurityToken))
                {
                    utility.ResultsManager.AddResultInfo(BareProfile, ResultTypeEnum.Error,
                                            "Incorrect Enterprise Security Token");
                    return false;
                }
            }

            if (PhValidRec.SecurityToken == BareProfile.SecurityToken)
            {
                PhValidRec.IsValiated = true;
                if (_MemberStorageAccess == null)
                    _MemberStorageAccess = new MemberStorageAccess();

                _MemberStorageAccess.SavePhoneValidationRecord(PhValidRec);

                return true;
            }
            return false;
        }

        private string GetDefaultEmail(User UserObj)
        {
            return (UserObj == null)
                ? string.Empty
                : (UserObj.LiveDetails == null || string.IsNullOrEmpty(UserObj.LiveDetails.LiveID))
                    ? UserObj.Email
                    : UserObj.LiveDetails.LiveID;
        }

        private async Task ValidateAndCreateProfile(Profile ProfileInst)
        {
            Model.Profile ProfRec = null;
            if (!string.IsNullOrEmpty(ProfileInst.MobileNumber))
                ProfRec = await _MemberRepository.GetProfileByMobileAsync(ProfileInst.MobileNumber);

            if (ProfRec == null)
            {
                await CreatUserAndProfile(ProfileInst);
                //CreateBareProfile
                utility.ResultsManager.AddResultInfo(ProfileInst, ResultTypeEnum.Information, "New Bare Profile Created");
            }
            else
            {
                Model.User UserRec = await _MemberRepository.GetUserByUserIDAsync(ProfRec.UserID);
                if (UserRec != null && UserRec.Email == ProfileInst.Email)
                {
                    // Existing User Record
                }
                else
                {
                    //Invalidate the Profile
                    //Change fone number to 00 & Invalidate
                    //Save the old invalid profile
                    await InvalidateProfile(ProfRec);

                    //Check if User Exists with the phone number
                    await CreatUserAndProfile(ProfileInst);
                }
            }
        }

        private async Task CreatUserAndProfile(Profile ProfileInst)
        {
            //if (string.IsNullOrEmpty(ProfileInst.Email)) ProfileInst.Email = ProfileInst.LiveDetails.LiveID;

            User user2Use = await CreateUserOnAbsenceForProfile(ProfileInst);


            if (user2Use != null)
            {
                //ProfileInst.ProfileID = Utility.TokenManager.GenerateNewProfileID();
                ProfileInst.UserID = user2Use.UserID;
                ProfileInst.IsValid = true;

                FillPostTokens(ProfileInst);
                bool SaveState = false;
                var entityProfile = new Model.Profile();
                SaveState = await SaveProfile(entityProfile, ProfileInst, false);
                ProfileInst.ProfileID = entityProfile.ProfileID;
                if (SaveState)
                {
                    utility.ResultsManager.AddResultInfo(ProfileInst, ResultTypeEnum.Success, "Created.");
                }
                else
                {
                    utility.ResultsManager.AddResultInfo(ProfileInst, ResultTypeEnum.Error, "Failed Creation.");
                }
            }
        }

        private void FillPostTokens(Profile ProfileInst)
        {
            ProfileInst.SessionID = utility.TokenManager.GenerateBasePostToken();
        }

        private void DeleteValidationRecord(string LiveID, string PhoneNumber)
        {
            //TODO: Delete The Validator Entry
            AzureStorageAccessLayer.Entities.PhoneValidation toDel = _MemberStorageAccess.LoadPhoneValidation(LiveID,
                PhoneNumber);

            _MemberStorageAccess.DeletePhoneValidationEntry(toDel);
        }

        private void FillFBDetails(Model.User eUser, User UserInst)
        {
            eUser.FBID = UserInst.FBID;
            eUser.FBAuthID = UserInst.FBAuthID;
        }

        private void FillLiveDetails(Model.User ReUser, User userInst, bool RestrictedUpdate)
        {
            if (userInst.LiveDetails != null)
            {
                if (!RestrictedUpdate)
                    ReUser.LiveID = userInst.LiveDetails.LiveID;
            }
        }

        private async Task<User> CreateUserOnAbsenceForProfile(User contractUser)
        {
            if (string.IsNullOrEmpty(contractUser.MobileNumber) || contractUser.LiveDetails == null)
            {
                utility.ResultsManager.AddResultInfo(contractUser, ResultTypeEnum.Error, "Live Details Required");
                return null;
            }
            var modelUser = new Model.User();

            Caster.FillEntityUser(modelUser, contractUser);
            long userId = await _MemberRepository.ManageUser(modelUser);
            contractUser.UserID = userId;

            return contractUser;
        }

        private async Task<bool> InvalidateProfile(Model.Profile Profile2Invalidate)
        {
            bool returnVal = false;
            if (Profile2Invalidate != null)
            {
                Profile2Invalidate.IsValid = false;
                Profile2Invalidate.MobileNumber = utility.Security.Encrypt("+000000000000");
                await _MemberRepository.SaveOrUpdateProfileAsync(Profile2Invalidate);
            }
            return returnVal;
        }


        private async Task<bool> SaveProfile(Model.Profile entityProfile, Profile ProfileToSave,
            bool RestrictPhoneUpdate)
        {
            //throw new NotImplementedException();
            if (ProfileToSave == null)
                return false;

            Caster.FillProfileEntity(entityProfile, ProfileToSave, RestrictPhoneUpdate);
            Model.Profile profileToSave = entityProfile;
            await _MemberRepository.SaveOrUpdateProfileAsync(profileToSave);

            entityProfile.ProfileID = profileToSave.ProfileID;
            return true;
        }

        /// <summary>
        ///     Create Update or Delete Buddy Relationship details only.
        ///     NOTE: Doesnot update's the Buddy's Profile's Phone Number if Profile Exists for a buddy.
        ///     That has to be triggered by the Buddy - user profile
        /// </summary>
        /// <param name="Profile2Update"></param>
        /// <returns></returns>
        public async Task<bool> ManageBuddyRelations(Profile Profile2Update)
        {
            if (Profile2Update == null)
                return false;
            bool isNewBuddyAdded = false;
            if (Profile2Update.MyBuddies != null && Profile2Update.MyBuddies.Count > 0)
            {
                foreach (Buddy bud in Profile2Update.MyBuddies)
                {
                    if (bud.UserID > 0)
                    {
                        //EDIT/Update/UnBuddy Buddy Info
                        if (bud.ToRemove)
                        {
                            //Remove Buddy Relation. = Unbuddy
                            long profileId = Profile2Update.ProfileID;
                            long userId = bud.UserID;
                            await _MemberRepository.RemoveBuddyRelationAsync(profileId, userId);
                        }
                        else if (bud.IsPrimeBuddy)
                        {
                            //to update primebuddy state
                            await _MemberRepository.SavePrimeBuddyAsync(bud.BuddyID, Profile2Update.ProfileID);
                        }
                    }
                    else
                    {
                        //Add new buddy but check if already exists or not.
                        //Check if User exists for the given number
                        // If so use the userid. + Given Name + Given Phone numbe + Given Email (if empty User's Mail)
                        //Else
                        //Add new User +  Given Name + Given Phone numbe + Given Email (if empty User's Mail) 
                        Model.Buddy buddy = bud.ConvertToModelBuddy();
                        buddy.ProfileID = Profile2Update.ProfileID;
                        await _MemberRepository.CreateBuddy(buddy);
                        isNewBuddyAdded = true;
                    }
                }
            }
            else
            {
                utility.ResultsManager.AddResultInfo(Profile2Update, ResultTypeEnum.Information, "No Buddy Change");
            }
            Profile2Update.MyBuddies = await GetBuddiesForProfileAsync(Profile2Update.ProfileID);
            if (Profile2Update.MyBuddies != null && isNewBuddyAdded)
                return true;
            return false;
        }

        public async Task<GroupMembers> GetAllProfilesAssociatedToGroup(string groupID)
        {
            int GroupID = Convert.ToInt32(groupID);
            if (GroupID == 0)
                return null;

            List<GroupMembership> rslt = await _MemberRepository.GetAllProfilesAssociatedToGroupAsync(GroupID);
            var grpMems = new GroupMembers { GroupID = GroupID };
            if (rslt != null && rslt.Count > 0)
            {
                grpMems.Profiles = new List<string>();
                foreach (GroupMembership gms in rslt)
                {
                    grpMems.Profiles.Add(gms.ProfileID.ToString());
                }
                utility.ResultsManager.AddResultInfo(grpMems, ResultTypeEnum.Information, "Data fetched");
            }
            else
            {
                utility.ResultsManager.AddResultInfo(grpMems, ResultTypeEnum.Error, "No data found.");
            }

            return grpMems;
        }

        public async Task<ProfileList> GetProfilesForLiveIDAsync(string LiveUserID)
        {
            var pll = new ProfileList { List = new List<Profile>() };

            Model.User user = null;
            if (!string.IsNullOrEmpty(LiveUserID))
                user = await _MemberRepository.ValidateAndGetUserAsync(LiveUserID);

            if (user == null)
            {
                utility.ResultsManager.AddResultInfo(pll, ResultTypeEnum.Information,
                    "No profiles associated for the User ");
                return pll;
            }

            List<Model.Profile> epll = await _MemberRepository.GetProfilesForUserIDAsync(user.UserID);
            Profile cpl = null;

            if (epll == null || epll.Count == 0)
            {
                utility.ResultsManager.AddResultInfo(pll, ResultTypeEnum.Information,
                    "No profiles associated for the User ");
                return pll;
            }

            List<Group> Grps = null;
            if (_GroupService == null) _GroupService = new GroupService();
            var _locationService = new LocationService();
            foreach (Model.Profile pl in epll)
            {
                List<Model.Buddy> eBud = null;
                List<ProfileLite> LBuds = null;

                var tasks = new List<Task>();

                Task<List<Model.Buddy>> getBuddyTask = _MemberRepository.GetBuddiesForProfileIDAsync(pl.ProfileID);

                Task<List<Group>> getGroupTask = _GroupService.GetGroupsForProfileID(pl.ProfileID);

                Task<List<ProfileLite>> getLbudsTask = _locationService.GetLocateBuddies(pl.UserID);

                tasks.Add(getBuddyTask);
                tasks.Add(getGroupTask);
                tasks.Add(getLbudsTask);

                Task.WaitAll(tasks.ToArray());

                eBud = getBuddyTask.Result;
                Grps = getGroupTask.Result;
                LBuds = getLbudsTask.Result;

                cpl = Caster.MakeContractProfile(pl, user,
                    Grps,
                    (eBud != null && eBud.Count > 0) ? eBud : new List<Model.Buddy>(),
                    (LBuds != null && LBuds.Count > 0) ? LBuds : new List<ProfileLite>()
                    );

                if (cpl != null)
                {
                    pll.List.Add(cpl);
                }
            }

            if (pll != null && pll.List != null && pll.List.Count > 0)
            {
                //resultset.Result = pll;
                if (Config.HasDataMigrated)
                    await _MemberRepository.UpdateProfileMap(user.UserID);
                utility.ResultsManager.AddResultInfo(pll, ResultTypeEnum.Success, "Data fetched");
            }
            else
            {
                utility.ResultsManager.AddResultInfo(pll, ResultTypeEnum.Error,
                    "GetProfilesForLiveID:Query yeilded result is empty");
            }

            return pll;
        }

        public async Task<string> GetMiniProfileForLiveID(string LiveUserID)
        {
            string userDetails = "";
            if (!string.IsNullOrEmpty(LiveUserID))
                userDetails = await _MemberRepository.GetProfileUserIDByLiveIDAsync(LiveUserID);

            return userDetails;
        }

        internal async Task<List<ProfileLite>> GetLocateBuddiesByProfileIDAsync(string UserProfileID)
        {
            List<ProfileLite> ProfileLiteList = null;
            var _locationService = new LocationService();
            ProfileLiteList profileLiteLst = await _locationService.GetBuddiesToLocateByProfileID(UserProfileID);
            ProfileLiteList = profileLiteLst.List;
            return ProfileLiteList;
        }

        internal async Task<List<Buddy>> GetBuddiesForProfileAsync(long ProfileID)
        {
            var retBudList = new List<Buddy>();
            List<Model.Buddy> entBudList = await _MemberRepository.GetBuddiesForProfileIDAsync(ProfileID);

            if (entBudList == null || entBudList.Count == 0)
                return null;
            retBudList = Caster.MakeContractBuddyFromEntity(entBudList);

            return retBudList;
        }

        private async Task<User> SubscribeBuddyForProfileAction(string profileID, string userID, int buddyState,
            string SubscribtionID)
        {
            long ProfileID = Convert.ToInt64(profileID);
            long UserID = Convert.ToInt64(userID);
            User user = null;

            Model.User resUser =
                await
                    _MemberRepository.SubscribeBuddyForProfileActionAsync(ProfileID, UserID, buddyState, SubscribtionID);
            if (resUser != null)
            {
                user = new User
                {
                    Name = resUser.Name,
                    MobileNumber = resUser.MobileNumber
                };
            }

            return user;
        }


        public async Task<User> ValidatedAndGetUser(string LiveUserID)
        {
            var returnUser = new User();
            if (string.IsNullOrEmpty(LiveUserID))
                utility.ResultsManager.AddResultInfo(returnUser, ResultTypeEnum.Error, "No Live User ID Passed.");

            Model.User eUser = await _MemberRepository.ValidateAndGetUserAsync(LiveUserID);

            if (eUser != null)
                Caster.MakeBasicContractUserForCache(returnUser, eUser);

            return returnUser;
        }
    }
}
