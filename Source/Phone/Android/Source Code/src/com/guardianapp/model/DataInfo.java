package com.guardianapp.model;

import java.io.Serializable;

import com.google.gson.annotations.SerializedName;

public class DataInfo implements Serializable{
	private static final long serialVersionUID = -2933576811863131725L;
	@SerializedName("Message")
	private String Message;
	@SerializedName("ResultType")
	private int ResultType;
	
	public String getMessage() {
		return Message;
	}
	public void setMessage(String message) {
		Message = message;
	}
	public int getResultType() {
		return ResultType;
	}
	public void setResultType(int resultType) {
		ResultType = resultType;
	}

}
