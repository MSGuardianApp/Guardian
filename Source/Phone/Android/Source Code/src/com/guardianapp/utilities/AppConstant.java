package com.guardianapp.utilities;


import java.io.BufferedReader;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.net.HttpURLConnection;
import java.net.URL;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Collections;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.Locale;
import java.util.TimeZone;
import java.util.Timer;
import java.util.TimerTask;
import java.util.UUID;
import java.util.concurrent.TimeUnit;
import java.util.regex.Pattern;

import org.json.JSONException;
import org.xmlpull.v1.XmlPullParser;
import org.xmlpull.v1.XmlPullParserException;
import org.xmlpull.v1.XmlPullParserFactory;

import android.app.Activity;
import android.app.ActivityManager;
import android.app.ActivityManager.RunningServiceInfo;
import android.app.AlarmManager;
import android.app.AlertDialog;
import android.app.PendingIntent;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.AssetManager;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Rect;
import android.net.ConnectivityManager;
import android.net.Uri;
import android.os.Build;
import android.telephony.SmsManager;
import android.telephony.TelephonyManager;
import android.util.Log;
import android.util.Patterns;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.TextView;

import com.google.i18n.phonenumbers.NumberParseException;
import com.google.i18n.phonenumbers.PhoneNumberUtil;
import com.google.i18n.phonenumbers.Phonenumber.PhoneNumber;
import com.guardianapp.R;
import com.guardianapp.database.DBQueries;
import com.guardianapp.helpercomponents.AlarmManagerBroadcastReceiver;
import com.guardianapp.helpercomponents.NotificationServiceManager;
import com.guardianapp.model.AscGroups;
import com.guardianapp.model.CountryCodes;
import com.guardianapp.model.LocalProfile;
import com.guardianapp.model.LocateBuddies;
import com.guardianapp.model.MyBuddies;
import com.guardianapp.model.User;
import com.guardianapp.services.GPSTracker;
import com.guardianapp.services.TinyUrl;
import com.guardianapp.ui.SOSActivity;
import com.guardianapp.webservicecomponents.MakeHTTPServices;

public class AppConstant {
	//The following are the Guardian Dev URL....
	public static String GuardianServiceUrl = "https://guardiansrvc-dev.cloudapp.net/";
	public static String guardianPortalLink = "https://guardianportal-dev.cloudapp.net/privacy.htm";
	public static String guardianPortalUrl = "https://guardianportal-dev.cloudapp.net/";
	public static String GuardianV1ServiceUrl = "https://guardiansrvc-dev.cloudapp.net/";
	public static String GuardianV1PortalUrl = "https://guardianportal-dev.cloudapp.net/";
	public static String CLIENT_ID = "000000004010A627";
	public static final String GUARDIAN_CORE_TEAM_MAIL = "v-dhmadd@microsoft.com";

	//The following are the Guardian Production details....uncomment when uploading build to store
	/*public static String GuardianServiceUrl = "https://guardiansrvc.cloudapp.net/";
	public static String guardianPortalLink = "https://guardianportal.cloudapp.net/privacy.htm";
	public static String guardianPortalUrl = "https://guardianportal.cloudapp.net/";
	public static String GuardianV1ServiceUrl = "https://guardiansrvc-dev.cloudapp.net/";
	public static String GuardianV1PortalUrl = "https://guardianportal-dev.cloudapp.net/";
	public static String CLIENT_ID = "0000000044105B2B";
	public static final String GUARDIAN_CORE_TEAM_MAIL = "guardianapp@outlook.com";
	*/ 

	//Play store 
	public static final String PLAYSTORE_URL = "https://play.google.com/store/apps/details?id=";
	public static final String DEVICE_PLATFORM = "2"; //2-Android device
	public static final String ShortUrlServiceUrl = "http://tinyurl.com/api-create.php?url=";
	public static final String T_URL = "http://tinyurl.com/api-create.php?url=";

	
	
	//Web Sevices
	public static String LocationServiceUrl = GuardianServiceUrl + "LocationService.svc/";
	public static String GeoServiceUrl = GuardianServiceUrl + "GeoUpdate.svc/";
	public static String GroupServiceUrl = GuardianServiceUrl + "GroupService.svc/";
	public static String MembershipServiceUrl = GuardianServiceUrl + "MembershipService.svc/";
	//Web services Url
	public static String MembershipServiceSyncUrl = MembershipServiceUrl+ "GetProfilesForLiveId";
	public static String PhoneValidatorUrl = MembershipServiceUrl + "CreatePhoneValidator";
	public static String updateProfile = MembershipServiceUrl + "UpdateProfile";
	public static String CreateProfileUrl = MembershipServiceUrl + "CreateProfile";
	public static String PostMyLocationUrl = GeoServiceUrl + "PostMyLocation";
	public static String StopPostingsUrl = GeoServiceUrl + "StopPostings/";
	public static String StopSOSOnlyUrl = GeoServiceUrl + "StopSOSOnly/";
	public static String PostLocationWithMedia = GeoServiceUrl + "PostLocationWithMedia";
	public static String ReportTeaseUrl = GeoServiceUrl + "ReportIncident";

	public static String UnregisterUrl = MembershipServiceUrl + "UnRegisterUser";
	public static String updatePhoneProfile = MembershipServiceUrl + "UpdateProfilePhone";
	public static String GetUserLocationUrl = LocationServiceUrl + "GetUserLocationArray/{0}/{1}";
	public static String LocateLiveTileUrl = LocationServiceUrl + "GetSOSTrackCount/{0}/{1}";
	public static String LocateBuddiesUrl = LocationServiceUrl + "GetBuddiesToLocateLastLocation/{0}/{1}";
	public static String CheckUpdatesFromServerUrl = MembershipServiceUrl+ "CheckPendingUpdates/";


	public static final int SOS_CONNECTION_TIMER = 5;
	//public static String Authentication_Token = "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwMDU4NDYyMywidWlkIjoiMjkzNTk1MTA3ZDY1ODFhZDZjNTA0MmU4ZGMzMTU5YTYiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.2ET03fi4iPO29Pe3UwTlIaajp94Q5MXZXmowtNH698w";
	public static String APP_SHARED_PREFRENCE = "GuardianAPPPref";
	public static int SMS_TIME_DELAY =1000 * 60 *10;
	public static int powerButtonClick = 0;
	public  static long PowerButPress_time =0; 
	public static int timer =3;
	public  static  TimerTask timerTask;
	public static String selectedFBGroup = "";

	public static String firstTimeLounch = "APPLICATION_LOUNCH_FIRST_TIME";
	public static String SOS_on = "APPLICATION_SOS_ON";
	public static String trackMe_on = "APPLICATION_TRACK_ME_ON";
	public static String show_rating_dialog = "SHOW_RATING_DIALOG";

	public static boolean isGPS_On = false;
	public static boolean isRagister = false;
	public static boolean isStartPushPin = false;
	public static  ProgressDialog loadingDialog; 
	public static boolean isGetDirection = false;
	public static String DEFAULT_COUNTRY_REGION_CODE = "IN"; 
	public static boolean isSosSmsSentForFirstTime = false;
	public static boolean IS_DEVICE_PLATFORM_DIFFERENT = false;

	/* public static boolean isTrackMeActivity = false;
	 public static Activity trackMeActivity = null;*/

	public static String Authentication_Token = "AUTHENTICATION_TOKEN";
	public static String Profile_ID = "PROFILE_ID";
	public static String User_ID = "USER_ID";
	public static String FB_Authentication_Token = "FBAccessToken";
	public static String isLiveRagister = "IS_LIVE_RAGISTER";
	public static String isProfileDone = "IS_PROFILE_DONE";
	public static String Access_Token = "ACCESS_TOKEN";
	public static String Referesh_Token = "REFERESH_TOKEN";
	public static String Live_user_name  = "LIVE_USER_NAME";
	public static String LiveEmail_id = "LIVE_EMAIL_ID";
	public static String Ragister_mobile = "LIVE_RAGISTER_MOBILE_NUMBER";
	public static String Ragister_countryCode = "LIVE_RAGISTER_COUNTRY_CODE";
	public static String Last_profileSyncTime = "LAST_PROFILE_SYNC_TIME";
	public static String isStartSOSOnPB = "START_SOS_ON_PB";
	public static String User_Prefs_Latitute = "GPS_LATITUTE";
	public static String User_prefs_logitute = "GPS_LOGITUTE";
	public static String Settings_Lounched_firstTime = "PREFERENCE_FIRST_TIME";
	public static final String SOS_RECURRENCE_DURATION = "SOS_RECURRENCE_DURATION";
	public static String FB_Selected_Group = "FB_SELECTED_GROUP";
	public static final String IS_CALLER_BUDDY_SELECTED = "IS_CALLER_BUDDY_SELECTED";
	public static final String CALL_USER = "phone";
	public static final String SEND_MESSAGE_TO_USER = "message";

	public static String TINY_URL = "";

	public static final String STOP_SOS_COMMAND = "STOPSOSONLY";
	public static final String DEFAULT_SOS_COMMAND = "DEFAULT";
	public static int service_Tag = 0;
	public static String TokenId;
	public static int NO_OF_BUDDIES =5;
	public static int fromIndex_PostLocation = 0;

	public static int WIFI_SETTINGS_ACTIVITY_RESULT = 10;
	public static int GPS_SETTINGS_ACTIVITY_RESULT = 20;
	public static int SIM_NET_SETTINGS_ACTIVITY_RESULT = 30;
	public static int CAMERA_ACTIVITY_RESULT = 40;
	public static int GET_COUNTRY_CODE_ACTIVITY_RESULT = 50;
	public static int CREATE_PROFILE_ACTIVITY_RESULT = 60;

	public static String TRACKING_TOKEN= "0";
	public static String SOS_TOKEN= "1";
	public static String PUBLIC_GROUP_TYPE = "0";
	public static String PRIVATE_GROUP_TYPE="1";

	public static int locateSyncServices;

	public static int WIFI_NETWORK = 11;
	public static int SIM_NETWORK = 22;
	public static int NO_NETWORK = 33;

	public static int GPS_ENABLED = 44;
	public static int GPS_DISABLED = 55;

	public static final int GPS_DIALOG = 1;
	public static final int RATING_DIALOG = 2;
	public static final int RATING_ON_SOS_DIALOG = 3;


	public static final String NORMAL_BUDDY = "1";
	public static final String UNSUBSCRIBED_BUDDY = "2";
	public static final String MARSHALL_BUDDY = "4";



	public static boolean isSoSActivity;

	public static String localUserName="";

	public static String localHelpLat,localHelpLong,localHelpAddress;


	public static  String client_scret = "/TOREPLACE-FBSECRET/";
	public static String strUrl = "https://graph.facebook.com/oauth/authorize?client_id=486681428059978&redirect_uri=http://www.facebook.com/connect/login_success.html&display=touch&scope=publish_stream,user_groups,manage_groups";
	public static String strTokenUrl = "https://graph.facebook.com/oauth/access_token?client_id=486681428059978&redirect_uri=http://www.facebook.com/connect/login_success.html&client_secret="+client_scret+"&code=";
	public static String strGetGroupsUrl = "https://graph.facebook.com/me?fields=id,groups.fields(id,name,owner)&access_token=";
	public static String strPostOnGroup = "https://graph.facebook.com/";


	public final static String LOG_TAG = "TinyUrl";



	static final String TrackMePageUrl = "/Pages/TrackMe.xaml";
	static final String MapPageUrl = "/Pages/MapPage.xaml";
	public static final int HTTP_RESULT = 0;
	public static final int HTTP_RESPONSE = 1;
	public static final int RETRY_MAX_COUNT = 3;
	public static final int RETRY_SLEEP_TIME_MS = 5*100;
	public static final int TIME_OUT_CONNECTION = 30*1000;
	public static final int TIME_OUT_SOCKET = 30*1000;
	public static final int SOS_TRACK_COUNT_TIMER = 30*1000;
	public static final int POST_LOC_TIMER = 30*1000;



	//=================================================
	public static final int MEMBERSHIP_SERVICE_TAG = 100;
	public static final int MOBILE_VALIDATION_SERVICE_TAG= 200;
	public static final int CREATE_PROFILE_SERVICE_TAG= 300;
	public static final int GROUP_SERVICE_TAG= 400;
	public static final int GET_LOCAL_HELP_TAG= 500;
	public static final int GET_LOCATE_BUDDIES_TAG= 600;
	public static final int GET_SOSTRACK_COUNT_TAG =700;
	public static final int UPDATE_PROFILE_SERVICE_TAG= 800;
	public static final int TRACKME_ACTIVITY_SEARCH_SERVICE_TAG= 900;
	public static final int TINY_URL_SERVICE_TAG= 1000;
	public static final int UNREGISTER_SERVICE_TAG= 1100;
	public static final int FB_GROUPS_SERVICE_TAG = 1200;
	public static final int POST_TO_FB_GROUP_SERVICE_TAG = 1300;
	public static final int REPORT_AN_INCIDENT_SERVICE_TAG = 1400;
	public static final int CHECK_UPDATES_FROM_SERVER_TAG = 1500;
	public static final int POST_LOCATION_WITH_MEDIA_SERVICE_TAG = 1600;
	public static final int GET_SEARCH_LOCATION_SERVICE_TAG = 1700;

	//================================================	 

	//public static boolean isLiveRagister = false;

	public static final String BingMapsKey = "AoBFMSS4EOyLV9jxIidneive6OtB21mVyzr520OsUwO51tFxCe9vgShVsHs2rz7U"; 
	public static final String MaxGetLocalHelpResults = "15";

	public static final String[] SCOPES = {
		//"wl.signin",
		//"wl.basic",
		"wl.emails",
		"wl.offline_access",
		"wl.skydrive_update",
		// "wl.contacts_create",
	};

	private AppConstant() {
		throw new AssertionError("Unable to create Config object.");
	}



	public static String GreenColorCode = "#FF10AA1E";
	public static String SaffronColorCode = "#FFF96511";
	public static String WhiteColorCode = "#FFFFFF";
	public static ArrayList<LocateBuddies> locateBuddies=new ArrayList<LocateBuddies>();
	public static LocateBuddies locateBuddy=new LocateBuddies();

	public static String aboutEmailText ="Guardian is the ultimate security app in your hand. Being easy to configure and the power of being tracked by buddies and security groups simultaneously, ensures multi-fold security for you. "
			+ "                           Download Guandian App :";
	public static String aboutEmailId ="guardianapp@outlook.com";
	public static String aboutEmailSubject ="Guardian App download";

	public static String aboutSMSText ="Download Guardian App at :";

	public static String aboutShareText ="Guardian is the ultimate security app in your hand. Being easy to configure and the power of being tracked by buddies and security groups simultaneously, ensures multi-fold security for you";
	public static String aboutShareLink ="http://www.windowsphone.com/en-in/store/app/guardian/178406e1-0363-43ee-8be0-e2945fa18d6b";
	public static final String[] version ={" Version 1.3 (February 3, 2014)", "Version 1.2 (December 15, 2013)"
		, "Version 1.1 (November 22, 2013)","Version 1.0 (November 12, 2013)"};
	public static final String[] versionHIstory ={"Enhancements and bug fixes.","Added 'Force Stop SMS' button in Settings, minor enhancements and minor bug fixes",
		"Bug fixes and minor enhancements","First release"};



	public static String MessageTemplateText = "I'm in serious trouble. Urgent help needed!";
	public static String SafeMessageText = "I'm safe now. Thanks for the help! I will get in touch with you shortly and share the details.";
	public static String LocateBuddyMessageText = "I'm reaching to help you.";
	public static String LocationDisabledMessageText = "Location Services are disabled either in phone's or Guardian's settings. Do you want to enable it?";
	public static String ReachSupportMessageText = " Please contact app support team @ guardianapp@outlook.com";
	public static int MaxNumberOfBuddies = 5;
	public static String PortalModeratorMessage = "Please enter your details needed to join the group";
	public static String PortalAutoOrgMailMessage = "Please enter your valid Org Email Id";
	public static String PortalAutoOrgMailErrorMessage = "* Please provide valid Org Email Id";
	public static String PortalAutoOrgMaillengthErrorMessage = "* Email Id should have atleast two characters before domain name.";
	public static String PortalErrorMessage = "* Please provide required details";
	public static String sendSMSFromPhone = "needs urgent help at ";


	public static User user = new  User();
	public static  ArrayList<AscGroups> ascGroups = new ArrayList<AscGroups>();
	public static  AscGroups ascGroup = new AscGroups();
	public static ArrayList<MyBuddies> globalBuddies = new ArrayList<MyBuddies>();
	public static MyBuddies callerBuddies = new MyBuddies();
	public static GPSTracker gpsTracker;

	public static boolean isNumberISUpdate =false;
	public static boolean isScreenRefreshRequired = false;


	public static LocalProfile userProfile = new  LocalProfile();


	public static final String LOGS_PARENT_DIR = "Guardian";
	public static final String LOGS_CHILD_DIR = "logs";
	public static final String DIRECTORY_SEPARATOR = System.getProperty("file.separator");
	public static final String LOG_EMAIL_BODY = "Your App crashed. Please find the attachment for more details";
	public static final String LOG_FILE_NAME = "guardian_log.txt";


	public static final int RESULT_TYPE_SUCCESS = 1;
	public static final int RESULT_TYPE_ERROR = 3;
	public static final int RESULT_TYPE_INVALID_TOKEN = 5;

	public static LocalProfile CreateDefaultProfile(Context context)
	{
		SharedPreferences AppPrefs= context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		LocalProfile profile = new LocalProfile();
		profile.setProfileID((long) 0);
		SharedPreferences.Editor editor = AppPrefs.edit();
		editor.putLong(AppConstant.Profile_ID, (long) 0);
		editor.commit();
		profile.setMobileNumber("+000000000000");
		profile.setCanMail(true);
		profile.setCanSMS(true);
		profile.setCanPost(false);
		profile.setLocationConsent(true);
		profile.setCountryCode(getCountryCodeFromTelephonyMgr(context));
		CountryCodes code = AppConstant.getCountryCodes(context, getCountryCodeFromTelephonyMgr(context));
		profile.setAmbulanceContact(code.getAmbulance());
		profile.setFireContact(code.getFire());
		profile.setPoliceContact(code.getPolice());
		profile.setPostLocationConsent(true);
		profile.setIsSOSStatusSynced(true);
		profile.setIsTrackingStatusSynced(true);
		profile.setMessageTemplate("I'm in serious trouble. Urgent help needed!");
		profile.setIsDataSynced(true);
		return profile;
	}

	public static String getCurrentDateTime () {
		SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy HH:mm:ss");
		String currentDateandTime = sdf.format(new Date());
		return currentDateandTime;
	}

	public static int check_networkConnectivity(Context act) {

		ConnectivityManager manager = (ConnectivityManager) 
				act.getSystemService(act.CONNECTIVITY_SERVICE);

		if(manager.getNetworkInfo(
				ConnectivityManager.TYPE_MOBILE).isConnectedOrConnecting())
			return AppConstant.SIM_NETWORK;
		if(manager.getNetworkInfo(
				ConnectivityManager.TYPE_WIFI).isConnectedOrConnecting())
			return AppConstant.WIFI_NETWORK;
		return AppConstant.NO_NETWORK;
	}

	public static boolean hasActiveInternetConnection(Context context) {
		if (check_networkConnectivity(context)!= AppConstant.NO_NETWORK) {
			try {
				HttpURLConnection urlc = (HttpURLConnection) (new URL("http://www.google.com").openConnection());
				urlc.setRequestProperty("User-Agent", "Test");
				urlc.setRequestProperty("Connection", "close");
				urlc.setConnectTimeout(1500); 
				urlc.connect();
				return (urlc.getResponseCode() == 200);
			} catch (IOException e) {
				Log.e(LOG_TAG, "Error checking internet connection", e);
			}
		} else {
			Log.d(LOG_TAG, "No network available!");
		}
		return false;
	}

	public static int checkGpsOnOrOffStatus(){
		if(AppConstant.gpsTracker!=null && AppConstant.gpsTracker.checkGPSStatusOn()&&AppConstant.userProfile.isLocationConsent())
			return AppConstant.GPS_ENABLED;
		else
			return AppConstant.GPS_DISABLED;
	}

	public static boolean intToBoolean(int i) {
		if(i==1)
			return true;
		else if(i==0)
			return false;
		return false;
	}

	public static int booleanToInt(boolean value){
		if(value)
			return 1;
		else
			return 0;
	}

	public static ArrayList<CountryCodes>getAllcountryCodes(Context context) {
		ArrayList<CountryCodes> cCodeList = new ArrayList<CountryCodes>();
		CountryCodes cCodes = new CountryCodes();
		XmlPullParserFactory factory = null;
		AssetManager assetManager = null;
		try {
			factory = XmlPullParserFactory.newInstance();
			factory.setNamespaceAware(true);
			assetManager = context.getAssets();
			InputStream is = assetManager.open("country_codes.xml");
			XmlPullParser parser = factory.newPullParser();
			parser.setFeature(XmlPullParser.FEATURE_PROCESS_NAMESPACES, false);
			parser.setInput(new InputStreamReader(is));
			cCodeList = parseXml(parser);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (XmlPullParserException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		return cCodeList;


	}

	public static long getTicks_time(long millisec) {
		Date date = new Date(millisec);
		Calendar mCalendar = new GregorianCalendar();  
		mCalendar.setTime(date);
		TimeZone mTimeZone = mCalendar.getTimeZone();
		long ticks_value = 621355968000000000L;
		return (date.getTime()+mTimeZone.getRawOffset())*10000+ticks_value;
	}

	public static long getTicksTimeInUTC(long millisec){
		Date date = new Date(millisec);
		TimeZone utc = TimeZone.getTimeZone("UTC");
		Calendar calendar = Calendar.getInstance(utc);
		calendar.setTime(date);
		long ticks_value = 621355968000000000L;
		return (date.getTime()+utc.getRawOffset())*10000+ticks_value;
	}

	public static Date getTimeFromTicks(long ticks_time){
		long ticks_at_epoch = 621355968000000000L;
		long ticks_per_millisecond = 10000;
		Date date = new Date((ticks_time - ticks_at_epoch) / ticks_per_millisecond);
		return date;
	}

	public static String getRecordedTimeFromTicks(long ticks_time){
		Date date = getTimeFromTicks(ticks_time);
		SimpleDateFormat sdf = new SimpleDateFormat("h:mm a");
		sdf.setTimeZone(TimeZone.getTimeZone("UTC"));
		String time = sdf.format(date);
		return time;
	}

	public static  ArrayList<CountryCodes> parseXml(XmlPullParser parser) throws XmlPullParserException,IOException{
		ArrayList<CountryCodes> cCodeList = null;
		CountryCodes countryCodes = null;


		int eventType = parser.getEventType();


		while (eventType != XmlPullParser.END_DOCUMENT){
			String name = null;
			switch (eventType){
			case XmlPullParser.START_DOCUMENT:
				cCodeList = new ArrayList<CountryCodes>();
				break;
			case XmlPullParser.START_TAG:
				name = parser.getName();
				if (name.equalsIgnoreCase("CountryCodes") ){
					countryCodes = new CountryCodes();
				} else if (countryCodes != null){
					if (name.equalsIgnoreCase("Country")){
						countryCodes.setCountryName(parser.getAttributeValue(null, "Name"));
						countryCodes.setISDCode(parser.getAttributeValue(null, "IsdCode"));

					} else if (name.equalsIgnoreCase("Properties")){

						countryCodes.setMaxPhoneDigits(parser.getAttributeValue(null, "MaxPhoneDigits"));
						countryCodes.setPolice(parser.getAttributeValue(null, "Police"));
						countryCodes.setFire(parser.getAttributeValue(null, "Fire"));
						countryCodes.setAmbulance(parser.getAttributeValue(null, "Ambulance"));

					}
				}
				break;
			case XmlPullParser.END_TAG:
				name = parser.getName();
				if (name.equalsIgnoreCase("Properties") && countryCodes != null){
					cCodeList.add(countryCodes);
					countryCodes = null;
					countryCodes = new CountryCodes();
				} 
			}
			eventType = parser.next();
		}



		return cCodeList;



	}

	public static  byte[] convertIntoBase64(Bitmap bm) {


		ByteArrayOutputStream baos = new ByteArrayOutputStream();  
		bm.compress(Bitmap.CompressFormat.JPEG, 100, baos); //bm is the bitmap object   
		byte[] b = baos.toByteArray(); 

		return b/* Base64.encodeToString(b , Base64.DEFAULT)*/;
	}


	public static String convertToUnsignedString(String gsonString)
	{
		String mediaContent=gsonString.substring(gsonString.indexOf('['), gsonString.indexOf(']'));
		String finalString=mediaContent.replace('[', ' ').replace(']',' ' );
		String[] newString= finalString.split(",");
		StringBuilder builder = new StringBuilder();
		builder.append('[');
		try
		{
			for(int i=0;i<newString.length;i++)
			{
				if(newString[i].contains("-"))
				{
					int signedInt=-(Integer.parseInt( newString[i].replace('-', ' ').trim()));
					if(signedInt<0)
					{
						int unsignedInt=signedInt & 0xff;
						newString[i]=Integer.toString(unsignedInt);
					}
				}
				builder.append(newString[i]);
				if(i!=(newString.length-1))
					builder.append(',');
			}

			return builder.toString();
		}
		catch(Exception e)
		{
			Log.e("error", e.getMessage());
			return "";
		}
	}


	public static void StopSOS(Context context) {

		try {
			AppConstant.stopSMSViaPhone(context);
			sendSafeSMS(context);
			postSafeStatusToFB(context);
			MakeHTTPServices sendfromServices = new MakeHTTPServices(context);
			DBQueries updateRecord = new DBQueries(context);
			if(AppConstant.userProfile.isIsSOSOn())
			{
				AppConstant.userProfile.setIsSOSOn(false);
				updateRecord.updateValue("IsSOSOn", AppConstant.userProfile.isIsSOSOn());
			}
			sendfromServices.stopSOSOnly(AppConstant.TokenId);

		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

	}

	public static void callOrSendMessageToUser(Context context, String number, String type){
		if(type.equalsIgnoreCase(AppConstant.SEND_MESSAGE_TO_USER)){

			if (Build.VERSION.SDK_INT > Build.VERSION_CODES.JELLY_BEAN_MR2){
				Intent intent = new Intent(Intent.ACTION_SENDTO);
				intent.setData(Uri.parse("smsto:" + Uri.encode(number)));
				intent.putExtra("sms_body",SafeMessageText);
				((Activity) context).startActivity(intent);

			}else{
				Intent intent = new Intent(Intent.ACTION_VIEW/*, smsUri*/);
				intent.putExtra("sms_body", SafeMessageText);
				intent.setType("vnd.android-dir/mms-sms"); 
				intent.putExtra("address", number);
				((Activity) context).startActivity(intent);

			}

		}else if(type.equalsIgnoreCase(AppConstant.CALL_USER)){
			Intent i = new Intent(Intent.ACTION_CALL);
			String p = "tel:" + number;
			i.setData(Uri.parse(p));
			((Activity) context).startActivity(i);

		}
	}

	public static void CustomDialog(final Context context, String msg, final String txttitle,
			String btn1, String btn2) {

		View view = ((Activity) context).getLayoutInflater().inflate(R.layout.common_dialog, null);
		final AlertDialog callMsgDialog = new AlertDialog.Builder(((Activity) context))
		.setView(view).create();

		TextView title = (TextView) view.findViewById(R.id.tv_title);	
		title.setText(txttitle);


		TextView msgTxt = (TextView) view.findViewById(R.id.tv_msgAleart);	
		msgTxt.setText(msg);
		Button callbtn = (Button) view.findViewById(R.id.btnOK);
		callbtn.setText(btn1);

		Button cancelBtn = (Button) view.findViewById(R.id.btnCancel);
		cancelBtn.setText(btn2);
		view.findViewById(R.id.btnOK).setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {


				if(txttitle.equalsIgnoreCase("Confirmation"))
				{
					if (Build.VERSION.SDK_INT > Build.VERSION_CODES.JELLY_BEAN_MR2){
						Intent intent = new Intent(Intent.ACTION_SENDTO);
						if(AppConstant.callerBuddies!=null && AppConstant.callerBuddies.getMobileNumber()!=null)
							intent.setData(Uri.parse("smsto:" + Uri.encode(AppConstant.callerBuddies.getMobileNumber())));
						else
							intent.setData(Uri.parse("smsto:"));
						intent.putExtra("sms_body",SafeMessageText);
						((Activity) context).startActivity(intent);

					}else{
						Intent intent = new Intent(Intent.ACTION_VIEW/*, smsUri*/);
						intent.putExtra("sms_body", SafeMessageText);
						intent.setType("vnd.android-dir/mms-sms"); 
						if(AppConstant.callerBuddies.getMobileNumber()!=null)
							intent.putExtra("address", AppConstant.callerBuddies.getMobileNumber());
						else
							intent.setData(Uri.parse("smsto:"));
						((Activity) context).startActivity(intent);

					}
				}
				else if(txttitle.equalsIgnoreCase("Phone"))
				{
					Intent i = new Intent(Intent.ACTION_CALL);
					String p = "tel:" +  AppConstant.callerBuddies.getMobileNumber();
					i.setData(Uri.parse(p));
					((Activity) context).startActivity(i);
				}

				callMsgDialog.dismiss();

			}
		});

		view.findViewById(R.id.btnCancel).setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {

				if(txttitle.equalsIgnoreCase("Phone")&&!isSoSActivity)
				{
					Intent intent = new Intent();
					intent.setClass((Activity) context, SOSActivity.class);
					((Activity) context).startActivity(intent);
				}
				callMsgDialog.dismiss();

			}
		});

		callMsgDialog.show();
	}


	public static String getDeviceName()
	{
		String manufacturer = Build.MANUFACTURER;
		String model = Build.MODEL;
		if (model.startsWith(manufacturer)) {
			return capitalize(model);
		} else {
			return capitalize(manufacturer) + " " + model;
		}

	}

	private static String capitalize(String s) {
		if (s == null || s.length() == 0) {
			return "";
		}
		char first = s.charAt(0);
		if (Character.isUpperCase(first)) {
			return s;
		} else {
			return Character.toUpperCase(first) + s.substring(1);
		}
	}

	public static void showProgressDialog(Context context) {
		loadingDialog = new ProgressDialog(context);
		loadingDialog.setCancelable(false);
		loadingDialog.setMessage("Loading ....");
		loadingDialog.show();
	}

	public static void showProgressDialogWithMessage(Context context, String message){
		loadingDialog = new ProgressDialog(context);
		loadingDialog.setCancelable(false);
		loadingDialog.setMessage(message);
		loadingDialog.show();
	}

	public static void dismissProgressDialog() {
		if(loadingDialog!=null &&loadingDialog.isShowing()){
			loadingDialog.dismiss();
			loadingDialog=null;
		}

	} 


	public static String SessionToken(Context context)
	{
		if(AppConstant.userProfile.getSessionToken()==null || AppConstant.userProfile.getSessionToken().equalsIgnoreCase("")){
			return "0";
		}
		else{
			return AppConstant.userProfile.getSessionToken();
		}
	}

	public static String SessionToken()
	{
		if(AppConstant.userProfile.getSessionToken()==null || AppConstant.userProfile.getSessionToken().equalsIgnoreCase("")){
			return "0";
		}
		else{
			return AppConstant.userProfile.getSessionToken();
		}
	}

	public static void InitiateTracking(Context context,Boolean IsNewSession)
	{
		try {
			DBQueries updateRecord = new DBQueries(context);
			MakeHTTPServices sendfromServices = new MakeHTTPServices(context);
			SharedPreferences  AppPrefs = context.getSharedPreferences(
					AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
			if(!AppPrefs.getBoolean(AppConstant.trackMe_on, false)){
				if(IsNewSession)
				{
					sendfromServices.stopPosting("0");
					sendfromServices.postLocationArray();
					AppConstant.userProfile.setSessionToken(UUID.randomUUID().toString());
					updateRecord.updateValue("SessionToken", AppConstant.userProfile.getSessionToken());
				}
				if(!AppConstant.userProfile.isIsTrackingOn()){
					AppConstant.userProfile.setIsTrackingOn(true);
					updateRecord.updateValue("IsTrackingOn", AppConstant.userProfile.isIsTrackingOn());
				}
			}	 

			if(AppPrefs.getBoolean(AppConstant.SOS_on, false)){
				AppConstant.isStartPushPin=true;
				AppConstant.TokenId = AppConstant.SOS_TOKEN;
				sendfromServices.postMyLocationAsync( null,AppConstant.DEFAULT_SOS_COMMAND);
				AppConstant.isStartPushPin=false;
				if(!AppConstant.userProfile.isIsSOSOn())
				{
					AppConstant.userProfile.setIsSOSOn(true);
					updateRecord.updateValue("IsSOSOn", AppConstant.userProfile.isIsSOSOn());
				}
			}

		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	public static void InitiateTrackingFromPB(Context context,Boolean IsNewSession)
	{
		try {
			DBQueries updateRecord = new DBQueries(context);
			MakeHTTPServices sendfromServices = new MakeHTTPServices(context);
			SharedPreferences  AppPrefs = context.getSharedPreferences(
					AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
			if(!AppPrefs.getBoolean(AppConstant.trackMe_on, false)){
				if(IsNewSession)
				{
					sendfromServices.stopPosting("0");
					//sendfromServices.postLocationArray();
					AppConstant.userProfile.setSessionToken(UUID.randomUUID().toString());
					updateRecord.updateValue("SessionToken", AppConstant.userProfile.getSessionToken());
				}
				AppConstant.userProfile.setIsTrackingOn(true);
				updateRecord.updateValue("IsTrackingOn", AppConstant.userProfile.isIsTrackingOn());
			}	 

			AppConstant.isStartPushPin=true;
			AppConstant.TokenId = AppConstant.SOS_TOKEN;
			sendfromServices.postLocationArray();
			sendfromServices.postMyLocationAsync( null,AppConstant.DEFAULT_SOS_COMMAND);
			AppConstant.isStartPushPin=false;
			AppConstant.userProfile.setIsSOSOn(true);
			updateRecord.updateValue("IsSOSOn", AppConstant.userProfile.isIsSOSOn());

		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}



	public static void StopTracking(Context context)
	{
		try {
			DBQueries updateRecord = new DBQueries(context);
			SharedPreferences  AppPrefs = context.getSharedPreferences(
					AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
			if(AppPrefs.getBoolean(AppConstant.trackMe_on, false)){


				if(AppConstant.userProfile.isIsTrackingOn()){
					AppConstant.userProfile.setIsTrackingOn(false);

					updateRecord.updateValue("IsTrackingOn", AppConstant.userProfile.isIsTrackingOn());
				}

				MakeHTTPServices sendfromServices = new MakeHTTPServices(context);
				sendfromServices.stopPosting( AppConstant.SessionToken(context));
				AppConstant.userProfile.setSessionToken("");
				updateRecord.updateValue("SessionToken", AppConstant.userProfile.getSessionToken());

			}
		}catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	public static void UnregisterLocally(Context context)
	{

		DBQueries dbqueries = new DBQueries(context);
		SharedPreferences  AppPrefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		SharedPreferences.Editor edit = AppPrefs.edit();
		edit.putBoolean(AppConstant.isLiveRagister,false);
		edit.putBoolean(AppConstant.isProfileDone,false);
		if(AppPrefs.getBoolean(AppConstant.SOS_on, false))
			edit.putBoolean(AppConstant.SOS_on, false);
		if(AppPrefs.getBoolean(AppConstant.trackMe_on, false))
			edit.putBoolean(AppConstant.trackMe_on, false);
		edit.commit();
		AppConstant.isRagister=false;
		dbqueries.updateValue("ProfileId", "0");
		dbqueries.removeAll(DBQueries.TABLE_BUDDY);
		dbqueries.removeAll(DBQueries.TABLE_GROUP);

		AppConstant.globalBuddies=dbqueries.SelectAllBuddies();

		if(AppConstant.globalBuddies.size()>0)
			AppConstant.callerBuddies = AppConstant.getPrimaryBuddy();

		AppConstant.ascGroups=new ArrayList<AscGroups>();
		//To do update profile
	}

	public static void clearProfile(Context context){
		DBQueries dbqueries = new DBQueries(context);
		dbqueries.updateValue("ProfileId", "0");
		dbqueries.removeAll(DBQueries.TABLE_BUDDY);
		dbqueries.removeAll(DBQueries.TABLE_GROUP);
		dbqueries.removeAll(DBQueries.TABLE_LOCATION_BUDDY);
		AppConstant.userProfile =AppConstant.CreateDefaultProfile(context);

	}


	public static void  sendSMSFromPhone(String smsText,String phoneNo) {

		try{

			SmsManager smsManager = SmsManager.getDefault();
			smsManager.sendTextMessage(phoneNo, null, smsText, null, null);
		}catch(Exception e){
			LogUtils.LOGE(LogUtils.makeLogTag(AppConstant.class), e.getLocalizedMessage());
		}
	}

	public static void sendLongSMSFromPhone(String smsText,String phoneNo){
		SmsManager sms = SmsManager.getDefault();
		ArrayList<String> parts = sms.divideMessage(smsText);
		sms.sendMultipartTextMessage(phoneNo, null, parts, null, null);
	}

	public static void UpdateIsDataSynced(Context context, Boolean value)
	{
		SharedPreferences  AppPrefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		if(AppPrefs.getBoolean(AppConstant.isProfileDone, false)){
			DBQueries dbqueries = new DBQueries(context);
			dbqueries.updateValue("IsDataSynced", value);
			AppConstant.userProfile.setIsDataSynced(value);
		}
	}

	public static boolean isSendSMSFromPB()
	{
		long clickTimeInterval = 2;
		long timeInterval = System.currentTimeMillis()- PowerButPress_time;
		long diffInSec = TimeUnit.MILLISECONDS.toSeconds(timeInterval);

		LogUtils.LOGE(LogUtils.makeLogTag(AppConstant.class), "diffInSec="+TimeUnit.MILLISECONDS.toSeconds(timeInterval)+"--clickTimeInterval="+clickTimeInterval);

		if(diffInSec<=clickTimeInterval&&PowerButPress_time!=0)
		{

			PowerButPress_time = System.currentTimeMillis();
			System.out.println("screen Sec   >>>>>>"+ diffInSec+">>>>>powerButtonClick >>>>>>>"+AppConstant.powerButtonClick);
			if(diffInSec<=clickTimeInterval&&AppConstant.powerButtonClick==1)
			{
				AppConstant.powerButtonClick=0;
				System.out.println("return true");
				return true;
			}
			else
			{
				if(AppConstant.powerButtonClick==1)
					AppConstant.powerButtonClick=0;
				else
				{
					AppConstant.powerButtonClick++;
					//setTimer();
				}

				return false;
			}



		}
		else
		{
			PowerButPress_time = System.currentTimeMillis();
			AppConstant.powerButtonClick++;

			setTimer();
			return false;
		}


		/*	if(AppConstant.powerButtonClick==1)
        	{
				long timeInterval = System.currentTimeMillis()- PowerButPress_time;
				long diffInSec = TimeUnit.MILLISECONDS.toSeconds(timeInterval);
				AppConstant.powerButtonClick =0;
				 System.out.println("screen Sec   >>>>>>"+ diffInSec);
        		if(diffInSec<15)
        		{
        			 return true;
        		}
        		else
        			return false;	

        	}
        	else
        	{
        		PowerButPress_time = System.currentTimeMillis();
        		AppConstant.powerButtonClick++;
        		return false;
        	}
		 */

	}

	private static void setTimer()
	{

		Timer ourtimer = new Timer();
		timer =3;
		System.out.println("call Timer..........."+timer);
		timerTask = new TimerTask() {

			@Override
			public void run() {

				if(timer==0)
				{
					AppConstant.powerButtonClick =0;
					System.out.println("cancle Timer...........");
					if(timerTask!=null) 
					{
						timerTask.cancel();
						timerTask=null;

					}

				}

				else
				{
					timer--;
				}

			}
		};
		ourtimer.schedule(timerTask, 0, 1000);
	}

	public static boolean postSafeStatusToFB(Context context){
		MakeHTTPServices sendfromServices = new MakeHTTPServices(context);
		SharedPreferences  AppPrefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		if(!AppConstant.userProfile.isCanPost())
			return false;
		sendfromServices.postToFBGroups(AppPrefs.getString(AppConstant.FB_Selected_Group, ""), AppConstant.SafeMessageText);
		return true;
	}


	public static boolean sendSafeSMS(Context context) {
		if(!AppConstant.userProfile.isCanSMS())
			return false;

		if(AppConstant.globalBuddies!=null && AppConstant.globalBuddies.size()>0)
		{
			AppConstant.globalBuddies.removeAll(Collections.singleton(null));
			for (int i = 0; i < AppConstant.globalBuddies.size(); i++) {
				if(AppConstant.globalBuddies.get(i).getMobileNumber()!=null){
					String number = AppConstant.globalBuddies.get(i).getMobileNumber().replace(" ", "");
					AppConstant.sendSMSFromPhone(AppConstant.SafeMessageText, 
							number.trim());
				}}
			return true;
		}
		return false;

	}

	public static String getOfflineSMS(Context context) {
		String offlineSMS = "";
		offlineSMS = AppConstant.MessageTemplateText+" I'm @ :";
		AesBase64Wrapper aesEncry = new AesBase64Wrapper();
		String encryptedParam = aesEncry.encryptAndEncode("p="+AppConstant.getUserProfileID(context)+"&s="+AppConstant.userProfile.getSessionToken()+"&f="+AppConstant.booleanToInt(AppConstant.userProfile.isIsSOSOn()));
		offlineSMS = offlineSMS+AppConstant.guardianPortalUrl+"/default.aspx?t="+encryptedParam+"&ut="+AppConstant.getTicksTimeInUTC(new Date().getTime())+"&d="+AppConstant.getTicks_time(new Date().getTime())+"&l="+GPSTracker.latitude+"&g="+GPSTracker.longitude;
		return offlineSMS;
	}

	public static String getOfflineSMS_unregisterd(Context context) {
		String offlineSMS_unregisterd;
		SharedPreferences  AppPrefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

		offlineSMS_unregisterd = AppConstant.MessageTemplateText+" I'm at "
				+AppConstant.guardianPortalUrl+"/default.aspx?d="+AppConstant.getTicks_time(new Date().getTime())+"&l="+AppPrefs.getString(AppConstant.User_Prefs_Latitute, "0")+"&g="+
				AppPrefs.getString(AppConstant.User_prefs_logitute, "0");	


		return offlineSMS_unregisterd;

	}


	public static void sendDistressText(Context context) {
		SharedPreferences  AppPrefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

		if(AppConstant.check_networkConnectivity(context)==AppConstant.NO_NETWORK)
		{
			String distressTextMessage = "";
			if(AppPrefs.getBoolean(AppConstant.isProfileDone, false))
				distressTextMessage = AppConstant.getOfflineSMS(context);
			else
				distressTextMessage = AppConstant.getOfflineSMS_unregisterd(context);

			AppConstant.sendDistressMessageToMedium(context, distressTextMessage, AppConstant.booleanToInt(true), true);
			AppConstant.sendDistressMessageToMedium(context, distressTextMessage, AppConstant.booleanToInt(false) , true);
		}
		else
		{
			if(AppPrefs.getBoolean(AppConstant.isProfileDone, false))
			{
				String tinyUrl = TinyUrl.getTinyUrl(context,AppConstant.guardianPortalUrl+"/default.aspx?pr="
						+AppConstant.getUserProfileID(context)+"&s="
						+AppConstant.userProfile.getSessionToken());
			}
			else {
				String tinyUrl = TinyUrl.getTinyUrl(context,AppConstant.guardianPortalUrl+"/default.aspx?d="+AppConstant.getTicks_time(new Date().getTime())+"&l="+AppPrefs.getString(AppConstant.User_Prefs_Latitute, "0")+"&g="+
						AppPrefs.getString(AppConstant.User_prefs_logitute, "0"));
			}

		}
	}

	public static void sendDistressMessageToMedium(Context context, String distressText, int isSentToMobile, boolean isLongMessage){
		SharedPreferences  AppPrefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

		switch(isSentToMobile){

		case 0 : 
			MakeHTTPServices sendfromServices = new MakeHTTPServices(context);
			if(!AppConstant.userProfile.isCanPost() || !AppPrefs.getBoolean(AppConstant.SOS_on, false))
				break;
			sendfromServices.postToFBGroups(AppPrefs.getString(AppConstant.FB_Selected_Group, ""), distressText); 
			break;

		case 1 : 
			if(!AppConstant.userProfile.isCanSMS() ||  
					!AppPrefs.getBoolean(AppConstant.SOS_on, false))
				break;

			if(AppConstant.globalBuddies!=null && AppConstant.globalBuddies.size()>0)
			{
				AppConstant.globalBuddies.removeAll(Collections.singleton(null));
				for (int i = 0; i < AppConstant.globalBuddies.size(); i++) {
					if(AppConstant.globalBuddies.get(i).getMobileNumber()!=null){
						String number = AppConstant.globalBuddies.get(i).getMobileNumber().replace(" ", "");
						if(isLongMessage)
							AppConstant.sendLongSMSFromPhone(distressText, number.trim());
						else
							AppConstant.sendSMSFromPhone(distressText,number.trim());
					}}
				break;
			}
			break;




		}

	}

	public static String formatPhoneNumber(String phoneNo , String countryLocale) {
		if(phoneNo.charAt(0) == '0')
			phoneNo = phoneNo.substring(1,phoneNo.length());
		PhoneNumberUtil p = PhoneNumberUtil.getInstance();
		PhoneNumber phNum = null;
		try {
			phNum = p.parse(phoneNo,countryLocale.toUpperCase());
		} catch (NumberParseException nupe) {
			// TODO Auto-generated catch block
			LogUtils.LOGE(LogUtils.makeLogTag(AppConstant.class), nupe.getLocalizedMessage());
			return null;
		} catch (Exception e) {
			// TODO: handle exception
			LogUtils.LOGE(LogUtils.makeLogTag(AppConstant.class), e.getLocalizedMessage());
			return null;
		}
		return String.valueOf(phNum.getNationalNumber());
	}


	public static String extractPhoneNumberFromCode(String mobileNumber){
		if(mobileNumber.charAt(0) == '0')
			return mobileNumber.substring(1,mobileNumber.length());
		return mobileNumber;
	}

	public static String extractCodeFromPhoneNumber(String phoneNo,String countryLocale){
		PhoneNumberUtil p = PhoneNumberUtil.getInstance();
		PhoneNumber phNum = null;
		try {
			if(phoneNo.charAt(0)=='+')
				phNum = p.parse(phoneNo, "");
			else
				phNum = p.parse(phoneNo,countryLocale);
		} catch (NumberParseException nupe) {
			// TODO Auto-generated catch block
			LogUtils.LOGE(LogUtils.makeLogTag(AppConstant.class), nupe.getLocalizedMessage());
			return null;
		} catch (Exception e) {
			// TODO: handle exception
			LogUtils.LOGE(LogUtils.makeLogTag(AppConstant.class), e.getLocalizedMessage());
			return null;
		}
		return String.valueOf(phNum.getCountryCode());
	}

	public static boolean isMyServiceRunning(Context context, Class<?> serviceClass) {
		ActivityManager manager = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
		for (RunningServiceInfo service : manager.getRunningServices(Integer.MAX_VALUE)) {
			if (serviceClass.getName().equals(service.service.getClassName())) {
				return true;
			}
		}
		return false;
	}

	public static String readUrl(String mapsApiDirectionsUrl) throws IOException {
		String data = "";
		InputStream iStream = null;
		HttpURLConnection urlConnection = null;
		try {
			URL url = new URL(mapsApiDirectionsUrl);
			urlConnection = (HttpURLConnection) url.openConnection();
			urlConnection.connect();
			iStream = urlConnection.getInputStream();
			BufferedReader br = new BufferedReader(new InputStreamReader(
					iStream));
			StringBuffer sb = new StringBuffer();
			String line = "";
			while ((line = br.readLine()) != null) {
				sb.append(line);
			}
			data = sb.toString();
			br.close();
		} catch (Exception e) {
			Log.d("Exception while reading url", e.toString());
		} finally {
			iStream.close();
			urlConnection.disconnect();
		}
		return data;
	}

	public static boolean isMyServiceRunning(Class<?> serviceClass , Context context) {
		ActivityManager manager = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
		for (RunningServiceInfo service : manager.getRunningServices(Integer.MAX_VALUE)) {
			if (serviceClass.getName().equals(service.service.getClassName())) {
				return true;
			}
		}
		return false;
	}

	public static void sendSMSViaPhone(Context context) {
		SharedPreferences  AppPrefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

		if(AppConstant.userProfile!=null && AppConstant.userProfile.isCanSMS()&& 
				AppPrefs.getBoolean(AppConstant.SOS_on, false))
		{
			AppConstant.startRepeatingAlarm(context);
		}

	}

	public static void stopSMSViaPhone(Context context) {

		AppConstant.cancelAlarm(context);
	}

	public static void startRepeatingAlarm(Context context)
	{
		long interval = 0;
		SharedPreferences sosprefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		AlarmManager am=(AlarmManager)context.getSystemService(Context.ALARM_SERVICE);
		Intent intent = new Intent(context, AlarmManagerBroadcastReceiver.class);
		if(sosprefs.getString(AppConstant.SOS_RECURRENCE_DURATION, "10")!=null && sosprefs.getString(AppConstant.SOS_RECURRENCE_DURATION, "10").length()>0)
			interval = Integer.parseInt(sosprefs.getString(AppConstant.SOS_RECURRENCE_DURATION, "10"));
		interval = interval*60*1000;

		PendingIntent pi = PendingIntent.getBroadcast(context, 0, intent, 0);
		am.setRepeating(AlarmManager.RTC_WAKEUP, System.currentTimeMillis(), interval , pi); 
	}

	public static void cancelAlarm(Context context)
	{
		Intent intent = new Intent(context, AlarmManagerBroadcastReceiver.class);
		PendingIntent sender = PendingIntent.getBroadcast(context, 0, intent, 0);
		AlarmManager alarmManager = (AlarmManager) context.getSystemService(Context.ALARM_SERVICE);
		alarmManager.cancel(sender);
	}

	public static void startRepeatingAlarmForNotifications(Context context){
		long interval = 10;
		SharedPreferences prefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		AlarmManager am=(AlarmManager)context.getSystemService(Context.ALARM_SERVICE);
		if((prefs.getBoolean(AppConstant.SOS_on, false) || prefs.getBoolean(AppConstant.trackMe_on, false))){
			Intent intent = new Intent(context, NotificationServiceManager.class);
			interval = interval * 60 *1000;
			PendingIntent pi = PendingIntent.getBroadcast(context, 0, intent, PendingIntent.FLAG_CANCEL_CURRENT);
			am.setRepeating(AlarmManager.RTC_WAKEUP, System.currentTimeMillis()+interval, interval , pi); 
		}
	}

	public static void cancelRepeatingNotificationsAlarm(Context context){
		Intent intent = new Intent(context, NotificationServiceManager.class);
		PendingIntent sender = PendingIntent.getBroadcast(context, 0, intent, 0);
		AlarmManager alarmManager = (AlarmManager) context.getSystemService(Context.ALARM_SERVICE);
		alarmManager.cancel(sender);
	}

	public static MyBuddies getPrimaryBuddy(){
		AppConstant.globalBuddies.removeAll(Collections.singleton(null));
		return AppConstant.globalBuddies.get(0);
	}

	public static boolean validEmail(String email) {
		Pattern pattern = Patterns.EMAIL_ADDRESS;
		return pattern.matcher(email).matches();
	}

	public static String arrayToJson(String[] data) {
		StringBuffer sb = new StringBuffer();
		sb.append("[");
		for (int i = 0; i < data.length; i++) {
			String d = data[i];
			if (i > 0)
				sb.append(",");
			sb.append(d);
		}
		sb.append("]");
		return sb.toString();
	}

	public static boolean isAPILevelGreaterThan8(){
		int currentapiVersion = android.os.Build.VERSION.SDK_INT;
		if (currentapiVersion > android.os.Build.VERSION_CODES.FROYO){
			// Do something for froyo above versions
			return true;
		} else{
			// do something for phones running an SDK Froyo and before froyo
			return false;
		}
	}

	public static boolean iSSOSOn(Context context) {
		SharedPreferences AppPrefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		if(AppPrefs.getBoolean(AppConstant.SOS_on, false))
		{
			AppConstant.TokenId = AppConstant.SOS_TOKEN;
			return true;
		}
		else
			return false;
	}
	public static boolean iSTrackOn(Context context) {
		SharedPreferences AppPrefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		if(AppPrefs.getBoolean(AppConstant.trackMe_on, false))
		{
			if(!AppPrefs.getBoolean(AppConstant.SOS_on, false))
				AppConstant.TokenId = AppConstant.TRACKING_TOKEN;
			else
				AppConstant.TokenId = AppConstant.SOS_TOKEN;

			return true;
		}

		else
			return false;
	}
	public static boolean isCanPostToServer(Context context) {
		if((iSSOSOn(context)||iSTrackOn(context)))
			return true;
		else
			return false; 

	}
	public static void setCountryCode(Context context , String value){
		SharedPreferences prefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		SharedPreferences.Editor edit = prefs.edit();
		edit.putString(AppConstant.Ragister_countryCode, value);
		edit.commit();
	}

	public static String getDialingCode(Context context){
		SharedPreferences prefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		if(AppConstant.userProfile!=null && AppConstant.userProfile.getCountryCode()!=null && !AppConstant.userProfile.getCountryCode().equalsIgnoreCase(""))
			return AppConstant.userProfile.getCountryCode();
		else if(prefs.getString(AppConstant.Ragister_countryCode, "+91")!=null && !prefs.getString(AppConstant.Ragister_countryCode, "+91").equalsIgnoreCase(""))
			return prefs.getString(AppConstant.Ragister_countryCode, "+91");
		else 
			return "+91";
	}

	public static Long getUserProfileID(Context context){
		SharedPreferences prefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		if(AppConstant.userProfile.getProfileID()!=null && AppConstant.userProfile.getProfileID()!=0)
			return AppConstant.userProfile.getProfileID();
		else if(prefs.getLong(AppConstant.Profile_ID, 0)!= 0)
			return prefs.getLong(AppConstant.Profile_ID, 0);
		return Long.valueOf(0);
	}

	public static Long getUserID(Context context){
		SharedPreferences prefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		if(AppConstant.user.getUserId()!=null && AppConstant.user.getUserId()!=0)
			return AppConstant.user.getUserId();
		else if(prefs.getLong(AppConstant.User_ID, 0)!=0)
			return prefs.getLong(AppConstant.User_ID, 0);
		return Long.valueOf(0);
	}

	public static CountryCodes getCountryCodes(Context context , String countryCodes)
	{
		if(countryCodes == null)
			countryCodes = "+91";

		CountryCodes codes = new CountryCodes(); 
		try {
			ArrayList<CountryCodes> ccodes = getAllcountryCodes(context);
			for (int i = 0; i < ccodes.size(); i++) {

				if(countryCodes.equalsIgnoreCase(ccodes.get(i).getISDCode()))
				{
					return ccodes.get(i);
				}
			}

		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		return codes;	

	}

	public static String getCountryCodeFromTelephonyMgr(Context context){
		TelephonyManager telMgr = (TelephonyManager) context.getSystemService(Context.TELEPHONY_SERVICE);
		PhoneNumberUtil phoneUtil = PhoneNumberUtil.getInstance();
		String curLocale = "";
		if(telMgr.getPhoneType() != TelephonyManager.PHONE_TYPE_CDMA){
			curLocale = telMgr.getNetworkCountryIso().toUpperCase();
		}else{
			Class<?> c = null;
			Method get = null;
			String homeOperator = null;
			try {
				c = Class.forName("android.os.SystemProperties");
				get = c.getMethod("get", String.class);
				homeOperator = ((String) get.invoke(c, "ro.cdma.home.operator.numeric"));
			} catch (ClassNotFoundException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			} catch (NoSuchMethodException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			catch (IllegalArgumentException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			} catch (IllegalAccessException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			} catch (InvocationTargetException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}

			curLocale = homeOperator.substring(0, 3); // the last three digits is MNC 
		}
		String curDCode = "+"+phoneUtil.getCountryCodeForRegion(curLocale);
		return curDCode;
	}

	public static Bitmap getResizedBitmap(Bitmap image) {
		int width = image.getWidth();
		int height = image.getHeight();
		int maxSize = 1000;

		if(width > maxSize || height > maxSize){
			width = width/2;
			height = height/2;
		}
		return Bitmap.createScaledBitmap(image, width, height, true);
	}

	public static Bitmap resizeBitmap(Bitmap org, int destWidth, int destHeight) {
		Bitmap blankResized = Bitmap.createBitmap(destWidth, destHeight, Bitmap.Config.ARGB_8888);
		int xPadding = 0;
		int yPadding = 0;
		int scaledWidth = blankResized.getWidth();
		int scaledHeight = blankResized.getHeight();
		float scale = 1;
		if (org.getWidth() > org.getHeight()) {
			scale = (float) blankResized.getWidth() / (float) org.getWidth();
			scaledHeight = (int) (org.getHeight() * scale);
			yPadding = (blankResized.getHeight() - scaledHeight) / 2;
		}
		else {
			scale = (float) blankResized.getHeight() / (float) org.getHeight();
			scaledWidth = (int) (org.getWidth() * scale);
			xPadding = (blankResized.getWidth() - scaledWidth) / 2;
		}

		Bitmap orgScale = Bitmap.createScaledBitmap(org, scaledWidth, scaledHeight, true);
		Canvas canvas = new Canvas(blankResized);
		canvas.drawBitmap(
				orgScale,
				new Rect(0, 0, scaledWidth, scaledHeight),
				new Rect(xPadding, yPadding, xPadding + scaledWidth, yPadding + scaledHeight),
				null);

		return blankResized;
	}

	public static String getUserCountry(Context context) {
		try {
			final TelephonyManager tm = (TelephonyManager) context.getSystemService(Context.TELEPHONY_SERVICE);
			final String simCountry = tm.getSimCountryIso();
			if (simCountry != null && simCountry.length() == 2) { // SIM country code is available
				return simCountry.toLowerCase(Locale.US);
			}
			else if (tm.getPhoneType() != TelephonyManager.PHONE_TYPE_CDMA) { // device is not 3G (would be unreliable)
				String networkCountry = tm.getNetworkCountryIso();
				if (networkCountry != null && networkCountry.length() == 2) { // network country code is available
					return networkCountry.toLowerCase(Locale.US);
				}
			}
		}
		catch (Exception e) { }
		return null;
	}

	public static String getRegionCodeForCountryCode(String countryCode){
		PhoneNumberUtil phoneUtil = PhoneNumberUtil.getInstance();
		if(countryCode!=null && !countryCode.equalsIgnoreCase(""))
			return phoneUtil.getRegionCodeForCountryCode(Integer.parseInt(countryCode));	
		return "IN";

	}

	public static boolean isValidPhoneNumber(String phoneNumber) {

		PhoneNumberUtil phoneUtil = PhoneNumberUtil.getInstance();

		try 
		{
			PhoneNumber numberProto = phoneUtil.parse(phoneNumber, "");
			numberProto = phoneUtil.parse(String.valueOf(numberProto.getNationalNumber()) , phoneUtil.getRegionCodeForCountryCode(numberProto.getCountryCode()));
			return phoneUtil.isValidNumber(numberProto);
		} 
		catch (NumberParseException e) 
		{
			System.err.println("NumberParseException was thrown: " + e.toString());
		}

		return false;

	}
	
	public static void reloadServiceURLs(){
		CLIENT_ID = "000000004010A627";
		GuardianServiceUrl = GuardianV1ServiceUrl;
		guardianPortalLink = GuardianV1PortalUrl;
		//Web Sevices
		LocationServiceUrl = GuardianServiceUrl + "LocationService.svc/";
		GeoServiceUrl = GuardianServiceUrl + "GeoUpdate.svc/";
		GroupServiceUrl = GuardianServiceUrl + "GroupService.svc/";
		MembershipServiceUrl = GuardianServiceUrl + "MembershipService.svc/";
		//Web services Url
	    MembershipServiceSyncUrl = MembershipServiceUrl+ "GetProfilesForLiveId";
		PhoneValidatorUrl = MembershipServiceUrl + "CreatePhoneValidator";
		updateProfile = MembershipServiceUrl + "UpdateProfile";
		CreateProfileUrl = MembershipServiceUrl + "CreateProfile";
		PostMyLocationUrl = GeoServiceUrl + "PostMyLocation";
		StopPostingsUrl = GeoServiceUrl + "StopPostings/";
		StopSOSOnlyUrl = GeoServiceUrl + "StopSOSOnly/";
		PostLocationWithMedia = GeoServiceUrl + "PostLocationWithMedia";
		ReportTeaseUrl = GeoServiceUrl + "ReportIncident";
		UnregisterUrl = MembershipServiceUrl + "UnRegisterUser";
		updatePhoneProfile = MembershipServiceUrl + "UpdateProfilePhone";
		GetUserLocationUrl = LocationServiceUrl + "GetUserLocationArray/{0}/{1}";
		LocateLiveTileUrl = LocationServiceUrl + "GetSOSTrackCount/{0}/{1}";
		LocateBuddiesUrl = LocationServiceUrl + "GetBuddiesToLocateLastLocation/{0}/{1}";
		CheckUpdatesFromServerUrl = MembershipServiceUrl+ "CheckPendingUpdates/";
	}



}
