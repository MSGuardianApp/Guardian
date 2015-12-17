package com.guardianapp.model;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.Collection;
import java.util.HashSet;
import java.util.List;

import com.google.gson.annotations.SerializedName;

public class GeoTags implements Serializable{

	private static final long serialVersionUID = 282227395359060651L;
	@SerializedName("Alt")
	private List<String> Alt = new ArrayList<String>();
	@SerializedName("Cmd")
	private String Cmd;
	@SerializedName("GroupID")
	private String GroupID ;
	@SerializedName("Id")
	private String Id;
	@SerializedName("Lat")
	private List<String> Lat = new ArrayList<String>();
	@SerializedName("LocCnt")
	private int LocCnt;
	@SerializedName("Long")
	private List<String> Long = new ArrayList<String>();
	@SerializedName("IsSOS")
	private List<Boolean> IsSOS = new ArrayList<Boolean>() ;
	@SerializedName("PID")
	private Long PID ;
	@SerializedName("Spd")
	private List<String> Spd = new ArrayList<String>();
	@SerializedName("TS")
	private List<String> TS = new ArrayList<String>();
	 
	public String getCmd() {
		return Cmd;
	}
	public void setCmd(String cmd) {
		Cmd = cmd;
	}
	public String getGroupID() {
		return GroupID;
	}
	public void setGroupID(String groupID) {
		GroupID = groupID;
	}
	public String getId() {
		return Id;
	}
	public void setId(String id) {
		Id = id;
	}
	public int getLocCnt() {
		return LocCnt;
	}
	public void setLocCnt(int locCnt) {
		LocCnt = locCnt;
	}
	public Long getPID() {
		return PID;
	}
	public void setPID(Long pID) {
		PID = pID;
	}
	public List<String> getAlt() {
		return Alt;
	}
	public void setAlt(List<String> alt) {
		Alt = alt;
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
	public List<Boolean> getIsSOS() {
		return IsSOS;
	}
	public void setIsSOS(List<Boolean> isSOS) {
		IsSOS = isSOS;
	}
	public List<String> getSpd() {
		return Spd;
	}
	public void setSpd(List<String> spd) {
		Spd = spd;
	}
	public List<String> getTS() {
		return TS;
	}
	public void setTS(List<String> tS) {
		TS = tS;
	}
	
	
	
	 
	
   
  
    
   
}
