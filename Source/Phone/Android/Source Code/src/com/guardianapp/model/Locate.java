package com.guardianapp.model;

import java.io.Serializable;

import com.google.gson.annotations.SerializedName;

public class Locate implements Serializable{

	/**
	 * 
	 */
	private static final long serialVersionUID = -7922898545023068558L;
	@SerializedName("name")
	private String name;
	@SerializedName("phoneNumber")
	private String phoneNumber;
	@SerializedName("location")
	private String location;

	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public String getPhoneNumber() {
		return phoneNumber;
	}
	public void setPhoneNumber(String phoneNumber) {
		this.phoneNumber = phoneNumber;
	}
	public String getLocation() {
		return location;
	}
	public void setLocation(String location) {
		this.location = location;
	}

}
