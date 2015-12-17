package com.guardianapp.ui;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import java.util.Observable;
import java.util.Observer;
import java.util.Timer;
import java.util.TimerTask;

import org.json.JSONArray;
import org.json.JSONException;

import android.app.AlertDialog;
import android.content.ActivityNotFoundException;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.ServiceConnection;
import android.content.SharedPreferences;
import android.location.Address;
import android.location.Geocoder;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Handler;
import android.os.IBinder;
import android.os.RemoteException;
import android.provider.Settings;
import android.util.DisplayMetrics;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.guardianapp.R;
import com.guardianapp.backgroundtasks.RunInBackground;
import com.guardianapp.database.DBQueries;
import com.guardianapp.helpercomponents.GpsChangeReceiver;
import com.guardianapp.helpercomponents.GpsConnectivityObervable;
import com.guardianapp.helpercomponents.NetworkChangeReceiver;
import com.guardianapp.helpercomponents.NetworkObservable;
import com.guardianapp.helpercomponents.SOSStatusChangeObservable;
import com.guardianapp.helpercomponents.StartSOSOnPBReceiver;
import com.guardianapp.model.Dictionary;
import com.guardianapp.services.GPSTracker;
import com.guardianapp.services.IBoundService;
import com.guardianapp.services.SMSService;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.ConnectivityUtils;
import com.guardianapp.utilities.LogUtils;
import com.guardianapp.webservicecomponents.MakeHTTPServices;
import com.guardianapp.webservicecomponents.OnTaskCompleted;

public class HomeScreenActivity extends BaseActivity implements OnClickListener, OnTaskCompleted, Observer {

	private RelativeLayout mReportLaout,mSettingLayout,mWifiSettingLayout,mSIMLayout,mGPSLayout; 
	private LinearLayout mSOSLayout, mLocateLayout;
	private TextView currentLocationTxt,LocateSosBuddiesText,LocateTrackMeBuddiesText, sosStatusText;
	private TextView tv_trackMeStatus;
	Handler handler1 , handler2;
	Timer t1 , t2;
	static boolean isGetCountDoInBackground;
	public SharedPreferences AppPrefs;
	private DBQueries queries;
	private IBoundService mBoundServiceInterface;
	private static boolean mServiceConnected = false;
	private StartSOSOnPBReceiver sosOnPBReceiver;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		AppPrefs = getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

		if(AppPrefs.getBoolean(AppConstant.isProfileDone, false))
			AppConstant.isRagister = true;

		queries = new DBQueries(this);

		initGPSTracker();

		if(queries.loadUserData().equalsIgnoreCase("Success"))
			queries.Load2CurrentProfile();

		if(AppConstant.globalBuddies!=null&& AppConstant.globalBuddies.size()==0)
			AppConstant.globalBuddies = queries.SelectAllBuddies();
		if(AppConstant.globalBuddies.size()>0)
			AppConstant.callerBuddies = AppConstant.getPrimaryBuddy();

		if(AppConstant.ascGroups!=null&& AppConstant.ascGroups.size()==0)
			AppConstant.ascGroups = queries.SelectAllAsgGroups();

		currentLocationTxt.setText("Getting location details....");

		getCurrentAddress();
		wifiSIMNetworkStatus();
		gpsOnOffStatus();
		initiateBackgroundEvents();
		initiateTimerForPostingLocationsArray();
		AppConstant.setCountryCode(this , AppConstant.userProfile.getCountryCode());
		(new NavigateToSettingsTask()).execute();
	}

	@Override
	protected void onStart() {
		super.onStart();
		if(AppPrefs.getBoolean(AppConstant.isStartSOSOnPB, false)){
			Intent intent = new Intent(this , SMSService.class); 
			startService(intent);		
			bindService(intent, mServiceConnection, Context.BIND_AUTO_CREATE);
		}
	}

	@Override
	protected void onStop() {
		super.onStop();
		if (mServiceConnected) {
			unbindService(mServiceConnection);
			mServiceConnected = false;
		}
	}


	@Override
	protected void initFields() {
		// TODO Auto-generated method stub
		mSOSLayout = (LinearLayout) findViewById(R.id.SOSRelOut);
		tv_trackMeStatus = (TextView) findViewById(R.id.textTrackmeStatus);
		mLocateLayout = (LinearLayout) findViewById(R.id.LocateRelOut);
		mReportLaout = (RelativeLayout) findViewById(R.id.ReportRelOut);
		currentLocationTxt = (TextView) findViewById(R.id.textCurrentLocation);
		mSettingLayout = (RelativeLayout) findViewById(R.id.SettingsLaout);
		mWifiSettingLayout = (RelativeLayout) findViewById(R.id.wifiSettingsLaout);
		mGPSLayout = (RelativeLayout) findViewById(R.id.GPSSettingsLaout);
		mSIMLayout = (RelativeLayout) findViewById(R.id.SIMSettingsLaout);
		LocateSosBuddiesText = (TextView)findViewById(R.id.textSOSLocate);
		LocateTrackMeBuddiesText = (TextView)findViewById(R.id.textTracking);
		sosStatusText = (TextView)findViewById(R.id.textSOSMenuDesText);
		this.registerSMSOnPBReceiver();

		//	Toast.makeText(this, getResources().getDisplayMetrics().density+"", Toast.LENGTH_LONG).show();
	}

	@Override
	protected void initCallBacks() {
		// TODO Auto-generated method stub
		mSOSLayout.setOnClickListener(this);
		mLocateLayout.setOnClickListener(this);
		mReportLaout.setOnClickListener(this);
		mSettingLayout.setOnClickListener(this);
		mWifiSettingLayout.setOnClickListener(this);
		mGPSLayout.setOnClickListener(this);
		mSIMLayout.setOnClickListener(this);
		findViewById(R.id.imageSoSBtn).setOnClickListener(this);
		findViewById(R.id.imageTrack).setOnClickListener(this);
		findViewById(R.id.TrackMeRelOut).setOnClickListener(this);
	}

	@Override
	protected void onResume() {
		super.onResume();
		NetworkChangeReceiver.getObservable().addObserver(this);
		GpsChangeReceiver.getObservable().addObserver(this);
		SOSStatusChangeObservable.getInstance().addObserver(this);
		sosOnOffStatus();
		gpsOnOffStatus();
		trackMeOnOffStatus();
		getCurrentAddress();

	}

	@Override
	protected void onPause() {
		// TODO Auto-generated method stub
		super.onPause();
	}

	@Override
	protected void onDestroy() {
		// TODO Auto-generated method stub
		super.onDestroy();
		NetworkChangeReceiver.getObservable().deleteObserver(this);
		GpsChangeReceiver.getObservable().deleteObserver(this);
		SOSStatusChangeObservable.getInstance().deleteObserver(this);
		unregisterReceiver(sosOnPBReceiver);
	}


	private boolean checkGPS() {
		if(AppConstant.gpsTracker!=null && !AppConstant.gpsTracker.checkGPSStatusOn()
				||!AppConstant.userProfile.isLocationConsent())
		{
			showConfirmationDialog(this.getString(R.string.info_txt) , this.getString(R.string.enable_loc_settings), AppConstant.GPS_DIALOG);
			return false;
		}
		else
			return true;
	}
	private void showConfirmationDialog(String dialogTitle, String message, final int source) {
		View view =  getLayoutInflater().inflate(R.layout.common_dialog, null);
		final AlertDialog gpsDialog = new AlertDialog.Builder(this)
		.setView(view).create();

		TextView title = (TextView) view.findViewById(R.id.tv_title);	
		title.setText(dialogTitle);

		TextView msgTxt = (TextView) view.findViewById(R.id.tv_msgAleart);	
		msgTxt.setText(message);
		Button callbtn = (Button) view.findViewById(R.id.btnOK);
		callbtn.setText(this.getString(R.string.dialog_ok_button));

		Button cancelBtn = (Button) view.findViewById(R.id.btnCancel);
		cancelBtn.setText(this.getString(R.string.dialog_cancel_button));
		view.findViewById(R.id.btnOK).setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View arg0) {

				switch(source){
				case AppConstant.GPS_DIALOG : 
					if(AppConstant.gpsTracker!=null && !AppConstant.gpsTracker.checkGPSStatusOn()&&AppConstant.userProfile.isLocationConsent())
						startActivityForResult(new Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS),AppConstant.GPS_SETTINGS_ACTIVITY_RESULT);
					else
					{
						Intent gpsInt = new Intent(HomeScreenActivity.this,SettingsActivity.class);
						gpsInt.putExtra("GPS_SETTINGS", "gpsSettings");
						startActivity(gpsInt);
					}
					break;

				case AppConstant.RATING_DIALOG : 
					SharedPreferences.Editor editorRate = AppPrefs.edit();
					editorRate.putBoolean(AppConstant.show_rating_dialog, false);
					editorRate.commit();
					Uri uri = Uri.parse("market://details?id=" + HomeScreenActivity.this.getApplicationContext().getPackageName()+"&hl=en");
					Intent goToMarket = new Intent(Intent.ACTION_VIEW, uri);
					try {
						startActivity(goToMarket);
					} catch (ActivityNotFoundException e) {
						startActivity(new Intent(Intent.ACTION_VIEW, Uri.parse(AppConstant.PLAYSTORE_URL + HomeScreenActivity.this.getApplicationContext().getPackageName()+"&hl=en")));
					}
					break;

				case AppConstant.RATING_ON_SOS_DIALOG :
					SharedPreferences.Editor editorTrack = AppPrefs.edit();

					if(AppPrefs.getBoolean(AppConstant.SOS_on, false)){
						editorTrack.putBoolean(AppConstant.SOS_on, false);
						editorTrack.commit();
						AppConstant.StopSOS(HomeScreenActivity.this);
					}

					if(AppPrefs.getBoolean(AppConstant.trackMe_on, false)){
						AppConstant.StopTracking(HomeScreenActivity.this);
						editorTrack.putBoolean(AppConstant.trackMe_on, false);
						editorTrack.commit();

					}
					gpsDialog.dismiss();
					finish();
					break;
				}

				gpsDialog.dismiss();
			}
		});

		view.findViewById(R.id.btnCancel).setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {
				switch(source){
				case AppConstant.GPS_DIALOG : 
					gpsDialog.dismiss();
					break;
				case AppConstant.RATING_DIALOG : 
					gpsDialog.dismiss();
					finish();
					break;
				case AppConstant.RATING_ON_SOS_DIALOG:
					gpsDialog.dismiss();
					break;

				}


			}
		});

		gpsDialog.show();
	}
	private void getCurrentAddress() {
		currentLocationTxt.setText("Getting location details....");

		if(AppConstant.userProfile.isLocationConsent())
		{
			if(!AppConstant.isGPS_On)
			{
				AppConstant.gpsTracker.startGPS();
				if (AppConstant.gpsTracker!=null && !AppConstant.gpsTracker.canGetLocation())
				{
					currentLocationTxt.setText(this.getString(R.string.loc_settngs_dis_msg));

				} 

			}
			(new GetAddressTask(this)).execute(GPSTracker.latitude, GPSTracker.longitude);

		}
	}

	private void initGPSTracker() {
		if(AppConstant.gpsTracker == null)
			AppConstant.gpsTracker = new GPSTracker(getApplicationContext());
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.main_menu, menu);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {

		switch (item.getItemId()) {

		case R.id.helpMenu:
			Intent helpInt = new Intent(HomeScreenActivity.this, HelpActivity.class);
			startActivity(helpInt);
			break;
		case R.id.aboutMenu:
			Intent AbtInt = new Intent(HomeScreenActivity.this, AboutActivity.class);
			startActivity(AbtInt);
			break;

		case R.id.policyMenu:
			Intent browserIntent = new Intent(this,PrivacyPolicyActivity.class);
			startActivity(browserIntent);
			break;

		case R.id.feedbackMenu:
			Uri uri = Uri.parse("market://details?id=" + HomeScreenActivity.this.getApplicationContext().getPackageName()+"&hl=en");
			Intent goToMarket = new Intent(Intent.ACTION_VIEW, uri);
			try {
				startActivity(goToMarket);
			} catch (ActivityNotFoundException e) {
				startActivity(new Intent(Intent.ACTION_VIEW, Uri.parse(AppConstant.PLAYSTORE_URL + HomeScreenActivity.this.getApplicationContext().getPackageName()+"&hl=en")));
			}
			break;
		case R.id.msgToBuddiesMenu:
			if(AppConstant.userProfile.isIsSOSOn())
				AppConstant.StopSOS(this);
			break;

		default:
			break;
		}
		return super.onOptionsItemSelected(item);
	}

	@Override
	public void onClick(View v) {
		switch (v.getId()) {
		case R.id.SOSRelOut:
			if(AppPrefs.getBoolean(AppConstant.SOS_on, false))
			{
				Intent sosInt = new Intent(HomeScreenActivity.this, SOSActivity.class);
				startActivity(sosInt);
			}
			else
			{
				Intent sosInt = new Intent(HomeScreenActivity.this, SOSConnectActivity.class);
				sosInt.setFlags(Intent.FLAG_ACTIVITY_NO_HISTORY);
				startActivity(sosInt);

			}

			break;

		case R.id.TrackMeRelOut:
			if(checkGPS())
			{
				SharedPreferences.Editor editorTrack = AppPrefs.edit();
				if(!AppPrefs.getBoolean(AppConstant.trackMe_on, false))
				{
					AppConstant.InitiateTracking(this,true);
					editorTrack.putBoolean(AppConstant.trackMe_on, true);
					editorTrack.commit();
					trackMeOnOffStatus();

					if(GPSTracker.GeoTagList.size()>0){
						GPSTracker.GeoTagList.clear();
						GPSTracker.recentCapturedLocation = null;
						GPSTracker.recentLocCapturedTime = 0;	
						AppConstant.fromIndex_PostLocation = 0;
					}

				}

				Intent TrackMeInt = new Intent(HomeScreenActivity.this, TrackMeActivity.class);
				startActivity(TrackMeInt);			
			}

			break;

		case R.id.LocateRelOut:
			if(checkGPS())
			{
				Intent locateInt = new Intent(HomeScreenActivity.this, LocateActivity.class);
				startActivity(locateInt);
			}
			break;

		case R.id.ReportRelOut:
			if(checkGPS())
			{
				Intent reincInt = new Intent(HomeScreenActivity.this, ReportAnIncidentActivity.class);
				startActivity(reincInt);
			}
			break;


		case R.id.SettingsLaout:
			Intent settingsInt = new Intent(HomeScreenActivity.this, SettingsActivity.class);
			startActivity(settingsInt);
			break;


		case R.id.wifiSettingsLaout:
			startActivityForResult(new Intent(Settings.ACTION_WIFI_SETTINGS),AppConstant.WIFI_SETTINGS_ACTIVITY_RESULT);
			break;
		case R.id.SIMSettingsLaout:
			Intent simSetting = new Intent(Intent.ACTION_MAIN);
			simSetting.setClassName("com.android.phone", "com.android.phone.NetworkSetting");
			startActivityForResult(simSetting,AppConstant.SIM_NET_SETTINGS_ACTIVITY_RESULT);
			break;
		case R.id.GPSSettingsLaout:	
			if(AppConstant.gpsTracker!=null && !AppConstant.gpsTracker.checkGPSStatusOn()&&AppConstant.userProfile.isLocationConsent())
				startActivityForResult(new Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS),AppConstant.GPS_SETTINGS_ACTIVITY_RESULT);
			else
			{
				Intent gpsInt = new Intent(this,SettingsActivity.class);
				gpsInt.putExtra("GPS_SETTINGS", "gpsSettings");
				startActivity(gpsInt);
			}

			break;


		case R.id.imageSoSBtn:	

			SharedPreferences.Editor editor = AppPrefs.edit();

			if(AppPrefs.getBoolean(AppConstant.SOS_on, false))
			{
				editor.putBoolean(AppConstant.SOS_on, false);
				editor.commit();
				AppConstant.StopSOS(this);
				AppConstant.CustomDialog(this,getResources().getString(R.string.Str_SOSDialog_text) , "Confirmation", "ok", "cancel");
				sosOnOffStatus();
				Toast.makeText(this, this.getString(R.string.save_batt_msg), Toast.LENGTH_LONG).show();
			}
			else
			{
				Intent sosInt = new Intent(HomeScreenActivity.this, SOSConnectActivity.class);
				sosInt.setFlags(Intent.FLAG_ACTIVITY_NO_HISTORY);
				startActivity(sosInt);
			} 


			break;


		case R.id.imageTrack:	

			setTrackMeOn();

			break;
		default:
			break;
		}
	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		// TODO Auto-generated method stub
		super.onActivityResult(requestCode, resultCode, data);
	}

	private void wifiSIMNetworkStatus() {

		if(ConnectivityUtils.isConnectedWifi(this))
			mWifiSettingLayout.setBackgroundResource(R.drawable.bg_table_main_green);
		else
			mWifiSettingLayout.setBackgroundResource(R.drawable.bg_table_main_orange);

		if(ConnectivityUtils.isConnectedMobile(this))
			mSIMLayout.setBackgroundResource(R.drawable.bg_table_main_green);
		else
			mSIMLayout.setBackgroundResource(R.drawable.bg_table_main_orange);
	}

	private void gpsOnOffStatus() {
		//GPSTracker gps = new GPSTracker(this);
		if(AppConstant.gpsTracker!=null && AppConstant.gpsTracker.checkGPSStatusOn()&&AppConstant.userProfile.isLocationConsent())
			mGPSLayout.setBackgroundResource(R.drawable.bg_table_main_green);
		else
			mGPSLayout.setBackgroundResource(R.drawable.bg_table_main_orange);
	}


	private void wifiSIMNetworkStatus(int data) {

		if(data == AppConstant.WIFI_NETWORK)
			mWifiSettingLayout.setBackgroundResource(R.drawable.bg_table_main_green);
		else
			mWifiSettingLayout.setBackgroundResource(R.drawable.bg_table_main_orange);

		if(data == AppConstant.SIM_NETWORK)
			mSIMLayout.setBackgroundResource(R.drawable.bg_table_main_green);
		else
			mSIMLayout.setBackgroundResource(R.drawable.bg_table_main_orange);
		
		if(data == AppConstant.NO_NETWORK)
			Toast.makeText(this, this.getString(R.string.internet_unavailable), Toast.LENGTH_LONG).show();
	}

	private void gpsOnOffStatus(int data) {
		if(data == AppConstant.GPS_ENABLED){
			mGPSLayout.setBackgroundResource(R.drawable.bg_table_main_green);
		}else if(data == AppConstant.GPS_DISABLED){
			mGPSLayout.setBackgroundResource(R.drawable.bg_table_main_orange);
		}
		trackMeOnOffStatus();
	}

	private void sosOnOffStatus() {

		ImageView sosBtn =  (ImageView) findViewById(R.id.imageSoSBtn);;
		TextView status = (TextView) findViewById(R.id.textSOSOnOFF);
		if(AppPrefs.getBoolean(AppConstant.SOS_on, false))
		{
			mSOSLayout.setBackgroundResource(R.drawable.bg_table_main_green);
			sosBtn.setBackgroundResource(R.drawable.stopsos);
			AppConstant.TokenId = AppConstant.SOS_TOKEN;
			status.setText(this.getString(R.string.sos_on));
			sosStatusText.setText(this.getString(R.string.sos_display_text));
		}

		else
		{
			mSOSLayout.setBackgroundResource(R.drawable.bg_table_main_orange);
			sosBtn.setBackgroundResource(R.drawable.sos);
			status.setText(this.getString(R.string.sos_off));
			sosStatusText.setText(this.getString(R.string.tap_this_text));
			if(AppPrefs.getBoolean(AppConstant.trackMe_on, false))
				AppConstant.TokenId = AppConstant.TRACKING_TOKEN;
		}

	}

	private boolean setTrackMeOn() {
		SharedPreferences.Editor editorTrack = AppPrefs.edit();

		if(!checkGPS())
			return false;

		if(AppPrefs.getBoolean(AppConstant.SOS_on, false))
			Toast.makeText(this, "Please check your SOS is on", Toast.LENGTH_LONG).show();
		else
		{

			if(AppPrefs.getBoolean(AppConstant.trackMe_on, false))
			{
				AppConstant.StopTracking(this);
				editorTrack.putBoolean(AppConstant.trackMe_on, false);
				editorTrack.commit();

				trackMeOnOffStatus();
				return false;
			}
			else
			{
				if(checkGPS())
				{
					AppConstant.InitiateTracking(this,true);
					editorTrack.putBoolean(AppConstant.trackMe_on, true);
					editorTrack.commit();
					if(GPSTracker.GeoTagList.size()>0){
						GPSTracker.GeoTagList.clear();
						GPSTracker.recentCapturedLocation = null;
						GPSTracker.recentLocCapturedTime = 0;	
						AppConstant.fromIndex_PostLocation = 0;
					}
					Intent TrackMeInt = new Intent(HomeScreenActivity.this, TrackMeActivity.class);
					startActivity(TrackMeInt);
					return true;
				}

			}

		}

		return false;

	}
	private void trackMeOnOffStatus() {

		ImageView trackMe =  (ImageView) findViewById(R.id.imageTrack);
		ImageView track_Iv = (ImageView) findViewById(R.id.imageTrackMe);
		LinearLayout trackMeLayout = (LinearLayout) findViewById(R.id.TrackMeRelOut);
		if(AppPrefs.getBoolean(AppConstant.trackMe_on, false)&&AppConstant.gpsTracker!=null && AppConstant.gpsTracker.checkGPSStatusOn() && AppConstant.userProfile.isLocationConsent())
		{
			tv_trackMeStatus.setText("ON");
			track_Iv.setBackgroundResource(R.drawable.trackme_on);
			trackMe.setBackgroundResource(R.drawable.stoptrack);
			AppConstant.TokenId = AppConstant.TRACKING_TOKEN;
			trackMeLayout.setBackgroundResource(R.drawable.bg_table_main_green);

			getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
		}

		else
		{
			// mTrackLayout.setBackgroundResource(R.drawable.track_me_off);
			tv_trackMeStatus.setText("OFF");
			track_Iv.setBackgroundResource(R.drawable.trackme_off);
			trackMe.setBackgroundResource(R.drawable.track);
			trackMeLayout.setBackgroundResource(R.drawable.bg_table_main_orange);
			DBQueries updateRecord = new DBQueries(HomeScreenActivity.this);
			updateRecord.updateValue("SessionToken", "0");

			getWindow().clearFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
		}

	}

	private void getSOSTrackCount() 
	{
		if(AppPrefs.getBoolean(AppConstant.isProfileDone, false)){
			MakeHTTPServices services =new MakeHTTPServices(this, null);
			services.GetLocateLiveTileCountAsync();	
		}
	}

	private void postLocations(){
		try {
			RunInBackground rib = new RunInBackground(this);
			rib.run();
		} catch (Exception e) {
			// TODO Auto-generated catch block
			LogUtils.LOGE(LogUtils.makeLogTag(HomeScreenActivity.class), e.getLocalizedMessage());
		}
	}
	private void initiateBackgroundEvents()
	{
		handler1 = new Handler();
		t1 = new Timer();
		doReload();
		isGetCountDoInBackground = true;
	}
	public void doReload() {

		TimerTask scanTask1 = new TimerTask() {
			public void run() {
				handler1.post(new Runnable() {
					public void run() {
						LogUtils.LOGD(LogUtils.makeLogTag(HomeScreenActivity.class), "----- calling getSOSTrackCount -----");                     
						getSOSTrackCount();

					}
				});
			}
		};

		t1.schedule(scanTask1, 0, AppConstant.SOS_TRACK_COUNT_TIMER);

	}

	private void initiateTimerForPostingLocationsArray(){
		handler2 = new Handler();
		t2 = new Timer();
		this.postLocationsPeriodically();
	}

	private void postLocationsPeriodically(){

		TimerTask postTask = new TimerTask() {

			@Override
			public void run() {
				// TODO Auto-generated method stub
				handler2.post(new Runnable() {

					@Override
					public void run() {
						// TODO Auto-generated method stub
						LogUtils.LOGD(LogUtils.makeLogTag(HomeScreenActivity.class), "***** calling postLocations *****");  
						postLocations();


					}
				});

			}
		};
		t2.schedule(postTask, 0, AppConstant.POST_LOC_TIMER);
	}

	@Override
	public void onTaskComplete(String result) {
		// TODO Auto-generated method stub
		AppConstant.dismissProgressDialog();
		JSONArray jsonArray ;
		try {
			jsonArray	= new JSONArray(result);
			String sosCount = "00", trackCount = "00";
			sosCount=jsonArray.getJSONObject(0).get("Value").toString();
			trackCount=jsonArray.getJSONObject(1).get("Value").toString();
			LocateSosBuddiesText.setText(sosCount.length() == 1 ?( "SOS: "+"0" + sosCount) : sosCount);
			LocateTrackMeBuddiesText.setText(trackCount.length() == 1 ?("Tracking: " +  "0" + trackCount) : trackCount);
			this.broadcastLocatedBuddies();
		}catch(JSONException jse){
			LogUtils.LOGE(LogUtils.makeLogTag(HomeScreenActivity.class), jse.getLocalizedMessage());

		}

	}

	@Override
	public void onGetObjectResult(Object obj) {
		// TODO Auto-generated method stub
		AppConstant.dismissProgressDialog();
		ArrayList<Dictionary>  liveCounts=(ArrayList<Dictionary>)obj;
		if (liveCounts != null )
		{	
			if(LocateSosBuddiesText!=null && LocateTrackMeBuddiesText!=null)
			{
				String sosCount = "00", trackCount = "00";
				sosCount=liveCounts.get(0).getValue();
				trackCount=liveCounts.get(1).getValue();
				LocateSosBuddiesText.setText(sosCount.length() == 1 ?( "SOS: "+"0" + sosCount) : sosCount);
				LocateTrackMeBuddiesText.setText(trackCount.length() == 1 ?("Tracking: " +  "0" + trackCount) : trackCount);
				this.broadcastLocatedBuddies();
			}

		}
	}


	@Override
	public void onBackPressed() {
		// TODO Auto-generated method stub
		//super.onBackPressed();
		// TODO show dialog 
		if(AppPrefs.getBoolean(AppConstant.SOS_on, false)|| AppPrefs.getBoolean(AppConstant.trackMe_on, false))
			showConfirmationDialog(this.getString(R.string.close_on_sos_title), this.getString(R.string.close_on_sos),AppConstant.RATING_ON_SOS_DIALOG);
		else if(AppPrefs.getBoolean(AppConstant.show_rating_dialog,true))
			showConfirmationDialog(this.getString(R.string.title_rate_app), this.getString(R.string.rate_app),AppConstant.RATING_DIALOG);	
		else
			super.onBackPressed();


	}

	private class NavigateToSettingsTask extends AsyncTask<Void, Void, Boolean> {
		boolean navigateToSettingsScreen = true;
		@Override
		protected void onPreExecute() {
			// TODO Auto-generated method stub
			super.onPreExecute();

			if(AppPrefs.getBoolean(AppConstant.isProfileDone, false)){
				navigateToSettingsScreen = false;
			}
			if(AppConstant.globalBuddies!=null&& AppConstant.globalBuddies.size()>0){
				navigateToSettingsScreen = false;
			}
			if(navigateToSettingsScreen)
				showProgressDialog("Please login using your Microsoft Account / add buddies");
		}

		@Override
		protected void onPostExecute(Boolean result) {
			// TODO Auto-generated method stub
			dismissProgressDialog();
			if(result){
				startActivity(new Intent(HomeScreenActivity.this , SettingsActivity.class));
			}
		}

		@Override
		protected Boolean doInBackground(Void... params) {
			// TODO Auto-generated method stub
			try {
				if(navigateToSettingsScreen){
					Thread.sleep(1000);
				}
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}		

			return navigateToSettingsScreen;
		}

	}


	private class GetAddressTask extends AsyncTask<Double, Void, String>{
		Context mContext;
		String curAddress = null;
		public GetAddressTask(Context context) {
			super();
			mContext = context;
		}

		/*
		 * When the task finishes, onPostExecute() displays the address. 
		 */
		@Override
		protected void onPostExecute(String address) {
			// Display the current address in the UI
			currentLocationTxt.setText(address);
		}
		@Override
		protected String doInBackground(Double... params) {
			Geocoder geocoder =
					new Geocoder(mContext, Locale.getDefault());
			List<Address> addresses = null;

			if(AppConstant.userProfile.isLocationConsent())
			{
				try {
					addresses = geocoder.getFromLocation(params[0].doubleValue(),params[1].doubleValue(), 1);
				} catch (IOException e1) {
					LogUtils.LOGE(LogUtils.makeLogTag(HomeScreenActivity.class), "IO Exception in getFromLocation() : "+e1.getLocalizedMessage());
				} catch (IllegalArgumentException e2) {
					// Error message to post in the log
					String errorString = "Illegal arguments " +Double.toString(params[0])+" , "+Double.toString(params[1]) +" passed to address service";
					LogUtils.LOGE(LogUtils.makeLogTag(HomeScreenActivity.class), errorString);
				}
				// If the reverse geocode returned an address
				if (addresses != null && addresses.size() > 0) {
					if(AppConstant.getDeviceName().contains("Nokia"))
						curAddress = addresses.get(0).getFeatureName()/*+ addresses.get(0).getAdminArea()+ addresses.get(0).getCountryName()*/;
					else
					{
						curAddress = addresses.get(0).getAddressLine(0);

						if(addresses.get(0).getAddressLine(1)!=null)
							curAddress = curAddress+" "+addresses.get(0).getAddressLine(1);
						if(addresses.get(0).getAddressLine(2)!=null)
							curAddress = curAddress+" "+addresses.get(0).getAddressLine(2);
						if(addresses.get(0).getAddressLine(3)!=null)
							curAddress = curAddress+" "+addresses.get(0).getAddressLine(3);

					}
				} else
				{
					curAddress = "Lat = "+AppConstant.gpsTracker.latitude+", Long ="+AppConstant.gpsTracker.longitude;
				} 
			}else
				curAddress = HomeScreenActivity.this.getString(R.string.loc_settngs_dis_msg);

			return curAddress;

		}
	}// AsyncTask class

	@Override
	public void update(Observable observable, Object data) {
		// TODO Auto-generated method stub
		SharedPreferences.Editor editorTrack = AppPrefs.edit();
		if(observable instanceof NetworkObservable){
			wifiSIMNetworkStatus((Integer)data);
		}else if(observable instanceof GpsConnectivityObervable){
			gpsOnOffStatus((Integer)data);
		}else if(observable instanceof SOSStatusChangeObservable){
			if((Boolean)data){
				editorTrack.putBoolean(AppConstant.SOS_on, true);
				editorTrack.putBoolean(AppConstant.trackMe_on, true);
				editorTrack.commit();
			}
			sosOnOffStatus();
			trackMeOnOffStatus();
		}

	}

	private void registerSMSOnPBReceiver(){
		IntentFilter filter = new IntentFilter(StartSOSOnPBReceiver.PROCESS_RESPONSE);
		filter.addCategory(Intent.CATEGORY_DEFAULT);
		sosOnPBReceiver = new StartSOSOnPBReceiver();
		registerReceiver(sosOnPBReceiver, filter);
	}



	private ServiceConnection mServiceConnection = new ServiceConnection() {
		@Override
		public void onServiceDisconnected(ComponentName name) {
			mServiceConnected = false;
		}

		@Override
		public void onServiceConnected(ComponentName name, IBinder service) {
			mBoundServiceInterface = IBoundService.Stub.asInterface(service);
			mServiceConnected = true;
			try {
				SOSStatusChangeObservable.getInstance().sosStatusChanged((mBoundServiceInterface).getSOSStatus());
			} catch (RemoteException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	};

	private void broadcastLocatedBuddies()
	{
		Intent intent = new Intent(LocateActivity.ACTION_LOCATE_BUDDY);
		this.sendBroadcast(intent);
	}

}
