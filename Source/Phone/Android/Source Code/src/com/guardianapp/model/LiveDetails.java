package com.guardianapp.model;

import java.io.Serializable;

import com.google.gson.annotations.SerializedName;

public class LiveDetails implements Serializable{

	/**
	 * 
	 */
	private static final long serialVersionUID = -1313163784032168573L;
	@SerializedName("LiveAuthID")
	private String LiveAuthID;
	@SerializedName("LiveAccessToken")
	private String LiveAccessToken;
	@SerializedName("LiveRefreshToken")
	private String LiveRefreshToken;
	@SerializedName("LiveID")
	private String LiveID;
    
    public String getLiveAuthID() {
		return LiveAuthID;
	}
	public void setLiveAuthID(String liveAuthID) {
		LiveAuthID = liveAuthID;
	}
	public String getLiveAccessToken() {
		return LiveAccessToken;
	}
	public void setLiveAccessToken(String liveAccessToken) {
		LiveAccessToken = liveAccessToken;
	}
	public String getLiveRefreshToken() {
		return LiveRefreshToken;
	}
	public void setLiveRefreshToken(String liveRefreshToken) {
		LiveRefreshToken = liveRefreshToken;
	}
	public String getLiveID() {
		return LiveID;
	}
	public void setLiveID(String liveID) {
		LiveID = liveID;
	}

}