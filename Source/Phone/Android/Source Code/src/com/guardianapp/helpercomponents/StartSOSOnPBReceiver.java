package com.guardianapp.helpercomponents;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;

import com.guardianapp.utilities.AppConstant;

public class StartSOSOnPBReceiver extends BroadcastReceiver {
	public static final String PROCESS_RESPONSE = "com.guardianapp.helpercomponents.SOSOnPB";
	private SharedPreferences AppPrefs;
	@Override
	public void onReceive(Context context, Intent intent) {
		AppPrefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		SharedPreferences.Editor editorTrack = AppPrefs.edit();
		editorTrack.putBoolean(AppConstant.SOS_on, true);
		editorTrack.putBoolean(AppConstant.trackMe_on, true);
		editorTrack.commit();
		// TODO Auto-generated method stub
		AppConstant.InitiateTrackingFromPB(context,true);
		
		AppConstant.startRepeatingAlarm(context);
	}

}
