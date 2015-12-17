using SOS.Service.Interfaces.DataContracts;
using SOS.Service.Interfaces.DataContracts.InBound;
using SOS.Service.Interfaces.DataContracts.OutBound;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace SOS.Service.Interfaces
{

    [ServiceContract(Namespace = "http://www.microsoft.com/sos/2013/02", Name = "MembershipService")]
    public interface IMembership
    {
        //[SelfAccess]
        [OperationContract]
        [WebGet(UriTemplate = "/GetProfileByProfileID/{ProfileID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<Profile> GetProfileByProfileID(string ProfileID);

        //[SelfAccess,LocateBuddyAccess]
        //[OwnGroupMembersAccess]
        [OperationContract]
        [WebGet(UriTemplate = "/GetProfileLiteByProfileID/{ProfileID}/{*GroupID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<ProfileLite> GetProfileLiteByProfileID(string ProfileID, string GroupID = "0");

        [OperationContract]
        [WebGet(UriTemplate = "/GetProfilesForLiveID", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<ProfileList> GetProfilesForLiveID();

        [OperationContract]
        [WebGet(UriTemplate = "/UnBuddy/{ProfileID}/{BuddyUserID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task UnBuddy(string ProfileID, string BuddyUserID);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST", BodyStyle = WebMessageBodyStyle.Bare)]
        PhoneValidation CreatePhoneValidator(PhoneValidation PhoneValidationIP);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST", BodyStyle = WebMessageBodyStyle.Bare)]
        Task<Profile> CreateProfile(Profile BareProfile);

        //[SelfAccess]
        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST", BodyStyle = WebMessageBodyStyle.Bare)]
        Task<Profile> UpdateProfilePhone(Profile Profile2Update);

        //[SelfAccess]
        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST", BodyStyle = WebMessageBodyStyle.Bare)]
        Task<Profile> UpdateProfile(Profile Profile2Update);

        //[SelfAccess]
        [OperationContract]
        [WebGet(UriTemplate = "/CheckPendingUpdates/{ProfileID}/{LastUpdateDate}/{CurrentTime}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<HealthUpdate> CheckPendingUpdates(string ProfileID, string LastUpdateDate, string CurrentTime);


        [OperationContract]
        [WebGet(UriTemplate = "/UnRegisterUser", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<ResultInfo> UnRegisterUser();

        //[OperationContract]
        //[WebGet(UriTemplate = "/SubscribeBuddyAction/{ProfileID}/{UserID}/{ActionType}/{SubscribtionID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<User> SubscribeBuddyAction(string ProfileID, string UserID, string ActionType, string SubscribtionID);

        //[OperationContract]
        //[WebGet(UriTemplate = "/UnAssignMarshalFromList/{TargetUserProfileID}/{BuddyUserID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<ResultInfo> UnAssignMarshalFromList(string TargetUserProfileID, string BuddyUserID);

        Task<string> GetMembersCountForGroup(string GroupID);

        Task<string> GetLocateBuddiesCountForUser(string UserID);

        Task<LiveUserStatusList> GetFilteredGroupMembers(string GroupID, string searchName);

        //[ValidUserAccess]
        Task<LiveUserStatusList> GetFilteredLocateBuddies(string UserID, string searchName);

        Task<List<Member>> GetMembersByGroupID(string groupID, string searchName,string startDate,string endDate);

        //[OwnGroupMembersAccess]
        Task<ResultInfo> SaveDispatchInfo(DispatchInfo userAssignedTo);

        Task<BasicProfile> GetBasicProfile(string profileID, string sessionID);
    }
}

