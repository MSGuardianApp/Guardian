package com.guardianapp.model;

import java.io.Serializable;

import com.google.gson.annotations.SerializedName;

public class Version implements Serializable{

	/**
	 * 
	 */
	private static final long serialVersionUID = 6516629874616824888L;
	
	@SerializedName("version")
	private String version;
	@SerializedName("versionHistory")
	private String versionHistory;
	
	public String getVersion() {
		return version;
	}
	public void setVersion(String version) {
		this.version = version;
	}
	public String getVersionHistory() {
		return versionHistory;
	}
	public void setVersionHistory(String versionHistory) {
		this.versionHistory = versionHistory;
	}
	
}
