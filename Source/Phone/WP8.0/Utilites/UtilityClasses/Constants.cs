using System.Windows.Media;

namespace SOS.Phone
{
    public static class Constants
    {
        #region FBUris

        //the correct url - but not working due to the WebBrowser fragment bug
        //private static string FbLoginURL = "https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri=http://www.facebook.com/connect/login_success.html&type=user_agent&display=touch&scope=publish_stream,user_hometown";
        public static string FbLoginUrl = "https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri=http://www.facebook.com/connect/login_success.html&display=touch&scope=publish_actions,user_managed_groups";
        public static string FbGetAccessTokenUrl = "https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri=http://www.facebook.com/connect/login_success.html&client_secret={1}&code={2}";
        public static string FbQueryUserUrl = "https://graph.facebook.com/me?fields=id,name,gender,link,hometown,picture&locale=en_US&access_token={0}";
        public static string FbLoadFriendsUrl = "https://graph.facebook.com/me/friends?access_token={0}";
        //public static string FbLoadGroupsUrl = "https://graph.facebook.com/me/Groups?fields=name,id,owner&access_token={0}";
        public static string FbLoadGroupsUrl = "https://graph.facebook.com/me?fields=id,groups.fields(id,name,owner)&access_token={0}";
        public static string FbPostMessageUrl = "https://graph.facebook.com/me/feed";
        public static string FbPostGroupMessageUrl = "https://graph.facebook.com/{0}/feed?message={1}&access_token={2}";
        public static string FbLogoutUrl = "https://graph.facebook.com/me/permissions?access_token={0}";
        #endregion

        public const string ShortUrlServiceUrl = "http://tinyurl.com/api-create.php?url=";

        public const string VirtualEarthUrl = "http://dev.virtualearth.net/REST/v1/Locations/";

        public const string GMapsUrl = "http://maps.googleapis.com/maps/api/geocode/xml?latlng=";

        //public const string GMapPlacesUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?";

        #region Global Variables

        public static string LocationServiceUrl = Config.GuardianServiceUrl + "LocationService.svc/";
        public static string GeoServiceUrl = Config.GuardianServiceUrl + "GeoUpdate.svc/";
        public static string GroupServiceUrl = Config.GuardianServiceUrl + "GroupService.svc/";
        public static string MembershipServiceUrl = Config.GuardianServiceUrl + "MembershipService.svc/";

        public static string PostMyLocationUrl = GeoServiceUrl + "PostMyLocation";
        public static string PostLocationWithMedia = GeoServiceUrl + "PostLocationWithMedia";
        public static string ReportIncidentUrl = GeoServiceUrl + "ReportIncident";
        public static string StopPostingsUrl = GeoServiceUrl + "StopPostings/{0}/{1}/{2}";
        public static string StopSOSOnlyUrl = GeoServiceUrl + "StopSOSOnly/{0}/{1}/{2}";
        public static string GetUserLocationUrl = LocationServiceUrl + "GetUserLocationArray/{0}/{1}";
        public static string LocateLiveTileUrl = LocationServiceUrl + "GetSOSTrackCount/{0}/{1}";
        public static string LocateBuddiesUrl = LocationServiceUrl + "GetBuddiesToLocateLastLocation/{0}/{1}";

        public const string TrackMePageUrl = "/Pages/TrackMe.xaml";
        public const string MapPageUrl = "/Pages/MapPage.xaml";

        public const string WindowsKeyPressUri = "app://external/";

        public const string TrackPinImage = @"\Assets\Images\trackpin.png";
        public const string TrackPinEndImage = @"\Assets\Images\trackpinend.png";
        public const string DestinationPin = @"\Assets\Images\destpin.png";
        public const string CurrentPin = @"\Assets\Images\pin.png";

        public const uint GeolocatorInterval = (uint)(7.5 * 1000); // In milli secs
        public const int RefreshInterval = 30 * 1000; // In milli secs
        public const int GeolocatorMovementThreshold = 25; // In meters
        public const int IdealAccuray = 200;
        public const int SOSCountdownCounter = 5;

        public static Brush GreenColor = new SolidColorBrush(Color.FromArgb(255, 0x10, 0xAA, 0x1E));//#FF10AA1E - Green
        public static Brush SaffronColor = new SolidColorBrush(Color.FromArgb(255, 0xF9, 0x65, 0x11));//#FFF96511 - Saffron

        public static string GreenColorCode = "#FF10AA1E";
        public static string SaffronColorCode = "#FFF96511";
        public static string WhiteColorCode = "#FFFFFF";

        public static string MessageTemplateText = "I'm in serious trouble. Urgent help needed!";
        public static string SafeMessageText = "I'm safe now. Thanks for the help! I will get in touch with you shortly and share the details.";
        public static string LocateBuddyMessageText = "I'm reaching to help you.";
        public static string LocationDisabledMessageText = "Location Services are disabled either in phone's or Guardian's settings. Do you want to enable it?";
        public static string ReachSupportMessageText = " Please contact app support team at guardianapp@outlook.com";
        public static int MaxNumberOfBuddies = 5;
        public static string GroupModeratorMessage = "Please enter your details needed to join the group";
        public static string GroupAutoOrgMailMessage = "Please enter your valid Org Email Id";
        public static string GroupAutoOrgMailErrorMessage = "* Please provide valid Org Email Id";
        public static string GroupAutoOrgMaillengthErrorMessage = "* Email Id should have atleast two characters before domain name.";
        public static string GroupErrorMessage = "* Please provide required details";
        public static string SyncDataMessage = "You have updated the settings. Do you want to sync the settings with Guardian Server?";

        public enum HealthStatus
        {
            OK,
            Bad,
            Unknown
        }

        public static string DefaultProfileID = "0";

        public static int TimeToDisableValidateButton = 120000;
        public static string CountryCode = "+91";
        //public static int APP_VERSION = 2;
        //public static string state="true";
        public static string CountryName = "India";
        public static int RetryMaxCount = 3;
        public static int RetryMaxCountForAppExit = 1;

        public static string PoliceContact = "100";
        public static string AmbulanceContact = "108";
        public static string FireBrigadeContact = "101";
        public static int MaxPhonedigits = 10;
        
        public static string MaxGetLocalHelpResults = "15";
        public static string MaxTrackMeSearchResults = "25";

        //public static bool hideSyncButton = false;

        #endregion
    }
}
