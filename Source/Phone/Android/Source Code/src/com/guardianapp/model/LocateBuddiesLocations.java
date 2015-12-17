package com.guardianapp.model;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

public class LocateBuddiesLocations implements Serializable {

	/**
	 * 
	 */
	private static final long serialVersionUID = -6638431632461222692L;

	private Long PID;
	private int LocCnt;
	private String id;
	private List<String> Alt = new ArrayList<String>();
	private List<Boolean> IsSOS  = new ArrayList<Boolean>();
	private List<Double> Accuracy = new ArrayList<Double>();
	private List<String> Lat = new ArrayList<String>();
	private List<String> Long = new ArrayList<String>();
	private List<Integer> speed = new ArrayList<Integer>();
	private List<Long> TS = new ArrayList<Long>();
	public Long getPID() {
		return PID;
	}
	public void setPID(Long pID) {
		PID = pID;
	}
	public int getLocCnt() {
		return LocCnt;
	}
	public void setLocCnt(int locCnt) {
		LocCnt = locCnt;
	}
	public String getId() {
		return id;
	}
	public void setId(String id) {
		this.id = id;
	}

	public List<String> getAlt() {
		return Alt;
	}
	public void setAlt(List<String> alt) {
		Alt = alt;
	}

	public List<Boolean> getIsSOS() {
		return IsSOS;
	}
	public void setIsSOS(List<Boolean> isSOS) {
		IsSOS = isSOS;
	}
	public List<Double> getAccuracy() {
		return Accuracy;
	}
	public void setAccuracy(List<Double> accuracy) {
		Accuracy = accuracy;
	}
	public List<String> getLat() {
		return Lat;
	}
	public void setLat(List<String> lat) {
		Lat = lat;
	}
	public List<String> getLong() {
		return Long;
	}
	public void setLong(List<String> l) {
		Long = l;
	}
	public List<Integer> getSpeed() {
		return speed;
	}
	public void setSpeed(List<Integer> speed) {
		this.speed = speed;
	}
	public List<Long> getTS() {
		return TS;
	}
	public void setTS(List<Long> tS) {
		TS = tS;
	}






}
