package com.guardianapp.model;

import java.io.Serializable;
import java.util.ArrayList;

import com.google.gson.annotations.SerializedName;

public class ProfileList implements Serializable{
	/**
	 * 
	 */
	private static final long serialVersionUID = -7213657947207308665L;
	@SerializedName("List")
	private ArrayList<Profile> List;
	@SerializedName("DataInfo")
	private ArrayList<DataInfo> DataInfo;
	
	public ArrayList<DataInfo> getDataInfo() {
		return DataInfo;
	}
	public void setDataInfo(ArrayList<DataInfo> DataInfo) {
		this.DataInfo = DataInfo;
	}
	
	
	public ArrayList<Profile> getList() {
		return List;
	}
	public void setList(ArrayList<Profile> list) {
		this.List = list;
	}

	
	

}
