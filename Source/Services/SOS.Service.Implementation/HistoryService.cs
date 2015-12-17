using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using SOS.AzureStorageAccessLayer;
using SOS.Service.Interfaces;
using SOS.Service.Interfaces.DataContracts;
using SOS.Service.Interfaces.DataContracts.OutBound;
using SOS.Service.Utility;
using entity = SOS.AzureStorageAccessLayer.Entities;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace SOS.Service.Implementation
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class HistoryService : IHistoryService
    {
        private readonly LocationHistoryStorageAccess _GPSA = new LocationHistoryStorageAccess();

        public HistoryList GetHistorySessions(string profileID, string startDate, string endDate)
        {
            //confirm if null should be returned in case of empty start and end dates
            if (profileID == "0" || string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
                return null;

            long ProfileID = Convert.ToInt64(profileID);

            var resultHistory = new HistoryList();

            DateTime vStartTime;
            DateTime vEndTime;

            vStartTime = Converter.ToDateTime(startDate).Date;
            vEndTime = Converter.ToMaxDateTime(endDate).Date.AddMinutes(1440).AddSeconds(-1);

            List<entity.SessionHistory> sessionHistoryData = _GPSA.GetSessionHistory(profileID, vStartTime, vEndTime);
            sessionHistoryData = sessionHistoryData.OrderByDescending(o => o.SessionStartTime).ToList();
            if (sessionHistoryData == null || sessionHistoryData.Count == 0)
            {
                ResultsManager.AddResultInfo(resultHistory, ResultTypeEnum.Error, "No history data found.");
                return resultHistory;
            }
            //Get History Media Data 

            resultHistory = Caster.MakeContractSessionHistoryList(sessionHistoryData);

            return resultHistory;
        }

        public GeoTagList GetHistoryLocationsBySessionID(string profileID, string sessionID)
        {
            List<entity.LocationHistory> locationHistoryData = _GPSA.GetHistoryLocationsBySessionID(profileID, sessionID);
            return Caster.MakeGeoTagList(locationHistoryData);
        }

        public List<GeoTag> GetHistoryLocation(string Token)
        {
            if (string.IsNullOrEmpty(Token))
                return null;

            List<entity.GeoTag> geoLocation = _GPSA.GetLocationDataFromHistory(Token);

            List<GeoTag> geoHistoryLocData = Caster.MakeContractGeoTagList(geoLocation);

            return geoHistoryLocData;
        }

        public async Task<bool> DeleteHistoryDetails(string ProfileID, string SessionID, string GroupID)
        {
            string LiveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            long profileID = Convert.ToInt64(ProfileID);
            bool isAuthorized = false;
            Authorization _authService = new Authorization();
            if (UType != null && Convert.ToChar(UType) == 'a')
                isAuthorized = await _authService.OwnGroupMembersAccess(LiveUserID, Convert.ToInt32(GroupID), profileID);
            else            
                isAuthorized = await _authService.SelfAccess(LiveUserID, profileID);

            if (isAuthorized)
            {
                _GPSA.DeleteHistoryDetails(ProfileID, SessionID);
                return true;
            }
            else
                return false;
        }
    }
}