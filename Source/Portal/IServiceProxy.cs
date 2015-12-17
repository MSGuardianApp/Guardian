using SOS.Service.Interfaces.DataContracts;
using SOS.Service.Interfaces.DataContracts.InBound;
using SOS.Service.Interfaces.DataContracts.OutBound;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace SOS.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServiceProxy" in both code and config file together.
    [ServiceContract]
    public interface IServiceProxy
    {
        [OperationContract]
        [WebGet(UriTemplate = "/Ping", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string Ping();

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/GetHistorySessions/{ProfileID}/{StartDate}/{EndDate}/{Ticks}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        HistoryList GetHistorySessions(string ProfileID, string StartDate, string EndDate, string Ticks);

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/GetHistoryLocations/{ProfileID}/{sessionID}/{Ticks}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        GeoTagList GetHistoryLocationsBySessionID(string profileID, string sessionID, string Ticks);

        //[ValidAuthentication]
        //[OwnGroupMembersAccess]
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, Method = "POST")]
        ResultInfo SaveMarshalInfo(MarshallToAdd MarshallObject);

        //[ValidAuthentication]
        //[OwnGroupMembersAccess]
        [OperationContract]
        [WebGet(UriTemplate = "/GetMarshalList/{GroupID}/{Token}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        MarshalList GetMarshalList(string GroupID, string Token);

        //[ValidAuthentication]
        //[OwnGroupMembersAccess]
        [OperationContract]
        [WebGet(UriTemplate = "/AssignBuddyToMarshal/{AdminID}/{GroupID}/{MarshalProfileID}/{MarshalUserID}/{TargetUserProfileID}/{Token}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResultInfo AssignBuddyToMarshal(string AdminID, string GroupID, string MarshalProfileID, string MarshalUserID, string TargetUserProfileID, string Token);

        //[ValidAuthentication]
        //[OwnGroupMembersAccess]
        [OperationContract]
        [WebGet(UriTemplate = "/RemoveBuddyFromMarshal/{AdminID}/{GroupID}/{MarshalProfileID}/{MarshalUserID}/{TargetUserProfileID}/{Token}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResultInfo RemoveBuddyFromMarshal(string AdminID, string GroupID, string MarshalProfileID, string MarshalUserID, string TargetUserProfileID, string Token);

        //[ValidAuthentication]
        //[OwnGroupMembersAccess]
        [OperationContract]
        [WebGet(UriTemplate = "/DeleteMarshal/{AdminID}/{GroupID}/{MarshalProfileID}/{Token}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResultInfo DeleteMarshal(string AdminID, string GroupID, string MarshalProfileID, string Token);

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/GetMiniProfile", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string GetMiniProfileForLiveID();

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/GetAdminProfile/{LiveMail}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Admin GetAdminProfile(string LiveMail);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "SwitchOnSOSviaSMS/{encryptedParms}/{utcTicks}/{ticks}/{lat}/{longi}", BodyStyle = WebMessageBodyStyle.Bare)]
        string SwitchOnSOSviaSMS(string encryptedParms, string utcTicks, string ticks, string lat, string longi);

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/GetUserLocationsByToken/{ProfileID}/{Token}/{LastUpdateTime}/{Ticks}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        BasicProfile GetUserLocationsByToken(string ProfileID, string Token, string LastUpdateTime, string Ticks);

        //[ValidAuthentication]
        //[OwnGroupMembersAccess]
        [OperationContract]
        [WebGet(UriTemplate = "/GetProfileLiteByProfileID/{ProfileID}/{GroupID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ProfileLite GetProfileLiteByProfileID(string ProfileID, string GroupID);

        [OperationContract]
        [WebGet(UriTemplate = "/GetBasicProfile/{ProfileID}/{SessionID}/{Ticks}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        BasicProfile GetBasicProfile(string ProfileID, string SessionID, string Ticks);

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/GetIncidents/{IdentificationToken}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IncidentList GetIncidents(string IdentificationToken);

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/UserCount/{AuthToken}/{UserType}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        int UserCount(string AuthToken, string UserType);

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/GroupUsers/{AuthToken}/{UserType}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        object GroupUsers(string AuthToken, string UserType);

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/MissedActivationCount/{AuthToken}/{UserType}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        int MissedActivationCount(string AuthToken, string UserType);

        //[ValidAuthentication]
        //[OperationContract]
        //[WebGet(UriTemplate = "/HistorySOSData/{AuthToken}/{UserType}/{startDate}/{endDate}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        object HistorySOSData(string AuthToken, string UserType, string startDate, string endDate);

        //[ValidAuthentication]
        //[OperationContract]
        //[WebGet(UriTemplate = "/HistorySOSAndTrackData/{AuthToken}/{UserType}/{startDate}/{endDate}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        object HistorySOSAndTrackData(string AuthToken, string UserType, string startDate, string endDate);

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/ActiveSOSData/{AuthToken}/{UserType}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<ActiveSOSReports> ActiveSOSData(string AuthToken, string UserType);

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/UserReport/{AuthToken}/{UserType}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DemographyReport> UserReport(string AuthToken, string UserType);

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/StopAllPostingsRpt/{AuthToken}/{UserType}/{ProfileID}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool StopAllPostingsRpt(string AuthToken, string UserType, string ProfileID);

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/IncidentsDataByFilterRpt/{IdentificationToken}/{UserType}/{startDate}/{endDate}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<Incident> IncidentsDataByFilterRpt(string IdentificationToken, string UserType, string startDate, string endDate);

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/GetAllGroupMembers/{AuthID}/{UType}/{GroupID}/{SearchKey}/{IdentificationToken}/{StartDate}/{EndDate}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<Member> GetAllGroupMembers(string AuthID, string UType, string GroupID, string searchKey, string IdentificationToken,string StartDate,string EndDate);

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/GetIncidentsbyDates/{IdentificationToken}/{startDate}/{enddate}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IncidentList GetIncidentsbyDates(string IdentificationToken, string startDate, string endDate);

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/GetMarshalsListToUnAssign/{GroupID}/{Token}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<BuddyAndMarshalRelation> GetMarshalsListToUnAssign(string GroupID, string Token);

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/UnAssignMarshalFromList/{TargetUserProfileID}/{BuddyUserID}/{Token}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ResultInfo UnAssignMarshalFromList(string TargetUserProfileID, string BuddyUserID, string Token);

        //[ValidAuthentication]
        //[OwnGroupMembersAccess]
        [OperationContract]
        [WebGet(UriTemplate = "/GetLocationDetails/{ProfileID}/{GroupID}/{LastUpdateDateTicks}/{Ticks}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ProfileLite GetLocationDetails(string ProfileID, string GroupID, string LastUpdateDateTicks, string Ticks);

        //[ValidAuthentication]
        //[OwnGroupMembersAccess]
        [OperationContract]
        [WebGet(UriTemplate = "/GetLiveGroupMembers/{GroupID}/{Ticks}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        LiveUserStatusList GetLiveGroupMembers(string GroupID, string Ticks);

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/GetLiveLocateBuddies/{UserID}/{Ticks}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        LiveUserStatusList GetLiveLocateBuddies(string UserID, string Ticks);

        //[ValidAuthentication]
        //[OwnGroupMembersAccess]
        [OperationContract]
        [WebGet(UriTemplate = "/GetGroupMembersCount/{GroupID}/{Ticks}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string GetGroupMembersCount(string GroupID, string Ticks);

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/GetUserBuddiesCount/{UserID}/{Ticks}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string GetUserBuddiesCount(string UserID, string Ticks);

        //[ValidAuthentication]
        //[OwnGroupMembersAccess]
        [OperationContract]
        [WebGet(UriTemplate = "/GetFilteredGroupMembers/{GroupID}/{SearchKey}/{Ticks}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        LiveUserStatusList GetFilteredGroupMembers(string GroupID, string SearchKey, string Ticks);

        //[ValidAuthentication]
        [OperationContract]
        [WebGet(UriTemplate = "/GetFilteredLocateBuddies/{UserID}/{SearchKey}/{Ticks}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        LiveUserStatusList GetFilteredLocateBuddies(string UserID, string SearchKey, string Ticks);

        //[ValidAuthentication]
        //[OwnGroupMembersAccess]
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, Method = "POST")]
        ResultInfo SaveDispatchInfo(DispatchInfo userAssignedTo);

        //[ValidAuthentication]
        //[OwnGroupMembersAccess or SelfAccess]        
        [OperationContract]
        [WebGet(UriTemplate = "/DeleteHistoryDetails/{ProfileID}/{SessionID}/{GroupID}/{Ticks}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool DeleteHistoryDetails(string ProfileID, string SessionID, string GroupID, string Ticks);
    }
}