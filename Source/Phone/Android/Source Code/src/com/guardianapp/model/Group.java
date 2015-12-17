package com.guardianapp.model;

import java.io.Serializable;

import com.google.gson.annotations.SerializedName;

public class Group implements Serializable{
		
	
	private static final long serialVersionUID = -2268963446847605505L;
	@SerializedName("Email")
	private String Email;
	@SerializedName("EnrollmentKey")
	private String EnrollmentKey;
	@SerializedName("EnrollmentType")
	private String EnrollmentType;
	@SerializedName("EnrollmentValue")
	private String EnrollmentValue;
	@SerializedName("GeoFence")
	private String GeoFence;
	@SerializedName("GroupID")
	private String GroupID;
	@SerializedName("GroupLocation")
	private String GroupLocation;
	@SerializedName("GroupName")
	private String GroupName;
	@SerializedName("IsActive")
	private boolean IsActive;
	@SerializedName("IsValidated")
	private boolean IsValidated;
	@SerializedName("LiveInfo")
	private String LiveInfo;
	@SerializedName("Members")
	private String Members;
	@SerializedName("PhoneNumber")
	private String PhoneNumber;
	@SerializedName("Tags")
	private String Tags;
	@SerializedName("ToRemove")
	private boolean ToRemove;
	@SerializedName("Type")
	private String Type;
	
	public String getEmail() {
		return Email;
	}
	public void setEmail(String email) {
		Email = email;
	}
	public String getEnrollmentKey() {
		return EnrollmentKey;
	}
	public void setEnrollmentKey(String enrollmentKey) {
		EnrollmentKey = enrollmentKey;
	}
	public String getEnrollmentType() {
		return EnrollmentType;
	}
	public void setEnrollmentType(String enrollmentType) {
		EnrollmentType = enrollmentType;
	}
	public String getEnrollmentValue() {
		return EnrollmentValue;
	}
	public void setEnrollmentValue(String enrollmentValue) {
		EnrollmentValue = enrollmentValue;
	}
	public String getGeoFence() {
		return GeoFence;
	}
	public void setGeoFence(String geoFence) {
		GeoFence = geoFence;
	}
	public String getGroupID() {
		return GroupID;
	}
	public void setGroupID(String groupID) {
		GroupID = groupID;
	}
	public String getGroupLocation() {
		return GroupLocation;
	}
	public void setGroupLocation(String groupLocation) {
		GroupLocation = groupLocation;
	}
	public String getGroupName() {
		return GroupName;
	}
	public void setGroupName(String groupName) {
		GroupName = groupName;
	}
	public boolean isIsActive() {
		return IsActive;
	}
	public void setIsActive(boolean isActive) {
		IsActive = isActive;
	}
	public boolean isIsValidated() {
		return IsValidated;
	}
	public void setIsValidated(boolean isValidated) {
		IsValidated = isValidated;
	}
	public String getLiveInfo() {
		return LiveInfo;
	}
	public void setLiveInfo(String liveInfo) {
		LiveInfo = liveInfo;
	}
	public String getMembers() {
		return Members;
	}
	public void setMembers(String members) {
		Members = members;
	}
	public String getPhoneNumber() {
		return PhoneNumber;
	}
	public void setPhoneNumber(String phoneNumber) {
		PhoneNumber = phoneNumber;
	}
	public String getTags() {
		return Tags;
	}
	public void setTags(String tags) {
		Tags = tags;
	}
	public boolean isToRemove() {
		return ToRemove;
	}
	public void setToRemove(boolean toRemove) {
		ToRemove = toRemove;
	}
	public String getType() {
		return Type;
	}
	public void setType(String type) {
		Type = type;
	}
	@Override
	public boolean equals(Object object) {
		// TODO Auto-generated method stub
		if(object!=null && object instanceof Group){
			Group group = (Group)object;
			if(group.getGroupName().equalsIgnoreCase(this.getGroupName()))
				return true;
		}
		return false;
	}
	
	
	
}
