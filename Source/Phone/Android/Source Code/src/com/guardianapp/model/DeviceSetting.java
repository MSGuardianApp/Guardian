package com.guardianapp.model;

import java.io.Serializable;

import com.google.gson.annotations.SerializedName;

public class DeviceSetting implements Serializable{
	private static final long serialVersionUID = 4544161312141505424L;
	@SerializedName("ProfileID")
	private Long ProfileID ;
	@SerializedName("DeviceID")
	private String DeviceID ;
	@SerializedName("PlatForm")
	private String PlatForm ;
	@SerializedName("CanEmail")
	private boolean CanEmail ;
	@SerializedName("CanSMS")
	private boolean CanSMS ;

	public Long getProfileID() {
		return ProfileID;
	}
	public void setProfileID(Long profileID) {
		ProfileID = profileID;
	}
	public String getDeviceID() {
		return DeviceID;
	}
	public void setDeviceID(String deviceID) {
		DeviceID = deviceID;
	}
	public String getPlatForm() {
		return PlatForm;
	}
	public void setPlatForm(String platForm) {
		PlatForm = platForm;
	}
	public boolean isCanEmail() {
		return CanEmail;
	}
	public void setCanEmail(boolean canEmail) {
		CanEmail = canEmail;
	}
	public boolean isCanSMS() {
		return CanSMS;
	}
	public void setCanSMS(boolean canSMS) {
		CanSMS = canSMS;
	}

}
