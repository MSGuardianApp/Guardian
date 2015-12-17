package com.guardianapp.model;

import java.io.Serializable;
import java.util.ArrayList;

import com.google.gson.annotations.SerializedName;

//phani
public class ProfileListLite implements Serializable{
	/**
	 * 
	 */
	private static final long serialVersionUID = 9190967207505603086L;
	
	@SerializedName("List")
	private ArrayList<ProfileLite> List;
	@SerializedName("ResultInfo")
	private ArrayList<DataInfo> ResultInfo;
	
	public ArrayList<DataInfo> getResultInfo() {
		return ResultInfo;
	}
	public void setDataInfo(ArrayList<DataInfo> ResultInfo) {
		this.ResultInfo = ResultInfo;
	}
	
	
	public ArrayList<ProfileLite> getList() {
		return List;
	}
	public void setList(ArrayList<ProfileLite> list) {
		this.List = list;
	}

}
