using Guardian.Common.Configuration;
using SOS.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SOS.AzureSQLAccessLayer
{
    public class AuthRepository
    {
        readonly GuardianContext _guardianContext;
        readonly IConfigManager configManager;

        public AuthRepository(IConfigManager configManager)
            : this(new GuardianContext(configManager.Settings.AzureSQLConnectionString))
        {
            this.configManager = configManager;
        }

        public AuthRepository(GuardianContext guardianContext)
        {
            if (guardianContext == null)
            {
                throw new ArgumentException("guardianContext");
            }
            _guardianContext = guardianContext;
        }

        public async Task<bool> SelfAccess(string LiveUserID, long ProfileID)
        {
            int resCount = await (from usr in _guardianContext.Users
                                  join prf in _guardianContext.Profiles on usr.UserID equals prf.UserID
                                  where usr.LiveID == LiveUserID && prf.ProfileID == ProfileID
                                  select usr.UserID).CountAsync();
            return (resCount > 0) ? true : false;
        }

        public async Task<bool> LocateBuddyAccess(string LiveUserID, long ProfileID)
        {
            int resCount = await (from bdy in _guardianContext.Buddies
                                  join bdyDetails in _guardianContext.Users on bdy.UserID equals bdyDetails.UserID
                                  join prf in _guardianContext.Profiles on bdy.ProfileID equals prf.ProfileID
                                  join usr in _guardianContext.Users on prf.UserID equals usr.UserID
                                  where bdyDetails.LiveID == LiveUserID && bdy.ProfileID == ProfileID
                                  select usr.UserID).CountAsync();
            return (resCount > 0) ? true : false;
        }

        public async Task<bool> SelfGroupMembersAccess(int GroupID, long ProfileID)
        {
            int resCount = await (from gm in _guardianContext.GroupMemberships
                                  where gm.GroupID == GroupID && gm.ProfileID == ProfileID
                                  select gm.ProfileID).CountAsync();
            return (resCount > 0) ? true : false;
        }

        public async Task<bool> ValidUserAccess(string LiveUserID, long UserID)
        {
            int resCount = await _guardianContext.Users
                .Where(w => w.LiveID == LiveUserID && w.UserID == UserID)
                .CountAsync();
            return (resCount > 0) ? true : false;
        }

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
