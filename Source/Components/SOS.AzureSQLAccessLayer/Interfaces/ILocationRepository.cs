using SOS.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOS.AzureSQLAccessLayer
{
    public interface ILocationRepository:IDisposable
    {
        Task<IEnumerable<LiveLocation>> GetLocationData(long ProfileID, long LastUpdatedTimeTicks);

        Task<IEnumerable<UserLocation>> GetBuddiesToLocateLastLocation(long userID);

        Task<IEnumerable<UserLocation>> GetBuddiesToLocateByProfileID(long userProfileID);

        Task<bool> IsSosAlreadyOn(long ProfileID, string sessionID, long ClientDateTime);

        Task<List<LiveLocation>> GetLocationDataByToken(long ProfileID, string sessionID, long LastUpdatedTime);

        Task<SOSTrackInfo> GetSOSTrackCountByGroupId(int groupID);

        Task<IEnumerable<LiveLocation>> GetAllLocationData(long LastUpdatedTime);

        Task RemoveLiveLocation(long profileID, string sessionID, long clientTicks);

        Task RemoveLiveLocationData(long profileID, long clientTicks, string sessionID = null);

        Task PostMyLocation(LiveLocation loc);

        Task StopSOSOnly(long profileID, string sessionID = null);

        Task<Dictionary<string, string>> GetSOSTrackCountAsync(long userID);

        Task<int> PurgeStaleLiveLocations();

        Task<List<LiveUserStatus>> GetLiveMembersByGroupID(int groupID, bool includeActiveUsers);

        Task<List<LiveUserStatus>> GetLiveLocateBuddiesByUserId(long UserID);
    }
}
