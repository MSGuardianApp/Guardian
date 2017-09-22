using SOS.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
namespace SOS.AzureSQLAccessLayer
{
    public class LocationRepository : ILocationRepository
    {
        private readonly GuardianContext _guardianContext;

        public LocationRepository()
            : this(new GuardianContext())
        {
        }

        public LocationRepository(GuardianContext guardianContext)
        {
            if (guardianContext == null)
            {
                throw new ArgumentException("guardianContext");
            }
            _guardianContext = guardianContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProfileID"></param>
        /// <param name="LastUpdatedTimeTicks"></param>
        /// <returns></returns>
        public async Task<IEnumerable<LiveLocation>> GetLocationData(long ProfileID, long LastUpdatedTimeTicks)
        {
            return await _guardianContext.LiveLocations
                  .Where(w => w.ProfileID == ProfileID && w.ClientTimeStamp >= LastUpdatedTimeTicks)
                  .OrderBy(o => o.ClientTimeStamp)
                  .AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="LastUpdatedTime"></param>
        /// <returns></returns>
        public async Task<IEnumerable<UserLocation>> GetBuddiesToLocateLastLocation(long userID)
        {
            var result = await (from bdy in _guardianContext.LiveLocateBuddiesViews
                                where bdy.UserID == userID && bdy.Email != Constants.InvalidEmail && bdy.MobileNumber != Constants.InvalidMobileNumber
                                select new UserLocation
                                {
                                    ProfileID = bdy.ProfileID,
                                    Name = bdy.Name,
                                    LastLocs = new List<GeoTagLocs>() { new GeoTagLocs { Lat = bdy.Lat, Long = bdy.Long, TimeStamp = bdy.ClientTimeStamp } },
                                    IsSOSOn = bdy.IsSOS.HasValue ? bdy.IsSOS.Value : false,
                                    IsTrackingOn = bdy.IsSOS.HasValue ? !bdy.IsSOS.Value : false,
                                    Email = bdy.Email,
                                    MobileNumber = bdy.MobileNumber
                                }).AsNoTracking().ToListAsync();

            Parallel.ForEach(result, r =>
            {
                if (string.IsNullOrWhiteSpace(r.LastLocs[0].Lat)) r.LastLocs = null;
            });

            return result;
        }

        public async Task<IEnumerable<UserLocation>> GetBuddiesToLocateByProfileID(long userProfileID)
        {
            return await (from bdy in _guardianContext.Buddies
                          join prf in _guardianContext.Profiles on bdy.UserID equals prf.UserID
                          join locateprf in _guardianContext.Profiles on bdy.ProfileID equals locateprf.ProfileID
                          join usr in _guardianContext.Users on locateprf.UserID equals usr.UserID
                          where prf.ProfileID == userProfileID
                          select new UserLocation
                          {
                              ProfileID = bdy.ProfileID,
                              Name = usr.Name
                          }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProfileID"></param>
        /// <param name="Token"></param>
        /// <param name="ClientDateTime"></param>
        /// <returns></returns>

        public async Task<bool> IsSosAlreadyOn(long ProfileID, string sessionID, long ClientDateTime)// IsSOS flag
        {
            int locs = await _guardianContext.LiveSessions
           .Where(w => (w.ProfileID == ProfileID && w.SessionID == sessionID) && w.Command != "STOP" && w.IsSOS)
           .AsNoTracking().CountAsync();

            return (locs > 0) ? true : false;
        }


        public async Task<bool> IsActiveSession(long ProfileID, string sessionID)
        {
            int locs = await _guardianContext.LiveSessions
           .Where(w => (w.ProfileID == ProfileID && w.SessionID == sessionID) && w.Command != "STOP")
           .AsNoTracking().CountAsync();

            return (locs > 0) ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProfileID"></param>
        /// <param name="Token"></param>
        /// <param name="LastUpdatedTime"></param>
        /// <returns></returns>

        public async Task<List<LiveLocation>> GetLocationDataByToken(long ProfileID, string sessionID, long LastUpdatedTime)
        {
            return await _guardianContext.LiveLocations
           .Where(w => (w.ProfileID == ProfileID && w.ClientTimeStamp >= LastUpdatedTime && w.SessionID == sessionID))
           .OrderBy(o => o.ClientTimeStamp)
           .AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public async Task<SOSTrackInfo> GetSOSTrackCountByGroupId(int groupID)
        {
            SOSTrackInfo _sosTrackCount = new SOSTrackInfo();

            var liveusers = (from loc in _guardianContext.LiveSessions
                             join grp in _guardianContext.GroupMemberships on loc.ProfileID equals grp.ProfileID
                             where grp.GroupID == groupID
                             select new
                             {
                                 ProfileID = loc.ProfileID,
                                 IsSOS = loc.IsSOS,
                             }).AsNoTracking().ToListAsync();

            var liveUsers = await liveusers;

            _sosTrackCount.SOSCount = liveUsers.Count(x => x.IsSOS);
            _sosTrackCount.TrackCount = liveUsers.Count(x => !x.IsSOS);

            liveUsers.ForEach(liveUser =>
            {
                if (liveUser.IsSOS)
                {
                    _sosTrackCount.SOSProfileIds += liveUser.ProfileID + "'";
                }
                else
                {
                    _sosTrackCount.TotalProfileIds += liveUser.ProfileID + "'";
                }
            });

            return _sosTrackCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LastUpdatedTime"></param>
        /// <returns></returns>
        public async Task<IEnumerable<LiveLocation>> GetAllLocationData(long LastUpdatedTime)//TODO: Check the usage
        {
            return await _guardianContext.LiveLocations
           .Where(w => (w.ClientTimeStamp > LastUpdatedTime))
           .OrderByDescending(o => o.ClientDateTime).AsNoTracking().ToListAsync();
        }


        public async Task RemoveLiveLocation(long profileID, string sessionID, long clientTicks)
        {
            int result = await _guardianContext.Database
             .ExecuteSqlCommandAsync("EXEC [dbo].[RemoveLiveLocation] @ProfileID,@SessionID,@ClientTimeStamp",
                 new SqlParameter("@ProfileID", profileID),
                 new SqlParameter("@SessionID", sessionID.OrDbNull()),
                 new SqlParameter("@ClientTimeStamp", clientTicks));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProfileID"></param>
        /// <returns></returns>
        public async Task RemoveLiveLocationData(long profileID, long clientTicks, string sessionID = null)
        {
            await RemoveLiveLocation(profileID, sessionID, clientTicks);
        }

        public async Task PostMyLocation(LiveLocation loc)
        {
            _guardianContext.LiveLocations.Add(loc);
            await _guardianContext.SaveChangesAsync();
        }


        /// <summary>
        /// Removes live location data for a profile id before certain timestamp.
        /// </summary>
        /// <param name="ProfileID">Profile ID</param>
        /// <param name="BeforeClientTime">Client Time ticks prior to which data has to be deleted.</param>
        //public async Task RemoveLiveLocationOnTimeStamp(long profileID, long BeforeClientTime)//Combine as above
        //{
        //    await RemoveLiveLocation(profileID, null, BeforeClientTime);
        //}

        public async Task<Dictionary<string, string>> GetSOSTrackCountAsync(long userID)
        {
            Dictionary<string, string> dReturn = new Dictionary<string, string>();
            LocateBuddyCountView loc = await ((from bdy in _guardianContext.LocateBuddyCountViews
                                               where bdy.UserID == userID
                                               select bdy).FirstOrDefaultAsync());

            if (loc != null)
            {
                dReturn.Add("SOSCount", loc.SosCount.ToString());
                dReturn.Add("TrackCount", loc.TrackCount.ToString());
                return dReturn;
            }
            dReturn.Add("SOSCount", "0");
            dReturn.Add("TrackCount", "0");

            return dReturn;
        }

        public async Task<int> PurgeStaleLiveLocations()
        {
            int result = await _guardianContext.Database.ExecuteSqlCommandAsync("EXEC [dbo].[PurgeStaleLiveLocations]");
            return result;
        }

        public async Task<List<LiveUserStatus>> GetLiveMembersByGroupID(int groupID, bool includeActiveUsers)
        {
            if (includeActiveUsers)
                return await ((from LS in _guardianContext.LiveSessions
                               join grp in _guardianContext.GroupMemberships on LS.ProfileID equals grp.ProfileID
                               where grp.GroupID == groupID && LS.Command != "STOP"
                               select new LiveUserStatus
                               {
                                   ProfileID = LS.ProfileID,
                                   Name = LS.Name,
                                   SessionID = LS.SessionID,
                                   Status = (LS.IsSOS) ? 2 : 1,
                                   MobileNumber = LS.MobileNumber,
                                   DispatchInfo = LS.DispatchInfo
                               }).AsNoTracking().ToListAsync());
            else
                return await ((from LS in _guardianContext.LiveSessions
                               join grp in _guardianContext.GroupMemberships on LS.ProfileID equals grp.ProfileID
                               where grp.GroupID == groupID && LS.Command != "STOP" && LS.IsSOS == true
                               select new LiveUserStatus
                               {
                                   ProfileID = LS.ProfileID,
                                   Name = LS.Name,
                                   SessionID = LS.SessionID,
                                   Status = (LS.IsSOS) ? 2 : 1,
                                   MobileNumber = LS.MobileNumber,
                                   DispatchInfo = LS.DispatchInfo
                               }).AsNoTracking().ToListAsync());
        }

        public async Task<List<LiveUserStatus>> GetLiveLocateBuddiesByUserId(long UserID)
        {
            var liveUsersStatus = (from B in _guardianContext.Buddies
                                   join LS in _guardianContext.LiveSessions on B.ProfileID equals LS.ProfileID
                                   where B.UserID == UserID && LS.Command != "STOP"
                                   select new LiveUserStatus
                                   {
                                       ProfileID = LS.ProfileID,
                                       Name = LS.Name,
                                       SessionID = LS.SessionID,
                                       Status = (LS.IsSOS) ? 2 : 1,
                                       MobileNumber = LS.MobileNumber,
                                       DispatchInfo = LS.DispatchInfo
                                   }).AsNoTracking().ToListAsync<LiveUserStatus>();

            return await liveUsersStatus;
        }


        public async Task<int> GetUserLiveStatus(string LiveID)
        {

            int lsUserStatus = 1;// initially it will set as user inactive.

            var profileID = (from usr in _guardianContext.Users
                             join prf in _guardianContext.Profiles on usr.UserID equals prf.UserID
                             where usr.LiveID == LiveID && usr.Email != Constants.InvalidEmail && usr.MobileNumber != Constants.InvalidMobileNumber && prf.IsValid
                             select prf.ProfileID).Take(1);

            if (!profileID.Any())
                throw new Exception("Profile not found for the user");

            var usrLiveSession = await (from ls in _guardianContext.LiveSessions
                                        where profileID.FirstOrDefault() == ls.ProfileID
                                        select new
                                        {
                                            ProfileID = ls.ProfileID,
                                            IsSOS = ls.IsSOS

                                        }).AsNoTracking().FirstOrDefaultAsync();

            if (usrLiveSession != null)
            {
                if (usrLiveSession.IsSOS)
                    lsUserStatus = 3;//set when user in SOS
                else
                    lsUserStatus = 2;//set when user in tracking

            }
            return lsUserStatus;
        }

        #region "Sync DB Calls for Reports"
        //we have made this method as sync to work for report 
        public async Task StopSOSOnly(long profileID, string sessionID = null)
        {

            int result = await _guardianContext.Database
             .ExecuteSqlCommandAsync("EXEC [dbo].[StopSOSOnly] @ProfileID,@SessionID",
                 new SqlParameter("@ProfileID", profileID),
                 new SqlParameter("@SessionID", sessionID));

        }
        #endregion

        #region Dispose Section
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _guardianContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}