using SOS.Service.Interfaces.DataContracts;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
namespace SOS.Service.Interfaces
{
    [ServiceContract(Namespace = "http://www.microsoft.com/sos/2013/02", Name = "GeoService")]
    public interface IGeoUpdates
    {
        //[SelfAccess]
        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST", BodyStyle = WebMessageBodyStyle.Bare)]
        Task<bool> PostMyLocation(GeoTags GeoTags);

        //[SelfAccess]
        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST", BodyStyle = WebMessageBodyStyle.Bare)]
        Task<bool> PostLocationWithMedia(GeoTag GeoTag);

        //[SelfAccess]
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "StopPostings/{ProfileID}/{SessionID}/{Ticks}", BodyStyle = WebMessageBodyStyle.Bare)]
        Task StopPostings(string ProfileID, string SessionID, string Ticks);

        //[OperationContract]
        //[WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "StopAllPostings/{ProfileID}/{Ticks}", BodyStyle = WebMessageBodyStyle.Bare)]
        //Task StopAllPostings(string ProfileID, string Ticks);

        //[SelfAccess]
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "StopSOSOnly/{ProfileID}/{SessionID}/{Ticks}", BodyStyle = WebMessageBodyStyle.Bare)]
        Task StopSOSOnly(string ProfileID, string SessionID, string Ticks);

        //[SelfAccess]
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "UpdateLastSMSPostedTime/{ProfileID}/{SessionID}/{SMSPostedTime}", BodyStyle = WebMessageBodyStyle.Bare)]
        Task UpdateLastSMSPostedTime(string ProfileID, string SessionID, string SMSPostedTime);

        //[SelfAccess]
        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST", BodyStyle = WebMessageBodyStyle.Bare)]
        string ReportIncident(IncidentTag incidentTag);

        //[OperationContract]
        //[WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "SwitchOnSOSviaSMS/{ProfileID}/{SessionID}/{ticks}/{lat}/{lng}/{utcTicks}", BodyStyle = WebMessageBodyStyle.Bare)]
        Task SwitchOnSOSviaSMS(string ProfileID, string sessionID, string SOS, string ticks, string lat, string lng, string utcTicks);

        //[OperationContract]
        //[WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetIncidents/{IdentificationToken}", BodyStyle = WebMessageBodyStyle.Bare)]
        IncidentList GetIncidents(string IdentificationToken);

        //[OperationContract]
        //[WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetIncidentsbyDates/{IdentificationToken}/{startDate}/{endDate}", BodyStyle = WebMessageBodyStyle.Bare)]
        IncidentList GetIncidentsbyDates(string IdentificationToken, string startDate, string endDate);
    }
}