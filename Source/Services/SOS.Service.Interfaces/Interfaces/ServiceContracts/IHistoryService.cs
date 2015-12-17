using SOS.Service.Interfaces.DataContracts;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SOS.Service.Interfaces
{
    [ServiceContract(Namespace = "http://www.microsoft.com/sos/2013/02", Name = "HistoryService")]
    public interface IHistoryService
    {

        //[OperationContract]
        //[WebGet(UriTemplate = "/GetHistorySessions/{ProfileID}/{StartDate}/{EndDate}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        HistoryList GetHistorySessions(string ProfileID, string StartDate, string EndDate);

        //[OperationContract]
        //[WebGet(UriTemplate = "/GetHistoryLocation/{Token}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<GeoTag> GetHistoryLocation(string Token);

        GeoTagList GetHistoryLocationsBySessionID(string profileID, string sessionID);

        Task<bool> DeleteHistoryDetails(string ProfileID, string SessionID, string GroupID);
    }
}
