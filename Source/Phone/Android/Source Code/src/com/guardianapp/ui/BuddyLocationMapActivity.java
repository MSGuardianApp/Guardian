package com.guardianapp.ui;

import android.annotation.SuppressLint;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.os.Bundle;
import android.os.Handler;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.WindowManager;
import android.webkit.JavascriptInterface;
import android.webkit.WebChromeClient;
import android.webkit.WebView;
import android.widget.ImageView;
import android.widget.Toast;

import com.guardianapp.R;
import com.guardianapp.services.GPSTracker;
import com.guardianapp.utilities.AppConstant;

public class BuddyLocationMapActivity extends BaseActivity implements OnClickListener{

	private ImageView RoutImgBtn , imageGPSPinBtn;
	private WebView mWebView;
	private Handler mHandler;
	double locatedBuddyLat = 0;
	double locatedBuddyLong = 0;
	public static final String ACTION_RECEIVE_LOCATE_BUDDY_LOCATION = "com.guardianapp.ui.LocateBuddyMap";

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		getSupportActionBar().setTitle("LOCATE BUDDY");
		setContentView(R.layout.activity_buddy_map);
		getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_HIDDEN);
	}

	@Override
	protected void onResume() {
		// TODO Auto-generated method stub
		super.onResume();
	}
	
	@Override
	public void onBackPressed() {
		// TODO Auto-generated method stub
		super.onBackPressed();
	}
	
	@Override
	protected void onDestroy() {
		// TODO Auto-generated method stub
		unregisterReceiver(reciever);
		super.onDestroy();
	}

	@Override
	protected void initFields() {
		// TODO Auto-generated method stub
		imageGPSPinBtn = (ImageView)findViewById(R.id.imageGPSPinBtn);
		imageGPSPinBtn.setOnClickListener(this);
		mHandler = new Handler();
		locatedBuddyLat = getIntent().getDoubleExtra("lat", 0);
		locatedBuddyLong = getIntent().getDoubleExtra("lng", 0);
		setUpMapIfNeeded();
		registerLocateBuddyReceiver();
		registerCurrentUserLocationReceiver();
	}

	@Override
	protected void initCallBacks() {
		// TODO Auto-generated method stub


	}

	@SuppressLint("SetJavaScriptEnabled") 
	private void setUpMapIfNeeded(){
		this.mWebView = ((WebView)findViewById(R.id.webView));
		this.mWebView.getSettings().setJavaScriptEnabled(true);
		this.mWebView.setWebChromeClient(new WebChromeClient());
		this.mWebView.addJavascriptInterface(new WebAppInterface(this),"Android");
		refreshWebView();
	}
	
	private void registerLocateBuddyReceiver(){
		IntentFilter intentFilter = new IntentFilter(BuddyLocationMapActivity.ACTION_RECEIVE_LOCATE_BUDDY_LOCATION);
		registerReceiver(reciever, intentFilter);
	}
	
	private void registerCurrentUserLocationReceiver(){
		IntentFilter intentFilter = new IntentFilter(TrackMeActivity.ACTION_RECEIVE_LOCATION);
		registerReceiver(reciever, intentFilter);
	}


	private void refreshWebView()
	{
		this.mWebView.loadUrl("file:///android_asset/map.html");
	}


	@Override
	public void onClick(View v) {
		// TODO Auto-generated method stub
		switch(v.getId()){
		
		case R.id.imageGPSPinBtn :
			String shouldDrawRouteToLocateBuddy = "true";
			this.mWebView.loadUrl("javascript:createRouteToDestLocation(\"" + GPSTracker.latitude + "\",\"" + GPSTracker.longitude + "\",\"" +locatedBuddyLat+"\",\""+locatedBuddyLong+"\",\""+shouldDrawRouteToLocateBuddy+"\")");
		    break;
		case R.id.imageRouteBtn:
			BuddyLocationMapActivity.this.mWebView.loadUrl("javascript:createRouteToSelectedAddress(\"" + GPSTracker.latitude + "\",\"" + GPSTracker.longitude +"\")");
			break;

		}

	}
	
	private BroadcastReceiver reciever = new BroadcastReceiver() {
		
		@Override
		public void onReceive(Context context, Intent intent) {
			// TODO Auto-generated method stub
			String type = "";
			if(intent.getAction().equalsIgnoreCase(BuddyLocationMapActivity.ACTION_RECEIVE_LOCATE_BUDDY_LOCATION)){
		    boolean isLocatedBuddy = true;
		    type = AppConstant.intToBoolean(intent.getIntExtra("loc_buddy_issos",0))?"sos":"track";
			BuddyLocationMapActivity.this.mWebView.loadUrl("javascript:drawPolyLine(\"" + intent.getDoubleExtra("loc_buddy_lat",0.0) + "\",\"" + intent.getDoubleExtra("loc_buddy_long",0.0) + "\",\"" +isLocatedBuddy+"\",\""+type+ "\")");
			}else if(intent.getAction().equalsIgnoreCase(TrackMeActivity.ACTION_RECEIVE_LOCATION)){
				boolean isLocatedBuddy = false;
				type = AppConstant.intToBoolean(GPSTracker.GeoTagList.get(GPSTracker.GeoTagList.size()-1).getIsSOS())?"sos":"track";
				BuddyLocationMapActivity.this.mWebView.loadUrl("javascript:drawPolyLine(\"" + GPSTracker.latitude + "\",\"" + GPSTracker.longitude + "\",\"" +isLocatedBuddy+"\",\""+type+ "\")");
			}
		}
	};
	
	public class WebAppInterface {

		Context mContext;

		public WebAppInterface(Context paramContext){
			this.mContext = paramContext;
		}

		@JavascriptInterface
		public void mapLoaded(){

			BuddyLocationMapActivity.this.mHandler.post(new Runnable() {

				@Override
				public void run() {
					// TODO Auto-generated method stub
					String isLocatedBuddy = "true";
					if(BuddyLocationMapActivity.this.locatedBuddyLat > 0 && BuddyLocationMapActivity.this.locatedBuddyLong > 0)
					BuddyLocationMapActivity.this.mWebView.loadUrl("javascript:locateBuddyOrUser(\"" + BuddyLocationMapActivity.this.locatedBuddyLat + "\",\"" + BuddyLocationMapActivity.this.locatedBuddyLong + "\",\"" +isLocatedBuddy+ "\")");
				}
			});
		}
		@JavascriptInterface
		public void showEmptyRouteToast(){
			Toast.makeText(BuddyLocationMapActivity.this,"Please tap on a location on the map to create route", Toast.LENGTH_LONG).show();
		}
		
		@JavascriptInterface
		public void showToast(String msg){
			Toast.makeText(BuddyLocationMapActivity.this, msg, Toast.LENGTH_LONG).show();
		}

	}


}
