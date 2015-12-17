package com.guardianapp.model;

import java.io.Serializable;

import com.google.gson.annotations.SerializedName;

public class GPSCoordinate implements Serializable{

	private static final long serialVersionUID = -331578126686299362L;

	@SerializedName("Lat")
	private String Lat;
	@SerializedName("Long")
	private String Long;
	@SerializedName("IsSOS")
	private int IsSOS;

	
	public String getLat() {
		return Lat;
	}

	public void setLat(String lat) {
		Lat = lat;
	}

	public String getLong() {
		return Long;
	}

	public void setLong(String longitude) {
		Long = longitude;
	}

	public int getIsSOS() {
		return IsSOS;
	}

	public void setIsSOS(int isSOS) {
		IsSOS = isSOS;
	}

	
	

	
}
