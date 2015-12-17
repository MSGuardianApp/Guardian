using Microsoft.Phone.Info;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.UserData;
using SOS.Phone.Utilites.HelperEntities;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using Windows.Devices.Geolocation;
//using Windows.Foundation;

namespace SOS.Phone
{
    public static class Utility
    {
        #region Tasks
        public static bool SendSMS(SMSMessage sms)
        {
            SmsComposeTask smsComposeTask = new SmsComposeTask();
            smsComposeTask.To = sms.PhoneNumbers;
            smsComposeTask.Body = sms.Message;
            smsComposeTask.Show();

            return true;
        }

        public static bool SendEmail(EmailMessage email)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.To = email.FromEmailAddress;
            emailComposeTask.To = email.ToEmailAddress;
            emailComposeTask.Body = email.HtmlBody;
            emailComposeTask.Subject = email.Subject;
            emailComposeTask.Show();

            return true;
        }

        public static bool InitiateCall(Callee defaultCallee)
        {
            PhoneCallTask phoneCallTask = new PhoneCallTask();
            phoneCallTask.DisplayName = defaultCallee.DisplayName;
            phoneCallTask.PhoneNumber = defaultCallee.PhoneNumber;
            phoneCallTask.Show();

            return true;
        }

        public static string GetBuddyNumbers()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var buddy in App.MyBuddies.Buddies)
                sb.Append(buddy.PhoneNumber + "; ");

            return sb.ToString();
        }

        #endregion

        #region FBMethods

        public static void PostMessagetoFacebook(string grpId, string strAccressToken, string message)
        {

            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(string.Format(Constants.FbPostGroupMessageUrl, grpId, message, strAccressToken)));
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";
            webRequest.BeginGetRequestStream(
                r =>
                {
                    HttpWebRequest request1 = (HttpWebRequest)r.AsyncState;
                    Stream postStream = request1.EndGetRequestStream(r);

                    postStream.Close();
                    request1.BeginGetResponse(
                    s =>
                    {
                        HttpWebRequest request2 = (HttpWebRequest)s.AsyncState;
                        HttpWebResponse response = (HttpWebResponse)request2.EndGetResponse(s);
                        Stream streamResponse = response.GetResponseStream();
                        StreamReader streamReader = new StreamReader(streamResponse);
                        string response2 = streamReader.ReadToEnd();
                        streamResponse.Close();
                        streamReader.Close();
                        response.Close();
                    },
                request1);
                }, webRequest
                );

        }

        public static Uri GetLoadFriendsUri(string strAccressToken)
        {
            return (new Uri(string.Format(Constants.FbLoadFriendsUrl, strAccressToken), UriKind.Absolute));
        }

        public static Uri GetLoadGroupsUri(string strAccressToken)
        {
            return (new Uri(string.Format(Constants.FbLoadGroupsUrl, strAccressToken), UriKind.Absolute));
        }

        public static Uri GetPostMessageUri()
        {
            return (new Uri(Constants.FbPostMessageUrl, UriKind.Absolute));
        }
        public static Uri GetQueryUserUri(string strAccressToken)
        {
            return (new Uri(string.Format(Constants.FbQueryUserUrl, strAccressToken), UriKind.Absolute));
        }
        public static Uri GetLoginUri()
        {
            return (new Uri(string.Format(Constants.FbLoginUrl, Config.FbAppId), UriKind.Absolute));
        }
        public static Uri GetLogoutUri(string access_token)
        {
            return (new Uri(string.Format(Constants.FbLogoutUrl, access_token), UriKind.Absolute));
        }

        public static Uri GetTokenLoadUri(string strCode)
        {
            return (new Uri(string.Format(Constants.FbGetAccessTokenUrl, Config.FbAppId, Config.FbAppSecret, strCode), UriKind.Absolute));
        }

        #endregion

        public static string GetWindowsLiveAnonymousID()
        {
            string result = string.Empty;
            int ANIDLength = 32;
            int ANIDOffset = 2;
            object anid;
            if (UserExtendedProperties.TryGetValue("ANID", out anid))
            {
                if (anid != null && anid.ToString().Length >= (ANIDLength + ANIDOffset))
                {
                    result = anid.ToString().Substring(ANIDOffset, ANIDLength);
                }
            }

            return result;
        }

        public static string GetDeviceUniqueID()
        {
            byte[] result = null;
            object uniqueId;
            if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
                result = (byte[])uniqueId;

            return Convert.ToBase64String(result, 0, result.Length);
        }

        public static T[] DeserializeJson<T>(String json)
        {
            var serializer = new DataContractJsonSerializer(typeof(T[]));

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return (T[])serializer.ReadObject(ms);
            }
        }

        public static async Task<GeoCoordinate> GetCurrentLocationAsync()
        {
            if (!Globals.IsLocationConsentEnabled)
                return null;

            Globals.IsPhoneLocationServiceEnabled = true;
            Geoposition geoposition = null;
            try
            {
                Geolocator geolocator = new Geolocator();
                geolocator.DesiredAccuracy = PositionAccuracy.High;
                Windows.Foundation.IAsyncOperation<Geoposition> locationTask = null;

                try
                {
                    locationTask = geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(5));
                    geoposition = await locationTask;
                }
                finally
                {
                    if (locationTask != null)
                    {
                        if (locationTask.Status == Windows.Foundation.AsyncStatus.Started)
                            locationTask.Cancel();

                        locationTask.Close();
                    }
                }
                geolocator = null;

                Globals.RecentLocation.Coordinate = geoposition.Coordinate.ToGeoCoordinate();
                Globals.RecentLocation.CapturedTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                    Globals.IsPhoneLocationServiceEnabled = false;
            }
            return Globals.IsPhoneLocationServiceEnabled ? Globals.RecentLocation.Coordinate : null;
        }

        public static string GetAddressFromGXml(string xmlAddress, string latitude, string longitude)
        {
            try
            {
                string address = string.Empty;
                XDocument xDoc = XDocument.Parse(xmlAddress);
                var xmlAddr = (from c in xDoc.Descendants("formatted_address")
                               select c).FirstOrDefault();
                if (xmlAddr != null)
                {
                    address = xmlAddr.Value;
                }
                else
                {
                    address = "Lat: " + latitude + "; " + "Long: " + longitude;
                }
                Globals.RecentLocation.Address = address;
                Globals.RecentLocation.AddressCapturedTime = DateTime.Now;

                return address;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GetAddressFromBXml(string xmlAddress, string latitude, string longitude)
        {
            try
            {
                string address = string.Empty;
                XDocument xDoc = XDocument.Parse(xmlAddress);
                XNamespace xn = "http://schemas.microsoft.com/search/local/ws/rest/v1";

                var xmlAddr = (from c in xDoc.Descendants(xn + "Address")
                               select c).FirstOrDefault();
                if (xmlAddr != null)
                {
                    string szConfidence = "LOW";
                    var xConfidence = (from c in xDoc.Descendants(xn + "Confidence")
                                       select c).FirstOrDefault();
                    if (xConfidence != null)
                    {
                        szConfidence = xConfidence.Value;
                    }
                    if ((szConfidence.ToUpper() == "HIGH") && xmlAddr.Descendants(xn + "FormattedAddress") != null && xmlAddr.Descendants(xn + "FormattedAddress").Count() > 0)
                    {
                        address = xmlAddr.Descendants(xn + "FormattedAddress").First().Value;
                    }
                    else if (xmlAddr.Descendants(xn + "AddressLine") != null && xmlAddr.Descendants(xn + "AddressLine").Count() > 0)
                    {
                        address = xmlAddr.Descendants(xn + "AddressLine").First().Value;
                        if (xmlAddr.Descendants(xn + "Locality") != null && xmlAddr.Descendants(xn + "Locality").Count() > 0)
                        {
                            address += ", " + xmlAddr.Descendants(xn + "Locality").First().Value;
                        }
                        if (xmlAddr.Descendants(xn + "AdminDistrict2") != null && xmlAddr.Descendants(xn + "AdminDistrict2").Count() > 0)
                        {
                            address += ", " + xmlAddr.Descendants(xn + "AdminDistrict2").First().Value;
                        }
                        if (xmlAddr.Descendants(xn + "AdminDistrict") != null && xmlAddr.Descendants(xn + "AdminDistrict").Count() > 0)
                        {
                            address += ", " + xmlAddr.Descendants(xn + "AdminDistrict").First().Value;
                        }
                        if (xmlAddr.Descendants(xn + "CountryRegion") != null && xmlAddr.Descendants(xn + "CountryRegion").Count() > 0)
                        {
                            address += ", " + xmlAddr.Descendants(xn + "CountryRegion").First().Value;
                        }
                        if (xmlAddr.Descendants(xn + "PostalCode") != null && xmlAddr.Descendants(xn + "PostalCode").Count() > 0)
                        {
                            address += ", " + xmlAddr.Descendants(xn + "PostalCode").First().Value;
                        }
                    }
                }
                else
                {
                    address = "Lat: " + latitude + "; " + "Long: " + longitude;
                }
                Globals.RecentLocation.Address = address;
                Globals.RecentLocation.AddressCapturedTime = DateTime.Now;

                return address;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static async Task<string> GetBingAddress(string latitude, string longitude)
        {
            try
            {
                WebClient proxy = new WebClient();
                var bingMapsKey = Config.BingMapsKey;
                var veServiceUri = Constants.VirtualEarthUrl + latitude + "," + longitude + "?o=xml&key=" + bingMapsKey;
                //TODO:BUG-Sporadic : Not returning correct data for address.
                string address = await proxy.DownloadStringTask(new Uri(veServiceUri));
                return GetAddressFromBXml(address, latitude, longitude);
            }
            catch (Exception ex)
            {
                return "Lat: " + latitude + "; " + "Long: " + longitude;
            }
        }

        public static async Task<string> GetAddress(string latitude, string longitude)
        {
            try
            {
                WebClient proxy = new WebClient();
                var gServiceUri = Constants.GMapsUrl + latitude.Trim() + "," + longitude.Trim() + "&sensor=false";
                string address = await proxy.DownloadStringTask(new Uri(gServiceUri));

                return Utility.GetAddressFromGXml(address, latitude, longitude);
            }
            catch (Exception ex)
            {
                return "Lat: " + latitude + "; " + "Long: " + longitude;
            }
        }

        public static async Task<string> GetDistressMessage()
        {
            string strMessage = Constants.MessageTemplateText;

            if (Globals.CurrentProfile.MessageTemplate.GetValue() != string.Empty)
                strMessage = Globals.CurrentProfile.MessageTemplate.Trim();

            try
            {
                GeoCoordinate gc = null;
                if (Globals.IsLocationConsentEnabled)
                    gc = await GetLocationQuick();

                if (gc != null)
                {
                    string location = await Utility.GetAddressQuick(gc);

                    if (location != string.Empty)
                        strMessage += " I'm @ " + location;

                    strMessage += " Track me: ";
                    strMessage += await Utility.GetTinyUrl(gc);
                }
            }
            catch
            {
                //Location is switched off
            }
            return strMessage;
        }

        public static async Task<string> GetTrackUrlMessage()
        {
            string strMessage = string.Empty;
            try
            {
                GeoCoordinate gc = null;
                if (Globals.IsLocationConsentEnabled)
                    gc = await GetLocationQuick();

                if (gc != null)
                {
                    string location = await Utility.GetAddressQuick(gc);

                    if (location != string.Empty)
                        strMessage += "I'm currently @ " + location;

                    strMessage += " Track me: ";
                    strMessage += await Utility.GetTinyUrl(gc);
                }
            }
            catch
            {
                //Location is switched off
            }
            return strMessage;
        }

        public static string GetPlainPhoneNumber(string phoneNumber)
        {
            phoneNumber = phoneNumber.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Replace(".", "");
            Regex r = new Regex(@"[a-zA-Z]");
            Match m = r.Match(phoneNumber);
            if (m.Success)
            {
                phoneNumber = phoneNumber.Substring(0, m.Index);
            }
            if (Constants.MaxPhonedigits != 10)
            {
                if (phoneNumber.StartsWith("00" + Constants.CountryCode.Substring(1)))
                    phoneNumber = phoneNumber.Replace("00" + Constants.CountryCode.Substring(1), "");
                else if (phoneNumber.StartsWith(Constants.CountryCode))
                    phoneNumber = phoneNumber.Replace(Constants.CountryCode, "");
                else if (phoneNumber.Contains("+"))
                    phoneNumber = phoneNumber.Replace("+", "");
                phoneNumber = phoneNumber.Length <= Constants.MaxPhonedigits ? phoneNumber : phoneNumber.Substring(0, Constants.MaxPhonedigits);
            }
            else
                phoneNumber = phoneNumber.Length <= Constants.MaxPhonedigits ? phoneNumber : phoneNumber.Substring(phoneNumber.Length - Constants.MaxPhonedigits, Constants.MaxPhonedigits);
            return phoneNumber;
        }

        public static string GetPlainIsdCode(string phoneNumber)
        {
            string isdCode = string.Empty;
            phoneNumber = phoneNumber.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Replace(".", "");
            Regex r = new Regex(@"[a-zA-Z]");
            Match m = r.Match(phoneNumber);
            if (m.Success)
            {
                phoneNumber = phoneNumber.Substring(0, m.Index);
            }
            if (Constants.MaxPhonedigits != 10)
            {
                isdCode = Constants.CountryCode;
            }
            else
                isdCode = phoneNumber.Length <= Constants.MaxPhonedigits ? Constants.CountryCode : phoneNumber.Substring(0, phoneNumber.Length - Constants.MaxPhonedigits);

            if (isdCode.Length == 1 && isdCode == "0")
                isdCode = Constants.CountryCode;
            else if (isdCode.StartsWith("00"))
                isdCode = isdCode.StartsWith("00") ? isdCode.Replace("00", "+") : isdCode;
            return isdCode;
        }

        public static async Task<GeoCoordinate> GetLocationQuick()
        {
            return (Globals.RecentLocation.Coordinate != null
                && Globals.RecentLocation.CapturedTime > DateTime.Now.AddMinutes(-3)) ?
                Globals.RecentLocation.Coordinate : await Utility.GetCurrentLocationAsync();
        }

        public static async Task<string> GetAddressQuick(GeoCoordinate gc = null)
        {
            if (Globals.RecentLocation.Address != string.Empty
                && Globals.RecentLocation.AddressCapturedTime > DateTime.Now.AddMinutes(-5))
            {
                return Globals.RecentLocation.Address;
            }
            else
            {
                if (gc == null)
                    gc = await GetLocationQuick();

                return await Utility.GetCombOfBingAndGMapsAddress(gc.Latitude.ToString(), gc.Longitude.ToString());
            }
        }

        public static async Task<string> GetCombOfBingAndGMapsAddress(string latitude, string longitude)
        {
            string finalAddress;
            string bingAddress = await Utility.GetBingAddress(latitude, longitude);
            if (string.IsNullOrEmpty(bingAddress) || bingAddress.StartsWith("Lat:"))
                finalAddress = await Utility.GetAddress(latitude, longitude);
            else
                finalAddress = bingAddress;
            if (string.IsNullOrEmpty(finalAddress))
                finalAddress = "Lat: " + latitude + "; " + "Long: " + longitude;
            return finalAddress;
        }

        public static async Task<GeoCoordinate> GetRecentLocation()
        {
            var gc = await Utility.GetCurrentLocationAsync();

            if (gc == null && Globals.RecentLocation.Coordinate != null
                && Globals.RecentLocation.CapturedTime > DateTime.Now.AddMinutes(-5))
                gc = Globals.RecentLocation.Coordinate;

            return gc;
        }

        public static async Task<string> GetRecentAddress(GeoCoordinate gc = null)
        {
            string address = "Unable to get location details...";

            if (gc == null)
                gc = await GetRecentLocation();

            if (gc == null)
                address = Globals.RecentLocation.Address;
            else
                address = await Utility.GetCombOfBingAndGMapsAddress(gc.Latitude.ToString(), gc.Longitude.ToString());

            return address;
        }


        public static async Task<string> GetTinyUrl(GeoCoordinate gc)
        {
            //https://<domain>/default.aspx?V=2&t=SKe/CBsBqQ3mMrXFT4s2UIk6ADFMpCqYljOpw00KNmkvVt8LnTxgiozDf9qIYF0MMSGsfzGnb9P8SEgPKmqo9w==&ut=635651427039540000&d=635651229039990000&l=17.4315747&g=78.3433867

            string url = Config.GuardianPortalUrl + @"default.aspx?V=2&";

            if (Globals.IsRegisteredUser && !Globals.IsDataNetworkAvailable)
            {
                string encryptedParameters = EncryptAndDecrypt.Encrypt(string.Format("p={0}&s={1}&f={2}", Globals.User.CurrentProfileId, Globals.CurrentProfile.SessionToken, Globals.CurrentProfile.IsSOSOn));
                string encodeEncryptedParams = EncryptAndDecrypt.EncodeString(encryptedParameters);

                url += string.Format("t={0}&ut={1}&", encodeEncryptedParams, DateTime.UtcNow.Ticks.ToString());
            }

            url += string.Format("d={0}&l={1}&g={2}", DateTime.Now.Ticks.ToString(), gc.Latitude, gc.Longitude);

            if (Globals.IsRegisteredUser && Globals.IsDataNetworkAvailable)
            {
                url = string.Format("pr={0}&s={1}", Globals.User.CurrentProfileId, Globals.CurrentProfile.SessionToken);
            }

            return await Utility.GetShortUrl(url);
        }

        #region Tracking Pushpin Drawing

        public static MapOverlay DrawPushpin(MapLayer pushpinMapLayer, GeoCoordinate coord, bool isCurrentUserLocation, bool isDestination = false)
        {
            //Creating a Grid element.
            Grid MyGrid = new Grid();
            MyGrid.RowDefinitions.Add(new RowDefinition());
            MyGrid.RowDefinitions.Add(new RowDefinition());
            MyGrid.Background = new SolidColorBrush(Colors.Transparent);
            Image img = new Image();
            img.Source = new BitmapImage(new Uri(isDestination ? Constants.DestinationPin : (isCurrentUserLocation ? Constants.TrackPinEndImage : Constants.TrackPinImage), UriKind.Relative));
            img.Margin = new Thickness(-15, -45, 0, 0);
            MyGrid.Children.Add(img);

            //Creating a MapOverlay and adding the Pushpin to it.
            MapOverlay MyOverlay = new MapOverlay();
            MyOverlay.Content = MyGrid;
            MyOverlay.GeoCoordinate = coord;
            MyOverlay.PositionOrigin = new Point(0, 0.5);

            //Add the MapOverlay containing the pushpin to the MapLayer
            pushpinMapLayer.Add(MyOverlay);
            return MyOverlay;
        }

        private static Grid CreatePushpinObject()
        {
            //Creating a Grid element.
            Grid MyGrid = new Grid();
            MyGrid.RowDefinitions.Add(new RowDefinition());
            MyGrid.RowDefinitions.Add(new RowDefinition());
            MyGrid.Background = new SolidColorBrush(Colors.Transparent);

            //Creating a Rectangle
            Rectangle MyRectangle = new Rectangle();
            MyRectangle.Fill = new SolidColorBrush(Colors.Black);
            MyRectangle.Height = 20;
            MyRectangle.Width = 20;
            MyRectangle.SetValue(Grid.RowProperty, 0);
            MyRectangle.SetValue(Grid.ColumnProperty, 0);


            //Adding the Rectangle to the Grid
            MyGrid.Children.Add(MyRectangle);

            //Creating a Polygon
            Polygon MyPolygon = new Polygon();
            MyPolygon.Points.Add(new Point(2, 0));
            MyPolygon.Points.Add(new Point(22, 0));
            MyPolygon.Points.Add(new Point(2, 40));
            MyPolygon.Stroke = new SolidColorBrush(Colors.Black);
            MyPolygon.Fill = new SolidColorBrush(Colors.Black);
            MyPolygon.SetValue(Grid.RowProperty, 1);
            MyPolygon.SetValue(Grid.ColumnProperty, 0);

            //Adding the Polygon to the Grid
            MyGrid.Children.Add(MyPolygon);
            return MyGrid;
        }

        public async static Task<string> GetShortUrl(string originalUrl)
        {
            string result = originalUrl;
            try
            {
                WebClient wc = new WebClient();
                wc.Headers[HttpRequestHeader.CacheControl] = "no-cache;";
                result = await wc.DownloadStringTask(new Uri(Constants.ShortUrlServiceUrl + originalUrl));
            }
            catch
            {
                //Do nothing
            }
            return result;
        }
        #endregion

        public static string GetPhoneNumber(IEnumerable<ContactPhoneNumber> numbers)
        {
            foreach (ContactPhoneNumber p in numbers)
            {
                if (p.PhoneNumber.Trim() != string.Empty)
                    return p.PhoneNumber;
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prevLocation">Start GeoCoordinate</param>
        /// <param name="currentLocation">End GeoCoordinate</param>
        /// <param name="time">Travel time in secs</param>
        /// <param name="InKMPH">In Km per hr else, in meters per sec</param>
        /// <returns></returns>
        public static double CalculateSpeed(GeoCoordinate prevLocation, GeoCoordinate currentLocation, double time, bool inKMPH = true)
        {
            var distance = prevLocation != null ? currentLocation.GetDistanceTo(prevLocation) : 0;
            double speed = time > 0 ? (double)(distance / time) : 0;

            if (inKMPH) speed *= 3.6;

            return speed;
        }


        public static string BingSearchUrl(string type, string location, string MaxResults, bool IsLocal = true)
        {
            StringBuilder requestUrl = new StringBuilder();
            requestUrl.Append("https://dev.virtualearth.net/services/v1/SearchService/SearchService.asmx/Search2?");
            requestUrl.Append("count=" + MaxResults);
            requestUrl.Append("&startingIndex=" + 0);
            requestUrl.Append("&mapBounds=");
            requestUrl.Append("&locationcoordinates=");
            if (IsLocal)
                requestUrl.Append("\"" + Globals.RecentLocation.Coordinate.Latitude.ToString() + ", " + Globals.RecentLocation.Coordinate.Longitude.ToString() + "\"");
            requestUrl.Append("&entityType=\"Business\"");
            requestUrl.Append("&sortorder=");
            requestUrl.Append("&query=");
            requestUrl.Append("&location=");
            if (!string.IsNullOrEmpty(location))
                requestUrl.Append("\"" + location + "\"");
            requestUrl.Append("&keyword=");
            if (!string.IsNullOrEmpty(type)) requestUrl.Append("\"" + type + "\"");
            requestUrl.Append("&jsonso=r229");
            requestUrl.Append("&jsonp=microsoftMapsNetworkCallback");
            requestUrl.Append("&culture=\"en-us\"");
            requestUrl.Append("&token=" + Config.BingMapsKey);
            return requestUrl.ToString();
        }


    }
}
