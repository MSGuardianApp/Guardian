package com.guardianapp.model;

import java.io.Serializable;
import java.util.ArrayList;

import com.google.gson.annotations.SerializedName;

public class MyBuddies implements Serializable{
	
	/**
	 * 
	 */
	private static final long serialVersionUID = 3300024912307234206L;
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
	@SerializedName("BuddyID")
    private Long BuddyID;
	@SerializedName("State")
    private String State;
	@SerializedName("IsPrimeBuddy")
    private boolean IsPrimeBuddy;
	@SerializedName("ToRemove")
    private boolean ToRemove;
    
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
	public Long getBuddyID() {
		return BuddyID;
	}
	public void setBuddyID(Long buddyID) {
		BuddyID = buddyID;
	}
	public String getState() {
		return State;
	}
	public void setState(String state) {
		State = state;
	}
	public boolean isIsPrimeBuddy() {
		return IsPrimeBuddy;
	}
	public void setIsPrimeBuddy(boolean isPrimeBuddy) {
		IsPrimeBuddy = isPrimeBuddy;
	}
	public boolean isToRemove() {
		return ToRemove;
	}
	public void setToRemove(boolean toRemove) {
		ToRemove = toRemove;
	}
	@Override
	public boolean equals(Object object) {
		// TODO Auto-generated method stub
		if(object!=null && object instanceof MyBuddies){
			MyBuddies buddy = (MyBuddies)object;
			if(buddy.getMobileNumber().equalsIgnoreCase(this.getMobileNumber()))
				return true;
		}
		return false;
	}
	
	

    

}
