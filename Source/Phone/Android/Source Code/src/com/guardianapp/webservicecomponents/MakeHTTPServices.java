package com.guardianapp.webservicecomponents;

import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;
import java.util.ArrayList;
import java.util.Date;

import org.json.JSONException;
import org.json.JSONObject;

import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.support.v4.app.Fragment;
import android.util.Log;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.JsonSyntaxException;
import com.guardianapp.R;
import com.guardianapp.database.DBQueries;
import com.guardianapp.model.AscGroups;
import com.guardianapp.model.DataInfo;
import com.guardianapp.model.DeviceSetting;
import com.guardianapp.model.GeoTag;
import com.guardianapp.model.GeoTags;
import com.guardianapp.model.GroupList;
import com.guardianapp.model.IncidentTag;
import com.guardianapp.model.LiveDetails;
import com.guardianapp.model.MyBuddies;
import com.guardianapp.model.Profile;
import com.guardianapp.model.ProfileList;
import com.guardianapp.model.ProfileListLite;
import com.guardianapp.model.VersionUpdate;
import com.guardianapp.services.GPSTracker;
import com.guardianapp.ui.BaseActivity;
import com.guardianapp.ui.ProfileFragment;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.LogUtils;

public class MakeHTTPServices {

	private Forground_Task fgTask;
	private Background_Task backTask;
	Activity act;
	Object currentUIClass;
	Context context;
	public SharedPreferences prefs;

	public MakeHTTPServices(Activity act_context, Fragment frag) {
		this.act = act_context;	
		this.context =act_context;
		prefs = this.context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

		if(frag!=null)
			currentUIClass = (Object)frag;
		else
			currentUIClass = (Object)act;
	}


	public MakeHTTPServices(Context argctx) {
		this.context = argctx;
		prefs = this.context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
	}


	public boolean postMyLocationAsync(byte[] basePic, String command) throws JSONException {
		AppConstant.service_Tag = AppConstant.POST_LOCATION_WITH_MEDIA_SERVICE_TAG;
		GeoTag gTag = new GeoTag();
		HttpParam header = new HttpParam();
		ArrayList <HttpParam>  params = new ArrayList<HttpParam>();
		int index = GPSTracker.GeoTagList.size()-1;
		Gson gson = new Gson();
		JSONObject jObject = null; 

		if(GPSTracker.GeoTagList.size()>0)
		{
			gTag.setAlt(GPSTracker.GeoTagList.get(index).getAlt());
			gTag.setLat(GPSTracker.GeoTagList.get(index).getLat());
			gTag.setLong(GPSTracker.GeoTagList.get(index).getLong());
			gTag.setSpeed(GPSTracker.GeoTagList.get(index).getSpeed());
			gTag.setTimeStamp(GPSTracker.GeoTagList.get(index).getTimeStamp());
		}else{
			gTag.setAlt("");
			gTag.setLat("");
			gTag.setLong("");
			gTag.setSpeed(0);
			gTag.setTimeStamp(AppConstant.getTicks_time((new Date()).getTime()));
		}

		gTag.setProfileID(AppConstant.getUserProfileID(this.context));
		gTag.setSessionID(AppConstant.SessionToken());
		gTag.setGeoDirection("1");

		if(basePic!=null)
			gTag.setMediaContent(basePic);
		else
			gTag.setMediaContent(null);
		gTag.setAdditionalInfo(null);
		gTag.setCommand(command);
		gTag.setMediaUri(null);
		gTag.setGroupID(null);
		String gsonString=gson.toJson(gTag);
		if(basePic!=null)
		{
			String UnsignedString= AppConstant.convertToUnsignedString(gsonString);
			String finalString =gsonString.replace(gsonString.substring(gsonString.indexOf('['), gsonString.indexOf(']')), UnsignedString);
			jObject = new JSONObject(finalString);
		}
		else
		{
			jObject = new JSONObject(gsonString);
		}
		header.setKey("AuthID");
		header.setValue(prefs.getString(AppConstant.Authentication_Token, ""));
		params.add(header);

		header = new HttpParam();
		header.setKey("Content-type");
		header.setValue("application/json");
		params.add(header);


		fgTask = new Forground_Task(this.context,params, currentUIClass , "POST",jObject);
		fgTask.execute(AppConstant.PostLocationWithMedia);

		return false;

	}

	public String tinyUrlServiceCall() {
		AppConstant.service_Tag = AppConstant.TINY_URL_SERVICE_TAG;
		HttpParam header = new HttpParam();	
		ArrayList <HttpParam>  params = new ArrayList<HttpParam>();

		header = new HttpParam();
		header.setKey("Cache-Control");
		header.setValue("no-cache;");
		params.add(header);

		fgTask = new Forground_Task(this.context,params, currentUIClass , "GET",null);
		String url = AppConstant.ShortUrlServiceUrl;
		fgTask.execute(url);

		return null;

	}

	public String ReportTeaseToServer(byte[] basePic,String command, String additionalInfo) throws JSONException 
	{
		AppConstant.service_Tag = AppConstant.REPORT_AN_INCIDENT_SERVICE_TAG;
		IncidentTag gTag = new IncidentTag();
		HttpParam header = new HttpParam();
		ArrayList <HttpParam>  params = new ArrayList<HttpParam>();
		int index = GPSTracker.GeoTagList.size()-1;
		Gson gson = new Gson();

		if(GPSTracker.GeoTagList.size()>0)
		{
			gTag.setName(AppConstant.userProfile.getName());
			gTag.setMobileNumber(AppConstant.userProfile.getMobileNumber());
			gTag.setAlt(GPSTracker.GeoTagList.get(index).getAlt());
			gTag.setLat(GPSTracker.GeoTagList.get(index).getLat());
			gTag.setLong(GPSTracker.GeoTagList.get(index).getLong());
			gTag.setProfileID(AppConstant.getUserProfileID(this.context));
			gTag.setSessionID(AppConstant.TokenId);
			gTag.setGeoDirection("1");
			gTag.setSpeed(GPSTracker.GeoTagList.get(index).getSpeed());
			gTag.setTimeStamp(GPSTracker.GeoTagList.get(index).getTimeStamp());
			gTag.setCommand(command);
			gTag.setMediaContent(basePic);
			gTag.setAdditionalInfo(additionalInfo);

			String gsonString=gson.toJson(gTag);
			String UnsignedString= AppConstant.convertToUnsignedString(gsonString);
			String finalString =gsonString.replace(gsonString.substring(gsonString.indexOf('['), gsonString.indexOf(']')), UnsignedString);

			JSONObject jObject = new JSONObject(finalString);
			header.setKey("AuthID");
			header.setValue(prefs.getString(AppConstant.Authentication_Token, ""));
			params.add(header);

			header = new HttpParam();
			header.setKey("Content-type");
			header.setValue("application/json");
			params.add(header);


			fgTask = new Forground_Task(this.context,params, currentUIClass , "POST",jObject);
			AppConstant.showProgressDialogWithMessage(context , context.getString(R.string.post_photo_to_server));
			fgTask.execute(AppConstant.ReportTeaseUrl);
		}
		return null;

	}

	public String stopPosting(String TokenId) {
		HttpParam header = new HttpParam();
		ArrayList <HttpParam>  params = new ArrayList<HttpParam>();

		header.setKey("AuthID");
		header.setValue(prefs.getString(AppConstant.Authentication_Token, ""));
		params.add(header);

		header = new HttpParam();
		header.setKey("Cache-Control");
		header.setValue("no-cache;");
		params.add(header);

		String url = AppConstant.StopPostingsUrl+AppConstant.getUserProfileID(this.context)+"/"
				+AppConstant.SessionToken(context)+"/"+AppConstant.getTicks_time(new Date().getTime());

		fgTask = new Forground_Task(this.context,params, currentUIClass , "GET",null);
		fgTask.execute(String.format(url));

		return TokenId;

	}

	public String stopSOSOnly(String TokenId) {
		HttpParam header = new HttpParam();
		ArrayList <HttpParam>  params = new ArrayList<HttpParam>();

		header.setKey("AuthID");
		header.setValue(prefs.getString(AppConstant.Authentication_Token, ""));
		params.add(header);

		header = new HttpParam();
		header.setKey("Cache-Control");
		header.setValue("no-cache;");
		params.add(header);

		String url = AppConstant.StopSOSOnlyUrl+AppConstant.getUserProfileID(this.context)+"/"
				+AppConstant.SessionToken(context)+"/"+AppConstant.getTicks_time(new Date().getTime());

		fgTask = new Forground_Task(this.context,params, currentUIClass , "GET",null);
		fgTask.execute(String.format(url));

		return TokenId;

	}


	public String postLocationArray() throws JSONException {
		GeoTags gTags = new GeoTags();
		HttpParam header = new HttpParam();
		ArrayList <HttpParam>  params = new ArrayList<HttpParam>();

		if(GPSTracker.GeoTagList.size()!=0)
		{
			Gson gson = new Gson();

			int maxCount = GPSTracker.GeoTagList.size();
			int index = AppConstant.fromIndex_PostLocation;
			int count = maxCount - index;

			if (maxCount != 0 && count != 0)
			{

				for(int i = index; i < maxCount; i++)
				{
					gTags.getAlt().add(GPSTracker.GeoTagList.get(i).getAlt());
					gTags.getLat().add(GPSTracker.GeoTagList.get(i).getLat());
					gTags.getLong().add(GPSTracker.GeoTagList.get(i).getLong());
					if(AppConstant.userProfile.isIsSOSOn()){
						gTags.getIsSOS().add(true);
					}else if(AppConstant.userProfile.isIsTrackingOn()){
						gTags.getIsSOS().add(false);
					}
					gTags.getTS().add(""+GPSTracker.GeoTagList.get(i).getTimeStamp());
					gTags.getSpd().add(""+GPSTracker.GeoTagList.get(i).getSpeed());
					gTags.setGroupID(GPSTracker.GeoTagList.get(i).getGroupID());

				}
				gTags.setCmd(AppConstant.isStartPushPin ? "B" : "E");
				gTags.setId(AppConstant.userProfile.getSessionToken());
				gTags.setLocCnt(count);
				gTags.setPID(AppConstant.getUserProfileID(this.context));
				JSONObject jObject = new JSONObject(gson.toJson(gTags));

				header.setKey("AuthID");
				header.setValue(prefs.getString(AppConstant.Authentication_Token, ""));
				params.add(header);

				header = new HttpParam();
				header.setKey("Content-type");
				header.setValue("application/json");
				params.add(header);


				backTask = new Background_Task(params, currentUIClass , "POST",jObject);
				backTask.execute(AppConstant.PostMyLocationUrl);
			}

			AppConstant.fromIndex_PostLocation = maxCount;

		}


		return null;

	}

	public String GetLocateMembers()
	{
		ArrayList <HttpParam> params=new ArrayList<HttpParam>();
		AppConstant.locateSyncServices= AppConstant.GET_LOCATE_BUDDIES_TAG;
		HttpParam header= new HttpParam();
		header.setKey("AuthID");
		header.setValue(prefs.getString(AppConstant.Authentication_Token, ""));
		params.add(header);
		backTask = new Background_Task(params, currentUIClass , "GET",null);
		String url = AppConstant.LocationServiceUrl+"GetBuddiesToLocateLastLocation/"+AppConstant.getUserID(this.context)+"/"+AppConstant.getTicks_time(new Date().getTime());

		backTask.execute(url);
		return null;
	}

	public String GetLocateLiveTileCountAsync()
	{
		ArrayList <HttpParam> params=new ArrayList<HttpParam>();
		AppConstant.locateSyncServices= AppConstant.GET_SOSTRACK_COUNT_TAG;
		HttpParam header= new HttpParam();
		header.setKey("AuthID");
		header.setValue(prefs.getString(AppConstant.Authentication_Token, ""));
		params.add(header);
		backTask = new Background_Task(params, currentUIClass , "GET",null);
		String url = AppConstant.LocationServiceUrl+"GetSOSTrackCount/"+AppConstant.getUserID(this.context)+"/"+AppConstant.getTicks_time(new Date().getTime());
		backTask.execute(url);
		return null;
	}


	public  String BingSearchUrl(String type,String location,String MaxResults ,Boolean IsLocal, String latitute, String longitute) throws UnsupportedEncodingException
	{

		StringBuilder requestUrl = new StringBuilder();
		requestUrl.append("https://dev.virtualearth.net/services/v1/SearchService/SearchService.asmx/Search2?");
		requestUrl.append("count=" + MaxResults);
		requestUrl.append("&startingIndex=" + 0);
		requestUrl.append("&mapBounds=");
		requestUrl.append("&locationcoordinates=");
		if (IsLocal)
			requestUrl.append( URLEncoder.encode("\"" + latitute + ", " + longitute + "\"", "UTF-8"));

		requestUrl.append("&entityType=");
		requestUrl.append(URLEncoder.encode("\"Business\"", "UTF-8"));
		requestUrl.append("&sortorder=");
		requestUrl.append("&query=");
		requestUrl.append("&location=");
		if (location!=null&&location.equalsIgnoreCase(""))
			requestUrl.append( URLEncoder.encode("\""+location+"\"", "UTF-8"));
		requestUrl.append("&keyword=");
		if (type!=null&&!type.equalsIgnoreCase(""))
			requestUrl.append(URLEncoder.encode("\"" + type + "\"", "UTF-8"));
		requestUrl.append("&jsonso=r229");
		requestUrl.append("&jsonp=microsoftMapsNetworkCallback");
		requestUrl.append("&culture=");
		requestUrl.append(URLEncoder.encode("\"en-us\"", "UTF-8"));
		requestUrl.append("&token=" + AppConstant.BingMapsKey);
		return requestUrl.toString();
	}

	public String GetNearByHelp() throws UnsupportedEncodingException {
		//CaLL Services for get near police station.
		//GPSTracker gpsTracker = new GPSTracker(context);
		String currentlat = String.valueOf(GPSTracker.latitude);
		String currentLong =  String.valueOf(GPSTracker.longitude);


		AppConstant.service_Tag = AppConstant.GET_LOCAL_HELP_TAG;
		String type = "police station";
		String requestUrl = BingSearchUrl(type, "", AppConstant.MaxGetLocalHelpResults,true,currentlat,currentLong).replace("%2C+", ",");
		fgTask = new Forground_Task(this.context,null, currentUIClass , "GET",null);
		fgTask.execute(requestUrl);

		//CaLL Services for get near Hospital.

		AppConstant.service_Tag = AppConstant.GET_LOCAL_HELP_TAG;
		type = "hospital";
		requestUrl = BingSearchUrl(type, "", AppConstant.MaxGetLocalHelpResults,true,currentlat,currentLong).replace("%2C+", ","); 
		fgTask = new Forground_Task(this.context,null, currentUIClass , "GET",null);
		fgTask.execute(requestUrl);

		return requestUrl;

	}


	public String searchLocation(String location) throws UnsupportedEncodingException {
		AppConstant.service_Tag = AppConstant.GET_SEARCH_LOCATION_SERVICE_TAG;
		String currentlat = String.valueOf(GPSTracker.latitude);
		String currentLong =  String.valueOf(GPSTracker.longitude);

		String type = location;
		String requestUrl = BingSearchUrl(type, "", AppConstant.MaxGetLocalHelpResults,true,currentlat,currentLong).replace("%2C+", ",");
		fgTask = new Forground_Task(this.context,null, currentUIClass , "GET",null);
		fgTask.execute(requestUrl);

		/*	//CaLL Services for get near Hospital.

			 	 	 type = "hospital";
			 	 	 requestUrl = BingSearchUrl(type, "", AppConstant.MaxGetLocalHelpResults,true,currentlat,currentLong).replace("%2C+", ","); 
			         bTask = new Background_Task(null, currentUIClass , "GET",null);
					 bTask.execute(requestUrl);*/
		return null;

	}

	public String getListOfGroups(String searchKey)
	{
		if(searchKey.equalsIgnoreCase(""))
			searchKey = "all";

		ArrayList <HttpParam>  params = new ArrayList<HttpParam>();
		AppConstant.service_Tag = AppConstant.GROUP_SERVICE_TAG;
		HttpParam header = new HttpParam();
		header.setKey("AuthID");
		header.setValue(prefs.getString(AppConstant.Authentication_Token, ""));
		params.add(header);
		fgTask = new Forground_Task(this.context,params, currentUIClass , "GET",null);
		String url = AppConstant.GroupServiceUrl+"GetListOfGroups/"+searchKey;
		fgTask.execute(url);


		return searchKey;


	}

	public String getListOfFBGroups(String url)
	{
		ArrayList <HttpParam>  params = new ArrayList<HttpParam>();
		AppConstant.service_Tag = AppConstant.FB_GROUPS_SERVICE_TAG;
		fgTask = new Forground_Task(this.context,params, currentUIClass , "GET",null);
		fgTask.execute(url);
		return url;

	}

	public String postToFBGroups(String groupId, String post){
		ArrayList <HttpParam>  params = new ArrayList<HttpParam>();
		AppConstant.service_Tag = AppConstant.POST_TO_FB_GROUP_SERVICE_TAG;
		HttpParam header = new HttpParam();
		header.setKey("Accept");
		header.setValue("application/json");
		params.add(header);
		header.setKey("Content-type");
		header.setValue("application/json");
		params.add(header);
		String url = AppConstant.strPostOnGroup+groupId+"/feed?message="+post+"&access_token="+prefs.getString("FBAccessToken", "");
		url = url.replace(" ", "%20");
		fgTask = new Forground_Task(this.context, params, currentUIClass , "POST",null);
		fgTask.execute(url);
		return post;
	}



	//This Service for create and edit new and existing profile
	public String createProfileServices(String userName, String countryCode, String mobileNo, 
			String securityCode) throws JSONException {

		ArrayList <HttpParam>  params = new ArrayList<HttpParam>();
		AppConstant.service_Tag = AppConstant.CREATE_PROFILE_SERVICE_TAG;
		HttpParam header = new HttpParam();

		if(!mobileNo.contains(countryCode))
			mobileNo = countryCode+mobileNo;
		//ProfileList profileList = new ProfileList();

		Profile profile  = new Profile();

		profile.setLiveDetails(new LiveDetails());
		profile.setPhoneSetting(new DeviceSetting());

		profile.getLiveDetails().setLiveAccessToken(prefs.getString(AppConstant.Access_Token, ""));
		profile.getLiveDetails().setLiveRefreshToken(prefs.getString(AppConstant.Referesh_Token, ""));
		profile.setEmail(prefs.getString(AppConstant.LiveEmail_id, ""));
		profile.setName(userName);
		profile.setRegionCode(countryCode);
		profile.setMobileNumber(mobileNo);
		profile.setSecurityToken(securityCode);

		profile.getPhoneSetting().setCanEmail(false);
		profile.getPhoneSetting().setCanSMS(true);
		profile.getPhoneSetting().setPlatForm(AppConstant.DEVICE_PLATFORM);

		if(AppConstant.userProfile.isCanSMS())
			profile.setCanSMS(true);
		else
			profile.setCanSMS(false);

		if(AppConstant.userProfile.isCanPost())
			profile.setCanPost(true);
		else
			profile.setCanPost(false);

		if(AppConstant.userProfile.isLocationConsent())
			profile.setLocationConsent(true);
		else
			profile.setLocationConsent(false);

		if(AppConstant.userProfile.isCanMail())
			profile.setCanMail(true);
		else
			profile.setCanMail(false);

		profile.setSMSText(AppConstant.ReachSupportMessageText);

		DBQueries query = new DBQueries(context);
		if(AppConstant.globalBuddies!=null)
			profile.setMyBuddies(query.SelectAllBuddies());
		else
			profile.setMyBuddies(new ArrayList<MyBuddies>());	

		if(AppConstant.ascGroups!=null)
			profile.setAscGroups(query.SelectAllAsgGroups());
		else
			profile.setAscGroups(new ArrayList<AscGroups>());

		if(AppConstant.isNumberISUpdate)
		{
			profile.setProfileID(AppConstant.getUserProfileID(this.context));
			profile.setUserID(AppConstant.getUserID(this.context));
			profile.getPhoneSetting().setProfileID(AppConstant.getUserProfileID(this.context));
		}

		Gson gson = new Gson();

		JSONObject jObject = new JSONObject(gson.toJson(profile));

		header.setKey("AuthID");
		header.setValue(prefs.getString(AppConstant.Authentication_Token, ""));
		params.add(header);

		header = new HttpParam();
		header.setKey("Content-type");
		header.setValue("application/json");
		params.add(header);
		AppConstant.showProgressDialogWithMessage(act, "Creating your profile.Please wait....");
		fgTask = new Forground_Task(this.context, params, currentUIClass , "POST",jObject);

		if(AppConstant.isNumberISUpdate)
		{
			fgTask.execute(AppConstant.updatePhoneProfile);
		}
		else
		{

			fgTask.execute(AppConstant.CreateProfileUrl);

		}

		return null;

	}

	public String updateProfileService() throws JSONException {

		ArrayList <HttpParam>  params = new ArrayList<HttpParam>();
		AppConstant.service_Tag = AppConstant.UPDATE_PROFILE_SERVICE_TAG;
		HttpParam header = new HttpParam();
		String mobileNo,countryCode;
		countryCode = prefs.getString(AppConstant.Ragister_countryCode, "");
		if(prefs.getString(AppConstant.Ragister_mobile, "").contains(countryCode))
		{
			mobileNo = prefs.getString(AppConstant.Ragister_mobile, "");

		}
		else
		{
			mobileNo = countryCode +prefs.getString(AppConstant.Ragister_mobile, "");
		}

		//ProfileList profileList = new ProfileList();

		Profile profile  = new Profile();
		profile.setProfileID(AppConstant.getUserProfileID(this.context));
		profile.setLiveDetails(new LiveDetails());
		profile.setPhoneSetting(new DeviceSetting());

		profile.getLiveDetails().setLiveAccessToken(prefs.getString(AppConstant.Access_Token, ""));
		profile.getLiveDetails().setLiveRefreshToken(prefs.getString(AppConstant.Referesh_Token, ""));
		profile.setEmail(prefs.getString(AppConstant.LiveEmail_id, ""));
		profile .setName(AppConstant.localUserName);//phani
		profile.setRegionCode(countryCode);
		profile.setMobileNumber(mobileNo);
		profile.setSecurityToken("");
		profile.setCanMail(AppConstant.userProfile.isCanMail());

		profile.getPhoneSetting().setCanEmail(false);
		profile.getPhoneSetting().setCanSMS(true);
		profile.getPhoneSetting().setPlatForm(AppConstant.DEVICE_PLATFORM);
		profile.getPhoneSetting().setProfileID(AppConstant.getUserProfileID(this.context));

		if(AppConstant.userProfile.isCanSMS())
			profile.setCanSMS(true);
		else
			profile.setCanSMS(false);

		if(AppConstant.userProfile.isCanPost())
			profile.setCanPost(true);
		else
			profile.setCanPost(false);
		profile.setLocationConsent(AppConstant.userProfile.isLocationConsent());
		profile.setSMSText(AppConstant.ReachSupportMessageText);
		profile.setUserID(AppConstant.getUserID(this.context));
		DBQueries query = new DBQueries(context);

		//ArrayList<AscGroups>asg = query.SelectAllAsgGroups();     
		profile.setAscGroups(query.SelectAllAsgGroups());

		if(AppConstant.globalBuddies!=null)
			profile.setMyBuddies(query.SelectAllBuddies());
		else
			profile.setMyBuddies(new ArrayList<MyBuddies>());	

		Gson gson = new Gson();

		JSONObject jObject = new JSONObject(gson.toJson(profile));



		header.setKey("AuthID");
		header.setValue(prefs.getString(AppConstant.Authentication_Token, ""));
		params.add(header);

		header = new HttpParam();
		header.setKey("Content-type");
		header.setValue("application/json");
		params.add(header);

		fgTask = new Forground_Task(this.context, params, currentUIClass , "POST",jObject);
		AppConstant.showProgressDialogWithMessage(context, "Updating your details.Please wait...");
		fgTask.execute(AppConstant.updateProfile);

		return null;

	}
	//This Service for Genrate Security key and validate to mobile number
	public String validateMobileNumberSyrice(String userName, String countryCode, String mobileNo) throws JSONException{

		ArrayList <HttpParam>  params = new ArrayList<HttpParam>();
		AppConstant.service_Tag = AppConstant.MOBILE_VALIDATION_SERVICE_TAG;
		HttpParam header = new HttpParam();

		mobileNo = countryCode+mobileNo;

		JSONObject jObject = new JSONObject();
		jObject.put("AuthenticatedLiveID",prefs.getString(AppConstant.LiveEmail_id, "") );
		jObject.put("Name", userName);
		jObject.put("PhoneNumber", mobileNo);
		jObject.put("RegionCode", countryCode);



		header.setKey("AuthID");
		header.setValue(prefs.getString(AppConstant.Authentication_Token, ""));
		params.add(header);

		header = new HttpParam();
		header.setKey("Content-type");
		header.setValue("application/json");
		params.add(header);
		AppConstant.showProgressDialogWithMessage(context, "Validating your mobile number....");
		fgTask = new Forground_Task(this.context,params, currentUIClass , "POST",jObject);

		fgTask.execute(AppConstant.PhoneValidatorUrl);

		return null;	
	}

	public String UnRegisterUser() {
		ArrayList <HttpParam> params=new ArrayList<HttpParam>();
		AppConstant.service_Tag= AppConstant.UNREGISTER_SERVICE_TAG;
		HttpParam header= new HttpParam();
		header.setKey("AuthID");
		header.setValue(prefs.getString(AppConstant.Authentication_Token, ""));
		params.add(header);

		header = new HttpParam();
		header.setKey("Cache-Control");
		header.setValue("no-cache;");
		params.add(header);

		fgTask = new Forground_Task(this.context,params, currentUIClass , "GET",null);
		String url = AppConstant.UnregisterUrl;
		AppConstant.showProgressDialogWithMessage(context, "Unregistering your profile.Please wait...");
		fgTask.execute(url);
		return null;
	}

	//this service is for create new profile from live Aouth ID
	public String membershipServiceUrl() {

		ArrayList <HttpParam>  params = new ArrayList<HttpParam>();
		AppConstant.service_Tag = AppConstant.MEMBERSHIP_SERVICE_TAG;
		HttpParam header = new HttpParam();
		header.setKey("AuthID");
		header.setValue(prefs.getString(AppConstant.Authentication_Token, ""));
		params.add(header);
		fgTask = new Forground_Task(this.context,params, currentUIClass , "GET",null);
		AppConstant.showProgressDialogWithMessage(context, "Loading your profile details. Please wait....");
		fgTask.execute(AppConstant.MembershipServiceSyncUrl);
		return null;
	}

	public String checkPendingUpdatesFromServer(){
		ArrayList <HttpParam>  params = new ArrayList<HttpParam>();
		AppConstant.service_Tag = AppConstant.CHECK_UPDATES_FROM_SERVER_TAG;
		HttpParam header = new HttpParam();
		header.setKey("AuthID");
		header.setValue(prefs.getString(AppConstant.Authentication_Token, ""));
		params.add(header);
		fgTask = new Forground_Task(this.context,params, currentUIClass , "GET",null);
		AppConstant.showProgressDialogWithMessage(context, "Checking for updates...");
		String url = AppConstant.CheckUpdatesFromServerUrl+AppConstant.getUserProfileID(this.context)+"/"+AppConstant.getTicks_time(new Date().getTime());
		fgTask.execute(url);
		return null;
	}


	public class Background_Task extends AsyncTask<String, Integer, String[]>{


		ArrayList <HttpParam> URL_Params = new ArrayList<HttpParam>();
		public OnTaskCompleted callback;
		public Activity activity;
		public Fragment fragment;
		Context context;
		public ServiceWrapper serviceWrapper;
		public JSONObject jsonObj;

		private String method;




		public Background_Task(ArrayList<HttpParam> URL_Params,Object obj,
				String argMethod, JSONObject jObject) {
			this.URL_Params = URL_Params;
			/*this.activity = act;
    			this.fragment = frag;*/
			this.callback = (OnTaskCompleted)obj;
			this.method = argMethod;
			this.jsonObj = jObject;
			serviceWrapper = new ServiceWrapper();
		}



		@Override
		protected void onPostExecute(String[] result) {
			// check is dialog open ? THEN HIDE DIALOG
			super.onPostExecute(result);
			try {
				Log.i("onPOST", "DONE");
				Log.i("onPOST", "" + result);


				if(act!=null)
				{
					if(result[AppConstant.HTTP_RESPONSE]!=null)
					{

						MakeHTTPServices mks = new MakeHTTPServices(act.getApplicationContext());
						Object obj = mks.handleSyncResp(result[AppConstant.HTTP_RESPONSE]);

						if(obj!=null)
							callback.onGetObjectResult(obj);
						else
							callback.onTaskComplete(result[AppConstant.HTTP_RESPONSE]);  
					}else if(result[AppConstant.HTTP_RESULT]!=null){
						AppConstant.dismissProgressDialog();

					}
				}



				/* if(AppConstant.service_Tag==AppConstant.CREATE_PROFILE_SERVICE_TAG)
					   {

					   }*/
			} catch (Exception e) {
				Log.e("onPost", "" + e);

			}

		}



		@Override
		protected void onPreExecute() {
			// TODO Auto-generated method stub
			super.onPreExecute();
		}




		@Override
		protected String[] doInBackground(String... url) {

			String[] resp = null;
			try {
				// this will send req in post
				// here [0] mean URL & passing params
				Log.i("onDO", "call");
				resp = serviceWrapper.makeHttpRequest(url[0], method, URL_Params,jsonObj);

			} catch (Exception e) {
				// TODO: handle exception
				Log.e("onDo", "" + e);

			}

			return resp;

		}

	}



	private class Forground_Task extends AsyncTask<String, Integer, String[]>{


		private ArrayList <HttpParam> URL_Params = new ArrayList<HttpParam>();
		private OnTaskCompleted callback;
		private Activity activity;
		private Fragment fragment;
		private Context con;
		private ServiceWrapper serviceWrapper;
		private JSONObject jsonObj;
		private String method;

		public Forground_Task(Context context, ArrayList<HttpParam> URL_Params,Object obj,
				String argMethod, JSONObject jObject) {
			this.URL_Params = URL_Params;
			/*this.activity = act;
    			this.fragment = frag;*/
			this.con = context;
			this.callback = (OnTaskCompleted)obj;
			this.method = argMethod;
			this.jsonObj = jObject;
			serviceWrapper = new ServiceWrapper();
		}



		@Override
		protected void onPostExecute(String[] result) {
			super.onPostExecute(result);
			try {
				if(act!=null)
				{
					if(result[AppConstant.HTTP_RESPONSE]!=null && result[AppConstant.HTTP_RESULT].equalsIgnoreCase("true"))
					{
						MakeHTTPServices mks = new MakeHTTPServices(act.getApplicationContext());
						Object obj = mks.handleResponse(result[AppConstant.HTTP_RESPONSE]);
						if(obj!=null)
							callback.onGetObjectResult(obj);
						else
							callback.onTaskComplete(result[AppConstant.HTTP_RESPONSE]);  
					}else if(result[AppConstant.HTTP_RESPONSE]!=null && result[AppConstant.HTTP_RESPONSE].contains("400")&&result[AppConstant.HTTP_RESULT].equalsIgnoreCase("false")){
						AppConstant.dismissProgressDialog();
						if(AppConstant.service_Tag == AppConstant.UPDATE_PROFILE_SERVICE_TAG){
							DBQueries db = new DBQueries(this.con);
							db.updateStatusOfGroupsMarkedForDeletion();
							updateProfileService();
						}
					}else if(result[AppConstant.HTTP_RESULT]!=null && result[AppConstant.HTTP_RESULT].equalsIgnoreCase("false")){
						AppConstant.dismissProgressDialog();
					}else if(result[AppConstant.HTTP_RESULT]!=null && result[AppConstant.HTTP_RESULT].equalsIgnoreCase("No network available!")){
						AppConstant.dismissProgressDialog();
						if(BaseActivity.isWindowFocused)
							Toast.makeText(con, "Please check your internet connection", Toast.LENGTH_LONG).show();
					}

				}


			}
			catch (NumberFormatException nfe){
				AppConstant.dismissProgressDialog();
				callback.onGetObjectResult(nfe);
			}
			catch (Exception e) {
				Log.e("onPost", "" + e);
				AppConstant.dismissProgressDialog();
			}

		}



		@Override
		protected void onPreExecute() {
			// TODO Auto-generated method stub
			super.onPreExecute();
		}




		@Override
		protected String[] doInBackground(String... url) {

			String[] resp = null;
			try {

				Log.i("onDO", "call");

				//if(AppConstant.hasActiveInternetConnection(con)){
				resp = serviceWrapper.makeHttpRequest(url[0], method, URL_Params,jsonObj);
				/*	}else
				{
					String[] res = new String[2];	
					res[AppConstant.HTTP_RESULT]="No network available!";
					resp = res;
				}
				 */

			} catch (Exception e) {

				//AppConstant.dismissProgressDialog();
				Log.e("onDo", "" + e);

			}

			return resp;

		}

	}



	public Object handleResponse(String response) {

		Gson gson = new Gson();
		switch (AppConstant.service_Tag) {

		case AppConstant.MEMBERSHIP_SERVICE_TAG:

			ProfileList profileList = new ProfileList();
			try{
				profileList = gson.fromJson(response,ProfileList.class );
			}catch (JsonSyntaxException jse) {
				// TODO: handle exception
				if(jse.getCause() instanceof NumberFormatException){
					AppConstant.dismissProgressDialog();
					LogUtils.LOGE(LogUtils.makeLogTag(MakeHTTPServices.class), jse.getLocalizedMessage());
					LogUtils.LOGE(LogUtils.makeLogTag(MakeHTTPServices.class), jse.getLocalizedMessage());
					AppConstant.UnregisterLocally(context);
					AppConstant.userProfile =AppConstant.CreateDefaultProfile(this.context);
					throw new NumberFormatException();
				}
			}catch (Exception e){
				LogUtils.LOGE(LogUtils.makeLogTag(MakeHTTPServices.class), e.getLocalizedMessage());
			}
			DBQueries dbQueries = new DBQueries(context);

			if(profileList.getList()!=null&&profileList.getList().size()>0)
			{
				if(dbQueries.storeIntoDB(profileList).equalsIgnoreCase(DBQueries.EXECUTE_SUCCESS))
				{
					prefs = this.context.getSharedPreferences(
							AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
					SharedPreferences.Editor edit = prefs.edit();
					edit.putBoolean(AppConstant.isProfileDone,true);
					edit.putString(AppConstant.Last_profileSyncTime, AppConstant.getCurrentDateTime());
					if(profileList.getList().get(0).getRegionCode()!=null && !profileList.getList().get(0).getRegionCode().trim().equalsIgnoreCase(""))
						edit.putString(AppConstant.Ragister_countryCode, profileList.getList().get(0).getRegionCode());
					else if(AppConstant.userProfile.getCountryCode()!=null && !AppConstant.userProfile.getCountryCode().trim().equalsIgnoreCase(""))
						edit.putString(AppConstant.Ragister_countryCode, AppConstant.userProfile.getCountryCode());
					else
						edit.putString(AppConstant.Ragister_countryCode, "+91");
					edit.putString(AppConstant.Ragister_mobile,profileList.getList().get(0).getMobileNumber());
					edit.putString(AppConstant.Live_user_name,profileList.getList().get(0).getName());
					edit.commit();

					try{
						dbQueries.Load2CurrentProfile();
						dbQueries.loadUserData();
						AppConstant.isRagister = true;
						AppConstant.globalBuddies=dbQueries.SelectAllBuddies();
						AppConstant.ascGroups=dbQueries.SelectAllAsgGroups();
						AppConstant.userProfile.setAscGroups(AppConstant.ascGroups);
						if(AppConstant.globalBuddies!=null)
						{

							AppConstant.userProfile.setMyBuddies(AppConstant.globalBuddies);
						}
						Toast.makeText(context, profileList.getDataInfo().get(0).getMessage(), Toast.LENGTH_LONG).show();
					}catch(NumberFormatException nfe){
						LogUtils.LOGE(LogUtils.makeLogTag(MakeHTTPServices.class), nfe.getLocalizedMessage());
						//AppConstant.reloadServiceURLs();
						//this.membershipServiceUrl();
					}catch(Exception e){
						LogUtils.LOGE(LogUtils.makeLogTag(MakeHTTPServices.class), e.getLocalizedMessage());
					}
				}
				else
				{
					Toast.makeText(context, "Unable To create profile ", Toast.LENGTH_LONG).show();
				}


			}
			else
			{
				prefs = this.context.getSharedPreferences(
						AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
				SharedPreferences.Editor edit = prefs.edit();
				edit.putBoolean(AppConstant.isProfileDone,false);
				edit.commit();
			}

			//AppConstant.dismissProgressDialog();
			return profileList;

		case AppConstant.CREATE_PROFILE_SERVICE_TAG:	

		case AppConstant.UPDATE_PROFILE_SERVICE_TAG:
			Profile profile = new Profile();
			profile = gson.fromJson(response,Profile.class );

			DBQueries dbqueries = new DBQueries(context);

			if(profile!=null && (profile.getDataInfo().get(0).getResultType()==3 || profile.getDataInfo().get(0).getResultType()==5)){
				return profile;
			}

			if(dbqueries.storeIntoDB(profile).equalsIgnoreCase(DBQueries.EXECUTE_SUCCESS))
			{
				prefs = this.context.getSharedPreferences(
						AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

				SharedPreferences.Editor edit = prefs.edit();
				edit.putBoolean(AppConstant.isProfileDone,true);
				edit.putString(AppConstant.Last_profileSyncTime, AppConstant.getCurrentDateTime());
				edit.putString(AppConstant.Ragister_countryCode, profile.getRegionCode());
				edit.putString(AppConstant.Ragister_mobile,profile.getMobileNumber());
				edit.putString(AppConstant.Live_user_name,profile.getName());
				edit.commit();

				Toast.makeText(context, profile.getDataInfo().get(0).getMessage(), Toast.LENGTH_LONG).show();
				//phani
				dbqueries.Load2CurrentProfile();
				dbqueries.loadUserData();
				dbqueries.cleanBuddies();
				dbqueries.cleanGroups();
				AppConstant.globalBuddies=dbqueries.SelectAllBuddies();
				AppConstant.userProfile.setMyBuddies(AppConstant.globalBuddies);
				AppConstant.ascGroups=dbqueries.SelectAllAsgGroups();
				AppConstant.userProfile.setAscGroups(AppConstant.ascGroups);

			}
			else
			{
				Toast.makeText(context, "Unable To create profile ", Toast.LENGTH_LONG).show();
			}

			//AppConstant.dismissProgressDialog();	
			return profile;

		case AppConstant.GROUP_SERVICE_TAG:
			GroupList groupList = new GroupList();
			groupList = gson.fromJson(response,GroupList.class );
			return groupList;


		case AppConstant.GET_LOCATE_BUDDIES_TAG:
			ProfileListLite profileListLite=new ProfileListLite();
			profileListLite=gson.fromJson(response, ProfileListLite.class);
			return profileListLite;



		case AppConstant.TINY_URL_SERVICE_TAG:	
			AppConstant.TINY_URL = response;
			return null;

		case AppConstant.MOBILE_VALIDATION_SERVICE_TAG:

			try {

				JSONObject json = new JSONObject(response);
				String msg  = json.getJSONArray("DataInfo").getJSONObject(0).getString("Message").toString();
				Toast.makeText(context, msg, Toast.LENGTH_LONG).show();

			} catch (JSONException e) {

				e.printStackTrace();
			}



			return null;

		case AppConstant.UNREGISTER_SERVICE_TAG:	
			DataInfo resultInfo=new DataInfo();
			resultInfo=gson.fromJson(response, DataInfo.class);
			if(resultInfo.getResultType()!=5)
			{
				AppConstant.UnregisterLocally(context);
				AppConstant.userProfile =AppConstant.CreateDefaultProfile(this.context);
				Toast.makeText(context, "You are successfully unregistered with Guardian App! All your data has been permanently deleted and cannot be retrieved.", Toast.LENGTH_LONG).show();
			}
			else
			{
				Toast.makeText(context, "Unable to unregister now! Please try after sometime. If you continue to receive this message, please contact GuardianApp@outlook.com", Toast.LENGTH_LONG).show();
			}
			return resultInfo;



		case AppConstant.CHECK_UPDATES_FROM_SERVER_TAG :
			VersionUpdate versionInfo = new VersionUpdate();
			versionInfo = gson.fromJson(response, VersionUpdate.class);
			return versionInfo;



		default:
			break;
		}

		return null;

	}

	public Object handleSyncResp(String result) {
		Gson gson = new Gson();
		try{
			switch (AppConstant.locateSyncServices) {
			case AppConstant.GET_LOCATE_BUDDIES_TAG:
				ProfileListLite profileListLite=new ProfileListLite();
				profileListLite=gson.fromJson(result, ProfileListLite.class);
				AppConstant.locateSyncServices = 0;//Danish
				return profileListLite;

			case AppConstant.GET_SOSTRACK_COUNT_TAG:
				break;
			}

		}
		catch(Exception e){
			return null;
		}

		return null;

	}


}
