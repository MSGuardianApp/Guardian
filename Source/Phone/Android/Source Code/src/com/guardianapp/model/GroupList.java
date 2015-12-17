package com.guardianapp.model;


import java.io.Serializable;
import java.util.ArrayList;

import com.google.gson.annotations.SerializedName;

public class GroupList implements Serializable{
	/**
	 * 
	 */
	private static final long serialVersionUID = 6155143250199846188L;
	@SerializedName("List")
	private ArrayList<Group> List;
	@SerializedName("DataInfo")
	private ArrayList<DataInfo> DataInfo;
	
	public ArrayList<DataInfo> getDataInfo() {
		return DataInfo;
	}
	public void setDataInfo(ArrayList<DataInfo> DataInfo) {
		this.DataInfo = DataInfo;
	}
	
	
	public ArrayList<Group> getList() {
		return List;
	}
	public void setList(ArrayList<Group> list) {
		this.List = list;
	}

	
	

}
