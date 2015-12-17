using SOS.Service.Interfaces.DataContracts;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SOS.Service.Interfaces
{
    [ServiceContract(Namespace = "http://www.microsoft.com/sos/2013/02", Name = "ReportService")]
    public interface IReportService
    {

        //[OperationContract]
        //[WebGet(UriTemplate = "/UserCount", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<int> UserCount();


        //[OperationContract]
        //[WebGet(UriTemplate = "/GroupUsers", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<object> GroupUsers();

        //[OperationContract]
        //[WebGet(UriTemplate = "/MissedActivationCount", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        int MissedActivationCount();

        //[OperationContract]
        //[WebGet(UriTemplate = "/UserReport", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<List<DemographyReport>> UserReport();
        
        //[OperationContract]
        //[WebGet(UriTemplate = "/SOSStats", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<SOSTrackingReport> SOSStats(string StartTicks, string EndTicks);


        //[OperationContract]
        //[WebGet(UriTemplate = "/SOSAndTrackStats", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<SOSAndTrackonReport> SOSAndTrackStats(string StartTicks, string EndTicks);


        //[OperationContract]
        //[WebGet(UriTemplate = "/ActiveModeStats", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<List<ActiveSOSReports>> ActiveModeStats();

        //[OperationContract]
        //[WebGet(UriTemplate = "/UnMarshalsReport/{GroupID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<List<BuddyAndMarshalRelation>> UnMarshalsReport(string GroupID);


    }
}
