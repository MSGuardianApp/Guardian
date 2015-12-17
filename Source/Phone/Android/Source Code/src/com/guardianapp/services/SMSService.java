package com.guardianapp.services;

import java.util.ArrayList;

import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.os.IBinder;
import android.os.RemoteException;
import android.support.v4.app.NotificationCompat;
import android.support.v4.app.TaskStackBuilder;

import com.guardianapp.R;
import com.guardianapp.database.DBQueries;
import com.guardianapp.helpercomponents.MySMSReceiver;
import com.guardianapp.helpercomponents.StartSOSOnPBReceiver;
import com.guardianapp.helpercomponents.VersionCheckReceiver;
import com.guardianapp.model.MyBuddies;
import com.guardianapp.ui.HomeScreenActivity;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.LogUtils;



/**
 * @author v-dhmadd
 * This is a background service which registers a receiver for power on/off , enables sos on double click of powerbutton .
 */
public class SMSService extends Service {
	BroadcastReceiver mReceiver;
	NotificationManager manager;
	TaskStackBuilder stackBuilder;
	public static int NOTIFICATION_ID = 1;
	private boolean isSOSEnabled = false;
	
	
	private IBoundService.Stub mBinder = new IBoundService.Stub() {
		
		@Override
		public boolean getSOSStatus() throws RemoteException {
			// TODO Auto-generated method stub
			return SMSService.this.isSOSEnabled;
		}
	};
	
	@Override
	public void onCreate() {
		super.onCreate();
		IntentFilter filter = new IntentFilter(Intent.ACTION_SCREEN_ON);
		filter.addAction(Intent.ACTION_SCREEN_OFF);
		mReceiver = new MySMSReceiver();
		registerReceiver(mReceiver, filter);
	}

	@Override
	public void onStart(Intent intent, int startId) {
		boolean startSOS = false;
    	
    	try{
    		// Get ON/OFF values sent from receiver ( AEScreenOnOffReceiver.java ) 
    		startSOS = intent.getBooleanExtra("screen_state", false);
            
    	}catch(Exception e){
    		LogUtils.LOGE(LogUtils.makeLogTag(SMSService.class), e.getLocalizedMessage());
    	}
    	
        if (startSOS) {
            
        	this.enableSOSFromPowerButton();
        
        } else {
            
        }
		
	}
	
	@Override
	public void onRebind(Intent intent) {
		super.onRebind(intent);
	}

	@Override
	public boolean onUnbind(Intent intent) {
		this.isSOSEnabled = false;
		return true;
	}

	private void enableSOSFromPowerButton(){
		SharedPreferences AppPrefs = getApplicationContext().getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		if(!AppPrefs.getBoolean(AppConstant.SOS_on, false))
		{
			Intent broadcastIntent = new Intent();
			broadcastIntent.setAction(StartSOSOnPBReceiver.PROCESS_RESPONSE);
			broadcastIntent.addCategory(Intent.CATEGORY_DEFAULT);
			sendBroadcast(broadcastIntent);
			this.startNotification();
			this.isSOSEnabled = true;
		}
	}

	@Override
	public IBinder onBind(Intent arg0) {
		// TODO Auto-generated method stub
		return mBinder;
	}

	@Override
	public void onDestroy() {
		if(mReceiver != null)
		unregisterReceiver(mReceiver);
	}

	protected void startNotification() {
		NotificationCompat.Builder notification = new NotificationCompat.Builder(
				this);
		notification.setContentTitle("Starting SOS!!!");
		notification.setContentText("This will send SMS to all your buddy mobiles notifying about your emergency...");
		notification.setTicker("SOS Alert!");
		notification.setSmallIcon(R.drawable.sos_on);
		notification.setAutoCancel(true);
		PendingIntent contentIntent = PendingIntent.getActivity(this, 0,
				new Intent(), PendingIntent.FLAG_UPDATE_CURRENT);
		notification.setContentIntent(contentIntent);
		manager = (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);
		manager.notify(NOTIFICATION_ID++, notification.build());

	}


}


