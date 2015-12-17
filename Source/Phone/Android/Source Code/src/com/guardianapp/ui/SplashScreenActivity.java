package com.guardianapp.ui;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.Handler;

import com.guardianapp.R;
import com.guardianapp.database.DatabaseHandler;
import com.guardianapp.utilities.AppConstant;



public class SplashScreenActivity extends BaseActivity {
	private static final int SPLASH_DISPLAY_TIME = 4000; // splash screen delay time
	private SharedPreferences prefs;
	private DatabaseHandler guardianDBHelper;
	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.splash_screen);
		this.navigateToHomeScreen();
	}
	@Override
	protected void initFields() {
		// TODO Auto-generated method stub

	}
	@Override
	protected void initCallBacks() {
		// TODO Auto-generated method stub

	}
	
	private void navigateToHomeScreen(){
		prefs = getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		if(!prefs.getBoolean(AppConstant.firstTimeLounch, false))
		{
			new Handler().postDelayed(new Runnable() {
				public void run() {
					SharedPreferences.Editor editor = prefs.edit();
					editor.putBoolean(AppConstant.firstTimeLounch, true);
					editor.putBoolean(AppConstant.show_rating_dialog, true);
					editor.commit();
					Intent intent = new Intent();
					intent.setClass(SplashScreenActivity.this, HomeScreenActivity.class);
					SplashScreenActivity.this.startActivity(intent);
				}
			}, SPLASH_DISPLAY_TIME);
		}
		else
		{
			Intent intent = new Intent(SplashScreenActivity.this, HomeScreenActivity.class);
			this.startActivity(intent);

		}
	}
}
