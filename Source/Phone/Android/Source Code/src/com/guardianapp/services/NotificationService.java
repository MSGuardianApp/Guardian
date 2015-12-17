package com.guardianapp.services;

import android.app.IntentService;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.IBinder;
import android.support.v4.app.NotificationCompat;
import android.support.v4.app.TaskStackBuilder;
import android.widget.Toast;

import com.guardianapp.R;
import com.guardianapp.ui.HomeScreenActivity;
import com.guardianapp.utilities.AppConstant;

public class NotificationService extends IntentService {

	private static int NOTIFICATION_ID = 11;
	private SharedPreferences appPrefs;
	private String title = null;
	private String notMsg = null;
	private TaskStackBuilder stackBuilder;
	private NotificationManager manager;

	public NotificationService() {
		super("NotificationService");
		// TODO Auto-generated constructor stub
	}

	private void sendNotification() {
		
		NotificationCompat.Builder notification = new NotificationCompat.Builder(
				this);
		notification.setContentTitle(this.title);
		notification.setContentText(this.notMsg);
		notification.setTicker(this.title);
		notification.setSmallIcon(R.drawable.sos_on);
		notification.setAutoCancel(true);
		Intent resultIntent = new Intent(this,
				HomeScreenActivity.class);
		resultIntent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_NEW_TASK);
		PendingIntent contentIntent = PendingIntent.getActivity(this, 0,
				resultIntent, PendingIntent.FLAG_UPDATE_CURRENT);
		notification.setContentIntent(contentIntent);
		stackBuilder = TaskStackBuilder.create(this);
		stackBuilder.addParentStack(HomeScreenActivity.class);

		stackBuilder.addNextIntent(resultIntent);

		manager = (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);
		manager.notify(NOTIFICATION_ID++, notification.build());
	}

	@Override
	protected void onHandleIntent(Intent intent) {
		// TODO Auto-generated method stub
		appPrefs = getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		if(appPrefs.getBoolean(AppConstant.SOS_on, false)){
			this.title = this.getString(R.string.sos_noti_title);
			this.notMsg = this.getString(R.string.sos_noti_body);
			this.sendNotification();
		}else if(appPrefs.getBoolean(AppConstant.trackMe_on, false)){
			this.title = this.getString(R.string.track_noti_title);
			this.notMsg = this.getString(R.string.track_noti_body);
			this.sendNotification();
		}

	}

}
