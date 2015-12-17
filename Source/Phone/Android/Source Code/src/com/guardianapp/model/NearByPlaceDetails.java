package com.guardianapp.model;

import java.io.Serializable;

import com.google.gson.annotations.SerializedName;

//phani
public class NearByPlaceDetails implements Serializable{
	
	/**
	 * 
	 */
	private static final long serialVersionUID = 8554630904147834241L;
	/*final String VISIBLE = "Visible";
    final String COLLAPSED = "Collapsed";*/
	@SerializedName("Name")
	private String Name;  
	@SerializedName("Vicinity")
	private String Vicinity;  
	@SerializedName("Latitude")
	private String Latitude;
	@SerializedName("Longitude")
	private String Longitude;  
	@SerializedName("Category")
	private String Category;  
	@SerializedName("PhoneNumber")
	private String PhoneNumber; 
    
    public String getName() {
		return Name;
	}
	public void setName(String name) {
		Name = name;
	}
	
	public String getVicinity(){
		return Vicinity;
	}
	public void setVicinity(String vicinity){
		Vicinity=vicinity;
	}
	public String getLatitude(){
		return Latitude;
	}
	public void setLatitude(String latitude){
		Latitude=latitude;
	}
	public String getLongitude(){
		return Longitude;
	}
	public void setLongitude(String longitude){
		Longitude=longitude;
	}
	public String getCategory(){
		return Category;
	}
	public void setCategory(String category){
		Category=category;
	}
	public String getPhoneNumber(){
		return PhoneNumber;
	}
	public void setPhoneNumber(String phoneNumber){
		PhoneNumber=phoneNumber;
	}
}
