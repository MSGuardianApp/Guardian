using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOS.Phone
{
    public static class Config
    {
        #region FBUris
        public static string FbAppId = "486681428059978";
        //AppSecret - only needed because of the fragment bug
        public static string FbAppSecret = "/TOREPLACE-FBSECRET/";
        #endregion

        public static bool UseGeoLocator = true;

        public static bool IsEnterpriseBuild = false;
        public static string EnterpriseEmailDomain = "@microsoft.com";

#if (!DEBUG)
        public static string GuardianServiceUrl = "https://guardianservice.cloudapp.net/";
        public static string GuardianPortalUrl = "https://guardianportal.cloudapp.net/";
        
        public const string LiveAuthClientId = "0000000044105B2B";

        public const string BingMapsKey = "/TOREPLACE-BINGKEY/";
#else
        public static string GuardianServiceUrl = "https://guardiansrvc-dev.cloudapp.net/";
        public static string GuardianPortalUrl = "https://guardianportal-dev.cloudapp.net/";

        public const string LiveAuthClientId = "000000004010A627";

        public const string BingMapsKey = "/TOREPLACE-BINGKEY/";

#endif
        
    }
}
