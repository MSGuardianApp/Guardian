package com.guardianapp.model;

import java.io.Serializable;

import com.google.gson.annotations.SerializedName;

public class GroupDO implements Serializable {
	
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	@SerializedName("strId")
	private String strId;
	@SerializedName("strName")
	private String strName;
	@SerializedName("isChecked")
	private boolean isChecked;
	
	public String getStrId() {
		return strId;
	}

	public void setStrId(String strId) {
		this.strId = strId;
	}

	public String getStrName() {
		return strName;
	}

	public void setStrName(String strName) {
		this.strName = strName;
	}

	public boolean isChecked() {
		return isChecked;
	}

	public void setChecked(boolean isChecked) {
		this.isChecked = isChecked;
	}

	
}
