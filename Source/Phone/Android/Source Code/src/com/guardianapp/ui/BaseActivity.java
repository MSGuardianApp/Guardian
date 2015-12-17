/**
 * 
 */
package com.guardianapp.ui;

import android.app.ProgressDialog;
import android.content.Context;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.v7.app.ActionBarActivity;
import android.view.MenuItem;
import android.view.WindowManager;

import com.guardianapp.Guardian;
import com.guardianapp.interfaces.AsyncActivity;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.ExceptionHandler;

/**
 * @author v-dhmadd
 *
 */
public abstract class BaseActivity extends ActionBarActivity implements AsyncActivity{

	private ProgressDialog progressDialog;
	private boolean destroyed = false;
	protected abstract void initFields();
	protected abstract void initCallBacks();

	public static boolean isAppWentToBg = false;
	public static boolean isWindowFocused = false;
	public static boolean isMenuOpened = false;
	public static boolean isBackPressed = false;
	
	private SharedPreferences AppPrefs;

	@Override
	protected void onCreate(Bundle savedInstance) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstance);
		AppPrefs = getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		Thread.setDefaultUncaughtExceptionHandler(new ExceptionHandler(this));
		if(AppPrefs.getBoolean(AppConstant.SOS_on, false)|| AppPrefs.getBoolean(AppConstant.trackMe_on, false)){
		getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
		}else{
			getWindow().clearFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
		}
	}

	@Override
	protected void onStart() {
		// TODO Auto-generated method stub
		applicationWillEnterForeground();
		super.onStart();
	}

	@Override
	protected void onStop() {
		// TODO Auto-generated method stub
		super.onStop();
		applicationdidenterbackground();
	}

	private void applicationdidenterbackground() {
		if (!isWindowFocused) {
			isAppWentToBg = true;
			AppConstant.startRepeatingAlarmForNotifications(this);
		}
	}

	private void applicationWillEnterForeground() {
		if (isAppWentToBg){
			isAppWentToBg = false;
			AppConstant.cancelRepeatingNotificationsAlarm(this);
		}
	}

	@Override
	public void onBackPressed() {
		// TODO Auto-generated method stub
		if (this instanceof HomeScreenActivity) {

		} else {
			isBackPressed = true;
		}
		super.onBackPressed();
	}

	@Override
	public void onWindowFocusChanged(boolean hasFocus) {
		// TODO Auto-generated method stub
		isWindowFocused = hasFocus;

		if (isBackPressed && !hasFocus) {
			isBackPressed = false;
			isWindowFocused = true;
		}
		super.onWindowFocusChanged(hasFocus);
	}


	@Override
	protected void onDestroy() {
		// TODO Auto-generated method stub
		super.onDestroy();
		this.destroyed = true;
		if(this.progressDialog != null){
			this.progressDialog.dismiss();
			this.progressDialog = null;
		}
	}
	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		// TODO Auto-generated method stub
		switch (item.getItemId()) {
		case android.R.id.home:
			onBackPressed();
			break;
		}
		return true;
	}


	@Override
	public void setContentView(int layoutResID) {
		// TODO Auto-generated method stub
		super.setContentView(layoutResID);
		initFields();
		initCallBacks();
	}

	@Override
	public void showLoadingProgressDialog() {
		// TODO Auto-generated method stub
		this.showProgressDialog("Loading. Please wait...");

	}

	@Override
	public void showProgressDialog(CharSequence message) {
		// TODO Auto-generated method stub

		if (this.progressDialog == null) {
			this.progressDialog = new ProgressDialog(this);
			this.progressDialog.setIndeterminate(true);
			this.progressDialog.setCancelable(false);
			this.progressDialog.setCanceledOnTouchOutside(false);
		}

		this.progressDialog.setMessage(message);
		this.progressDialog.show();
	}

	@Override
	public void dismissProgressDialog() {
		// TODO Auto-generated method stub
		if (this.progressDialog != null && !this.destroyed) {
			this.progressDialog.dismiss();
		}

	}

	@Override
	public Guardian getApplicationContext(){
		return (Guardian)super.getApplicationContext();

	}


}
