package com.guardianapp.model;

import java.io.Serializable;

import com.google.gson.annotations.SerializedName;

public class User implements Serializable{

	
	/**
	 * 
	 */
	private static final long serialVersionUID = -808345728965726826L;
	
	@SerializedName("autoUserId")
	private int autoUserId;
	@SerializedName("userId")
	private Long userId;
	@SerializedName("name")
	private String name;
	@SerializedName("liveEmail")
	private String liveEmail;
	@SerializedName("liveAuthId")
	private String liveAuthId;
	@SerializedName("fbAuthId")
	private String fbAuthId;
	@SerializedName("currentProfileId")
	private Long currentProfileId;
    
    public int getAutoUserId() {
		return autoUserId;
	}
	public void setAutoUserId(int autoUserId) {
		this.autoUserId = autoUserId;
	}
	public Long getUserId() {
		return userId;
	}
	public void setUserId(Long userId) {
		this.userId = userId;
	}
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public String getLiveEmail() {
		return liveEmail;
	}
	public void setLiveEmail(String liveEmail) {
		this.liveEmail = liveEmail;
	}
	public String getLiveAuthId() {
		return liveAuthId;
	}
	public void setLiveAuthId(String liveAuthId) {
		this.liveAuthId = liveAuthId;
	}
	public String getFbAuthId() {
		return fbAuthId;
	}
	public void setFbAuthId(String fbAuthId) {
		this.fbAuthId = fbAuthId;
	}
	public Long getCurrentProfileId() {
		return currentProfileId;
	}
	public void setCurrentProfileId(Long currentProfileId) {
		this.currentProfileId = currentProfileId;
	}
	
    
}
