package com.guardianapp.model;

import java.io.Serializable;

import com.google.gson.annotations.SerializedName;

public class CountryCodes implements Serializable{
	private static final long serialVersionUID = 1589262345353201165L;
	@SerializedName("countryName")
	private String countryName;
	@SerializedName("ISDCode")
	private String ISDCode;
	@SerializedName("maxPhoneDigits")
	private String maxPhoneDigits;
	@SerializedName("police")
	private String police;
	@SerializedName("fire")
	private String fire;
	@SerializedName("ambulance")
	private String ambulance;
	
	public String getCountryName() {
		return countryName;
	}
	public void setCountryName(String countryName) {
		this.countryName = countryName;
	}
	public String getISDCode() {
		return ISDCode;
	}
	public void setISDCode(String iSDCode) {
		ISDCode = iSDCode;
	}
	public String getMaxPhoneDigits() {
		return maxPhoneDigits;
	}
	public void setMaxPhoneDigits(String maxPhoneDigits) {
		this.maxPhoneDigits = maxPhoneDigits;
	}
	public String getPolice() {
		return police;
	}
	public void setPolice(String police) {
		this.police = police;
	}
	public String getFire() {
		return fire;
	}
	public void setFire(String fire) {
		this.fire = fire;
	}
	public String getAmbulance() {
		return ambulance;
	}
	public void setAmbulance(String ambulance) {
		this.ambulance = ambulance;
	}
	
}
