package com.guardianapp.ui;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Observable;
import java.util.Observer;

import org.json.JSONException;

import android.annotation.SuppressLint;
import android.app.AlertDialog;
import android.content.ContentValues;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.net.Uri;
import android.os.Bundle;
import android.provider.MediaStore;
import android.provider.Settings;
import android.support.v4.view.ViewPager;
import android.support.v7.app.ActionBar;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.guardianapp.R;
import com.guardianapp.adapters.SOSFragmentStatePageAdapter;
import com.guardianapp.helpercomponents.GpsChangeReceiver;
import com.guardianapp.helpercomponents.GpsConnectivityObervable;
import com.guardianapp.services.CallDetectService;
import com.guardianapp.services.GPSTracker;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.ImageCompressor;
import com.guardianapp.webservicecomponents.MakeHTTPServices;
import com.guardianapp.webservicecomponents.OnTaskCompleted;


@SuppressLint("NewApi")
public class SOSActivity extends BaseActivity implements OnClickListener , OnTaskCompleted , Observer{
	private String[] tabs = { "My Buddies", "My Group", "Get Local Help" };
	Button switchONGpsBut;
	private ViewPager mViewPager;
	private TextView GpsAlert;
	public SharedPreferences AppPrefs;
	private SOSFragmentStatePageAdapter pagerAdapter;
	private Uri fileUri;
	@SuppressLint("NewApi") @Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.sos_screen);
		ActionBar mActionBar = getSupportActionBar();
		mActionBar.setDisplayShowHomeEnabled(false);
		mActionBar.setDisplayShowTitleEnabled(false);

		LayoutInflater mInflater = LayoutInflater.from(this);
		View mCustomView = mInflater.inflate(R.layout.custom_actionbar, null);
		switchONGpsBut = (Button)mCustomView.findViewById(R.id.switch_gps);
		if((AppConstant.gpsTracker!=null && !AppConstant.gpsTracker.checkGPSStatusOn())||!AppConstant.userProfile.isLocationConsent()){
			switchONGpsBut.setVisibility(View.VISIBLE);
		}else{
			switchONGpsBut.setVisibility(View.GONE);
		}
		switchONGpsBut.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				if((AppConstant.gpsTracker!=null && !AppConstant.gpsTracker.checkGPSStatusOn())&&AppConstant.userProfile.isLocationConsent()){
					startActivityForResult(new Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS),AppConstant.GPS_SETTINGS_ACTIVITY_RESULT);
				}else if((AppConstant.gpsTracker!=null && AppConstant.gpsTracker.checkGPSStatusOn())&& !AppConstant.userProfile.isLocationConsent()){
					AppConstant.userProfile.setLocationConsent(true);
					AppConstant.gpsTracker.startGPS();
					switchONGpsBut.setVisibility(View.GONE);
					trackMeOnOffStatus();
				}

			}
		});

		mActionBar.setCustomView(mCustomView);
		mActionBar.setDisplayShowCustomEnabled(true);

		if(AppConstant.isMyServiceRunning(getApplicationContext(),CallDetectService.class))
			stopService(new Intent(this, CallDetectService.class));  
	}

	@Override
	protected void onResume() {
		// TODO Auto-generated method stub
		super.onResume();
		GpsChangeReceiver.getObservable().addObserver(this);
		trackMeOnOffStatus();
	}

	@Override
	protected void onDestroy() {
		// TODO Auto-generated method stub
		super.onDestroy();
		GpsChangeReceiver.getObservable().deleteObserver(this);
	}



	@Override
	protected void initFields() {
		// TODO Auto-generated method stub
		AppPrefs = getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		AppConstant.isSoSActivity = true;
		mViewPager = (ViewPager) findViewById(R.id.sosFragmentLayout);
		pagerAdapter=new SOSFragmentStatePageAdapter(getSupportFragmentManager(),tabs);
		mViewPager.setAdapter(pagerAdapter);
		mViewPager.setOffscreenPageLimit(pagerAdapter.getCount());
		GpsAlert = (TextView) findViewById(R.id.tv_GpsMsg);

		if(AppConstant.gpsTracker!=null && AppConstant.userProfile!=null && AppConstant.gpsTracker.checkGPSStatusOn()&&AppConstant.userProfile.isLocationConsent())
			GpsAlert.setVisibility(View.GONE);
		else
		{
			GpsAlert.setVisibility(View.VISIBLE);

		}
		trackMeOnOffStatus();
		sosOnOffStatus();

		if(AppConstant.isMyServiceRunning(getApplicationContext(),CallDetectService.class))
			stopService(new Intent(this, CallDetectService.class));  


	}


	@Override
	protected void initCallBacks() {
		// TODO Auto-generated method stub
		findViewById(R.id.imageCamera).setOnClickListener(this);
		findViewById(R.id.imageSoSBtn).setOnClickListener(this);
		findViewById(R.id.imageTrack).setOnClickListener(this);
		GpsAlert.setOnClickListener(this);
	}

	@Override
	public void onClick(View v) {
		switch (v.getId()) {
		case R.id.tv_GpsMsg:
			if(AppConstant.gpsTracker!=null && !AppConstant.gpsTracker.checkGPSStatusOn()&&AppConstant.userProfile.isLocationConsent())
				startActivityForResult(new Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS),AppConstant.GPS_SETTINGS_ACTIVITY_RESULT);
			else
			{
				Intent gpsInt = new Intent(SOSActivity.this,SettingsActivity.class);
				gpsInt.putExtra("GPS_SETTINGS", "gpsSettings");
				startActivity(gpsInt);
			}
			break;

		case R.id.imageCamera:
			String timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss").format(new Date());
			ContentValues values = new ContentValues();
			values.put(MediaStore.Images.Media.TITLE, "IMG_" + timeStamp + ".jpg");
			Intent intent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
			fileUri = getContentResolver().insert(MediaStore.Images.Media.EXTERNAL_CONTENT_URI, values); 
			intent.putExtra( MediaStore.EXTRA_OUTPUT,  fileUri);
			startActivityForResult(intent,AppConstant.CAMERA_ACTIVITY_RESULT);			
			break;

		case R.id.imageSoSBtn:
			SharedPreferences.Editor editor = AppPrefs.edit();
			if(AppPrefs.getBoolean(AppConstant.SOS_on, false))
			{
				editor.putBoolean(AppConstant.SOS_on, false);
				editor.commit();
				AppConstant.StopSOS(this);
				sosOnOffStatus();
			}
			else
			{
				editor.putBoolean(AppConstant.SOS_on, true);
				editor.commit();
				AppConstant.sendSMSViaPhone(SOSActivity.this);
				sosOnOffStatus();
				setTrackMeOn();
			} 

			break;

		case R.id.imageTrack:	
			setTrackMeOn();
			break;
		default:
			break;
		}

	}

	private boolean setTrackMeOn() {
		SharedPreferences.Editor editorTrack = AppPrefs.edit();

		if(!checkGPS())
			return false;


		if(AppPrefs.getBoolean(AppConstant.trackMe_on, false))
		{
			if(AppPrefs.getBoolean(AppConstant.SOS_on, false)){
				AppConstant.InitiateTracking(this,true);
				editorTrack.putBoolean(AppConstant.trackMe_on, true);
				editorTrack.commit();
				trackMeOnOffStatus();
				Toast.makeText(this, this.getString(R.string.turn_off_safe_text), Toast.LENGTH_LONG).show();
			}
			else
			{
				AppConstant.StopTracking(this);
				editorTrack.putBoolean(AppConstant.trackMe_on, false);
				editorTrack.commit();
				trackMeOnOffStatus();
				Toast.makeText(this, this.getString(R.string.tracking_off_msg), Toast.LENGTH_LONG).show();
			}
			return false;
		}
		else
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
			trackMeOnOffStatus();
			Toast.makeText(this, this.getString(R.string.save_batt_msg), Toast.LENGTH_LONG).show();
			return true;
		}
	}



	private void sosOnOffStatus() {

		ImageView sosBtn =  (ImageView) findViewById(R.id.imageSoSBtn);;
		TextView btntext = (TextView) findViewById(R.id.textSOS);
		if(AppPrefs.getBoolean(AppConstant.SOS_on, false))
		{
			sosBtn.setBackgroundResource(R.drawable.stopsos);
			AppConstant.TokenId = AppConstant.SOS_TOKEN;
			btntext.setText(this.getString(R.string.stop_sos));
		}
		else
		{
			sosBtn.setBackgroundResource(R.drawable.sos);
			btntext.setText(this.getString(R.string.start_sos));
			AppConstant.CustomDialog(this,getResources().getString(R.string.Str_SOSDialog_text) , "Confirmation", "ok", "cancel");
			if(AppPrefs.getBoolean(AppConstant.trackMe_on, false))
				AppConstant.TokenId = AppConstant.TRACKING_TOKEN;
			Toast.makeText(this, this.getString(R.string.save_batt_msg), Toast.LENGTH_LONG).show();
		}

	}
	private void trackMeOnOffStatus() {
		ImageView trackMe =  (ImageView) findViewById(R.id.imageTrack);
		TextView btntext = (TextView) findViewById(R.id.textTrackme);
		if(AppPrefs.getBoolean(AppConstant.trackMe_on, false)&&AppConstant.gpsTracker!=null && AppConstant.gpsTracker.checkGPSStatusOn() && AppConstant.userProfile.isLocationConsent())
		{

			trackMe.setBackgroundResource(R.drawable.stoptrack);
			AppConstant.TokenId = AppConstant.TRACKING_TOKEN;
			btntext.setText(this.getString(R.string.stop_tracking));
		}

		else
		{

			trackMe.setBackgroundResource(R.drawable.track);
			btntext.setText(this.getString(R.string.start_tracking));
		}

	}


	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {

		super.onActivityResult(requestCode,  resultCode, data);
		if (requestCode == AppConstant.CAMERA_ACTIVITY_RESULT && resultCode == RESULT_OK) {  

			if(AppConstant.check_networkConnectivity(this)!= AppConstant.NO_NETWORK)
			{
				if(AppPrefs.getBoolean(AppConstant.isProfileDone, false)
						&&AppConstant.userProfile.isPostLocationConsent())
				{
					if(AppConstant.isCanPostToServer(this)){
						ImageCompressor imageComp = new ImageCompressor(this);
						Bitmap photo = imageComp.compressImage(imageComp.getRealPathFromURI(this,fileUri));
						this.postCapturedPhotoToServer(photo);
					}else{

					}
				}else{
					Toast.makeText(this, this.getString(R.string.unregistered_captured), Toast.LENGTH_LONG).show();
				}
			}
		}
	}

	@Override
	public void onTaskComplete(String result) {
		// TODO Auto-generated method stub
		AppConstant.dismissProgressDialog();
		if(result.contains("true")){
			dismissProgressDialog();
			Toast.makeText(this, this.getString(R.string.image_captured_msg), Toast.LENGTH_LONG).show();
		}

	}

	@Override
	public void onGetObjectResult(Object obj) {
		// TODO Auto-generated method stub
		AppConstant.dismissProgressDialog();

	}

	@Override
	public void update(Observable observable, Object data) {
		// TODO Auto-generated method stub
		if(observable instanceof GpsConnectivityObervable){
			gpsOnOffStatus((Integer)data);
		}

	}

	private void gpsOnOffStatus(int data) {

		if(data == AppConstant.GPS_ENABLED){
			switchONGpsBut.setVisibility(View.GONE);
		}else if(data == AppConstant.GPS_DISABLED){
			switchONGpsBut.setVisibility(View.VISIBLE);
			trackMeOnOffStatus();
		}}

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
						Intent gpsInt = new Intent(SOSActivity.this,SettingsActivity.class);
						gpsInt.putExtra("GPS_SETTINGS", "gpsSettings");
						startActivity(gpsInt);
					}
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
				}


			}
		});

		gpsDialog.show();
	}

	private void postCapturedPhotoToServer(Bitmap result){
		Bitmap photo = result;
		MakeHTTPServices sendfromServices = new MakeHTTPServices(this , null);
		try {
			AppConstant.dismissProgressDialog();
			showProgressDialog(this.getString(R.string.post_photo_to_server));
			sendfromServices.postMyLocationAsync( AppConstant.convertIntoBase64(photo),AppConstant.DEFAULT_SOS_COMMAND);
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

}