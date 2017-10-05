using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using SOS.AzureSQLAccessLayer;
using SOS.AzureSQLAccessLayer.Entities;
using SOS.AzureStorageAccessLayer;
using Guardian.Common;
using SOS.Mappers;
using SOS.Service.Interfaces;
using SOS.Service.Interfaces.DataContracts;
using SOS.Service.Interfaces.DataContracts.OutBound;
using entities = SOS.AzureStorageAccessLayer.Entities;
using model = SOS.Model;
using utility = SOS.Service.Utility;

namespace SOS.Service.Implementation
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class GroupService : IGroupService
    {
        public static DateTime LastUpdateDate;
        private static List<entities.Group> _AllGroups;
        private readonly GroupRepository _GroupRepository;
        private readonly LocationService _LocationService;
        private readonly MemberRepository _MemberRepository;
        private GroupStorageAccess _GroupStorageAccess;

        private MemberService _MS;
        private MemberStorageAccess _MemberStorageAccess;

        public GroupService()
        {
            _GroupStorageAccess = new GroupStorageAccess();
            _GroupRepository = new GroupRepository();
            _MemberStorageAccess = new MemberStorageAccess();
            _LocationService = new LocationService();
            _MemberRepository = new MemberRepository();
        }

        public static List<entities.Group> AllGroups
        {
            get
            {
                IsCacheResetRequired();
                if (_AllGroups == null)
                {
                    var grpService = new GroupService();
                    _AllGroups = grpService.GroupUsers(null);
                }
                return _AllGroups;
            }
        }

        public static List<entities.Group> ParentGroup
        {
            get { return AllGroups.Where(x => (!x.ParentGroupID.HasValue || x.ParentGroupID.Value == 0)).ToList(); }
        }

        public static List<entities.Group> SubGroups
        {
            get { return AllGroups.Where(x => (x.ParentGroupID.HasValue && x.ParentGroupID.Value != 0)).ToList(); }
        }


        public async Task<GroupList> GetListOfGroups(string SearchKey)
        {
            string[] SearchArray = SearchKey.Split(';');
            var returnV = new GroupList { List = new List<Group>() };
            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];

            //TODO: Cache Get the user id from cache once its implemented instead of going to storage
            bool isValidLiveId = await _MemberRepository.ValidateUser(liveUserID);

            if (!isValidLiveId)
            {
                utility.ResultsManager.AddResultInfo(returnV, ResultTypeEnum.AuthError,
                    "You are not authorized to access this method");
                return returnV;
            }


            if (_GroupStorageAccess == null) _GroupStorageAccess = new GroupStorageAccess();
            List<entities.Group> entityGrps = null;
            if (SearchKey.ToLower() != "all")
            {
                foreach (string s in SearchArray)
                {
                    entityGrps = _GroupStorageAccess.GetGroupsForNameMatch(s.Replace("Name#", ""));
                    if (entityGrps != null && entityGrps.Count > 0)
                    {
                        foreach (entities.Group grp in entityGrps)
                        {
                            returnV.List.Add(Caster.MakeContractGroupLite(grp));
                        }
                    }
                }
            }
            else
            {
                entityGrps = _GroupStorageAccess.GetAllGroups();
                if (entityGrps != null && entityGrps.Count > 0)
                {
                    foreach (entities.Group grp in entityGrps)
                    {
                        returnV.List.Add(Caster.MakeContractGroupLite(grp));
                    }
                }
            }
            return returnV;
        }

        public async Task<List<Group>> GetListOfVolunteerGroups()
        {
            GroupList grpList = await GetListOfGroups("all");
            return grpList.List.FindAll(x => x.Type == GroupType.Social);
        }

        //no change
        /// <summary>
        ///     Gets Amdin profile if the provide Live Unique user ID matches
        /// </summary>
        /// <param name="LUID">Live Unique User ID obtained by decrypting the JWT</param>
        /// <returns></returns>
        public Admin GetAdminProfile(string LUID)
        {
            var oResult = new Admin();

            if (string.IsNullOrEmpty(LUID))
            {
                utility.ResultsManager.AddResultInfo(oResult, ResultTypeEnum.Error, "The LiveMail provided was empty.");
                return oResult;
            }

            if (_GroupStorageAccess == null)
                _GroupStorageAccess = new GroupStorageAccess();

            entities.AdminUser oAdmin = _GroupStorageAccess.GetAdminUser(LUID);

            if (oAdmin == null)
            {
                utility.ResultsManager.AddResultInfo(oResult, ResultTypeEnum.Error,
                    "The LiveMail did not yeild an admin.");
                return oResult;
            }
            if (oAdmin.GroupIDCSV.Trim().Trim(',') == string.Empty)
            {
                oResult = Caster.MakeContractAdmin(oAdmin);
                utility.ResultsManager.AddResultInfo(oResult, ResultTypeEnum.Error,
                    "The admin is not associated to any groups.");
                return oResult;
            }
            oResult = Caster.MakeContractAdmin(oAdmin);
            string[] gArr = oResult.GroupIDCSV.Split(',');
            entities.Group grp = null;
            int grpID = 0;
            Group cGrop = null;
            oResult.Groups = new List<Group>();
            foreach (string s in gArr)
            {
                if (!string.IsNullOrEmpty(s) && Int32.TryParse(s, out grpID) && grpID > 0)
                {
                    grp = _GroupStorageAccess.GetGroupByID(grpID);

                    if (grp != null && grp.GroupID > 0)
                    {
                        cGrop = Caster.MakeContractGroupLite(grp);
                        if (cGrop != null && !string.IsNullOrEmpty(cGrop.GroupName) &&
                            !string.IsNullOrEmpty(cGrop.GroupID))
                        {
                            oResult.Groups.Add(cGrop);
                        }
                        else
                        {
                            utility.ResultsManager.AddResultInfo(oResult, ResultTypeEnum.Information,
                                "The Group ID: " + s + " failed parsing");
                        }
                    }
                    else
                    {
                        utility.ResultsManager.AddResultInfo(oResult, ResultTypeEnum.Information,
                            "The Group ID: " + s + " is not available");
                    }
                }
            }

            return oResult;
        }

        //ssm  check the return type

        public async Task<ResultInfo> ValidateGroupMember(string ValidationID, string profileID)
        {
            long ProfileID = Convert.ToInt64(profileID);
            var result = new ResultInfo();
            try
            {
                if (!string.IsNullOrEmpty(ValidationID) && ProfileID != 0)
                {
                    entities.GroupMemberValidator GMV = _GroupStorageAccess.GetValidateGroupMemberRec(ValidationID,
                        profileID);

                    if (GMV != null)
                    {
                        await _GroupRepository.UpdateGroupMembership(GMV.GroupID, ProfileID);
                        result.ResultType = ResultTypeEnum.Success;
                        result.Message = "Verified";

                        _GroupStorageAccess.DeleteGroupMemberValidatorRec(GMV);
                    }
                    else
                    {
                        result.ResultType = ResultTypeEnum.Error;
                        result.Message = "Invalid VaidationID";
                    }
                }
                else
                {
                    result.ResultType = ResultTypeEnum.Error;
                    result.Message = "Invalid Input";
                }
            }
            catch (Exception e)
            {
                result.ResultType = ResultTypeEnum.Exception;
                result.Message = "Failed-" + e.Message;
            }
            return result;
        }

        //no change
        public async Task<ResultInfo> ValidateGroupMarshal(string ValidationID, string ProfileID)
        {
            var result = new ResultInfo();
            try
            {
                long profileid = Convert.ToInt64(ProfileID);
                if (!string.IsNullOrEmpty(ValidationID) && profileid != 0)
                {
                    entities.GroupMarshalValidator GMV = _GroupStorageAccess.GetGroupMarshalValidator(ValidationID,
                        ProfileID);
                    int groupID = Convert.ToInt32(GMV.GroupID);
                    long profileID = Convert.ToInt64(GMV.ProfileID);
                    if (GMV != null && groupID != 0 && profileID != 0)
                    {
                        bool isMarshalSaved = await _GroupRepository.SaveMarshal(groupID, profileID, true);
                        if (isMarshalSaved)
                        {
                            result.ResultType = ResultTypeEnum.Success;
                            result.Message = "Verified";
                            _GroupStorageAccess.DeleteGroupMarshalValidator(GMV);
                        }
                        else
                        {
                            result.ResultType = ResultTypeEnum.Error;
                            result.Message = "Failed";
                        }
                    }
                    else
                    {
                        result.ResultType = ResultTypeEnum.Error;
                        result.Message = "Invalid VaidationID";
                    }
                }
                else
                {
                    result.ResultType = ResultTypeEnum.Error;
                    result.Message = "Invalid Input";
                }
            }
            catch (Exception e)
            {
                result.ResultType = ResultTypeEnum.Exception;
                result.Message = "Failed-" + e.Message;
            }
            return result;
        }

        //set isvalidated as false first and then after validation set it to false
        public async Task<ResultInfo> SaveMarshalInfo(MarshallToAdd MarshallObject)
        {
            string LiveMail, PhoneNumber;
            int GroupID = Convert.ToInt32(MarshallObject.GroupID);
            var result = new ResultInfo();
            if (GroupID == 0)
            {
                result.ResultType = ResultTypeEnum.Error;
                result.Message = "Invalid Group ID";
            }
            if (MarshallObject != null)
            {
                LiveMail = MarshallObject.LiveMail;
                PhoneNumber = MarshallObject.PhoneNumber;
            }
            else
            {
                return new ResultInfo()
                {
                    ResultType = ResultTypeEnum.Error,
                    Message = "Invalid Parameters, Empty Object."
                };
            }

            try
            {
                string encryptedMobileNumber = utility.Security.Encrypt(PhoneNumber);
                MarshalStatusInfo resMarsahlInfo =
                    await _GroupRepository.ValidateAndSaveMarshal(GroupID, LiveMail, encryptedMobileNumber, false);
                if (resMarsahlInfo.Code == 1)
                {
                    string ValidationID = utility.TokenManager.GenerateNewValidationID();
                    entities.GroupMarshalValidator GMV = Caster.MakeGroupMarshalValidator(resMarsahlInfo.ProfileID,
                        GroupID, LiveMail, ValidationID);
                    if (utility.Email.SendGroupValidationMail(LiveMail, ValidationID,
                        resMarsahlInfo.ProfileID.ToString(), "GroupMarshal"))
                    {
                        GMV.NotificationSent = true;
                    }
                    _GroupStorageAccess.SaveGroupMarshalValidator(GMV);
                    result.ResultType = ResultTypeEnum.Success;
                    result.Message = resMarsahlInfo.MessageInfo;
                }
                else
                {
                    result.ResultType = ResultTypeEnum.Error;
                    result.Message = resMarsahlInfo.MessageInfo;
                }
            }
            catch
            {
                result.ResultType = ResultTypeEnum.Exception;
                result.Message = "Failed";
            }

            return result;
        }

        //List of all Marshals associated to Group

        //gm,profile and user
        public async Task<MarshalList> GetMarshalList(string groupID)
        {
            int GroupID = 0;
            var marshals = new MarshalList { List = new List<Marshal>() };
            try
            {
                if (int.TryParse(groupID, out GroupID))
                {
                    if (GroupID == 0)
                        return null;

                    List<model.Profile> marshalProfiles = await _GroupRepository.GetLiveMarshalsByGroupID(GroupID);
                    foreach (model.Profile marshalProfile in marshalProfiles)
                    {
                        ProfileLite marshalProfileLite = Caster.MakeProfileLiteOnCombination(marshalProfile,
                            marshalProfile.User);
                        if (marshalProfile.LiveLocations != null && marshalProfile.LiveLocations.ToList().Count > 0)
                        {
                            List<GeoTag> sGeo = marshalProfile.LiveLocations.ToList().ConvertToGeoTagList();

                            marshalProfileLite.IsSOSOn = sGeo[0].IsSOS.HasValue ? sGeo[0].IsSOS.Value : false; //1
                            marshalProfileLite.IsTrackingOn = !(sGeo[0].IsSOS.HasValue ? sGeo[0].IsSOS.Value : false) ||
                                                              !string.IsNullOrEmpty(sGeo[0].Lat);

                            marshalProfileLite.LastLocs = sGeo;
                        }

                        List<ProfileLite> locateBuddyProfiles =
                            await _LocationService.GetLocateBuddies(marshalProfileLite.UserID);

                        if (marshalProfileLite != null)
                        {
                            marshals.List.Add(Caster.MakeContractMarshal(marshalProfileLite, locateBuddyProfiles, true));
                        }
                        else
                        {
                            utility.ResultsManager.AddResultInfo(marshals, ResultTypeEnum.Error, "Invalid IDs");
                        }
                    }
                    utility.ResultsManager.AddResultInfo(marshals, ResultTypeEnum.Success, "Success");
                }
                else
                    utility.ResultsManager.AddResultInfo(marshals, ResultTypeEnum.Error, "Invalid Group ID");
            }
            catch (Exception e)
            {
                utility.ResultsManager.AddResultInfo(marshals, ResultTypeEnum.Exception, "Failed-" + e.Message);
            }
            return marshals;
        }

        public async Task<ResultInfo> AssignBuddyToMarshal(string AdminID, string groupID, string marshalProfileID,
            string marshalUserID, string targetUserProfileID)
        {
            int GroupID = Convert.ToInt32(groupID);
            long MarshalProfileID = Convert.ToInt64(marshalProfileID);
            long MarshalUserID = Convert.ToInt64(marshalUserID);
            long TargetUserProfileID = Convert.ToInt64(targetUserProfileID);

            var result = new ResultInfo();
            try
            {
                if (string.IsNullOrEmpty(AdminID) || GroupID == 0 || MarshalProfileID == 0 || MarshalUserID == 0 ||
                    TargetUserProfileID == 0)
                {
                    result.ResultType = ResultTypeEnum.Error;
                    result.Message = "Marshal Not assigned Buddy List.Invalid IDs !";
                }
                else if (MarshalValidationsForBuddy(AdminID, GroupID, MarshalProfileID, TargetUserProfileID, true))
                {
                    //right people fall under right places. 
                    model.User marshalUser = await _MemberRepository.GetUserByUserIDAsync(MarshalUserID);
                    if (marshalUser != null)
                    {
                        var buddy = new model.Buddy
                        {
                            ProfileID = TargetUserProfileID,
                            UserID = marshalUser.UserID,
                            BuddyName = marshalUser.Name,
                            MobileNumber = marshalUser.MobileNumber,
                            Email = marshalUser.Email,
                            State = model.BuddyState.Marshal,
                            SubscribtionId = Guid.NewGuid()
                        };

                        bool isBuddyAdded = await _MemberRepository.AddBuddyAsync(buddy);
                        if (isBuddyAdded)
                        {
                            result.ResultType = ResultTypeEnum.Success;
                            result.Message = "Marshal added as a Buddy !";
                        }
                    }
                    else
                    {
                        result.ResultType = ResultTypeEnum.Error;
                        result.Message = "Marshal Not Added as a Buddy.Invalid MarshalUserIDs !";
                    }
                }
                else
                {
                    result.ResultType = ResultTypeEnum.Error;
                    result.Message = "Marshal Not Added as a Buddy.Invalid IDs !";
                }
            }
            catch (Exception e)
            {
                result.ResultType = ResultTypeEnum.Exception;
                result.Message = "Failed-" + e.Message;
            }
            return result;
        }

        public async Task<ResultInfo> RemoveBuddyFromMarshal(string AdminID, string groupID, string marshalProfileID,
            string marshalUserID, string targetUserProfileID)
        {
            int GroupID = Convert.ToInt32(groupID);
            long MarshalProfileID = Convert.ToInt64(marshalProfileID);
            long MarshalUserID = Convert.ToInt64(marshalUserID);
            long TargetUserProfileID = Convert.ToInt64(targetUserProfileID);

            var result = new ResultInfo();

            try
            {
                if (string.IsNullOrEmpty(AdminID) || GroupID == 0 || MarshalProfileID == 0 || MarshalUserID == 0 ||
                    TargetUserProfileID == 0)
                {
                    result.ResultType = ResultTypeEnum.Error;
                    result.Message = "Marshal Not Removed from the Buddy List.Invalid IDs !";
                }
                else if (MarshalValidationsForBuddy(AdminID, GroupID, MarshalProfileID, TargetUserProfileID, true))
                {
                    //right people fall under right places. 
                    await _MemberRepository.RemoveBuddyRelationAsync(TargetUserProfileID, MarshalUserID);
                    result.ResultType = ResultTypeEnum.Information;
                    result.Message = "Marshal Removed from the Buddy List !";
                }
                else
                {
                    result.ResultType = ResultTypeEnum.Error;
                    result.Message = "Marshal Not Removed from the Buddy List.Invalid IDs !";
                }
            }
            catch (Exception e)
            {
                result.ResultType = ResultTypeEnum.Exception;
                result.Message = "Failed-" + e.Message;
            }
            return result;
        }

        public async Task<ResultInfo> DeleteMarshal(string AdminID, string groupID, string marshalProfileID)
        {
            int GroupID = Convert.ToInt32(groupID);
            long MarshalProfileID = Convert.ToInt64(marshalProfileID);

            var result = new ResultInfo();
            try
            {
                if (string.IsNullOrEmpty(AdminID) || GroupID == 0 || MarshalProfileID == 0)
                {
                    result.ResultType = ResultTypeEnum.Error;
                    result.Message = "Marshal Not Deleted.Invalid IDs !";
                }
                else if (MarshalValidationsForBuddy(AdminID, GroupID, MarshalProfileID, 0, false))
                {
                    ProfileLiteList locateBuddies =
                        await _LocationService.GetBuddiesToLocateByProfileID(marshalProfileID);

                    if (locateBuddies == null || locateBuddies.List.Count == 0)
                    {
                        bool isMarshalDeleted = await _GroupRepository.DeleteMarshal(GroupID, MarshalProfileID);
                        if (isMarshalDeleted)
                        {
                            result.ResultType = ResultTypeEnum.Success;
                            result.Message = "Marshal Deleted";
                        }
                    }
                    else
                    {
                        result.ResultType = ResultTypeEnum.Error;
                        result.Message = "Marshal can not be deleted As People have added him as a buddy";
                    }
                }
                else
                {
                    result.ResultType = ResultTypeEnum.Error;
                    result.Message = "Failed";
                }
            }
            catch (Exception e)
            {
                result.ResultType = ResultTypeEnum.Exception;
                result.Message = "Failed-" + e.Message;
            }
            return result;
        }

        internal async Task ExecuteGroupEnrollWF(Profile ProfileInst, Group Grp)
        {
            switch (Grp.EnrollmentType)
            {
                case Enrollment.None:
                    await EnrollToPublicGroup(ProfileInst, Grp);
                    break;
                case Enrollment.AutoOrgMail:
                    await EnrollOnAutoOrgMail(ProfileInst, Grp);
                    break;
                case Enrollment.Moderator:
                    EnrollByModeration(ref ProfileInst, Grp);
                    break;
            }
        }

        //on enrolling to a group a mail is sent to the entered email id
        private async Task EnrollOnAutoOrgMail(Profile ProfileInst, Group Grp)
        {
            long ProfileID = ProfileInst.ProfileID;

            int iGroupID = Convert.ToInt32(Grp.GroupID);
            bool isValidGroupMember = await _GroupRepository.ValidateGroupMembership(iGroupID, ProfileID);
            if (!isValidGroupMember)
            {
                bool isValid = (Grp.EnrollmentValue != null && !Grp.EnrollmentValue.Contains("@") &&
                                Grp.EnrollmentKey == Grp.EnrollmentValue);

                model.GroupMembership eGrp = Caster.MakeGroupMemberEntity(ProfileID, ProfileInst.Name, Grp, isValid);

                if (eGrp != null)
                {
                    // Insert Group Membership Invalid record. This prevents users from re-applying even after re-install.
                    if(isValid)
                        await _GroupRepository.CreateGroupMembership(eGrp);
                    else if (eGrp.EnrollmentKeyValue.Contains("@"))
                    {
                        if (eGrp.EnrollmentKeyValue.EndsWith(Grp.EnrollmentKey))
                        {
                            string ValidationGUID = utility.TokenManager.GenerateNewValidationID();
                            // Prep Group Member Validator (GMV)
                            entities.GroupMemberValidator GMV = Caster.MakeGroupMemberValidator(ProfileID, Grp, ValidationGUID);

                            // Save GMV with NotificationSent = False
                            GMV.NotificationSent = false;
                            // Send Mail
                            if (utility.Email.SendGroupValidationMail(GMV.NotificationIdentity, GMV.ValidationID, GMV.ProfileID, "GroupMember"))
                            {
                                GMV.NotificationSent = true;
                            }
                            _GroupStorageAccess.SaveUpdateGroupMemberValidator(GMV);
                            await _GroupRepository.CreateGroupMembership(eGrp);
                        }
                        else
                        {
                            utility.ResultsManager.AddResultInfo(ProfileInst, ResultTypeEnum.Information, "Enrollment Value is invalid for Group " + Grp.GroupName);
                        }
                    }
                    // Save GMV with NotificaiotnSent = True
                }
            }
        }


        //change - enrolling to a group initiation
        private async Task EnrollToPublicGroup(Profile ProfileInst, Group Grp)
        {
            int GroupID = Convert.ToInt32(Grp.GroupID);
            await _GroupRepository.CreatePublicGroupMembership(GroupID, ProfileInst.ProfileID, ProfileInst.Name);
        }

        public void DeleteGroup(int GrpID)
        {
            _GroupStorageAccess.DeleteGroup(GrpID);
        }

        private static void IsCacheResetRequired()
        {
            bool isCacheResetRequired = false;

            if (LastUpdateDate == DateTime.MinValue)
                isCacheResetRequired = true;
            else
            {
                if (DateTime.UtcNow.Subtract(LastUpdateDate).TotalMinutes >= Config.TimeToResetCacheInMinutes)
                {
                    isCacheResetRequired = true;
                }
            }
            if (isCacheResetRequired)
            {
                _AllGroups = null;
                LastUpdateDate = DateTime.UtcNow;
            }
        }

        public List<entities.Group> GroupUsers(bool? RetriveOnlyChilds)
        {
            List<entities.Group> groupListwithGroupNames = _GroupStorageAccess.GetAllGroups(RetriveOnlyChilds);
            return groupListwithGroupNames;
        }

        public async Task<List<GroupMemberLiveSession>> GetFilteredParentGroupLiveMemberSession()
        {
            List<GroupMemberLiveSession> sessions = await _GroupRepository.GetAllGroupMembershipLite();

            List<entities.Group> parentGrp = ParentGroup;
            IEnumerable<GroupMemberLiveSession> result =
                sessions.ToList()
                    .Where(
                        x =>
                            parentGrp.Exists(
                                grp =>
                                    grp.GroupID.Equals(x.GrpId) && !String.IsNullOrWhiteSpace(grp.ShapeFileID) &&
                                    grp.NotifySubgroups));

            return result.ToList();
        }

        //confirm
        private void EnrollByModeration(ref Profile ProfileInst, Group Grp)
        {
            //validate if all Moderator enrollment has some value.
            List<Group> grps =
                ProfileInst.AscGroups.FindAll(
                    x =>
                        !x.ToRemove && x.EnrollmentType == Enrollment.Moderator &&
                        x.EnrollmentValue.Trim() == string.Empty);
            if (grps != null && grps.Count > 0)
            {
                utility.ResultsManager.AddResultInfo(ProfileInst, ResultTypeEnum.Error,
                    "Invalid Group Enrollment values");
                return;
            }
            throw new NotImplementedException();
        }


        internal async Task DisEnroll(Group Grp, long ProfileID)
        {
            int groupID = Convert.ToInt32(Grp.GroupID);
            long profileID = ProfileID;

            if (groupID == 0 || profileID == 0)
                return;
            await _GroupRepository.DeleteGroupMembership(groupID, profileID);
        }

        internal async Task<List<Group>> GetGroupsForProfileID(long ProfileID)
        {
            if (ProfileID == 0)
                return null;

            var retGroups = new List<Group>();
            List<model.GroupMembership> eGrops = await _GroupRepository.GetGroupMembershipForProfile(ProfileID);
            if (eGrops != null && eGrops.Count > 0)
            {
                Group tmpGrp = null;

                entities.Group tmpEGrp = null;

                foreach (model.GroupMembership egrpmem in eGrops)
                {
                    tmpEGrp = _GroupStorageAccess.GetGroupByID(egrpmem.GroupID);
                    if (tmpEGrp != null)
                    {
                        tmpGrp = Caster.MakeContractGroupLite(tmpEGrp);
                        tmpGrp.IsValidated = egrpmem.IsValidated;
                        retGroups.Add(tmpGrp);
                    }
                }
            }
            return retGroups;
        }

        public ResultInfo JoinGroupToValidate(long ProfileID, int GroupID, string EmailIdentity)
        {
            //read Grp info from Group table by group id
            //Check type of group
            //if(group is public group) Isvalidated=true
            //if(group is private) then if modeOfValidation is email, send email else, leave it.
            //Write another method to allow admin to manually make IsValidated = true for a Group Member -- Future
            var result = new ResultInfo();

            try
            {
                if (ProfileID != 0 && GroupID != 0 && !String.IsNullOrEmpty(EmailIdentity))
                {
                    if (_GroupStorageAccess.AddGroupMemberToValidate(ProfileID.ToString(), GroupID.ToString(),
                        EmailIdentity))
                    {
                        result.ResultType = ResultTypeEnum.Success;
                        result.Message = "Added to Group";
                    }
                    else
                    {
                        result.ResultType = ResultTypeEnum.Error;
                        result.Message = "Failed";
                    }
                }
                else
                {
                    result.ResultType = ResultTypeEnum.Error;
                    result.Message = "Invalid IDs";
                }
            }
            catch
            {
                result.ResultType = ResultTypeEnum.Exception;
                result.Message = "Failed to Add to Group";
            }
            return result;
        }


        public bool MarshalValidationsForBuddy(string AdminID, int GroupID, long MarshalProfileID,
            long TargetUserProfileID, bool isProfileValidationForGroupRequired)
        {
            bool isAdminGroupValid = false;
            bool isMarshalGroupValidated = false;
            bool isProfileGroupValid = true;

            var tasks = new List<Task>();
            Task adminGroupValidation =
                Task.Run(
                    () =>
                    {
                        isAdminGroupValid = _GroupStorageAccess.ValidateAdminForGroup(GroupID.ToString(), AdminID);
                    });
            Task<bool> marshalGroupValidation = _GroupRepository.ValidateMarshalForGroup(GroupID, MarshalProfileID);

            tasks.Add(adminGroupValidation);
            tasks.Add(marshalGroupValidation);

            Task.WaitAll(tasks.ToArray());
            isMarshalGroupValidated = marshalGroupValidation.Result;

            if (isProfileValidationForGroupRequired)
                isProfileGroupValid = _GroupRepository.ValidateGroupMembership(GroupID, TargetUserProfileID).Result;

            if (isProfileGroupValid && isAdminGroupValid && isMarshalGroupValidated)
            {
                return true;
            }
            return false;
        }

        public void EditGroup(Group grp, string partitionKey = null, string rowkey = null, bool isCreate = false)
        {
            LastUpdateDate = DateTime.UtcNow;

            var entityGrp = new entities.Group();

            entityGrp.GroupID = Convert.ToInt16(grp.GroupID);
            entityGrp.GroupName = grp.GroupName;
            entityGrp.EnrollmentKey = grp.EnrollmentKey;
            entityGrp.Email = grp.Email;
            entityGrp.PhoneNumber = grp.PhoneNumber;
            entityGrp.Location = grp.GroupLocation;
            entityGrp.GroupType = Convert.ToInt32(grp.Type);
            entityGrp.IsActive = grp.IsActive;

            entityGrp.EnrollmentType = Convert.ToInt32(grp.EnrollmentType);
            entityGrp.RowKey = grp.GroupID;
            entityGrp.PartitionKey = grp.GroupLocation;
            entityGrp.ParentGroupID = grp.ParentGroupID;
            entityGrp.NotifySubgroups = grp.NotifySubgroups;
            entityGrp.SubGroupIdentificationKey = grp.SubGroupIdentificationKey;
            entityGrp.ShapeFileID = grp.ShapeFileID;
            entityGrp.GeoLocation = grp.GeoLocation;
            entityGrp.ShowIncidents = grp.ShowIncidents;

            var _GroupStorageAccess = new GroupStorageAccess();
            _GroupStorageAccess.UpdateGroup(entityGrp);

            var entityAdmin = new entities.AdminUser();

            entityAdmin.AdminID = Convert.ToInt16(grp.GroupID);
            entityAdmin.Email = grp.Email;
            entityAdmin.GroupIDCSV = grp.GroupID;
            entityAdmin.Name = grp.GroupName;
            entityAdmin.MobileNumber = grp.PhoneNumber;
            entityAdmin.LiveUserID = grp.LiveInfo.LiveID;
            entityAdmin.AllowGroupManagement = grp.AllowGroupManagement;
            entityAdmin.PartitionKey = isCreate ? grp.GroupLocation : partitionKey;
            entityAdmin.RowKey = isCreate ? Guid.NewGuid().ToString() : rowkey;

            var _MemberStorageAccess = new MemberStorageAccess();
            _MemberStorageAccess.SaveGroupAdmin(entityAdmin);
        }

        public List<GroupDTO> GetAllGroupWithAdmins()
        {
            _GroupStorageAccess = new GroupStorageAccess();
            List<entities.Group> entityGrps = null;
            entityGrps = _GroupStorageAccess.GetAllGroups(null);
            List<entities.AdminUser> grpAdmin = _GroupStorageAccess.GetAllGroupAdmins();

            var result = new List<GroupDTO>();

            if (entityGrps != null && entityGrps.Count > 0 && grpAdmin != null && grpAdmin.Count > 0)
            {
                result = (from grp in entityGrps
                          join admin in grpAdmin on grp.GroupID equals admin.AdminID into groupadmins
                          from grpadmins in groupadmins.DefaultIfEmpty()
                          select new GroupDTO
                          {
                              GroupID = grp.GroupID,
                              GroupName = grp.GroupName,
                              EnrollmentKey = grp.EnrollmentKey,
                              EnrollmentType = (Enrollment)grp.EnrollmentType,
                              Email = grp.Email,
                              PhoneNumber = grp.PhoneNumber,
                              Location = grp.Location,
                              GroupType = (GroupType)grp.GroupType,
                              IsActive = grp.IsActive,
                              GroupKey = grp.SubGroupIdentificationKey,
                              GeoLocation = grp.GeoLocation,
                              NotifySubgroups = grp.NotifySubgroups,
                              ParentGroupID = grp.ParentGroupID,
                              ParentGroupName = GetParentgroupName(entityGrps, grp),
                              ShapeFileID = grp.ShapeFileID,
                              ShowIncidents = grp.ShowIncidents,
                              LiveID = grpadmins != null ? grpadmins.LiveUserID : String.Empty,
                              RowKey = grpadmins != null ? grpadmins.RowKey : string.Empty,
                              PartitionKey = grpadmins != null ? grpadmins.PartitionKey : string.Empty,
                              AllowGroupManagement = grpadmins != null ? grpadmins.AllowGroupManagement : false
                          }).ToList();
            }
            return result;
        }

        private string GetParentgroupName(List<entities.Group> entityGrps, entities.Group grp)
        {
            entities.Group entity =
                entityGrps.FirstOrDefault(x => grp.ParentGroupID.HasValue && x.GroupID == grp.ParentGroupID);
            if (entity != null)
            {
                return entity.GroupName;
            }
            return string.Empty;
        }
    }
}