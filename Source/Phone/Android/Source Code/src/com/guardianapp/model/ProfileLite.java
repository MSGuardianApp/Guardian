package com.guardianapp.model;

import java.io.Serializable;
import java.util.List;

import com.google.gson.annotations.SerializedName;

public class ProfileLite implements Serializable{

	/**
	 * 
	 */
	private static final long serialVersionUID = 3286737048140878716L;
	
	@SerializedName("ProfileID")
	private Long ProfileID ;
	@SerializedName("SOSToken")
	private String SOSToken ;
	@SerializedName("SessionID")
	private String SessionID ;
	@SerializedName("LastLocs")
	private List<GeoTag> LastLocs ;
	@SerializedName("IsTrackingOn")
	private boolean IsTrackingOn ;
	@SerializedName("IsSOSOn")
	private boolean IsSOSOn ;
	@SerializedName("TinyURI")
	private String TinyURI ;
	@SerializedName("UserID")
	private Long UserID ;
	@SerializedName("Name")
	private String Name ;
	@SerializedName("Email")
	private String Email;
	@SerializedName("LiveDetails")
	private LiveDetails LiveDetails ;
	@SerializedName("FBID")
	private String FBID ;
	@SerializedName("FBAuthID")
	private String FBAuthID ;
	@SerializedName("MobileNumber")
	private String MobileNumber ;
	@SerializedName("RegionCode")
	private String RegionCode ;
	@SerializedName("DataInfo")
	private List<DataInfo> DataInfo ;
    
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
	public String getSessionID() {
		return SessionID;
	}
	public void setSessionID(String sessionID) {
		SessionID = sessionID;
	}
	public List<GeoTag> getLastLocs() {
		return LastLocs;
	}
	public void setLastLocs(List<GeoTag> lastLocs) {
		LastLocs = lastLocs;
	}
	public boolean isIsTrackingOn() {
		return IsTrackingOn;
	}
	public void setIsTrackingOn(boolean isTrackingOn) {
		IsTrackingOn = isTrackingOn;
	}
	public boolean isIsSOSOn() {
		return IsSOSOn;
	}
	public void setIsSOSOn(boolean isSOSOn) {
		IsSOSOn = isSOSOn;
	}
	public String getTinyURI() {
		return TinyURI;
	}
	public void setTinyURI(String tinyURI) {
		TinyURI = tinyURI;
	}
	public Long getUserID() {
		return UserID;
	}
	public void setUserID(Long userID) {
		UserID = userID;
	}
	public String getName() {
		return Name;
	}
	public void setName(String name) {
		Name = name;
	}
	public String getEmail() {
		return Email;
	}
	public void setEmail(String email) {
		Email = email;
	}
	public LiveDetails getLiveDetails() {
		return LiveDetails;
	}
	public void setLiveDetails(LiveDetails liveDetails) {
		LiveDetails = liveDetails;
	}
	public String getFBID() {
		return FBID;
	}
	public void setFBID(String fBID) {
		FBID = fBID;
	}
	public String getFBAuthID() {
		return FBAuthID;
	}
	public void setFBAuthID(String fBAuthID) {
		FBAuthID = fBAuthID;
	}
	public String getMobileNumber() {
		return MobileNumber;
	}
	public void setMobileNumber(String mobileNumber) {
		MobileNumber = mobileNumber;
	}
	public String getRegionCode() {
		return RegionCode;
	}
	public void setRegionCode(String regionCode) {
		RegionCode = regionCode;
	}
	public List<DataInfo> getDataInfo() {
		return DataInfo;
	}
	public void setDataInfo(List<DataInfo> dataInfo) {
		DataInfo = dataInfo;
	}
	
	

 
}
