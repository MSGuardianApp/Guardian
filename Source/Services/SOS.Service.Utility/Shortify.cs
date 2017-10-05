using System.Net;
using System.IO;
using Guardian.Common;

namespace SOS.Service.Utility
{
    //http://guardianportal.cloudapp.net/default.aspx
    //http://tinyurl.com/api-create.php
    public static class Shortify
    {
        private static string GuardianPortalUri = Config.GuardianPortalUri;
        private static string TinyServiceUri = Config.TinyServiceUri;
        public static string CreateStaticLocateMeURI(string profileID)
        {
            string inpUri = GuardianPortalUri + string.Format("/default.aspx?V=2&{0}={1}", "profileid", profileID);
            string tinyUri = CreateTinyUri(inpUri);
            return tinyUri;
        }

        public static string CreateDynamicLocateMeURI(string profileID, string Token)
        {
            //create the uri for the profile landing and locate and shorten it.
            string inpUri = GuardianPortalUri + string.Format("/default.aspx?V=2&{0}={1}&{2}={3}", "pr", profileID, "s", Token);
            string tinyUri = CreateTinyUri(inpUri);
            return tinyUri;
        }

        public static string CreateSubscribeBuddyActionURI(string profileId, string buddyUserId, string subscribtionId, string actionCode)
        {
            string inputUri = GuardianPortalUri + string.Format("/SubscribeAction.aspx?V=2&{0}={1}&{2}={3}&{4}={5}&{6}={7}", "pr", profileId, "uid", buddyUserId, "s", subscribtionId, "at", actionCode);
            string tinyUri = CreateTinyUri(inputUri);
            return tinyUri;
        }

        private static string CreateTinyUri(string longuri)
        {
            string tinyUri = TinyServiceUri + string.Format("?{0}={1}", "url", longuri);
            var request = WebRequest.Create(tinyUri);
            try
            {
                var response = request.GetResponse();
                string outputUrl;

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    outputUrl = reader.ReadToEnd();
                }
                if (string.IsNullOrEmpty(outputUrl))
                    return GuardianPortalUri;
                else
                    return outputUrl;
            }
            catch
            {
                return longuri;
            }
        }

    }
}