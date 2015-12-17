using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using SOS.AzureSQLAccessLayer;
using SOS.Service.Interfaces;

namespace SOS.Service.Implementation
{
    public class Authorization : IAuthorization
    {
        private static readonly ConcurrentDictionary<string, bool> AuthCache = new ConcurrentDictionary<string, bool>();
        //Local Cache for method inputs

        private static DateTime lastCacheRefreshedTime = DateTime.Now;
        private readonly AuthRepository authRepository;

        public Authorization()
        {
            authRepository = new AuthRepository();

            if (lastCacheRefreshedTime < DateTime.Now.AddHours(-1)) // Local Cache reset interval - 1hr
            {
                AuthCache.Clear();
                lastCacheRefreshedTime = DateTime.Now;
            }
        }

        public async Task<bool> SelfAccess(string LiveUserID, long ProfileID)
        {
            string key = "S-" + LiveUserID + "-" + ProfileID;

            bool result = false;
            if (AuthCache.TryGetValue(key, out result)) return result;

            result = await authRepository.SelfAccess(LiveUserID, ProfileID);
            AuthCache.TryAdd(key, result);

            return result;
        }

        public async Task<bool> LocateBuddyAccess(string LiveUserID, long ProfileID)
        {
            string key = "L-" + LiveUserID + "-" + ProfileID;

            bool result = false;
            if (AuthCache.TryGetValue(key, out result)) return result;

            result = await authRepository.LocateBuddyAccess(LiveUserID, ProfileID);
            AuthCache.TryAdd(key, result);

            return result;
        }

        public async Task<bool> OwnGroupMembersAccess(string LiveUserID, int GroupID, long ProfileID)
        {
            string key = "G-" + LiveUserID + "-" + GroupID + "-" + ProfileID;

            bool result = false;
            if (AuthCache.TryGetValue(key, out result)) return result;

            result = await authRepository.SelfGroupMembersAccess(GroupID, ProfileID);
            AuthCache.TryAdd(key, result);

            return result;
        }

        public async Task<bool> ValidUserAccess(string LiveUserID, long UserID)
        {
            string key = "U-" + LiveUserID + "-" + UserID;

            bool result = false;
            if (AuthCache.TryGetValue(key, out result)) return result;

            result = await authRepository.ValidUserAccess(LiveUserID, UserID);
            AuthCache.TryAdd(key, result);

            return result;
        }
    }
}