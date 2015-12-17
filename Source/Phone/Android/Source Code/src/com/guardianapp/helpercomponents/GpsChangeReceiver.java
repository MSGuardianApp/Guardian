package com.guardianapp.helpercomponents;

import com.guardianapp.utilities.AppConstant;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.widget.Toast;

public class GpsChangeReceiver extends BroadcastReceiver {
	
	public static final String ACTION_GPS_CHANGE_RECEIVER = "com.guardianapp.GpsChangeReceiver.locationConsentChanged";

	@Override
	public void onReceive(Context context, Intent intent) {
		// TODO Auto-generated method stub
		getObservable().gpsProviderChanged(AppConstant.checkGpsOnOrOffStatus());
	}

	public static GpsConnectivityObervable getObservable() {
		return GpsConnectivityObervable.getInstance();
	}


}
