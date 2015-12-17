package com.guardianapp.model;

import java.io.Serializable;

import com.google.gson.annotations.SerializedName;

public class GeoTag implements Serializable{
	private static final long serialVersionUID = 8077598165655720051L;
	
	@SerializedName("SessionID")
	private String SessionID ;
	@SerializedName("Lat")
	private String Lat ;
	@SerializedName("Long")
	private String Long ;
	@SerializedName("Alt")
	private String Alt ;
	@SerializedName("GeoDirection")
	private String GeoDirection ;
	@SerializedName("Speed")
	private int Speed ;
	@SerializedName("TimeStamp")
	private long TimeStamp ;
	@SerializedName("ProfileID")
	private Long ProfileID ;
	@SerializedName("Command")
	private String Command ;
	@SerializedName("MediaContent")
	private byte[] MediaContent ;
	@SerializedName("MediaUri")
	private String MediaUri ;
	@SerializedName("GroupID")
	private String GroupID ;
	@SerializedName("AdditionalInfo")
	private String AdditionalInfo ;
	@SerializedName("IsSOS")
	private int IsSOS;
    
 
    public byte[] getMediaContent() {
		return MediaContent;
	}
	public void setMediaContent(byte[] mediaContent) {
		MediaContent = mediaContent;
	}
	public String getSessionID() {
		return SessionID;
	}
	public void setSessionID(String sessionID) {
		SessionID = sessionID;
	}
	public String getLat() {
		return Lat;
	}
	public void setLat(String lat) {
		Lat = lat;
	}
	public String getLong() {
		return Long;
	}
	public void setLong(String l) {
		Long = l;
	}
	public String getAlt() {
		return Alt;
	}
	public void setAlt(String alt) {
		Alt = alt;
	}
	public String getGeoDirection() {
		return GeoDirection;
	}
	public void setGeoDirection(String geoDirection) {
		GeoDirection = geoDirection;
	}
	public int getSpeed() {
		return Speed;
	}
	public void setSpeed(int speed) {
		Speed = speed;
	}
	public long getTimeStamp() {
		return TimeStamp;
	}
	public void setTimeStamp(long timeStamp) {
		TimeStamp = timeStamp;
	}
	public Long getProfileID() {
		return ProfileID;
	}
	public void setProfileID(Long profileID) {
		ProfileID = profileID;
	}
	public String getCommand() {
		return Command;
	}
	public void setCommand(String command) {
		Command = command;
	}
	
	public String getMediaUri() {
		return MediaUri;
	}
	public void setMediaUri(String mediaUri) {
		MediaUri = mediaUri;
	}
	public String getGroupID() {
		return GroupID;
	}
	public void setGroupID(String groupID) {
		GroupID = groupID;
	}
	public String getAdditionalInfo() {
		return AdditionalInfo;
	}
	public void setAdditionalInfo(String additionalInfo) {
		AdditionalInfo = additionalInfo;
	}
	public int getIsSOS() {
		return IsSOS;
	}
	public void setIsSOS(int isSOS) {
		IsSOS = isSOS;
	}
	
	
	
	
	
}
