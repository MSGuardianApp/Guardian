using Guardian.Common.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SOS.Service.Utility
{
    //http://guardianportal.cloudapp.net/default.aspx
    //http://tinyurl.com/api-create.php
    public class Shortify
    {
        Settings settings;
        public Shortify(Settings settings)
        {
            this.settings = settings;
        }

        public async Task<string> CreateStaticLocateMeURI(string profileID)
        {
            string inpUri = settings.GuardianPortalUri + string.Format("/default.aspx?V=2&{0}={1}", "profileid", profileID);
            string tinyUri = await CreateTinyUri(inpUri);
            return tinyUri;
        }

        public async Task<string> CreateDynamicLocateMeURI(string profileID, string Token)
        {
            //create the uri for the profile landing and locate and shorten it.
            string inpUri = settings.GuardianPortalUri + string.Format("/default.aspx?V=2&{0}={1}&{2}={3}", "pr", profileID, "s", Token);
            string tinyUri = await CreateTinyUri(inpUri);
            return tinyUri;
        }

        public async Task<string> CreateSubscribeBuddyActionURI(string profileId, string buddyUserId, string subscribtionId, string actionCode)
        {
            string inputUri = settings.GuardianPortalUri + string.Format("/SubscribeAction.aspx?V=2&{0}={1}&{2}={3}&{4}={5}&{6}={7}", "pr", profileId, "uid", buddyUserId, "s", subscribtionId, "at", actionCode);
            string tinyUri = await CreateTinyUri(inputUri);
            return tinyUri;
        }

        private async Task<string> CreateTinyUri(string longuri)
        {
            string tinyUri = settings.TinyServiceUri + string.Format("?{0}={1}", "url", longuri);
            var request = WebRequest.Create(tinyUri);
            try
            {
                var response = await request.GetResponseAsync();
                string outputUrl;

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    outputUrl = reader.ReadToEnd();
                }
                if (string.IsNullOrEmpty(outputUrl))
                    return settings.GuardianPortalUri;
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