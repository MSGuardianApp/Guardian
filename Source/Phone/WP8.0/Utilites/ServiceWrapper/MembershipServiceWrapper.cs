using SOS.Phone.MembershipServiceRef;
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SOS.Phone
{
    public static class MembershipServiceWrapper
    {
        public static bool IsErrorOccured { get; set; }

        public static async Task<ProfileList> GetProfilesForLiveId(string liveAuthId = "")
        {
            string serviceUrl = Constants.MembershipServiceUrl + "GetProfilesForLiveId";

            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.CacheControl] = "no-cache;";
            wc.Headers["AuthID"] = liveAuthId == "" ? Globals.User.LiveAuthId : liveAuthId;

            string result = await wc.DownloadStringTask(new Uri(serviceUrl));

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ProfileList));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var profiles = (ProfileList)ser.ReadObject(ms);

            return profiles;
        }

        public static async void ValidateMobileNumberAsync(string userName, string liveId, string countryCode, string mobileNumber, string enterpriseEmail)
        {
            PhoneValidation validationInfo = new PhoneValidation();
            validationInfo.AuthenticatedLiveID = liveId;
            validationInfo.Name = userName;
            validationInfo.PhoneNumber = countryCode + mobileNumber;
            validationInfo.RegionCode = countryCode;
            validationInfo.EnterpriseEmailID = enterpriseEmail;
            string serviceUrl = Constants.MembershipServiceUrl + "CreatePhoneValidator";

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(PhoneValidation));
            using (MemoryStream ms = new MemoryStream())
            {
                ser.WriteObject(ms, validationInfo);
                string data = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);

                WebClient wc = new WebClient();
                wc.Headers["Content-type"] = "application/json";
                wc.Headers["AuthID"] = Globals.User.LiveAuthId;
                wc.Encoding = Encoding.UTF8;
                string result = await wc.UploadStringTask(new Uri(serviceUrl), data);
            }
        }


        public static async Task<Profile> CreateProfile(string userName, string liveId, string countryCode, string mobileNumber, string securityCode, string enterpriseEmail, string enterpriseSecurityToken, string AccessToken, string RefreshToken)
        {
            Profile profile = App.MyProfiles.GetNewProfileObject(userName, liveId, countryCode, mobileNumber, securityCode, enterpriseEmail, enterpriseSecurityToken, AccessToken, RefreshToken);
            string profileString = string.Empty;
            string serviceUrl = Constants.MembershipServiceUrl + "CreateProfile";

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Profile));
            using (MemoryStream ms = new MemoryStream())
            {
                ser.WriteObject(ms, profile);
                string data = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);

                WebClient wc = new WebClient();
                wc.Headers["Content-type"] = "application/json";
                wc.Headers["AuthID"] = Globals.User.LiveAuthId;
                wc.Encoding = Encoding.UTF8;
                profileString = await wc.UploadStringTask(new Uri(serviceUrl), data);
            }

            if (profileString == string.Empty)
                return null;
            else
            {
                DataContractJsonSerializer returnProfile = new DataContractJsonSerializer(typeof(Profile));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(profileString));
                Profile serverProfile = (Profile)returnProfile.ReadObject(ms);

                return serverProfile;
            }
        }

        public static async Task<HealthUpdate> CheckPendingUpdates(string profileID, DateTime lastSyncDateTime)
        {
            string serviceUrl = Constants.MembershipServiceUrl + "CheckPendingUpdates/" + profileID + "/" + lastSyncDateTime.Ticks.ToString() + "/" + DateTime.Now.Ticks.ToString();

            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.CacheControl] = "no-cache;";
            wc.Headers["AuthID"] = Globals.User.LiveAuthId;
            string result = await wc.DownloadStringTask(new Uri(serviceUrl));

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(HealthUpdate));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var healthUpdate = (HealthUpdate)ser.ReadObject(ms);

            return healthUpdate;
        }

        public static async Task<Profile> UpdateProfile(Profile profObject)
        {
            string serviceUrl = Constants.MembershipServiceUrl + "UpdateProfile";

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Profile));
            using (MemoryStream ms = new MemoryStream())
            {
                ser.WriteObject(ms, profObject);
                string data = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);

                WebClient wc = new WebClient();
                wc.Headers["Content-type"] = "application/json";
                wc.Headers["AuthID"] = Globals.User.LiveAuthId;
                wc.Encoding = Encoding.UTF8;
                string serverProfileString = await wc.UploadStringTask(new Uri(serviceUrl), data);

                DataContractJsonSerializer profSer = new DataContractJsonSerializer(typeof(Profile));
                MemoryStream mem = new MemoryStream(Encoding.UTF8.GetBytes(serverProfileString));
                var serverProfile = (Profile)profSer.ReadObject(mem);

                return serverProfile;
            }
        }

        public static async Task<Profile> UpdateProfilePhone(Profile profObject)
        {
            string serviceUrl = Constants.MembershipServiceUrl + "UpdateProfilePhone";

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Profile));
            using (MemoryStream ms = new MemoryStream())
            {
                ser.WriteObject(ms, profObject);
                string data = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);

                WebClient wc = new WebClient();
                wc.Headers["Content-type"] = "application/json";
                wc.Headers["AuthID"] = Globals.User.LiveAuthId;
                wc.Encoding = Encoding.UTF8;
                string serverProfileString = await wc.UploadStringTask(new Uri(serviceUrl), data);

                DataContractJsonSerializer profSer = new DataContractJsonSerializer(typeof(Profile));
                MemoryStream mem = new MemoryStream(Encoding.UTF8.GetBytes(serverProfileString));
                var serverProfile = (Profile)profSer.ReadObject(mem);

                return serverProfile;
            }
        }

        public static async Task<bool> UnRegisterUser()
        {
            string serviceUrl = Constants.MembershipServiceUrl + "UnRegisterUser";

            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.CacheControl] = "no-cache;";
            wc.Headers["AuthID"] = Globals.User.LiveAuthId;
            string result = await wc.DownloadStringTask(new Uri(serviceUrl));

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ResultInfo));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var resultObject = (ResultInfo)ser.ReadObject(ms);

            if (resultObject.ResultType == ResultTypeEnum.AuthError)
                return false;

            return true;
        }

        public static async Task<SOS.Phone.GroupServiceRef.GroupList> GetListOfGroups(string searchKey)
        {
            string result = "";
            if (searchKey == String.Empty)
            {
                searchKey = "all";
            }
            string serviceUrl = Constants.GroupServiceUrl + "GetListOfGroups/" + searchKey;
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.CacheControl] = "no-cache;";
            wc.Headers["AuthID"] = Globals.User.LiveAuthId;
            result = await wc.DownloadStringTask(new Uri(serviceUrl));

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(SOS.Phone.GroupServiceRef.GroupList));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            SOS.Phone.GroupServiceRef.GroupList lstGrpItems = (SOS.Phone.GroupServiceRef.GroupList)ser.ReadObject(ms);

            return lstGrpItems;
        }
    }
}
