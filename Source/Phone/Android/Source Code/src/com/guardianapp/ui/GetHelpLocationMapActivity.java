package com.guardianapp.ui;

import android.annotation.SuppressLint;
import android.content.Context;
import android.os.Bundle;
import android.os.Handler;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.WindowManager;
import android.webkit.JavascriptInterface;
import android.webkit.WebChromeClient;
import android.webkit.WebView;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.Toast;

import com.guardianapp.R;
import com.guardianapp.services.GPSTracker;
import com.guardianapp.utilities.AppConstant;

public class GetHelpLocationMapActivity extends BaseActivity implements OnClickListener{

	private ImageView imageGPSPinBtn;
	private WebView mWebView;
	private Handler mHandler;
	double localHelpLat = 0;
	double localHelpLong = 0;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		getSupportActionBar().setTitle("LOCAL HELP");
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
		super.onDestroy();
	}

	@Override
	protected void initFields() {
		// TODO Auto-generated method stub
		imageGPSPinBtn = (ImageView)findViewById(R.id.imageGPSPinBtn);
		//RoutImgBtn.setVisibility(View.GONE);
		imageGPSPinBtn.setVisibility(View.GONE);
		mHandler = new Handler();
		localHelpLat = getIntent().getDoubleExtra("lat", 0);
		localHelpLong = getIntent().getDoubleExtra("lng", 0);

		setUpMapIfNeeded();
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

	private void refreshWebView()
	{
		this.mWebView.loadUrl("file:///android_asset/map.html");
	}

	@Override
	public void onClick(View v) {
		// TODO Auto-generated method stub
		switch(v.getId()){
		case R.id.imageRouteBtn://for Route
			GetHelpLocationMapActivity.this.mWebView.loadUrl("javascript:createRouteToSelectedAddress(\"" + GPSTracker.latitude + "\",\"" + GPSTracker.longitude +"\")");
			break;

		}

	}

	public class WebAppInterface {

		Context mContext;

		public WebAppInterface(Context paramContext){
			this.mContext = paramContext;
		}

		@JavascriptInterface
		public String mapLoaded(){

			GetHelpLocationMapActivity.this.mHandler.post(new Runnable() {

				@Override
				public void run() {
					// TODO Auto-generated method stub
					String shouldDrawRouteToLocateBuddy = "false";
					GetHelpLocationMapActivity.this.mWebView.loadUrl("javascript:createRouteToDestLocation(\"" + GPSTracker.latitude + "\",\"" + GPSTracker.longitude + "\",\"" +GetHelpLocationMapActivity.this.localHelpLat+"\",\""+GetHelpLocationMapActivity.this.localHelpLong+"\",\""+shouldDrawRouteToLocateBuddy+"\")");

				}
			});
			return "";

		}
		@JavascriptInterface
		public void showEmptyRouteToast(){
			Toast.makeText(GetHelpLocationMapActivity.this,"Please tap on a location on the map to create route", Toast.LENGTH_LONG).show();
		}

		@JavascriptInterface
		public void showCreateRouteDialog(){
			GetHelpLocationMapActivity.this.mHandler.post(new Runnable() {

				@Override
				public void run() {
					// TODO Auto-generated method stub
					AppConstant.showProgressDialogWithMessage(GetHelpLocationMapActivity.this, "Creating route..Please wait..");

				}
			});

		}

		@JavascriptInterface
		public void dismissCreateRouteDialog(){
			AppConstant.dismissProgressDialog();

		}


	}

}



