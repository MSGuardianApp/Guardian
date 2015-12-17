package com.guardianapp.helpercomponents;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

import com.guardianapp.utilities.AppConstant;

public class AlarmManagerBroadcastReceiver extends BroadcastReceiver {

	@Override
	public void onReceive(Context context, Intent intent) {
		// TODO Auto-generated method stub
		AppConstant.sendDistressText(context);

	}

}
