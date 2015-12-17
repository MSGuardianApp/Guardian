using SOS.ConfigManager;
using SOS.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SOS.AzureSQLAccessLayer
{
    public class LiveSessionRepository: ILiveSessionRepository
    {
        private readonly GuardianContext _guardianContext;

        public LiveSessionRepository()
            : this(new GuardianContext())
        {
        }

        public LiveSessionRepository(GuardianContext guardianContext)
        {
            if (guardianContext == null)
            {
                throw new ArgumentException("guardianContext");
            }
            _guardianContext = guardianContext;
        }

        public async Task PostMyLocationAsync(LiveLocation loc)
        {
            int result = await _guardianContext.Database
                      .ExecuteSqlCommandAsync("EXEC [dbo].[PostLiveLocation] @ProfileID,@SessionID,@ClientTimeStamp,@ClientDateTime,@Lat,@Long,@IsSOS,@Alt,@Speed,@MediaUri,@ExtendedCommand,@Accuracy",
                          new SqlParameter("@ProfileID", loc.ProfileID),//@ProfileID bigint
                          new SqlParameter("@SessionID", loc.SessionID),//@SessionID NVARCHAR(50)
                          new SqlParameter("@ClientTimeStamp", loc.ClientTimeStamp),//@ClientTimeStamp BIGINT
                          new SqlParameter("@ClientDateTime", loc.ClientDateTime),//@ClientDateTime DATETIME
                          new SqlParameter("@Lat", loc.Lat.OrDbNull()),//@Lat NVARCHAR(20)
                          new SqlParameter("@Long", loc.Long.OrDbNull()),//@Long NVARCHAR(20)
                          new SqlParameter("@IsSOS", loc.IsSOS),//@IsSOS BIT
                          new SqlParameter("@Alt", loc.Alt.OrDbNull()),//@Alt VARCHAR(10)
                          new SqlParameter("@Speed", loc.Speed),//@Speed INT
                          new SqlParameter("@MediaUri", loc.MediaUri.OrDbNull()),//@MediaUri VARCHAR(250)
                          new SqlParameter("@ExtendedCommand", DBNull.Value),//@ExtendedCommand NVARCHAR(50)
                          new SqlParameter("@Accuracy", loc.Accuracy.OrDbNull()));
        }

        public async Task ClearProcessingAsync(string roleID)
        {
            int result = await _guardianContext.Database
                                    .ExecuteSqlCommandAsync("EXEC [dbo].[ClearInProcessLiveSessions] @RoleID",
                                    new SqlParameter("@RoleID", roleID));
        }

        public async Task PurgeStaleSessionsAsync(List<LiveSession> locs)
        {
            foreach (var l in locs)
                _guardianContext.Entry(l).State = EntityState.Deleted;

            await _guardianContext.SaveChangesAsync();
        }

        public async Task<List<LiveSession>> GetLiveSessionsAsync()
        {
            var lastArchivedTime = DateTime.UtcNow.AddMinutes(-Config.ArchiveTimeGapInMinutes);

            return await _guardianContext.LiveSessions
                           .Where(w => w.LastModifiedDate < lastArchivedTime)
                           .AsNoTracking().ToListAsync();
        }

        public async Task<List<LiveSession>> GetSessionsForNotifications(string roleID, Guid processKey, bool sendSMS, int smsInterval, int emailInterval, int fbInterval)
        {
            var result = await _guardianContext.Database
                                            .SqlQuery<LiveSession>("EXEC [dbo].[GetSessionsForNotifications] @RoleID,@ProcessKey,@SendSMS,@SMSInterval,@EmailInterval,@FBInterval",
                                                new SqlParameter("@RoleID", roleID),
                                                new SqlParameter("@ProcessKey", processKey),
                                                new SqlParameter("@SendSMS", sendSMS),
                                                new SqlParameter("@SMSInterval", smsInterval),
                                                new SqlParameter("@EmailInterval", emailInterval),
                                                new SqlParameter("@FBInterval", fbInterval)).ToListAsync();
            return result;
        }

        public async Task<int> UpdateNotificationComplete(string roleID, Guid processKey, string updatedSessionsXML)
        {
            var result = await _guardianContext.Database
                        .ExecuteSqlCommandAsync("EXEC [dbo].[UpdateNotificationComplete] @RoleID,@ProcessKey,@UpdatedSessionXML",
                            new SqlParameter("@RoleID", roleID),
                            new SqlParameter("@ProcessKey", processKey),
                            new SqlParameter("@UpdatedSessionXML", updatedSessionsXML));
            return result;
        }
      
        public async Task<LiveSession> GetNotificationDetails(long profileID, string sessionID)
        {
            return await _guardianContext.LiveSessions
                .Where(w => w.ProfileID == profileID && w.SessionID == sessionID && w.Command != "STOP")
                .AsNoTracking().FirstOrDefaultAsync();
        }

        #region "Sync DB Calls for Reports"
        //we have made this method to work as sync  for report 
        public async Task<Dictionary<long, Tuple<short, DateTime>>> GetSOSLiveSessionData()
        {

            return _guardianContext.LiveSessions
                                .Where(w => w.IsSOS && w.Command != "STOP")
                                .Select(p => new
                                {
                                    ProfileID = p.ProfileID,
                                    SOSAlerts = p.NoOfSMSSent.Value,
                                    StartTime = p.SessionStartTime

                                }

                                )
                           .AsNoTracking().ToDictionary(k => k.ProfileID, v => Tuple.Create<short, DateTime>(v.SOSAlerts, v.StartTime));

        }

        public async Task UpdateLastSMSPostedTime(long ProfileID, string SessionID, DateTime SMSPostedTime)
        {
            int result = await _guardianContext.Database
                     .ExecuteSqlCommandAsync("EXEC [dbo].[UpdateLastSMSPostedTime] @ProfileID,@SessionID,@SMSPostedTime",
                         new SqlParameter("@ProfileID", ProfileID),
                         new SqlParameter("@SessionID", SessionID),
                         new SqlParameter("@SMSPostedTime", SMSPostedTime));
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
