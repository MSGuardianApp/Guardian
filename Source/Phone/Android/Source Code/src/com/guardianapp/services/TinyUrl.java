package com.guardianapp.services;

import android.content.Context;

import com.guardianapp.backgroundtasks.GetTinyUrlTask;


public class TinyUrl {
	
	public static String tinyUrl;
	
	public static String getTinyUrl(Context context,String url) {
		GetTinyUrlTask gtask = new GetTinyUrlTask(context);
		gtask.execute(url);
		return tinyUrl;
	}
	
	
}
