package com.guardianapp.model;

import java.io.Serializable;
import java.util.List;

import com.google.gson.annotations.SerializedName;

//phani
public class LocateBuddies implements Serializable{
	/**
	 * 
	 */
	private static final long serialVersionUID = 1067416772476682157L;
	@SerializedName("name")
	private String name;
	@SerializedName("borderThickness")
	private String borderThickness;
	@SerializedName("orderNumber")
	private int orderNumber;
	@SerializedName("buddyStatusColor")
	private String buddyStatusColor;
	@SerializedName("bid")
	private int bid;
	@SerializedName("myProfileId")
	private Long myProfileId;
	@SerializedName("buddyUserId")
	private Long buddyUserId;
	@SerializedName("buddyProfileId")
	private Long buddyProfileId;
	@SerializedName("shortTrackingURL")
	private String shortTrackingURL;
	@SerializedName("email")
	private String email;
	@SerializedName("lastLocation")
	private String lastLocation;
	@SerializedName("phoneNumber")
	private String phoneNumber;
	@SerializedName("SessionID")
	private String SessionID;
	@SerializedName("isDeleted")
	private Boolean isDeleted;
	@SerializedName("lastLocs")
	private List<GeoTag> lastLocs;

	public List<GeoTag> getLastLocs() {
		return lastLocs;
	}
	public void setLastLocs(List<GeoTag> lastLocs) {
		this.lastLocs = lastLocs;
	}
	public int getBID() {
		return bid;
	}
	public void setBID(int bid) {
		this.bid = bid;
	}

	public Long getMyProfileId() {
		return myProfileId;
	}
	public void setMyProfileId(Long myProfileId) {
		this.myProfileId = myProfileId;
	}

	public Long getBuddyUserId() {
		return buddyUserId;
	}
	public void setBuddyUserId(Long buddyUserId) {
		this.buddyUserId = buddyUserId;
	}

	public Long getBuddyProfileId() {
		return buddyProfileId;
	}
	public void setBuddyProfileId(Long buddyProfileId) {
		this.buddyProfileId = buddyProfileId;
	}

	public String getShortTrackingURL() {
		return shortTrackingURL;
	}
	public void setShortTrackingURL(String shortTrackingURL) {
		this.shortTrackingURL = shortTrackingURL;
	}

	public String getEmail() {
		return email;
	}
	public void setEmail(String email) {
		this.email = email;
	}

	public String getLastLocation() {
		return lastLocation;
	}
	public void setLastLocation(String lastLocation) {
		this.lastLocation = lastLocation;
	}

	public String getPhoneNumber() {
		return phoneNumber;
	}
	public void setPhoneNumber(String phoneNumber) {
		this.phoneNumber = phoneNumber;
	}

	public int getBid() {
		return bid;
	}
	public void setBid(int bid) {
		this.bid = bid;
	}
	public String getSessionID() {
		return SessionID;
	}
	public void setSessionID(String sessionID) {
		SessionID = sessionID;
	}
	public Boolean getIsDeleted() {
		return isDeleted;
	}
	public void setIsDeleted(Boolean isDeleted) {
		this.isDeleted = isDeleted;
	}

	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public String getBorderThickness() {
		return borderThickness;
	}
	public void setBorderThickness(String borderThickness) {
		this.borderThickness = borderThickness;
	}
	public int getOrderNumber() {
		return orderNumber;
	}
	public void setOrderNumber(int orderNumber) {
		this.orderNumber = orderNumber;
	}
	public String getBuddyStatusColor() {
		return buddyStatusColor;
	}
	public void setBuddyStatusColor(String buddyStatusColor) {
		this.buddyStatusColor = buddyStatusColor;
	}

	@Override
	public boolean equals(Object o) {
		// TODO Auto-generated method stub
		if(o == null)
			return false;
		if(!(o instanceof LocateBuddies))
			return false;

		LocateBuddies other = (LocateBuddies)o;
		if(this.getBuddyProfileId().longValue() == other.getBuddyProfileId().longValue())
			return true;
		
		return false;
	}
}
