package com.guardianapp.helpercomponents;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.telephony.PhoneStateListener;
import android.telephony.TelephonyManager;
import android.widget.Toast;

import com.guardianapp.ui.SOSActivity;
import com.guardianapp.utilities.AppConstant;

/**
 * v-dhmadd
 * Helper class to detect incoming and outgoing calls. *
 */
public class CallHelper {

	/**
	 * Listener to detect incoming calls. 
	 */
	private class CallStateListener extends PhoneStateListener {
		private boolean isPhoneCalling = false;
		SharedPreferences AppPrefs = ctx.getSharedPreferences(
   			 AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		@Override
		public void onCallStateChanged(int state, String incomingNumber) {
			switch (state) {
			case TelephonyManager.CALL_STATE_RINGING:
				// called when someone is ringing to this phone
				break;
			
			
			case TelephonyManager.CALL_STATE_OFFHOOK:
				isPhoneCalling = true;
				break;	
				
			case TelephonyManager.CALL_STATE_IDLE:
				if (isPhoneCalling && (AppPrefs.getBoolean(AppConstant.SOS_on, false))) {
					Intent intent = new Intent();
					intent.setClass(ctx, SOSActivity.class);
					intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
					ctx.startActivity(intent);
				}
				break;	
		}
		}
	}
	
	/**
	 * Broadcast receiver to detect the outgoing calls.
	 */
	public class OutgoingReceiver extends BroadcastReceiver {
	    public OutgoingReceiver() {
	    }

	    @Override
	    public void onReceive(Context context, Intent intent) {
	        String number = intent.getStringExtra(Intent.EXTRA_PHONE_NUMBER);
	    }
  
	}

	private Context ctx;
	private TelephonyManager tm;
	private CallStateListener callStateListener;
	
	private OutgoingReceiver outgoingReceiver;

	public CallHelper(Context ctx) {
		this.ctx = ctx;
		
		callStateListener = new CallStateListener();
		outgoingReceiver = new OutgoingReceiver();
	}
	
	/**
	 * Start calls detection.
	 */
	public void start() {
		tm = (TelephonyManager) ctx.getSystemService(Context.TELEPHONY_SERVICE);
		tm.listen(callStateListener, PhoneStateListener.LISTEN_CALL_STATE);
		
		IntentFilter intentFilter = new IntentFilter(Intent.ACTION_NEW_OUTGOING_CALL);
		ctx.registerReceiver(outgoingReceiver, intentFilter);
	}
	
	/**
	 * Stop calls detection.
	 */
	public void stop() {
		tm.listen(callStateListener, PhoneStateListener.LISTEN_NONE);
		ctx.unregisterReceiver(outgoingReceiver);
	}

}
