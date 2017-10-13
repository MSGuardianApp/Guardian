using SOS.Service.Interfaces.DataContracts;
using SOS.Service.Interfaces.DataContracts.OutBound;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace SOS.Service.Interfaces
{
    [ServiceContract(Namespace = "http://www.microsoft.com/sos/2013/02", Name = "GroupService")]
    public interface IGroupService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/GetListOfGroups/{SearchKey}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<GroupList> GetListOfGroups(string SearchKey);

        [OperationContract]
        [WebGet(UriTemplate = "/GetListOfVolunteerGroups", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<List<Group>> GetListOfVolunteerGroups();

        //[OperationContract]
        //[WebGet(UriTemplate = "/GetAdminProfile/{LiveMail}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Admin GetAdminProfile(string LiveMail);

        //[OperationContract]
        //[WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, Method = "POST")]
        Task<ResultInfo> SaveMarshalInfo(MarshallToAdd MarshallObject);

        //[OperationContract]
        //[WebGet(UriTemplate = "/GetMarshalList/{GroupID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<MarshalList> GetMarshalList(string GroupID);

        //[OperationContract]
        //[WebGet(UriTemplate = "/AssignBuddyToMarshal/{AdminID}/{GroupID}/{MarshalProfileID}/{MarshalUserID}/{TargetUserProfileID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<ResultInfo> AssignBuddyToMarshal(string AdminID, string GroupID, string MarshalProfileID, string MarshalUserID, string TargetUserProfileID);

        //[OperationContract]
        //[WebGet(UriTemplate = "/RemoveBuddyFromMarshal/{AdminID}/{GroupID}/{MarshalProfileID}/{MarshalUserID}/{TargetUserProfileID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<ResultInfo> RemoveBuddyFromMarshal(string AdminID, string GroupID, string MarshalProfileID, string MarshalUserID, string TargetUserProfileID);

        //[OperationContract]
        //[WebGet(UriTemplate = "/DeleteMarshal/{AdminID}/{GroupID}/{MarshalProfileID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<ResultInfo> DeleteMarshal(string AdminID, string GroupID, string MarshalProfileID);

        //[OperationContract]
        //[WebGet(UriTemplate = "/ValidateGroupMember/{ValidationID}/{ProfileID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<ResultInfo> ValidateGroupMember(string ValidationID, string ProfileID);

        //[OperationContract]
        //[WebGet(UriTemplate = "/ValidateGroupMarshal/{ValidationID}/{ProfileID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<ResultInfo> ValidateGroupMarshal(string ValidationID, string ProfileID);

        //Task<List<GroupMemberLiveSession>> GetFilteredParentGroupLiveMemberSession();
    }

}