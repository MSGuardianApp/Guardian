package com.guardianapp.ui;

import java.util.Collections;
import java.util.Timer;
import java.util.TimerTask;

import android.app.AlertDialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.net.Uri;
import android.os.Bundle;
import android.os.Handler;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.TextView;

import com.guardianapp.R;
import com.guardianapp.services.CallDetectService;
import com.guardianapp.utilities.AppConstant;

public class SOSConnectActivity extends BaseActivity {

	private TimerTask timerTask;
	private SharedPreferences sosprefs;
	private int count_down = AppConstant.SOS_CONNECTION_TIMER;

	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		AppConstant.isSoSActivity = false;
		setContentView(R.layout.sos_connect_screen);
		startTimer();
	}

	@Override
	protected void onResume() {
		// TODO Auto-generated method stub
		super.onResume();
	}

	@Override
	protected void initFields() {
		// TODO Auto-generated method stub
		sosprefs = getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
	}

	@Override
	protected void initCallBacks() {
		// TODO Auto-generated method stub
		findViewById(R.id.rl_SOSconnect).setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View arg0) {
				stopTimer(); 
				SharedPreferences.Editor editor = sosprefs.edit();
				editor.putBoolean(AppConstant.SOS_on, true);
				editor.commit();
				AppConstant.sendSMSViaPhone(SOSConnectActivity.this);
				AppConstant.InitiateTracking(SOSConnectActivity.this,true);
				editor.putBoolean(AppConstant.trackMe_on, true);
				editor.commit();
				if(sosprefs.getBoolean(AppConstant.IS_CALLER_BUDDY_SELECTED, false)&& AppConstant.globalBuddies.size()>0){
					if(!AppConstant.isMyServiceRunning(getApplicationContext(),CallDetectService.class))
						startService(new Intent(SOSConnectActivity.this, CallDetectService.class));  
					AppConstant.callOrSendMessageToUser(SOSConnectActivity.this, AppConstant.callerBuddies.getMobileNumber(), AppConstant.CALL_USER);
				}else
					SOSConnectActivity.this.gotoSOSConnectActivity();

			}
		});
		findViewById(R.id.btnCancelSOS).setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				stopTimer();
				finish();

			}
		});
	}

	@Override
	protected void onPause() {
		super.onPause();
		stopTimer();
	}
	
	


	@Override
	protected void onDestroy() {
		// TODO Auto-generated method stub
		super.onDestroy();
	}

	public void startTimer() {

		final Handler handler = new Handler();
		Timer ourtimer = new Timer();

		timerTask = new TimerTask() {
			public void run() {
				handler.post(new Runnable() {
					public void run() {
						TextView timer = (TextView)findViewById(R.id.tv_counting);
						timer.setText(count_down + "");
						if(count_down==0)
						{
							stopTimer(); 
							SharedPreferences.Editor editor = sosprefs.edit();
							editor.putBoolean(AppConstant.SOS_on, true);
							editor.commit();
							AppConstant.sendSMSViaPhone(SOSConnectActivity.this);
							AppConstant.InitiateTracking(SOSConnectActivity.this,true);
							editor.putBoolean(AppConstant.trackMe_on, true);
							editor.commit();
							if(sosprefs.getBoolean(AppConstant.IS_CALLER_BUDDY_SELECTED, false)&& AppConstant.globalBuddies.size()>0){
								if(!AppConstant.isMyServiceRunning(getApplicationContext(),CallDetectService.class))
									startService(new Intent(SOSConnectActivity.this, CallDetectService.class));  
								AppConstant.callOrSendMessageToUser(SOSConnectActivity.this, AppConstant.callerBuddies.getMobileNumber(), AppConstant.CALL_USER);
							}else
								SOSConnectActivity.this.gotoSOSConnectActivity();						}
						else
							count_down--;

					}
				});
			}};


			ourtimer.schedule(timerTask, 0, 1000);

	}

	public void stopTimer() {
		if(timerTask!=null) 
		{
			timerTask.cancel();
			timerTask=null;

		}

	}

	private void gotoSOSConnectActivity(){
		Intent intent = new Intent();
		intent.setClass(SOSConnectActivity.this, SOSActivity.class);
		startActivity(intent);
		finish();
	}

}
