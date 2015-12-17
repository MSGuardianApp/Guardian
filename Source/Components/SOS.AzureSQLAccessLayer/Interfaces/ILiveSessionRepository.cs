using SOS.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOS.AzureSQLAccessLayer
{
    public interface ILiveSessionRepository : IDisposable
    {
        Task PostMyLocationAsync(LiveLocation loc);

        Task ClearProcessingAsync(string roleID);

        Task PurgeStaleSessionsAsync(List<LiveSession> locs);

        Task<List<LiveSession>> GetLiveSessionsAsync();

        Task<List<LiveSession>> GetSessionsForNotifications(string roleID, Guid processKey, bool sendSMS, int smsInterval, int emailInterval, int fbInterval);

        Task<int> UpdateNotificationComplete(string roleID, Guid processKey, string updatedSessionsXML);

        Task<Dictionary<long, Tuple<short, DateTime>>> GetSOSLiveSessionData();

        Task<LiveSession> GetNotificationDetails(long profileID, string sessionID);
    }
}
