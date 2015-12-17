package com.guardianapp.database;




import java.util.ArrayList;

import android.content.ContentValues;
import android.content.Context;
import android.content.SharedPreferences;
import android.database.Cursor;

import com.guardianapp.model.AscGroups;
import com.guardianapp.model.Group;
import com.guardianapp.model.LocateBuddies;
import com.guardianapp.model.MyBuddies;
import com.guardianapp.model.Profile;
import com.guardianapp.model.ProfileList;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.LogUtils;

public class DBQueries {


	private Context context;

	SharedPreferences AppPrefs;
	// Table Names
	public static final String TABLE_BUDDY = "BuddyTableEntity";
	public static final String TABLE_GROUP = "GroupTableEntity";
	public static final String TABLE_HEALTH = "HealthTableEntity";
	public static final String TABLE_LOCATION_BUDDY = "LocateBuddyTableEntity";
	public static final String TABLE_PROFILE = "ProfileTableEntity";
	public static final String TABLE_USER = "UserTableEntity";


	public static final String EXECUTE_SUCCESS = "Query run sucessfuly";
	//Buddy table create statement
	public static final String CREATE_TABLE_BUDDY = "CREATE TABLE "
			+ TABLE_BUDDY + "(" + "BID" + " INTEGER PRIMARY KEY," 
			+"BuddyRelationshipId" +" TEXT," +"State" +" TEXT," +"MyProfileId" +" TEXT,"
			+"BuddyUserId" +" TEXT," +"BuddyProfileId" +" TEXT,"  +"Name" +" TEXT," 
			+"ShortTrackingURL" +" TEXT,"+"Email" +" TEXT,"  +"LastLocation" +" TEXT,"
			+"PhoneNumber" +" TEXT," +"TrackingToken" +" TEXT,"+"IsDeleted" +" INTEGER,"
			+"IsPrimeBuddy" +" INTEGER," +"BuddyStatusColor" +" TEXT,"
			+"OrderNumber" +" INTEGER,"+"BorderThickness" +" TEXT"+")";


	//Groups table create statement
	public static final String CREATE_TABLE_GROUPS = "CREATE TABLE "
			+ TABLE_GROUP + "(" +"GID" + " INTEGER PRIMARY KEY,"+ "GroupId" + " TEXT,"
			+ "MyProfileId" + " TEXT," + "Name" + " TEXT," + "PhoneNumber" + " TEXT,"
			+ "Email" + " TEXT," + "Type" + " TEXT," + "EnrollmentType" + " TEXT,"
			+ "EnrollmentKey" + " TEXT," + "EnrollmentValue" + " TEXT," + "IsValidated" + " INTEGER,"
			+ "IsDeleted" + " INTEGER," + "BuddyStatusColor" + " TEXT,"+ "BorderThickness" + " TEXT,"
			+ "LastLocation" + " TEXT"+")";


	//Health table create statement
	public static final String CREATE_TABLE_HEALTH = "CREATE TABLE "
			+ TABLE_HEALTH + "("+"SystemHealth" + " TEXT"+")";

	//Location Buddy table create statement
	public static final String CREATE_TABLE_LOCATION_BUDDY = "CREATE TABLE "
			+ TABLE_LOCATION_BUDDY + "("+"BID" + " INTEGER PRIMARY KEY,"+"MyProfileId" + " TEXT,"
			+"BuddyUserId" + " TEXT,"+"BuddyProfileId"  + " TEXT,"+"Name"  + " TEXT,"
			+"ShortTrackingURL"  + " TEXT," +"Email"  + " TEXT,"  +"LastLocation"  + " TEXT,"
			+"PhoneNumber"  + " TEXT," +"TrackingToken"  + " TEXT," +"IsDeleted"  + " INTEGER,"
			+"BuddyStatusColor"  + " TEXT," +"OrderNumber"  + " INTEGER,"
			+"BorderThickness"  + " TEXT"+")";

	//Profile table create statement
	public static final String CREATE_TABLE_PROFILE = "CREATE TABLE "
			+ TABLE_PROFILE + "("+"AutoProfileId" + " INTEGER PRIMARY KEY,"+"PostLocationConsent" + " INTEGER,"
			+"IsTrackingStatusSynced" + " INTEGER,"+"IsSOSStatusSynced" + " INTEGER,"+"LastSynced" + " TEXT,"+"MapView" + " TEXT,"
			+"IsDataSynced" + " INTEGER," +"ProfileId" + " TEXT," +"CountryCode" + " TEXT,"  +"MobileNumber" + " TEXT,"
			+"SessionToken" + " TEXT," +"FBGroupId" + " TEXT," +"FBGroupName" + " TEXT,"+"LocationConsent" + " INTEGER,"
			+"MessageTemplate" + " TEXT,"+"CanSMS" + " INTEGER,"+"CanEmail" + " INTEGER,"
			+"CanFBPost" + " INTEGER," +"CanArchiveEvidence" + " TEXT,"+"ArchiveFolder" + " TEXT,"
			+"TinyUri" + " TEXT," +"IsTrackingOn" + " INTEGER,"+"IsSOSOn" + " INTEGER," +"AllowOthersToTrackYou" + " INTEGER,"
			+"DeviceId" + " TEXT,"+"Platform" + " TEXT," +"PoliceContact" + " TEXT,"  +"AmbulanceContact" + " TEXT,"
			+"FireContact" + " TEXT,"+"CountryName" + " TEXT,"+"MaxPhonedigits" + " TEXT"+")";

	//User table create statement
	public static final String CREATE_TABLE_USER = "CREATE TABLE "
			+ TABLE_USER + "("+"AutoUserId" + " INTEGER PRIMARY KEY,"+"UserId" + " TEXT,"
			+"Name" + " TEXT,"+"LiveEmail" + " TEXT,"+"LiveAuthId" + " TEXT,"+"FBAuthId" + " TEXT,"
			+"CurrentProfileId" + " TEXT"+")";




	public DBQueries(Context argContext) {
		this.context =argContext;
		AppPrefs= this.context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

	}

	public DBQueries() {

	}

	public String insertBuddyIntoDB(MyBuddies buddy) {
		if(this.isBuddyAlreadyExistsInDB(buddy.getMobileNumber()))
			return "Success";
		ContentValues values = new ContentValues();
		values.put("Name", buddy.getName());
		values.put("PhoneNumber", buddy.getMobileNumber());
		values.put("Email", buddy.getEmail());
		values.put("BuddyUserId",String.valueOf(0));
		values.put("IsDeleted", false);
		values.put("IsPrimeBuddy", false);
		values.put("State", buddy.getState());
		values.put("BuddyRelationshipId",String.valueOf(0));
		values.put("MyProfileId", String.valueOf(AppConstant.getUserProfileID(this.context)));
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		try {
			dbHandler.insertRecord(TABLE_BUDDY, values);
			return "Success";
		} catch (Exception e) {
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), e.getLocalizedMessage());
			return "Fail";
		}finally{
			try{
				dbHandler.closeDatabase();
			}
			catch(Exception e){
				LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), e.getLocalizedMessage());
			}
		}

	}

	public String deleteBuddyContectIntoDB(String value) {
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		try {
			dbHandler.deleteOneRecord(TABLE_BUDDY, "PhoneNumber", value);
			return "Success";
		} catch (Exception e) {
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), e.getLocalizedMessage());
			return "Fail";
		}finally{
			try{
				dbHandler.closeDatabase();
			}
			catch(Exception e){
				LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), e.getLocalizedMessage());
			}
		}
	}

	public  String deleteBuddyValue(String value) {
		ContentValues values = new ContentValues();
		values.put("IsDeleted",true);
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		try {
			dbHandler.updateRecord(TABLE_BUDDY, values, "PhoneNumber "+" = '"+ value + "'", null);
			return "Success";

		} catch (Exception e) {
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), e.getLocalizedMessage());
			return "Fail";
		}   
		finally{
			try{
				dbHandler.closeDatabase();
			}
			catch(Exception e){
				LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), e.getLocalizedMessage());
			}
		}
	}

	public boolean isBuddyAlreadyExistsInDB(String mobileNumber){
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		Cursor cur = null;
		dbHandler.openDatabase();
		try{
			cur = dbHandler.selectRecords("SELECT * FROM "+TABLE_BUDDY+" WHERE PhoneNumber = "+"'"+mobileNumber+"'");
			if(cur != null && cur.moveToNext()){
				if(mobileNumber!=null && mobileNumber.equalsIgnoreCase(cur.getString(cur.getColumnIndex("PhoneNumber")))){
					this.updateDeleteStatusOfBuddy(mobileNumber , cur.getInt(cur.getColumnIndex("IsDeleted")));
					return true;
				}
			}}catch(Exception e1){
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class), e1.getLocalizedMessage());
				return false;
			}finally{
				try{
					if(cur!=null && !cur.isClosed())
						cur.close();
					dbHandler.closeDatabase();
				}
				catch(Exception e){
					LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), e.getLocalizedMessage());
				}		}

		return false;
	}

	public boolean isGroupAlreadyExistsInDB(String groupName){
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		Cursor cur = null;
		dbHandler.openDatabase();
		try{
			cur = dbHandler.selectRecords("SELECT * FROM "+TABLE_GROUP+" WHERE Name = '"+groupName+"'");
			if(cur != null && cur.moveToNext()){
				if(groupName!=null && groupName.equalsIgnoreCase(cur.getString(cur.getColumnIndex("Name")))){
					this.updateDeleteStatusOfGroup(groupName , cur.getInt(cur.getColumnIndex("IsDeleted")));
					return true;
				}
			}}catch(Exception e1){
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class), e1.getLocalizedMessage());
				return false;
			}finally{
				try{
					if(cur!=null && !cur.isClosed())
						cur.close();
					dbHandler.closeDatabase();
				}
				catch(Exception e){
					LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), e.getLocalizedMessage());
				}		}

		return false;
	}

	public void cleanBuddies()
	{
		if(AppConstant.userProfile.isIsDataSynced()){
			DatabaseHandler dbHandler = new DatabaseHandler(context);
			dbHandler.openDatabase();
			Cursor cur = dbHandler.selectRecords("SELECT * FROM "+TABLE_BUDDY+" WHERE IsDeleted = 1");
			try{
				if (cur != null && cur.moveToFirst())
				{
					do {
						if(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("IsDeleted"))))         {
							dbHandler.deleteOneRecord(TABLE_BUDDY, "PhoneNumber",cur.getString(cur.getColumnIndex("PhoneNumber")) );
						}
					} while (cur.moveToNext());
				}

			}catch(Exception dbe){
				LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), dbe.getLocalizedMessage());
			}finally{
				try{
					if(cur!=null && !cur.isClosed())
						cur.close();
					dbHandler.closeDatabase();
				}catch(Exception dce){
					LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), dce.getLocalizedMessage());
				}

			}

		}
	}


	public String Load2CurrentProfile() throws NumberFormatException{
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		Cursor cur = dbHandler.selectRecords("SELECT * FROM "+TABLE_PROFILE+" ORDER BY ROWID ASC LIMIT 1");
		if (cur != null)
		{
			if (cur.moveToFirst()) {			 	

				AppConstant.userProfile.setMobileNumber(cur.getString(cur.getColumnIndex("MobileNumber")));
				AppConstant.userProfile.setFBGroupID(cur.getString(cur.getColumnIndex("FBGroupId")));
				AppConstant.userProfile.setFBGroupName(cur.getString(cur.getColumnIndex("FBGroupName")));
				AppConstant.userProfile.setLocationConsent(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("LocationConsent"))));
				AppConstant.userProfile.setCanSMS(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("CanSMS"))));
				AppConstant.userProfile.setCanMail(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("CanEmail"))));
				AppConstant.userProfile.setCanPost(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("CanFBPost"))));
				AppConstant.userProfile.setCanArchive(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("CanArchiveEvidence"))));
				AppConstant.userProfile.setTinyURI(cur.getString(cur.getColumnIndex("TinyUri")));
				AppConstant.userProfile.setIsTrackingOn(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("IsTrackingOn"))));
				AppConstant.userProfile.setIsSOSOn(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("IsSOSOn"))));
				AppConstant.userProfile.setLastLocs(cur.getString(cur.getColumnIndex("LastSynced")));
				AppConstant.userProfile.setPoliceContact(cur.getString(cur.getColumnIndex("PoliceContact")));
				AppConstant.userProfile.setCountryCode(cur.getString(cur.getColumnIndex("CountryCode")));
				AppConstant.userProfile.setAmbulanceContact(cur.getString(cur.getColumnIndex("AmbulanceContact")));
				AppConstant.userProfile.setFireContact(cur.getString(cur.getColumnIndex("FireContact")));
				AppConstant.userProfile.setIsSOSStatusSynced(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("IsSOSStatusSynced"))));
				AppConstant.userProfile.setIsTrackingStatusSynced(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("IsTrackingStatusSynced"))));
				AppConstant.userProfile.setMessageTemplate(cur.getString(cur.getColumnIndex("MessageTemplate")));
				AppConstant.userProfile.setIsDataSynced(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("IsDataSynced"))));
				AppConstant.userProfile.setPostLocationConsent(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("PostLocationConsent"))));
				AppConstant.userProfile.setSessionToken(cur.getString(cur.getColumnIndex("SessionToken")));
				
				AppConstant.userProfile.setProfileID(Long.parseLong(cur.getString(cur.getColumnIndex("ProfileId"))));
				SharedPreferences.Editor editor = AppPrefs.edit();
				editor.putLong(AppConstant.Profile_ID, Long.parseLong(cur.getString(cur.getColumnIndex("ProfileId"))));
				editor.commit();


				try{
					if(cur!=null && !cur.isClosed())
						cur.close();
					dbHandler.closeDatabase();
				}catch(Exception e){
					LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), e.getLocalizedMessage());
				}


			}
			else
			{
				ContentValues values = new ContentValues();
				values.put("MobileNumber", AppConstant.userProfile.getMobileNumber());
				values.put("FBGroupId", AppConstant.userProfile.getFBGroupID());
				values.put("FBGroupName", AppConstant.userProfile.getFBGroupName());
				values.put("LocationConsent",AppConstant.userProfile.isLocationConsent());
				values.put("CanSMS", AppConstant.userProfile.isCanSMS());
				values.put("CanEmail", AppConstant.userProfile.isCanMail());
				values.put("CanFBPost", AppConstant.userProfile.isCanPost());
				values.put("CanArchiveEvidence", AppConstant.userProfile.isCanArchive());
				values.put("ArchiveFolder", AppConstant.userProfile.isCanArchive());
				values.put("TinyUri", AppConstant.userProfile.getTinyURI());
				values.put("IsTrackingOn",AppConstant.userProfile.isIsTrackingOn());
				values.put("IsSOSOn", AppConstant.userProfile.isIsSOSOn());
				values.put("LastSynced", AppConstant.userProfile.getLastLocs());

				values.put("PoliceContact", AppConstant.userProfile.getPoliceContact());
				values.put("CountryCode", AppConstant.userProfile.getCountryCode());
				values.put("AmbulanceContact", AppConstant.userProfile.getAmbulanceContact());
				values.put("FireContact", AppConstant.userProfile.getFireContact());
				values.put("IsSOSStatusSynced", AppConstant.userProfile.isIsSOSStatusSynced());
				values.put("IsTrackingStatusSynced", AppConstant.userProfile.isIsTrackingStatusSynced());
				values.put("MessageTemplate", AppConstant.userProfile.getMessageTemplate());
				values.put("IsDataSynced", AppConstant.userProfile.isIsDataSynced());
				values.put("PostLocationConsent",AppConstant.userProfile.isPostLocationConsent());
				
				AppConstant.userProfile =AppConstant.CreateDefaultProfile(this.context);
				values.put("ProfileId", String.valueOf(AppConstant.getUserProfileID(this.context)));
				SharedPreferences.Editor editor = AppPrefs.edit();
				editor.putLong(AppConstant.Profile_ID, AppConstant.getUserProfileID(this.context));
				editor.commit();

				try {
					dbHandler.insertRecord(TABLE_PROFILE, values);
					return "Success";
				} catch (Exception e) {
					LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), e.getLocalizedMessage());
					return "Fail";
				}finally{
					try{
						if(cur!=null && !cur.isClosed())
							cur.close();
						dbHandler.closeDatabase();
					}
					catch(Exception e){
						LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), e.getLocalizedMessage());
					}
				}
			}
		}


		return null;

	}

	public String loadUserData() throws NumberFormatException{

		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		SharedPreferences.Editor editor = AppPrefs.edit();
		Cursor cur = dbHandler.selectRecords("SELECT * FROM "+TABLE_USER+";");
		if (cur != null)
		{
			if (cur.moveToFirst()) {

				AppConstant.user.setUserId(Long.parseLong(cur.getString(cur.getColumnIndex("UserId"))));
				editor.putLong(AppConstant.User_ID, Long.parseLong(cur.getString(cur.getColumnIndex("UserId"))));
				editor.commit();
				AppConstant.user.setName(cur.getString(cur.getColumnIndex("Name")));
				AppConstant.user.setLiveEmail(cur.getString(cur.getColumnIndex("LiveEmail")));
				AppConstant.user.setLiveAuthId(cur.getString(cur.getColumnIndex("LiveAuthId")));
				AppConstant.user.setFbAuthId(cur.getString(cur.getColumnIndex("FBAuthId")));
				AppConstant.user.setCurrentProfileId(Long.parseLong(cur.getString(cur.getColumnIndex("CurrentProfileId"))));

				try{
					if(cur!=null && !cur.isClosed())
						cur.close();
					dbHandler.closeDatabase();
				}catch(Exception te){
					LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class), te.getLocalizedMessage());
				}


				AppConstant.localUserName = AppConstant.user.getName();
				return "Success";
			}
			else
			{
				ContentValues values = new ContentValues();
				values.put("CurrentProfileId","0");
				values.put("UserId","0");
				AppConstant.user.setCurrentProfileId((long)0);
				try {
					dbHandler.insertRecord(TABLE_USER, values);
					return "Success";
				} catch (Exception e) {
					LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class), e.getLocalizedMessage());
					return "Fail";
				} finally {
					try{
						if(cur!=null && !cur.isClosed())
							cur.close();
						dbHandler.closeDatabase();
					}catch(Exception e1){
						LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class), e1.getLocalizedMessage());
					}
				}
			}
		}
		return "Fail";
	}
	public String deleteGrpopIntoDB(String value) {

		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		try {
			dbHandler.deleteOneRecord(TABLE_GROUP, "Name", value);
			return "Success";
		} catch (Exception e) {
			LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class), e.getLocalizedMessage());
			return "Fail";
		}  finally{
			try{
				dbHandler.closeDatabase();
			}catch(Exception e1){
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class), e1.getLocalizedMessage());
			}

		}

	}

	public  String deleteGroupValue(String value) {
		ContentValues values = new ContentValues();
		values.put("IsDeleted",true);
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		try {
			dbHandler.updateRecord(TABLE_GROUP, values, "Name "+" = '"+ value + "'", null);
			return "Success";
		} catch (Exception e) {
			LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class), e.getLocalizedMessage());
			return "Fail";
		} finally{
			try{
				dbHandler.closeDatabase();
			}catch(Exception e1){
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class), e1.getLocalizedMessage());
			}

		}
	}

	public void cleanGroups()
	{
		if(AppConstant.userProfile.isIsDataSynced()){
			DatabaseHandler dbHandler = new DatabaseHandler(context);
			dbHandler.openDatabase();
			Cursor cur = dbHandler.selectRecords("SELECT * FROM "+TABLE_GROUP+" WHERE IsDeleted = 1");
			if (cur != null && cur.moveToFirst()){
				do {
					if(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("IsDeleted"))))         {
						try{
							dbHandler.deleteOneRecord(TABLE_GROUP, "Name",cur.getString(cur.getColumnIndex("Name")) );
						}catch(Exception e){
							LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class), e.getLocalizedMessage());
						}

					}

				} while (cur.moveToNext());
			}
			try{
				if(cur!=null && !cur.isClosed())
					cur.close();

				dbHandler.closeDatabase();
			}catch(Exception e){
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class), e.getLocalizedMessage());
			}
		}
	}

	private String UpdateUser( Profile profile, DatabaseHandler dbHandler){
		ContentValues values = new ContentValues();
		SharedPreferences prefs;
		prefs = this.context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

		values.put("Name", profile.getName());
		values.put("UserId", String.valueOf(profile.getUserID()));
		values.put("LiveEmail", profile.getEmail());
		if (profile.getProfileID() != null)
			values.put("CurrentProfileId", String.valueOf(profile.getProfileID()));
		values.put("LiveAuthId", prefs.getString(AppConstant.Authentication_Token, ""));
		values.put("FBAuthId", profile.getFBAuthID());

		try{
			dbHandler.updateRecord(TABLE_USER, values, "UserId "+" = '0'", null);
			return "Success";
		}catch(Exception e){
			LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class), e.getLocalizedMessage());
			return "Fail";
		}



	}

	public String insertLocateBuddyIntoDB(LocateBuddies locateBuddies){
		ContentValues values = new ContentValues();

		values.put("Name", locateBuddies.getName());
		values.put("BorderThickness", locateBuddies.getBorderThickness());
		values.put("OrderNumber", locateBuddies.getOrderNumber());
		values.put("BuddyStatusColor", locateBuddies.getBuddyStatusColor());
		values.put("BuddyProfileId",String.valueOf(locateBuddies.getBuddyProfileId()));
		values.put("Email", locateBuddies.getEmail());
		values.put("LastLocation", locateBuddies.getLastLocation());
		values.put("PhoneNumber", locateBuddies.getPhoneNumber());
		values.put("TrackingToken", locateBuddies.getSessionID());
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		try {

			dbHandler.insertRecord(TABLE_LOCATION_BUDDY, values);
			return "Success";
		} catch (Exception e) {
			LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e.getLocalizedMessage());
			return "Fail";

		}finally{
			try{
				dbHandler.closeDatabase();	
			}catch(Exception e1){
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e1.getLocalizedMessage());
			}
		}

	}

	public String deleteLocateBuddyIntoDB(String value) {
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		try {
			dbHandler.deleteOneRecord(TABLE_LOCATION_BUDDY, "BuddyProfileId", value);
			return "Success";
		} catch (Exception e) {
			LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e.getLocalizedMessage());
			return "Fail";
		}finally{
			try{
				dbHandler.closeDatabase();	
			}catch(Exception e1){
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e1.getLocalizedMessage());
				return "Fail";
			}
		}
	}

	public ArrayList<LocateBuddies> selectRecordFromLocateBuddy() {
		ArrayList<LocateBuddies> locateBuddies = new ArrayList<LocateBuddies>();
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		Cursor cur = dbHandler.selectRecords("SELECT * FROM "+TABLE_LOCATION_BUDDY+";");

		if (cur != null && cur.moveToFirst()){
			do {
				LocateBuddies locBuddies = new LocateBuddies();
				locBuddies.setName(cur.getString(cur.getColumnIndex("Name")));
				locBuddies.setBorderThickness(cur.getString(cur.getColumnIndex("BorderThickness")));
				locBuddies.setOrderNumber(cur.getInt(cur.getColumnIndex("OrderNumber")));
				locBuddies.setBuddyStatusColor(cur.getString(cur.getColumnIndex("BuddyStatusColor")));
				locBuddies.setBuddyProfileId(Long.parseLong(cur.getString(cur.getColumnIndex("BuddyProfileId")))); 
				locBuddies.setEmail(cur.getString(cur.getColumnIndex("Email")));  
				locBuddies.setLastLocation(cur.getString(cur.getColumnIndex("LastLocation")));  
				locBuddies.setPhoneNumber(cur.getString(cur.getColumnIndex("PhoneNumber")));  
				locBuddies.setSessionID(cur.getString(cur.getColumnIndex("TrackingToken"))); 
				locateBuddies.add(locBuddies);
			} while (cur.moveToNext());
		}
		if(cur!=null && !cur.isClosed())
			cur.close();

		dbHandler.closeDatabase();
		return locateBuddies;


	}

	public void sanitizeLocateBuddiesList(ArrayList<LocateBuddies> locateBuddiesList){
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		Cursor cur = dbHandler.selectRecords("SELECT * FROM "+TABLE_LOCATION_BUDDY);

		if (cur != null && cur.moveToFirst()){
			do {
				LocateBuddies locBuddies = new LocateBuddies();
				locBuddies.setName(cur.getString(cur.getColumnIndex("Name")));
				locBuddies.setBorderThickness(cur.getString(cur.getColumnIndex("BorderThickness")));
				locBuddies.setOrderNumber(cur.getInt(cur.getColumnIndex("OrderNumber")));
				locBuddies.setBuddyStatusColor(cur.getString(cur.getColumnIndex("BuddyStatusColor")));
				locBuddies.setBuddyProfileId(Long.parseLong(cur.getString(cur.getColumnIndex("BuddyProfileId")))); 
				locBuddies.setEmail(cur.getString(cur.getColumnIndex("Email")));  
				locBuddies.setLastLocation(cur.getString(cur.getColumnIndex("LastLocation")));  
				locBuddies.setPhoneNumber(cur.getString(cur.getColumnIndex("PhoneNumber")));  
				locBuddies.setSessionID(cur.getString(cur.getColumnIndex("TrackingToken"))); 

				if(!locateBuddiesList.contains(locBuddies))
					deleteLocateBuddyIntoDB(String.valueOf(locBuddies.getBuddyProfileId()));


			} while (cur.moveToNext());
		}
		if(cur!=null && !cur.isClosed())
			cur.close();

		dbHandler.closeDatabase();

	}
	public boolean checkLocateBuddyIntoDB(LocateBuddies buddy) {
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		Cursor cur = dbHandler.selectRecords("select * from "+TABLE_LOCATION_BUDDY+" where BuddyProfileId = "+"'"
				+buddy.getBuddyProfileId()+"'");

		if (cur != null && cur.moveToFirst())
		{
			cur.close();
			dbHandler.closeDatabase();
			return true;
		}else{
			dbHandler.closeDatabase();
			return false;
		}

	}
	public  String updateLocateBuddyValue(LocateBuddies buddy) {

		ContentValues values = new ContentValues();

		values.put("Name",buddy.getName());
		values.put("BorderThickness",buddy.getBorderThickness());
		values.put("OrderNumber",buddy.getOrderNumber());
		values.put("BuddyStatusColor",buddy.getBuddyStatusColor());

		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();

		try {
			dbHandler.updateRecord(TABLE_LOCATION_BUDDY, values, "BuddyProfileId "+" = '"+ buddy.getBuddyProfileId() + "'", null);
			return DBQueries.EXECUTE_SUCCESS;

		} catch (Exception e) {
			LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e.getLocalizedMessage());
			return e.getMessage().toString();
		}finally{
			try{
				dbHandler.closeDatabase();				
			}catch(Exception e1){
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e1.getLocalizedMessage());
				return e1.getLocalizedMessage();
			}
		}
	}
	public  String updateDeleteStatusOfBuddy(String mobileNumber , int isDeleted) {
		ContentValues values = new ContentValues();
		values.put("IsDeleted",!AppConstant.intToBoolean(isDeleted));
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();

		try {
			dbHandler.updateRecord(TABLE_BUDDY, values, "PhoneNumber"+" = '"+mobileNumber+"'", null);
			return DBQueries.EXECUTE_SUCCESS;

		} catch (Exception e) {
			LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e.getLocalizedMessage());
			return e.getMessage().toString();
		}finally{
			try{
				dbHandler.closeDatabase();
			}catch(Exception e1){
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e1.getLocalizedMessage());
				return e1.getLocalizedMessage();
			}
		}
	}

	public  String updateDeleteStatusOfGroup(String groupName , int isDeleted) {
		ContentValues values = new ContentValues();
		values.put("IsDeleted",!AppConstant.intToBoolean(isDeleted));
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();

		try {
			dbHandler.updateRecord(TABLE_GROUP, values, "Name"+" = '"+groupName+"'", null);
			return DBQueries.EXECUTE_SUCCESS;

		} catch (Exception e) {
			LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e.getLocalizedMessage());
			return e.getMessage().toString();
		}finally{
			try{
				dbHandler.closeDatabase();
			}catch(Exception e1){
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e1.getLocalizedMessage());
				return e1.getLocalizedMessage();
			}
		}
	}

	public  ArrayList<Group> updateStatusOfGroupsMarkedForDeletion() {
		ArrayList<Group> gList = new ArrayList<Group>();
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		Cursor cur = dbHandler.selectRecords("SELECT * FROM "+TABLE_GROUP+" WHERE IsDeleted = 1");
		if (cur != null && cur.moveToFirst())
		{
			do {
				updateDeleteStatusOfGroup(cur.getString(cur.getColumnIndex("Name")), cur.getInt(cur.getColumnIndex("IsDeleted")));

			} while (cur.moveToNext());
		}
		if(cur!=null && !cur.isClosed())
			cur.close();
		dbHandler.closeDatabase();
		return gList;
	}


	public  String updateValue(String column, String value) {
		ContentValues values = new ContentValues();
		values.put(column,value);
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();

		try {
			dbHandler.updateRecord(TABLE_PROFILE, values, "ProfileId "+" = '"+ AppConstant.getUserProfileID(this.context) + "'", null);
			return DBQueries.EXECUTE_SUCCESS;
		} catch (Exception e) {
			LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e.getLocalizedMessage());
			return e.getMessage().toString();
		}finally{
			try{
				dbHandler.closeDatabase();
			}catch(Exception e1){
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e1.getLocalizedMessage());
				return e1.getLocalizedMessage();
			}
		}

	}

	public  String updateValue(String column, boolean value) {
		ContentValues values = new ContentValues();
		values.put(column,value);
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		try {
			if(column=="CanSMS" || column == "CanEmail" || column == "CanFBPost" || column == "LocationConsent"){
				values.put("IsDataSynced",false);

			}
			dbHandler.updateRecord(TABLE_PROFILE, values, "ProfileId "+" = '"+ AppConstant.getUserProfileID(this.context) + "'", null);
			return DBQueries.EXECUTE_SUCCESS;

		} catch (Exception e) {
			LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e.getLocalizedMessage());
			return e.getMessage().toString();
		}finally{
			try{
				dbHandler.closeDatabase();
			}catch(Exception e1){
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e1.getLocalizedMessage());
				return e1.getLocalizedMessage();
			}
		}
	}

	public String storeIntoDB(Object obj) {

		String DbResponseMsg = null;
		ContentValues values = new ContentValues();
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		String tableName;

		switch (AppConstant.service_Tag) {

		case AppConstant.MEMBERSHIP_SERVICE_TAG:

			tableName = TABLE_PROFILE;
			ProfileList profileList = (ProfileList) obj;
			dbHandler.openDatabase();
			if(profileList.getList().size()>1)
				dbHandler.deleteAllRecords(tableName); 

			for(int i =0; i<profileList.getList().size(); i++)
			{
				values.put("ProfileId", String.valueOf(profileList.getList().get(i).getProfileID()));
				values.put("MobileNumber", profileList.getList().get(i).getMobileNumber());
				values.put("FBGroupId", profileList.getList().get(i).getFBGroupID());
				values.put("FBGroupName", profileList.getList().get(i).getFBGroupName());
				values.put("LocationConsent", profileList.getList().get(i).isLocationConsent());
				values.put("CanSMS", profileList.getList().get(i).isCanSMS());
				values.put("CanEmail", profileList.getList().get(i).isCanMail());
				values.put("CanFBPost", profileList.getList().get(i).isCanPost());
				values.put("CanArchiveEvidence", profileList.getList().get(i).isCanArchive());
				values.put("ArchiveFolder", profileList.getList().get(i).isCanArchive());
				values.put("TinyUri", profileList.getList().get(i).getTinyURI());
				values.put("IsTrackingOn", profileList.getList().get(i).isIsTrackingOn());
				values.put("IsSOSOn", profileList.getList().get(i).isIsSOSOn());
				values.put("LastSynced", profileList.getList().get(i).getLastLocs());
				values.put("SessionToken", profileList.getList().get(i).getSecurityToken());
				values.put("CanArchiveEvidence", profileList.getList().get(i).isCanArchive());//duplicate
				values.put("AllowOthersToTrackYou", profileList.getList().get(i).isIsTrackingOn());//duplicate
				if(profileList.getList().get(i).getRegionCode()!=null && !profileList.getList().get(i).getRegionCode().trim().equalsIgnoreCase(""))
					values.put("CountryCode", profileList.getList().get(i).getRegionCode());
				else
					values.put("CountryCode", AppConstant.userProfile.getCountryCode());	
				values.put("IsDataSynced", true);

				try {
					if(profileList.getList().size()>1)
						dbHandler.insertRecord(tableName, values);
					else
						dbHandler.updateRecord(tableName, values, "ProfileId "+" = '"+ AppConstant.getUserProfileID(this.context) + "'", null);

					UpdateUser(profileList.getList().get(i),dbHandler);

					if(profileList.getList().get(i).getMyBuddies()!=null && profileList.getList().get(i).getMyBuddies().size()>0)
						updateBuddyIntoDB(dbHandler,profileList.getList().get(i).getMyBuddies());

					if(profileList.getList().get(i).getAscGroups()!=null && profileList.getList().get(i).getAscGroups().size()>0)
						updateGroupsIntoDB(dbHandler,profileList.getList().get(i).getAscGroups());

					DbResponseMsg = DBQueries.EXECUTE_SUCCESS;

				} catch (Exception e) {
					AppConstant.dismissProgressDialog();
					LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e.getLocalizedMessage());
					DbResponseMsg = e.getMessage().toString();
					return e.getMessage().toString();
				}finally{
					dbHandler.closeDatabase();
				}

			}
			dbHandler.closeDatabase();
			break;

		case AppConstant.CREATE_PROFILE_SERVICE_TAG:
		case AppConstant.UPDATE_PROFILE_SERVICE_TAG:
			tableName = TABLE_PROFILE;

			Profile profile = (Profile) obj;
			dbHandler.openDatabase();

			values.put("ProfileId", String.valueOf(profile.getProfileID()));
			values.put("MobileNumber", profile.getMobileNumber());
			values.put("FBGroupId", profile.getFBGroupID());
			values.put("FBGroupName", profile.getFBGroupName());
			values.put("LocationConsent",profile.isLocationConsent());
			values.put("CanSMS", profile.isCanSMS());
			values.put("CanEmail", profile.isCanMail());
			values.put("CanFBPost", profile.isCanPost());
			values.put("CanArchiveEvidence", profile.isCanArchive());
			values.put("ArchiveFolder", profile.isCanArchive());
			values.put("TinyUri", profile.getTinyURI());
			values.put("IsTrackingOn",profile.isIsTrackingOn());
			values.put("IsSOSOn", profile.isIsSOSOn());
			values.put("SessionToken", AppConstant.userProfile.getSessionToken());
			values.put("CanArchiveEvidence", profile.isCanArchive());//duplicate
			values.put("AllowOthersToTrackYou",profile.isIsTrackingOn());//duplicate
			values.put("IsDataSynced", true);
			if(profile.getRegionCode()!=null)
				values.put("CountryCode", profile.getRegionCode());
			else
				values.put("CountryCode", AppConstant.userProfile.getCountryCode());	
			try {
				dbHandler.updateRecord(tableName, values, "ProfileId "+" = '"+ AppConstant.getUserProfileID(this.context) + "'", null);
				UpdateUser(profile,dbHandler);

				if(profile.getMyBuddies()!=null && profile.getMyBuddies().size()>0)
					updateBuddyIntoDB(dbHandler,profile.getMyBuddies());

				if(profile.getAscGroups()!=null && profile.getAscGroups().size()>0)
					updateGroupsIntoDB(dbHandler,profile.getAscGroups());
				DbResponseMsg = DBQueries.EXECUTE_SUCCESS;

			} catch (Exception e) {
				AppConstant.dismissProgressDialog();
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e.getLocalizedMessage());
				DbResponseMsg = e.getMessage().toString();
				return e.getMessage().toString();
			}finally{
				try{
					dbHandler.closeDatabase();
				}catch(Exception e1){
					LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class), e1.getLocalizedMessage());
				}
			}


			break;
		case AppConstant.GROUP_SERVICE_TAG:

			tableName = TABLE_GROUP;

			Group glist = (Group) obj;

			if(this.isGroupAlreadyExistsInDB(glist.getGroupName())){
				DbResponseMsg = DBQueries.EXECUTE_SUCCESS;
				break;
			}

			dbHandler.openDatabase();

			values.put("GroupId", glist.getGroupID());
			//values.put("MyProfileId", glist.getList().get(i).get);
			values.put("Name", glist.getGroupName());
			values.put("PhoneNumber", glist.getPhoneNumber());
			values.put("Email", glist.getEmail());
			values.put("Type", glist.getType());
			values.put("EnrollmentType", glist.getEnrollmentType());
			values.put("EnrollmentKey", glist.getEnrollmentKey());
			values.put("EnrollmentValue", glist.getEnrollmentValue());
			values.put("IsValidated", AppConstant.booleanToInt(glist.isIsValidated()));
			values.put("IsDeleted", glist.isToRemove());
			//values.put("BuddyStatusColor", glist.getList().get(i).get);
			//values.put("BorderThickness", glist.getList().get(i).get);
			values.put("LastLocation", glist.getGroupLocation());


			try {
				dbHandler.insertRecord(tableName, values);
				DbResponseMsg = DBQueries.EXECUTE_SUCCESS;

			} catch (Exception e) {
				AppConstant.dismissProgressDialog();
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e.getLocalizedMessage());
				DbResponseMsg = e.getMessage().toString();
				return e.getMessage().toString();
			}finally{
				try{
					dbHandler.closeDatabase();
				}catch(Exception e1){
					LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class), e1.getLocalizedMessage());
				}
			}
			break;

		default:
			break;
		}


		return DbResponseMsg;


	}

	private void updateBuddyIntoDB(DatabaseHandler db, ArrayList<MyBuddies> buddies) {
		ContentValues values = new ContentValues();
		String tableName = TABLE_BUDDY;
		for (int i = 0; i < buddies.size(); i++) {
			values.put("Name", buddies.get(i).getName());
			values.put("PhoneNumber", buddies.get(i).getMobileNumber());
			values.put("Email", buddies.get(i).getEmail());
			values.put("State", buddies.get(i).getState());
			values.put("BuddyUserId", String.valueOf(buddies.get(i).getUserID()));
			values.put("BuddyProfileId", String.valueOf(AppConstant.getUserProfileID(this.context)));
			values.put("BuddyRelationshipId",String.valueOf(buddies.get(i).getBuddyID()));
			values.put("IsDeleted", buddies.get(i).isToRemove());
			values.put("IsPrimeBuddy", buddies.get(i).isIsPrimeBuddy());

			Cursor cur = db.selectRecords("select * from "+tableName+" where PhoneNumber = "+"'"
					+buddies.get(i).getMobileNumber()+"'");
			try{
				if (cur.moveToFirst())
					db.updateRecord(tableName, values, "PhoneNumber "+" = '"+ buddies.get(i).getMobileNumber() + "'", null);
				else
					db.insertRecord(tableName, values);

				if(cur!=null && !cur.isClosed())
					cur.close();

			}catch(Exception e){
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e.getLocalizedMessage());
			}



		}

	}

	//Groups table create statement

	private void updateGroupsIntoDB(DatabaseHandler db, ArrayList<AscGroups> groups) {
		ContentValues values = new ContentValues();
		String tableName = TABLE_GROUP;

		for (int i = 0; i < groups.size(); i++) {

			values.put("GroupId",groups.get(i).getGroupID());
			values.put("Name", groups.get(i).getGroupName());
			values.put("PhoneNumber",  groups.get(i).getPhoneNumber());
			values.put("Email",groups.get(i).getEmail());
			values.put("EnrollmentType", groups.get(i).getEnrollmentType());
			values.put("EnrollmentKey",groups.get(i).getEnrollmentKey());
			values.put("EnrollmentValue", groups.get(i).getEnrollmentValue());
			values.put("IsValidated", groups.get(i).isIsValidated());
			values.put("IsDeleted", groups.get(i).isToRemove());
			values.put("LastLocation", groups.get(i).getGroupLocation());
			values.put("IsValidated", groups.get(i).isIsValidated());


			Cursor cur = db.selectRecords("select * from "+tableName+" where Name = "+"'"
					+groups.get(i).getGroupName()+"'"+";");

			try{
				if (cur.moveToFirst())
					db.updateRecord(tableName, values, "Name "+" = '"+ groups.get(i).getGroupName() + "'", null);
				else
					db.insertRecord(tableName, values);

				if(cur!=null && !cur.isClosed())
					cur.close();
			}catch(Exception e){
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e.getLocalizedMessage());

			}


		}



	}

	public  ArrayList<MyBuddies> SelectAllBuddies() {
		ArrayList<MyBuddies> bList = new ArrayList<MyBuddies>();
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		Cursor cur = dbHandler.selectRecords("SELECT * FROM "+TABLE_BUDDY);
		try{
			if (cur != null && cur.moveToFirst())
			{
				do {
					MyBuddies buddies = new MyBuddies();
					buddies.setName(cur.getString(cur.getColumnIndex("Name")));
					buddies.setMobileNumber(cur.getString(cur.getColumnIndex("PhoneNumber")));
					buddies.setEmail(cur.getString(cur.getColumnIndex("Email")));
					buddies.setState(cur.getString(cur.getColumnIndex("State")));
					buddies.setUserID(Long.parseLong(cur.getString(cur.getColumnIndex("BuddyUserId"))));
					buddies.setBuddyID(Long.parseLong(cur.getString(cur.getColumnIndex("BuddyRelationshipId"))));
					buddies.setToRemove(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("IsDeleted"))));
					buddies.setIsPrimeBuddy(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("IsPrimeBuddy"))));
					bList.add(buddies);
				} while (cur.moveToNext());

			}
		}catch(Exception e){
			LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class), e.getLocalizedMessage());
		}finally{
			try{
				if(cur!=null && !cur.isClosed())
					cur.close();
				dbHandler.closeDatabase();	
			}catch(Exception e1){
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class), e1.getLocalizedMessage());
			}
		}


		return bList;
	}

	public  ArrayList<MyBuddies> SelectAllActiveBuddies() {
		ArrayList<MyBuddies> bList = new ArrayList<MyBuddies>();
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		Cursor cur = dbHandler.selectRecords("SELECT * FROM "+TABLE_BUDDY+" WHERE IsDeleted = 0 AND PhoneNumber IS NOT NULL");
		try{
			if (cur != null && cur.moveToFirst())
			{
				do {
					MyBuddies buddies = new MyBuddies();
					buddies.setName(cur.getString(cur.getColumnIndex("Name")));
					buddies.setMobileNumber(cur.getString(cur.getColumnIndex("PhoneNumber")));
					buddies.setEmail(cur.getString(cur.getColumnIndex("Email")));
					buddies.setState(cur.getString(cur.getColumnIndex("State")));
					buddies.setUserID(Long.parseLong(cur.getString(cur.getColumnIndex("BuddyUserId"))));
					buddies.setBuddyID(Long.parseLong(cur.getString(cur.getColumnIndex("BuddyRelationshipId"))));
					buddies.setToRemove(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("IsDeleted"))));
					buddies.setIsPrimeBuddy(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("IsPrimeBuddy"))));
					bList.add(buddies);
				} while (cur.moveToNext());

			}
		}catch(Exception e){
			LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class), e.getLocalizedMessage());
		}finally{
			try{
				if(cur!=null && !cur.isClosed())
					cur.close();
				dbHandler.closeDatabase();	
			}catch(Exception e1){
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class), e1.getLocalizedMessage());
			}
		}


		return bList;
	}



	public  ArrayList<Group> SelectAllGroups() {
		ArrayList<Group> gList = new ArrayList<Group>();

		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		Cursor cur = dbHandler.selectRecords("SELECT * FROM "+TABLE_GROUP+" WHERE IsDeleted = 0");
		if (cur != null && cur.moveToFirst())
		{
			do {
				Group groups = new Group();
				groups.setEmail(cur.getString(cur.getColumnIndex("Email")));
				groups.setEnrollmentKey(cur.getString(cur.getColumnIndex("EnrollmentKey")));
				groups.setEnrollmentType(cur.getString(cur.getColumnIndex("EnrollmentType")));
				groups.setEnrollmentValue(cur.getString(cur.getColumnIndex("EnrollmentValue")));
				//groups.setGeoFence(geoFence);
				groups.setGroupLocation(cur.getString(cur.getColumnIndex("LastLocation")));
				groups.setGroupName(cur.getString(cur.getColumnIndex("Name")));
				groups.setPhoneNumber(cur.getString(cur.getColumnIndex("PhoneNumber")));
				groups.setType(cur.getString(cur.getColumnIndex("Type")));
				groups.setGroupID(cur.getString(cur.getColumnIndex("GroupId")));
				groups.setIsValidated(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("IsValidated"))));
				gList.add(groups);
			} while (cur.moveToNext());
		}
		if(cur!=null && !cur.isClosed())
			cur.close();
		dbHandler.closeDatabase();
		return gList;
	}


	public  ArrayList<AscGroups> SelectAllAsgGroups() {
		ArrayList<AscGroups> gList = new ArrayList<AscGroups>();
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		Cursor cur = dbHandler.selectRecords("SELECT * FROM "+TABLE_GROUP+";");
		if (cur != null && cur.moveToFirst())
		{
			do {

				AscGroups groups = new AscGroups();
				groups.setEmail(cur.getString(cur.getColumnIndex("Email")));
				groups.setEnrollmentKey(cur.getString(cur.getColumnIndex("EnrollmentKey")));
				groups.setEnrollmentType(cur.getString(cur.getColumnIndex("EnrollmentType")));
				groups.setEnrollmentValue(cur.getString(cur.getColumnIndex("EnrollmentValue")));
				//groups.setGeoFence(geoFence);
				groups.setGroupLocation(cur.getString(cur.getColumnIndex("LastLocation")));
				groups.setGroupName(cur.getString(cur.getColumnIndex("Name")));
				groups.setPhoneNumber(cur.getString(cur.getColumnIndex("PhoneNumber")));
				groups.setToRemove(AppConstant.intToBoolean(cur.getInt(cur.getColumnIndex("IsDeleted"))));
				groups.setType(cur.getString(cur.getColumnIndex("Type")));
				groups.setGroupID(cur.getString(cur.getColumnIndex("GroupId")));
				gList.add(groups);

			} while (cur.moveToNext());
		}
		if(cur!=null && !cur.isClosed())
			cur.close();
		dbHandler.closeDatabase();
		return gList;
	}
	public  ArrayList<String> SelectAllBuddyName() {
		ArrayList<String> bList = new ArrayList<String>();

		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		Cursor cur = dbHandler.selectRecords("SELECT * FROM "+TABLE_BUDDY+";");
		if (cur != null && cur.moveToFirst())
		{
			do {
				bList.add(cur.getString(cur.getColumnIndex("Name")));
			} while (cur.moveToNext());
		}
		if(cur!=null && !cur.isClosed())
			cur.close();
		dbHandler.closeDatabase();
		return bList;
	}

	public  ArrayList<String> SelectAllGroupsName() {
		ArrayList<String> bList = new ArrayList<String>();

		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		Cursor cur = dbHandler.selectRecords("SELECT * FROM "+TABLE_GROUP+" WHERE IsDeleted = 0");
		if (cur != null && cur.moveToFirst())
		{
			do {
				bList.add(cur.getString(cur.getColumnIndex("Name")));
			} while (cur.moveToNext());
		}
		if(cur!=null && !cur.isClosed())
			cur.close();
		dbHandler.closeDatabase();
		return bList;
	}

	public void removeAll(String tableName)
	{

		// If whereClause is null, it will delete all rows.
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		dbHandler.deleteAllRecords(tableName);
		dbHandler.closeDatabase();

	}

	public void markAllBuddiesAsDelete()
	{
		ArrayList<MyBuddies> bList= SelectAllBuddies();
		DatabaseHandler dbHandler = new DatabaseHandler(context);
		dbHandler.openDatabase();
		ContentValues values = new ContentValues();
		String tableName = TABLE_BUDDY;
		for (int i = 0; i < bList.size(); i++) {
			values.put("IsDeleted", true);
			Cursor cur = dbHandler.selectRecords("select * from "+tableName+" where PhoneNumber = "+"'"
					+bList.get(i).getMobileNumber()+"'");
			try{
				if (cur.moveToFirst())
					dbHandler.updateRecord(tableName, values, "PhoneNumber "+" = '"+ bList.get(i).getMobileNumber() + "'", null);
				if(cur!=null && !cur.isClosed())
					cur.close();
			}catch(Exception e){
				LogUtils.LOGE(LogUtils.makeLogTag(DBQueries.class),e.getLocalizedMessage());
			}
		}
		dbHandler.closeDatabase();
	}
}