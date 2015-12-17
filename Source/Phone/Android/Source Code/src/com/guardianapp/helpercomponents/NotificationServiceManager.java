package com.guardianapp.helpercomponents;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.support.v4.app.NotificationCompat;
import android.support.v4.app.TaskStackBuilder;

import com.guardianapp.R;
import com.guardianapp.ui.HomeScreenActivity;
import com.guardianapp.utilities.AppConstant;

public class NotificationServiceManager extends BroadcastReceiver {
	private static int NOTIFICATION_ID = 111;
	private SharedPreferences appPrefs;
	private String title = null;
	private String notMsg = null;
	private TaskStackBuilder stackBuilder;
	private NotificationManager manager;
	
	@Override
	public void onReceive(Context context, Intent intent) {
		// TODO Auto-generated method stub
		appPrefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		this.clearExistingNotifications(context);
		if(appPrefs.getBoolean(AppConstant.SOS_on, false)){
			this.title = context.getString(R.string.sos_noti_title);
			this.notMsg = context.getString(R.string.sos_noti_body);
			this.sendNotification(context);
		}else if(appPrefs.getBoolean(AppConstant.trackMe_on, false)){
			this.title = context.getString(R.string.track_noti_title);
			this.notMsg = context.getString(R.string.track_noti_body);
			this.sendNotification(context);
		}
		

	}
	
private void sendNotification(Context context) {
		
		NotificationCompat.Builder notification = new NotificationCompat.Builder(
				context);
		notification.setContentTitle(this.title);
		notification.setContentText(this.notMsg);
		notification.setTicker(this.title);
		notification.setSmallIcon(R.drawable.application_icon);
		notification.setAutoCancel(true);
		notification.setDefaults(Notification.DEFAULT_SOUND);
		Intent resultIntent = new Intent(context,
				HomeScreenActivity.class);
		resultIntent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_NEW_TASK);
		PendingIntent contentIntent = PendingIntent.getActivity(context, 0,
				resultIntent, PendingIntent.FLAG_UPDATE_CURRENT);
		notification.setContentIntent(contentIntent);
		stackBuilder = TaskStackBuilder.create(context);
		stackBuilder.addParentStack(HomeScreenActivity.class);

		stackBuilder.addNextIntent(resultIntent);

		manager = (NotificationManager) context.getSystemService(Context.NOTIFICATION_SERVICE);
		manager.notify(NOTIFICATION_ID, notification.build());
	}

private void clearExistingNotifications(Context context){
	String ns = Context.NOTIFICATION_SERVICE;
    NotificationManager nMgr = (NotificationManager) context.getApplicationContext().getSystemService(ns);
    nMgr.cancel(NOTIFICATION_ID);
}


}
