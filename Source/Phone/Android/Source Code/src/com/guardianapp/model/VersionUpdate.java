package com.guardianapp.model;

import java.io.Serializable;

import com.google.gson.annotations.SerializedName;

public class VersionUpdate implements Serializable {

	/**
	 * 
	 */
	private static final long serialVersionUID = -2263248312952579330L;
	
	@SerializedName("IsProfileActive")
	private boolean IsProfileActive;
	@SerializedName("IsGroupModified")
	private boolean IsGroupModified;
	@SerializedName("ServerVersion")
	private String ServerVersion;
	public boolean isIsProfileActive() {
		return IsProfileActive;
	}
	public void setIsProfileActive(boolean isProfileActive) {
		IsProfileActive = isProfileActive;
	}
	public boolean isIsGroupModified() {
		return IsGroupModified;
	}
	public void setIsGroupModified(boolean isGroupModified) {
		IsGroupModified = isGroupModified;
	}
	public String getServerVersion() {
		return ServerVersion;
	}
	public void setServerVersion(String serverVersion) {
		ServerVersion = serverVersion;
	}

	
}
