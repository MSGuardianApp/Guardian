package com.guardianapp.model;

import java.io.Serializable;
import java.util.ArrayList;

import com.google.gson.annotations.SerializedName;

public class LocalProfile implements Serializable{

	/**
	 * 
	 */
	private static final long serialVersionUID = 4732901164757633879L;
	@SerializedName("DataInfo")
	private ArrayList<DataInfo> DataInfo;
	@SerializedName("Email")
	private String Email;
	@SerializedName("FBAuthID")
	private String FBAuthID;
	@SerializedName("FBID")
	private String FBID;
	@SerializedName("LiveDetails")
	private LiveDetails LiveDetails;
	@SerializedName("MobileNumber")
	private String MobileNumber;
	@SerializedName("Name")
	private String Name;
	@SerializedName("RegionCode")
	private String RegionCode;
	@SerializedName("UserID")
	private Long UserID;
	@SerializedName("IsSOSOn")
	private boolean IsSOSOn;
	@SerializedName("IsTrackingOn")
	private boolean IsTrackingOn;
	@SerializedName("LastLocs")
	private String LastLocs;
	@SerializedName("ProfileID")
	private Long ProfileID;
	@SerializedName("SOSToken")
	private String SOSToken;
	@SerializedName("TinyURI")
	private String TinyURI;
	@SerializedName("SessionID")
	private String SessionID;
	@SerializedName("AscGroups")
	private ArrayList<AscGroups> AscGroups;
	@SerializedName("IsSOSStatusSynced")
	private boolean IsSOSStatusSynced;
	@SerializedName("IsTrackingStatusSynced")
	private boolean IsTrackingStatusSynced;
	@SerializedName("CanArchive")
	private boolean CanArchive;
	@SerializedName("CanMail")
	private boolean CanMail;
	@SerializedName("CanPost")
	private boolean CanPost;
	@SerializedName("CanSMS")
	private boolean CanSMS;
	@SerializedName("FBGroupID")
	private String FBGroupID;
	@SerializedName("FBGroupName")
	private String FBGroupName;
	@SerializedName("IsValid")
	private boolean IsValid;
	@SerializedName("LocateBuddies")
	private ArrayList<ProfileLite> LocateBuddies;
	@SerializedName("LocationConsent")
	private boolean LocationConsent;
	@SerializedName("MyBuddies")
	private ArrayList<MyBuddies> MyBuddies;
	@SerializedName("PhoneSetting")
	private DeviceSetting PhoneSetting;
	@SerializedName("PrimeGroupID")
	private String PrimeGroupID;
	@SerializedName("SMSText")
	private String SMSText;
	@SerializedName("SecurityToken")
	private String SecurityToken;
	@SerializedName("SiteSetting")
	private PortalSetting SiteSetting;
	@SerializedName("CountryCode")
	private String CountryCode;
	@SerializedName("PoliceContact")
	private String PoliceContact;
	@SerializedName("AmbulanceContact")
	private String AmbulanceContact;
	@SerializedName("FireContact")
	private String FireContact;
	@SerializedName("PostLocationConsent")
	private boolean PostLocationConsent;
	@SerializedName("IsDataSynced")
	private boolean IsDataSynced;
	@SerializedName("MessageTemplate")
	private String MessageTemplate;
	@SerializedName("SessionToken")
	private String SessionToken;
	
	
	public String getSessionToken() {
		return SessionToken;
	}
	public void setSessionToken(String sessionToken) {
		SessionToken = sessionToken;
	}
	public String getCountryCode() {
		return CountryCode;
	}
	public void setCountryCode(String countryCode) {
		CountryCode = countryCode;
	}
	public String getPoliceContact() {
		return PoliceContact;
	}
	public void setPoliceContact(String policeContact) {
		PoliceContact = policeContact;
	}
	public String getAmbulanceContact() {
		return AmbulanceContact;
	}
	public void setAmbulanceContact(String ambulanceContact) {
		AmbulanceContact = ambulanceContact;
	}
	public String getFireContact() {
		return FireContact;
	}
	public void setFireContact(String fireContact) {
		FireContact = fireContact;
	}

	public boolean isPostLocationConsent() {
		return PostLocationConsent;
	}
	public void setPostLocationConsent(boolean postLocationConsent) {
		PostLocationConsent = postLocationConsent;
	}
	public boolean isIsSOSStatusSynced() {
		return IsSOSStatusSynced;
	}
	public void setIsSOSStatusSynced(boolean isSOSStatusSynced) {
		IsSOSStatusSynced = isSOSStatusSynced;
	}
	public boolean isIsTrackingStatusSynced() {
		return IsTrackingStatusSynced;
	}
	public void setIsTrackingStatusSynced(boolean isTrackingStatusSynced) {
		IsTrackingStatusSynced = isTrackingStatusSynced;
	}
	public boolean isIsDataSynced() {
		return IsDataSynced;
	}
	public void setIsDataSynced(boolean isDataSynced) {
		IsDataSynced = isDataSynced;
	}
	public String getMessageTemplate() {
		return MessageTemplate;
	}
	public void setMessageTemplate(String messageTemplate) {
		MessageTemplate = messageTemplate;
	}
	
	////////////////////////////////////////////////////////////
	//profile.CountryCode = "+91";
    //  profile.PoliceContact = "100";
   //   profile.AmbulanceContact = "108";
    //  profile.FireContact = "101";
	
	
	
	
	public ArrayList<DataInfo> getDataInfo() {
		return DataInfo;
	}
	public void setDataInfo(ArrayList<DataInfo> dataInfo) {
		DataInfo = dataInfo;
	}
	public String getEmail() {
		return Email;
	}
	public void setEmail(String email) {
		Email = email;
	}
	public String getFBAuthID() {
		return FBAuthID;
	}
	public void setFBAuthID(String fBAuthID) {
		FBAuthID = fBAuthID;
	}
	public String getFBID() {
		return FBID;
	}
	public void setFBID(String fBID) {
		FBID = fBID;
	}
	public LiveDetails getLiveDetails() {
		return LiveDetails;
	}
	public void setLiveDetails(LiveDetails liveDetails) {
		LiveDetails = liveDetails;
	}
	public String getMobileNumber() {
		return MobileNumber;
	}
	public void setMobileNumber(String mobileNumber) {
		MobileNumber = mobileNumber;
	}
	public String getName() {
		return Name;
	}
	public void setName(String name) {
		Name = name;
	}
	public String getRegionCode() {
		return RegionCode;
	}
	public void setRegionCode(String regionCode) {
		RegionCode = regionCode;
	}
	public Long getUserID() {
		return UserID;
	}
	public void setUserID(Long userID) {
		UserID = userID;
	}
	public boolean isIsSOSOn() {
		return IsSOSOn;
	}
	public void setIsSOSOn(boolean isSOSOn) {
		IsSOSOn = isSOSOn;
	}
	public boolean isIsTrackingOn() {
		return IsTrackingOn;
	}
	public void setIsTrackingOn(boolean isTrackingOn) {
		IsTrackingOn = isTrackingOn;
	}
	public String getLastLocs() {
		return LastLocs;
	}
	public void setLastLocs(String lastLocs) {
		LastLocs = lastLocs;
	}
	public Long getProfileID() {
		return ProfileID;
	}
	public void setProfileID(Long profileID) {
		ProfileID = profileID;
	}
	public String getSOSToken() {
		return SOSToken;
	}
	public void setSOSToken(String sOSToken) {
		SOSToken = sOSToken;
	}
	public String getTinyURI() {
		return TinyURI;
	}
	public void setTinyURI(String tinyURI) {
		TinyURI = tinyURI;
	}
	
	public String getSessionID() {
		return SessionID;
	}
	public void setSessionID(String sessionID) {
		SessionID = sessionID;
	}
	public ArrayList<AscGroups> getAscGroups() {
		return AscGroups;
	}
	public void setAscGroups(ArrayList<AscGroups> ascGroups) {
		AscGroups = ascGroups;
	}
	public boolean isCanArchive() {
		return CanArchive;
	}
	public void setCanArchive(boolean canArchive) {
		CanArchive = canArchive;
	}
	public boolean isCanMail() {
		return CanMail;
	}
	public void setCanMail(boolean canMail) {
		CanMail = canMail;
	}
	public boolean isCanPost() {
		return CanPost;
	}
	public void setCanPost(boolean canPost) {
		CanPost = canPost;
	}
	public boolean isCanSMS() {
		return CanSMS;
	}
	public void setCanSMS(boolean canSMS) {
		CanSMS = canSMS;
	}
	public String getFBGroupID() {
		return FBGroupID;
	}
	public void setFBGroupID(String fBGroupID) {
		FBGroupID = fBGroupID;
	}
	public String getFBGroupName() {
		return FBGroupName;
	}
	public void setFBGroupName(String fBGroupName) {
		FBGroupName = fBGroupName;
	}
	public boolean isIsValid() {
		return IsValid;
	}
	public void setIsValid(boolean isValid) {
		IsValid = isValid;
	}
	public ArrayList<ProfileLite> getLocateBuddies() {
		return LocateBuddies;
	}
	public void setLocateBuddies(ArrayList<ProfileLite> locateBuddies) {
		LocateBuddies = locateBuddies;
	}
	public boolean isLocationConsent() {
		return LocationConsent;
	}
	public void setLocationConsent(boolean locationConsent) {
		LocationConsent = locationConsent;
	}
	public ArrayList<MyBuddies> getMyBuddies() {
		return MyBuddies;
	}
	public void setMyBuddies(ArrayList<MyBuddies> myBuddies) {
		MyBuddies = myBuddies;
	}
	public DeviceSetting getPhoneSetting() {
		return PhoneSetting;
	}
	public void setPhoneSetting(DeviceSetting phoneSetting) {
		PhoneSetting = phoneSetting;
	}
	public String getPrimeGroupID() {
		return PrimeGroupID;
	}
	public void setPrimeGroupID(String primeGroupID) {
		PrimeGroupID = primeGroupID;
	}
	public String getSMSText() {
		return SMSText;
	}
	public void setSMSText(String sMSText) {
		SMSText = sMSText;
	}
	public String getSecurityToken() {
		return SecurityToken;
	}
	public void setSecurityToken(String securityToken) {
		SecurityToken = securityToken;
	}
	public PortalSetting getSiteSetting() {
		return SiteSetting;
	}
	public void setSiteSetting(PortalSetting siteSetting) {
		SiteSetting = siteSetting;
	}
	
	
	
}
