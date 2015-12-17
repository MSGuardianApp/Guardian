using SOS.Service.Interfaces.DataContracts;
using SOS.Service.Interfaces.DataContracts.OutBound;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace SOS.Service.Interfaces
{
    [ServiceContract(Namespace = "http://www.microsoft.com/sos/2013/02", Name = "LocationService")]
    public interface ILocationService
    {
        //[SelfAccess,LocateBuddyAccess]
        //[OperationContract]
        //[WebGet(UriTemplate = "/GetUserLocation/{ProfileID}/{LastUpdateTime}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<ProfileLite> GetUserLocation(string ProfileID, string LastUpdateTime);

        [OperationContract]
        [WebGet(UriTemplate = "/GetUserLocationArray/{ProfileID}/{LastUpdateTime}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<GeoTags> GetUserLocationArray(string ProfileID, string LastUpdateTime);

        //[SelfAccess,LocateBuddyAccess]
        //[OwnGroupMembersAccess]
        [OperationContract]
        [WebGet(UriTemplate = "/GetUserLocations/{ProfileID}/{LastUpdateTime}/{*GroupID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<GeoTagList> GetUserLocations(string ProfileID, string LastUpdateTime, string GroupID = "0");


        [OperationContract]
        [WebGet(UriTemplate = "/GetUserLocationsByToken/{ProfileID}/{SessionID}/{LastUpdateTime}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<BasicProfile> GetUserLocationsByToken(string ProfileID, string SessionID, string LastUpdateTime);

        /// <summary>
        /// GetBuddiesToLocate
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        //[ValidUserAccess]
        [OperationContract]
        [WebGet(UriTemplate = "/GetBuddiesToLocate/{UserID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<ProfileLiteList> GetBuddiesToLocate(string UserID);

        //[ValidUserAccess]
        [OperationContract]
        [WebGet(UriTemplate = "/GetBuddiesToLocateLastLocation/{UserID}/{DummyTicks}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<ProfileLiteList> GetBuddiesToLocateLastLocation(string UserID, string DummyTicks);

        [OperationContract]
        [WebGet(UriTemplate = "/GetSOSTrackCount/{UserID}/{DummyTicks}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<Dictionary<string, string>> GetSOSTrackCount(string UserID, string DummyTicks);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<SOSTrackCounts> GetSOSTrackCountByGroupId(string GroupId);

        Task<ProfileLite> GetLocationDetails(string ProfileID, string GroupID, string LastUpdateDate);

        Task<LiveUserStatusList> GetLiveMembersByGroupID(string groupID);

        //[ValidUserAccess]
        Task<LiveUserStatusList> GetLiveLocateBuddiesByUserId(string UserID);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<int> GetUserStatus();
    }
}

