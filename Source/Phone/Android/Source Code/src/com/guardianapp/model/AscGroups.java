package com.guardianapp.model;

import java.io.Serializable;
import java.util.ArrayList;

import com.google.gson.annotations.SerializedName;

public class AscGroups implements Serializable{
	private static final long serialVersionUID = -513434220458962099L;
	@SerializedName("GroupID")  
	private String GroupID ;
	@SerializedName("GroupName")
	private String GroupName; 
	@SerializedName("GroupLocation")
	private String GroupLocation;
	@SerializedName("EnrollmentKey")
	private String EnrollmentKey;
	@SerializedName("EnrollmentValue")
	private String EnrollmentValue;
	@SerializedName("ToRemove")
	private boolean ToRemove; 
	@SerializedName("PhoneNumber")
	private String PhoneNumber;
	@SerializedName("Email")
	private String Email; 
	@SerializedName("IsActive")
	private boolean IsActive; 
	@SerializedName("GeoFence")
	private ArrayList<GeoTag> GeoFence ;
	@SerializedName("LiveInfo")
	private LiveDetails LiveInfo;
	@SerializedName("Members")
	private ArrayList<Profile> Members ;
	@SerializedName("Tags")
	private ArrayList<String> Tags;
	@SerializedName("Type")
	private String Type;
	@SerializedName("EnrollmentType")
	private String EnrollmentType;
	@SerializedName("IsValidated")
	private boolean IsValidated;
	
	public String getGroupID() {
		return GroupID;
	}
	public void setGroupID(String groupID) {
		GroupID = groupID;
	}
	public String getGroupName() {
		return GroupName;
	}
	public void setGroupName(String groupName) {
		GroupName = groupName;
	}
	public String getGroupLocation() {
		return GroupLocation;
	}
	public void setGroupLocation(String groupLocation) {
		GroupLocation = groupLocation;
	}
	public String getEnrollmentKey() {
		return EnrollmentKey;
	}
	public void setEnrollmentKey(String enrollmentKey) {
		EnrollmentKey = enrollmentKey;
	}
	public String getEnrollmentValue() {
		return EnrollmentValue;
	}
	public void setEnrollmentValue(String enrollmentValue) {
		EnrollmentValue = enrollmentValue;
	}
	public boolean isToRemove() {
		return ToRemove;
	}
	public void setToRemove(boolean toRemove) {
		ToRemove = toRemove;
	}
	public String getPhoneNumber() {
		return PhoneNumber;
	}
	public void setPhoneNumber(String phoneNumber) {
		PhoneNumber = phoneNumber;
	}
	public String getEmail() {
		return Email;
	}
	public void setEmail(String email) {
		Email = email;
	}
	public boolean isIsActive() {
		return IsActive;
	}
	public void setIsActive(boolean isActive) {
		IsActive = isActive;
	}
	public ArrayList<GeoTag> getGeoFence() {
		return GeoFence;
	}
	public void setGeoFence(ArrayList<GeoTag> geoFence) {
		GeoFence = geoFence;
	}
	public LiveDetails getLiveInfo() {
		return LiveInfo;
	}
	public void setLiveInfo(LiveDetails liveInfo) {
		LiveInfo = liveInfo;
	}
	public ArrayList<Profile> getMembers() {
		return Members;
	}
	public void setMembers(ArrayList<Profile> members) {
		Members = members;
	}
	public ArrayList<String> getTags() {
		return Tags;
	}
	public void setTags(ArrayList<String> tags) {
		Tags = tags;
	}
	public String getType() {
		return Type;
	}
	public void setType(String type) {
		Type = type;
	}
	public String getEnrollmentType() {
		return EnrollmentType;
	}
	public void setEnrollmentType(String enrollmentType) {
		EnrollmentType = enrollmentType;
	}
	public boolean isIsValidated() {
		return IsValidated;
	}
	public void setIsValidated(boolean isValidated) {
		IsValidated = isValidated;
	}
			
}
