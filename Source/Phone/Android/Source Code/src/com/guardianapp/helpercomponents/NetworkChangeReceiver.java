package com.guardianapp.helpercomponents;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

import com.guardianapp.utilities.AppConstant;

/**
 * @author v-dhmadd
 * Reciever to detect the Network change status
 */
public class NetworkChangeReceiver extends BroadcastReceiver {

	@Override
	public void onReceive(Context context, Intent intent) {
		// TODO Auto-generated method stub
        getObservable().connectionChanged(AppConstant.check_networkConnectivity(context));

	}
	public static NetworkObservable getObservable() {
		return NetworkObservable.getInstance();
	}

}
