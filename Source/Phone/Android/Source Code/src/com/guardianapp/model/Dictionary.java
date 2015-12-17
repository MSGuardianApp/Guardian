package com.guardianapp.model;

import java.io.Serializable;

import com.google.gson.annotations.SerializedName;

//phani
public class Dictionary implements Serializable{
	private static final long serialVersionUID = 324097656499032765L;
	
	@SerializedName("Key")
	private String Key;
	@SerializedName("Value")
	private String Value;
	
	public String getKey() {
		return Key;
	}
	public void setKey(String key) {
		Key = key;
	}
	public String getValue() {
		return Value;
	}
	public void setValue(String value) {
		Value = value;
	}
	
}
