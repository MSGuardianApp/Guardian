package com.guardianapp.helpercomponents;


import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.widget.Toast;

import com.guardianapp.services.SMSService;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.LogUtils;

/**
 * @author v-dhmadd
 * Receiver which gets fired on poweron and poweroff button actions and identifies the double click on power button
 *
 */
public class MySMSReceiver extends BroadcastReceiver {
	private boolean startSOS;
	private static long prevTime=0;
	private static long currTime=0;
	public SharedPreferences prefs;
	
	@Override
	public void onReceive(Context context, Intent intent) {
		
		prefs=context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

		if (intent.getAction().equals(Intent.ACTION_SCREEN_OFF)) {
			prevTime = System.currentTimeMillis();

		} else if (intent.getAction().equals(Intent.ACTION_SCREEN_ON)) {
			currTime = System.currentTimeMillis();
		}
		
		if ((currTime - prevTime) < 1000 && (currTime - prevTime)>-1000 ) {
	        if ((currTime - prevTime) < 1000 ) {
	            startSOS = true;
	            currTime = 0;
	            prevTime = 0;
	        }
	    }else{
	    	startSOS = false;
	    }

		// Send Current screen ON/OFF value to service
		if(prefs.getBoolean(AppConstant.isStartSOSOnPB, false)){
		Intent i = new Intent(context, SMSService.class);
		i.putExtra("screen_state", startSOS);
		context.startService(i);
		}
	}

}
