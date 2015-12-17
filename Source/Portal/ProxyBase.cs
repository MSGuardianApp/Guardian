using System;
using System.Text;
using SOS.Service.Interfaces.DataContracts;
using SOS.Service.Interfaces.DataContracts.OutBound;
using SOS.Service.Implementation;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Collections.Concurrent;

namespace SOS.Web
{
    public abstract class ProxyBase
    {
        private static ConcurrentDictionary<string, CacheUser> AuthCache = new ConcurrentDictionary<string, CacheUser>(); //Local Cache for method inputs
        private static DateTime lastCacheRefreshedTime = DateTime.Now;

        /// <summary>
        /// Authorize User based on existence in the app.
        /// </summary>
        /// <param name="AuthToken">Live Auth Token </param>
        /// <param name="Type">User ('u') and Admin ('a')</param>
        /// <returns> 
        /// Cached User if authorized.
        /// If user exists, SOSUserID will be filled, otherwise null or empty.
        /// If exists in app, uses cache        
        /// </returns>
        protected CacheUser AuthorizeUser(string AuthToken, char Type, string GroupID = "")
        {
            if (lastCacheRefreshedTime < DateTime.Now.AddHours(-2))// Local Cache reset interval - 2hr
            {
                AuthCache.Clear();
                lastCacheRefreshedTime = DateTime.Now;
            }

            //TODO: TO IMPLEMENT Access Tags (SelfCreateEdit, SelfCreateOnly - createprofile only, etc)
            CacheUser retUser = new CacheUser() { UserType = Type };

            //Check in CACHE
            //If Not Exist Verify and load into Cache
            var key = AuthToken + "-" + Type.ToString() + "-" + GroupID;

            CacheUser cachedUser = new CacheUser() { IsValid = false };
            if (!AuthCache.TryGetValue(key, out cachedUser))
            {
                AuthCache.TryAdd(key, cachedUser);

                var secValidator = new SOS.Service.Security.AuthTokenValidator(AuthToken);
                var securityrslt = secValidator.Result;
                if (securityrslt.IsValid)
                {
                    if (Type == 'a')
                    {
                        retUser.IsValid = securityrslt.IsValid;
                        retUser.IsExpired = securityrslt.IsExpired;
                        retUser.UserType = 'a';
                        retUser.LiveUserID = securityrslt.UserID;

                        //Get Admin User and load it
                        GroupService GA = new GroupService();
                        var cAdmin = GA.GetAdminProfile(securityrslt.UserID);
                        if (cAdmin != null)
                        {
                            retUser.SOSUserID = cAdmin.AdminID.ToString();

                            if (string.IsNullOrEmpty(GroupID) || cAdmin.GroupIDCSV == GroupID)
                                retUser.IsValid = true;
                            else
                                retUser.IsValid = false;

                            AuthCache.AddOrUpdate(key, retUser, (k, oldValue) => retUser);
                        }
                    }
                    else if (Type == 'u')
                    {
                        retUser.IsValid = securityrslt.IsValid;
                        retUser.IsExpired = securityrslt.IsExpired;
                        retUser.UserType = 'u';
                        retUser.LiveUserID = securityrslt.UserID;

                        //Get Normal User and Load It.
                        MemberService MS = new MemberService();
                        User cUser = MS.ValidatedAndGetUser(securityrslt.UserID).Result;
                        if (cUser != null)
                        {
                            retUser.SOSUserID = cUser.UserID.ToString();
                            AuthCache.AddOrUpdate(key, retUser, (k, oldValue) => retUser);
                        }
                    }
                }
            }
            else
            {
                retUser = cachedUser as CacheUser;
            }
            return retUser;
        }

        protected string HandleAuthError(string AuthToken)
        {
            var serl = new DataContractJsonSerializer(typeof(ResultInfo));
            using (MemoryStream stream = new MemoryStream())
            {
                ResultInfo result = new ResultInfo()
                {
                    ResultType = ResultTypeEnum.AuthError,
                    Message = "Your Authentication failed."
                };
                serl.WriteObject(stream, result);
                return Encoding.Default.GetString(stream.ToArray());
            }
        }

        protected bool CheckGroupManagementAccess(string liveUserID)
        {
            GroupService GS = new GroupService();
            Admin cAdmin = GS.GetAdminProfile(liveUserID);
            if (cAdmin != null)
                return cAdmin.AllowGroupManagement;
            else
                return false;
        }

        //Start ToBaseCache
    }
}
