using GeoReference = SOS.Phone.GeoServiceRef;
using SOS.Phone.LocationServiceRef;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;

namespace SOS.Phone.ServiceWrapper
{
    public static class LocationServiceWrapper
    {

        public static bool IsErrorOccured { get; set; }

        public static async Task<Dictionary<string, string>> GetLocateLiveTileCountAsync()
        {
            IsErrorOccured = false;
            try
            {
                Uri uri = new Uri(string.Format(Constants.LocateLiveTileUrl, Globals.User.UserId, DateTime.Now.Ticks.ToString()));
                WebClient proxy = new WebClient();
                proxy.Headers[HttpRequestHeader.CacheControl] = "no-cache;";
                proxy.Headers["AuthID"] = Globals.User.LiveAuthId;
                string result = await proxy.DownloadStringTask(uri);

                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                Dictionary<string, string> liveCounts = (Dictionary<string, string>)ser.ReadObject(ms);

                return liveCounts;
            }
            catch (Exception)
            {
                IsErrorOccured = true;
            }
            return null;
        }

        public static async Task<bool> GetLocateMembersAsync(Action<ProfileLiteList> target)
        {
            IsErrorOccured = false;
            try
            {
                var uri = new Uri(string.Format(Constants.LocateBuddiesUrl, Globals.User.UserId, DateTime.Now.Ticks.ToString()));
                WebClient proxy = new WebClient();
                proxy.Headers[HttpRequestHeader.CacheControl] = "no-cache;";
                proxy.Headers["AuthID"] = Globals.User.LiveAuthId;
                string result = await proxy.DownloadStringTask(uri);

                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ProfileLiteList));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                ProfileLiteList profileList = (ProfileLiteList)ser.ReadObject(ms);

                target(profileList);
            }
            catch (Exception)
            {
                IsErrorOccured = true;
            }
            return true;
        }

        private static GeoReference.GeoTags GetGeotagsObject(int LocCount)
        {
            if (string.IsNullOrEmpty(Globals.CurrentProfile.SessionToken))
                App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.SessionToken, Guid.NewGuid().ToString());

            GeoReference.GeoTags objGeoTags = new GeoReference.GeoTags();
            objGeoTags.Alt = new ObservableCollection<string>();
            objGeoTags.Lat = new ObservableCollection<string>();
            objGeoTags.Long = new ObservableCollection<string>();
            objGeoTags.IsSOS = new ObservableCollection<bool>();
            objGeoTags.TS = new ObservableCollection<long>();
            objGeoTags.Id = Globals.CurrentProfile.SessionToken;
            objGeoTags.LocCnt = LocCount;
            objGeoTags.PID = Convert.ToInt64(Globals.CurrentProfile.ProfileId);
            objGeoTags.Spd = new ObservableCollection<int>();
            objGeoTags.Accuracy = new ObservableCollection<double>();
            return objGeoTags;
        }

        public static async Task<bool> PostLocationArray(bool isStartPushPin = false)
        {
            try
            {
                int maxCount = Globals.TagList.Count;
                int index = Globals.PostedLocationIndex; //these variables are needed as global values may change in other async parallel calls
                int count = maxCount - index;
                if (maxCount != 0 && count != 0)
                {
                    GeoReference.GeoTags objGeoTags = GetGeotagsObject(count);
                    for (int i = index; i < maxCount; i++)
                    {
                        objGeoTags.Alt.Add(Globals.TagList[i].Alt);
                        objGeoTags.Lat.Add(Globals.TagList[i].Lat.ToString());
                        objGeoTags.Long.Add(Globals.TagList[i].Long.ToString());
                        objGeoTags.IsSOS.Add(Globals.TagList[i].IsSOS);
                        objGeoTags.TS.Add(Globals.TagList[i].TimeStamp.Ticks);
                        objGeoTags.Spd.Add(Globals.TagList[i].Speed);
                        objGeoTags.Accuracy.Add(Globals.TagList[i].Accuracy);
                    }
                    string result = string.Empty;
                    using (MemoryStream mem = new MemoryStream())
                    {
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(GeoReference.GeoTags));
                        ser.WriteObject(mem, objGeoTags);
                        string data = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);

                        WebClient webClient = new WebClient();
                        webClient.Headers["Content-type"] = "application/json;";
                        webClient.Headers["AuthID"] = Globals.User.LiveAuthId;
                        webClient.Encoding = Encoding.UTF8;
                        result = await webClient.UploadStringTask(new Uri(Constants.PostMyLocationUrl), data);
                        if (Globals.AddOrUpdateValue("DeactivateTime", DateTimeOffset.Now))
                        {
                            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
                            settings.Save();
                        }
                    }
                    Globals.PostedLocationIndex = maxCount;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> PostMyLocationArrayAsync()
        {
            if (Globals.IsDataNetworkAvailable && Globals.IsRegisteredUser)
            {
                if (Globals.TagList.Count <= 3 || Globals.IsSOSJustStopped || Globals.TagList[Globals.PostedLocationIndex - 1].Accuracy > 150)
                {
                    bool result = await PostLocationArray(true);
                    if (result && Globals.IsSOSJustStopped)
                        Globals.IsSOSJustStopped = false;
                    return result;
                }
                else
                {
                    DateTime dateLastsynced = Globals.TagList[Globals.PostedLocationIndex].TimeStamp;
                    if (dateLastsynced.AddSeconds(30) <= DateTime.Now)
                    {
                        return await PostLocationArray();
                    }
                    else
                        return true;
                }
            }
            else
                return false;
        }


        public static async Task<bool> PostMyLocationAsync(GeoCoordinate pos, Stream photo, bool isReportIncident = false)
        {
            try
            {
                if (string.IsNullOrEmpty(Globals.CurrentProfile.SessionToken))
                    App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.SessionToken, Guid.NewGuid().ToString());

                if (Globals.IsDataNetworkAvailable)
                {
                    if (Globals.IsRegisteredUser)
                    {
                        if (!(Globals.CurrentProfile.IsSOSOn || Globals.CurrentProfile.IsTrackingOn || isReportIncident))
                            if (!(Globals.CurrentProfile.IsTrackingOn && Globals.CurrentProfile.PostLocationConsent))
                                return false;

                        byte[] b = null;

                        if (photo != null)
                        {
                            using (BinaryReader br = new BinaryReader(Phone.Utilites.PhotoResizer.ReduceSize(photo)))
                            {
                                b = br.ReadBytes((Int32)photo.Length);
                            }
                        }

                        GeoServiceRef.GeoTag tag = new GeoServiceRef.GeoTag()
                        {
                            ProfileID = Convert.ToInt64(Globals.CurrentProfile.ProfileId),
                            SessionID = Globals.CurrentProfile.SessionToken,
                            Lat = (pos == null) ? null : pos.Latitude.ToString(),
                            Long = (pos == null) ? null : pos.Longitude.ToString(),
                            Alt = (pos == null) ? null : pos.Altitude.ToString(),
                            Speed = (pos == null || double.IsNaN(pos.Speed)) ? 0 : Convert.ToInt32(pos.Speed),
                            Accuracy = (pos == null || double.IsNaN(pos.HorizontalAccuracy)) ? 0 : pos.HorizontalAccuracy,
                            IsSOS = Globals.CurrentProfile.IsSOSOn,
                            GeoDirection = "1",//"TODO"
                            TimeStamp = DateTime.Now.Ticks,
                            MediaContent = b
                        };
                        string result = string.Empty;
                        using (MemoryStream mem = new MemoryStream())
                        {
                            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(GeoServiceRef.GeoTag));
                            ser.WriteObject(mem, tag);
                            string data = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);

                            WebClient webClient = new WebClient();
                            webClient.Headers["Content-type"] = "application/json;";
                            webClient.Headers["AuthID"] = Globals.User.LiveAuthId;
                            webClient.Encoding = Encoding.UTF8;
                            result = await webClient.UploadStringTask(new Uri(Constants.PostLocationWithMedia), data);
                        }
                    }
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> StopPostingsAsync(string TokenId)
        {
            try
            {
                if (Globals.IsDataNetworkAvailable)
                {
                    if (Globals.IsRegisteredUser)
                    {
                        await PostLocationArray();
                        WebClient webClient = new WebClient();
                        webClient.Headers["AuthID"] = Globals.User.LiveAuthId;
                        webClient.Headers[HttpRequestHeader.CacheControl] = "no-cache;";
                        string result = await webClient.DownloadStringTask(new Uri(string.Format(Constants.StopPostingsUrl, Globals.User.CurrentProfileId, TokenId, DateTime.Now.Ticks.ToString())));
                    }
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> StopSOSOnlyAsync(string TokenId)
        {
            try
            {
                if (Globals.IsDataNetworkAvailable)
                {
                    if (Globals.IsRegisteredUser)
                    {
                        await PostLocationArray();
                        WebClient webClient = new WebClient();
                        webClient.Headers["AuthID"] = Globals.User.LiveAuthId;
                        webClient.Headers[HttpRequestHeader.CacheControl] = "no-cache;";
                        string result = await webClient.DownloadStringTask(new Uri(string.Format(Constants.StopSOSOnlyUrl, Globals.User.CurrentProfileId, TokenId, DateTime.Now.Ticks.ToString())));
                    }
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static async Task<string> ReportIncident(GeoServiceRef.IncidentTag tag)
        {
            string result = string.Empty;

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(GeoServiceRef.IncidentTag));
            using (MemoryStream mem = new MemoryStream())
            {
                ser.WriteObject(mem, tag);
                string data = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);

                WebClient webClient = new WebClient();
                webClient.Headers["Content-type"] = "application/json;";
                webClient.Headers["AuthID"] = Globals.User.LiveAuthId;
                webClient.Encoding = Encoding.UTF8;
                result = await webClient.UploadStringTask(new Uri(Constants.ReportIncidentUrl), data);
            }
            return result;
        }
    }
}
