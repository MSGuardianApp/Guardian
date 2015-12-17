//using SOS.Service.Interfaces.DataContracts;
//using SOS.Service.Utility;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//namespace SOS.Service.Implementation
//{
//    public abstract class ImplementationBase
//    {

//        /// <summary>
//        /// Authorize User based on existence in the app.
//        /// </summary>
//        /// <param name="AuthToken">Live Auth Token </param>
//        /// <param name="Type">User ('u') and Admin ('a')</param>
//        /// <returns> 
//        /// Cached User if authorized.
//        /// If user exists, SOSUserID will be filled, otherwise null or empty.
//        /// If exists in app, uses cache        
//        /// </returns>
//        public CacheUser AuthorizeUser(string AuthToken, char Type)
//        {
//            //TODO: TO IMPLEMENT Access Tags (SelfCreateEdit, SelfCreateOnly - createprofile only, etc)

//            CacheUser retUser = new CacheUser() { UserType = Type, };
//            //Check in CACHE
//            //If Not Exist Verify and load into Cache
//            var cachedUser = CacheAgent.LoadFromAuthCache(AuthToken);
//            if (cachedUser == null)
//            {
//                var secValidator = new SOS.Service.Security.AuthTokenValidator(AuthToken);
//                var securityrslt = secValidator.Result;
//                if (securityrslt.IsValid)
//                {
//                    if (Type == 'a')
//                    {

//                        retUser.IsValid = securityrslt.IsValid;
//                        retUser.IsExpired = securityrslt.IsExpired;
//                        retUser.UserType = 'a';
//                        retUser.LiveUserID = securityrslt.UserID;

//                        //Get Admin User and load it
//                        GroupService GA = new GroupService();
//                        var cAdmin = GA.GetAdminProfile(securityrslt.UserID);
//                        if (cAdmin == null)
//                        {
//                            retUser.SOSUserID = cAdmin.AdminID.ToString();
//                            CacheAgent.SaveIntoAuthCache(securityrslt.UserID, retUser);
//                        }
//                    }
//                    else if (Type == 'u')
//                    {
//                        retUser.IsValid = securityrslt.IsValid;
//                        retUser.IsExpired = securityrslt.IsExpired;
//                        retUser.UserType = 'u';
//                        retUser.LiveUserID = securityrslt.UserID;

//                        //Get Normal User and Load It.
//                        MemberService MS = new MemberService();
//                        var cUser = MS.ValidatedAndGetUser(securityrslt.UserID);
//                        if (cUser != null)
//                        {
//                            retUser.SOSUserID = cUser.UserID;
//                            CacheAgent.SaveIntoAuthCache(securityrslt.UserID, retUser);
//                        }
//                    }
//                }
//            }
//            else
//            {
//                retUser = cachedUser as CacheUser;
//            }
//            return retUser;
//        }
//    }
//}

