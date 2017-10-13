using SOS.AzureStorageAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOS.AzureStorageAccessLayer
{
    public interface ILocationHistoryStorageAccess
    {
        Task SaveToLocationHistoryAsync(LocationHistory locData);

        List<LocationHistory> GetLocationHistory(string profileID, DateTime startDate, DateTime endDate);

        List<SessionHistory> GetSessionHistory(string profileID, DateTime startDate, DateTime endDate);

        List<LocationHistory> GetHistoryLocationsBySessionID(string profileID, string sessionID);

        List<GeoTag> GetLocationDataFromHistory(string Token);

        bool IsEntryThereInHistoryTable(string ProfileID, string Token, long ClientDateTime);

        void DeleteHistoryDetails(string ProfileID, string SessionID);

        Task DeleteLocationHistoryAsync(string ProfileID, string SessionID);

        Task DeleteSessionHistoryAsync(string ProfileID, string SessionID);
    }

}
