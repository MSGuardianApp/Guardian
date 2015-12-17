package com.guardianapp.ui;

import java.io.UnsupportedEncodingException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.annotation.SuppressLint;
import android.app.AlertDialog;
import android.content.BroadcastReceiver;
import android.content.ContentValues;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.location.Location;
import android.location.LocationListener;
import android.net.Uri;
import android.os.Bundle;
import android.os.Handler;
import android.provider.MediaStore;
import android.provider.Settings;
import android.view.KeyEvent;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.WindowManager;
import android.view.inputmethod.EditorInfo;
import android.webkit.JavascriptInterface;
import android.webkit.WebChromeClient;
import android.webkit.WebView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.guardianapp.R;
import com.guardianapp.adapters.TrackMeSearchResultAdapter;
import com.guardianapp.model.GPSCoordinate;
import com.guardianapp.model.NearByPlaceDetails;
import com.guardianapp.services.GPSTracker;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.ImageCompressor;
import com.guardianapp.webservicecomponents.MakeHTTPServices;
import com.guardianapp.webservicecomponents.OnTaskCompleted;

public class TrackMeActivity extends BaseActivity implements OnClickListener, LocationListener,OnTaskCompleted{

	private ImageView SOSImgBtn, TrackMeImgBtn, RoutImgBtn, cameraImgBtn;
	private TextView speedTv, recordedTimeTv;
	private EditText et_search; 
	private SharedPreferences AppPrefs;
	private ArrayList <NearByPlaceDetails> trackAddressList;
	ArrayList<GPSCoordinate> coordinateList;
	private TrackMeSearchResultAdapter resultAdapter;
	private ListView addressListView;
	private WebView mWebView;
	private Handler mHandler;
	private String[] latLongArray;
	private String latLongArrayStr;
	public static final String ACTION_RECEIVE_LOCATION = "com.guardianapp.ui.TrackMeActivity.ACTION_RECEIVE_LOCATION";
	private Uri fileUri;

	@Override
	protected void onCreate(Bundle savedInstance) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstance);
		getSupportActionBar().setTitle("TRACK ME");
		setContentView(R.layout.activity_google_map);
		getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_HIDDEN);
	}



	@Override
	protected void onResume() {
		// TODO Auto-generated method stub
		super.onResume();
	}

	@Override
	protected void onStart() {
		// TODO Auto-generated method stub
		super.onStart();
		checkGPS();
	}

	@Override
	public void onBackPressed() {
		// TODO Auto-generated method stub
		super.onBackPressed();
	}

	@Override
	protected void onDestroy() {
		// TODO Auto-generated method stub
		unregisterReceiver(receiver);
		super.onDestroy();
	}



	@Override
	protected void initFields() {
		// TODO Auto-generated method stub
		if(AppConstant.isGetDirection)
			AppConstant.isGetDirection =false;

		AppConstant.userProfile.setLocationConsent(true);

		AppPrefs = getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

		et_search = (EditText) findViewById(R.id.et_Search);
		et_search.setImeOptions(EditorInfo.IME_ACTION_SEARCH);
		trackAddressList = new ArrayList<NearByPlaceDetails>();
		speedTv = (TextView)findViewById(R.id.tv_oval);
		speedTv.setText(GPSTracker.latestSpeed+"".trim());
		recordedTimeTv = (TextView)findViewById(R.id.tv_recordedat_time);
		recordedTimeTv.setText(AppConstant.getRecordedTimeFromTicks(GPSTracker.recordedTime)+"".trim());
		mHandler = new Handler();

		setUpMapIfNeeded();
		initBottomBar();
		onOffSOS();
		onOffTrackMe();
		registerMapReceiver();
	}

	@Override
	protected void initCallBacks() {

		et_search.setOnEditorActionListener(new TextView.OnEditorActionListener() {
			@Override
			public boolean onEditorAction(TextView v, int actionId, KeyEvent event) {
				if (actionId == EditorInfo.IME_ACTION_SEARCH) {
					serchTrackMe();
					return true;
				}
				return false;
			}
		});


	}

	private void registerMapReceiver(){
		IntentFilter intentFilter = new IntentFilter(TrackMeActivity.ACTION_RECEIVE_LOCATION);
		registerReceiver(receiver, intentFilter);
	}

	private void serchTrackMe() {

		if(et_search!=null&&!et_search.getText().toString().equalsIgnoreCase(""))
		{
			MakeHTTPServices mkh = new MakeHTTPServices(this,null);
			//AppConstant.service_Tag = AppConstant.TRACKME_ACTIVITY_SEARCH_SERVICE_TAG;
			try {
				mkh.searchLocation(et_search.getText().toString());
			} catch (UnsupportedEncodingException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}



	@SuppressLint("SetJavaScriptEnabled") private void setUpMapIfNeeded() {
		ArrayList<GPSCoordinate> coordinateList = null;
		coordinateList = getGeoTagData();
		latLongArray = new String[coordinateList.size()];
		this.populateLatLongArray(coordinateList);
		latLongArrayStr = AppConstant.arrayToJson(this.latLongArray);
		this.mWebView = ((WebView)findViewById(R.id.webView));
		this.mWebView.getSettings().setJavaScriptEnabled(true);
		this.mWebView.setWebChromeClient(new WebChromeClient());
		this.mWebView.addJavascriptInterface(new WebAppInterface(this),"Android");
		refreshWebView();
	}

	private void populateLatLongArray(ArrayList<GPSCoordinate> coordinateList){
		for(int i=0;i<coordinateList.size();i++){
			GPSCoordinate gpsCoordinate = coordinateList.get(i);
			latLongArray[i]=gpsCoordinate.getLat().trim()+"-"+gpsCoordinate.getLong().trim()+"-"+gpsCoordinate.getIsSOS();
		}
	}


	private void refreshWebView()
	{
		this.mWebView.loadUrl("file:///android_asset/map.html");
	}

	@Override
	public void onTaskComplete(String result) {
		AppConstant.dismissProgressDialog();
		// TODO Auto-generated method stub
		if(AppConstant.service_Tag == AppConstant.POST_LOCATION_WITH_MEDIA_SERVICE_TAG)
		{
			if(result.contains("true")){
				dismissProgressDialog();
				Toast.makeText(this, this.getString(R.string.image_captured_msg), Toast.LENGTH_LONG).show();
			}

		}else if(AppConstant.service_Tag == AppConstant.GET_SEARCH_LOCATION_SERVICE_TAG){
			this.processSearchedAdressResponse(result);
		}

	}

	@Override
	public void onGetObjectResult(Object obj) {
		// TODO Auto-generated method stub
		AppConstant.dismissProgressDialog();

	}

	public void processSearchedAdressResponse(String result){
		try {
			JSONObject parsedData = new JSONObject(result.replace("microsoftMapsNetworkCallback(", "").replace(".d},'r229');", "}"));

			JSONObject  data= parsedData.getJSONObject("response");
			JSONObject  value= data.getJSONObject("d");
			String category=value.getJSONArray("ParseResults").getJSONObject(0).getString("Keyword");
			JSONArray searchResults= value.getJSONArray("SearchResults");

			if(trackAddressList.size()>0)
				trackAddressList.clear();

			for(int i = 0 ; i < searchResults.length() ; i++){
				JSONObject obj=searchResults.getJSONObject(i);
				NearByPlaceDetails nearByPlaceDetails=new  NearByPlaceDetails();
				nearByPlaceDetails.setName(obj.getString("Name"));

				StringBuilder address=new StringBuilder();
				address.append(obj.getString("Address"));
				if(!obj.getString("City").equalsIgnoreCase("null") && !obj.getString("City").equalsIgnoreCase("")){
					address.append(",");
					address.append(obj.getString("City"));
				}
				if(!obj.getString("State").equalsIgnoreCase("null") && !obj.getString("State").equalsIgnoreCase("")){
					address.append(",");
					address.append(obj.getString("State"));
				}
				if(!obj.getString("PostalCode").equalsIgnoreCase("null") && !obj.getString("PostalCode").equalsIgnoreCase("")){
					address.append(",");
					address.append(obj.getString("PostalCode"));
				}
				if(!obj.getString("Country").equalsIgnoreCase("null") && !obj.getString("Country").equalsIgnoreCase("")){
					address.append(",");
					address.append(obj.getString("Country"));
				}
				if(!obj.getString("Phone").equalsIgnoreCase("null") && !obj.getString("Phone").equalsIgnoreCase("")){
					address.append(",");
					address.append(obj.getString("Phone"));
				}

				nearByPlaceDetails.setVicinity(address.toString());
				nearByPlaceDetails.setLatitude(obj.getJSONObject("Location").getString("Latitude"));
				nearByPlaceDetails.setLongitude(obj.getJSONObject("Location").getString("Longitude"));
				nearByPlaceDetails.setCategory(category);
				nearByPlaceDetails.setPhoneNumber(obj.getString("Phone"));
				trackAddressList.add(nearByPlaceDetails);
			}	

			if(trackAddressList!=null){

				addressListView = (ListView)findViewById(R.id.lv_serchResult);				
				resultAdapter = new TrackMeSearchResultAdapter(this,this.mWebView,trackAddressList,addressListView);
				addressListView.setVisibility(View.VISIBLE);
				addressListView.setAdapter(resultAdapter);

			}


		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

	}


	@Override
	public void onClick(View v) {
		// TODO Auto-generated method stub
		switch (v.getId()) {
		case R.id.imageSOSBtn:
			SharedPreferences.Editor editorsos = AppPrefs.edit();
			if(AppPrefs.getBoolean(AppConstant.SOS_on, false))
			{
				editorsos.putBoolean(AppConstant.SOS_on, false);
				editorsos.commit();
				SOSImgBtn.setBackgroundResource(R.drawable.sos);

			}
			else
			{
				SOSImgBtn.setBackgroundResource(R.drawable.stopsos);
				Intent sosInt = new Intent(TrackMeActivity.this, SOSConnectActivity.class);
				startActivity(sosInt);

			}
			break;

		case R.id.imageTrackBtn:
			SharedPreferences.Editor editorTrack = AppPrefs.edit();
			if(AppPrefs.getBoolean(AppConstant.trackMe_on, false))
			{
				if(AppPrefs.getBoolean(AppConstant.SOS_on, false)){
					AppConstant.InitiateTracking(this,true);
					editorTrack.putBoolean(AppConstant.trackMe_on, true);
					editorTrack.commit();
					TrackMeImgBtn.setBackgroundResource(R.drawable.stoptrack);
					TextView trackStatus = (TextView) findViewById(R.id.tv_status);
					trackStatus.setText("ON");
					getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
					Toast.makeText(this, this.getString(R.string.turn_off_safe_text), Toast.LENGTH_LONG).show();
				}
				else
				{
					AppConstant.StopTracking(this);
					editorTrack.putBoolean(AppConstant.trackMe_on, false);
					editorTrack.commit();
					Toast.makeText(this, this.getString(R.string.tracking_off_msg), Toast.LENGTH_LONG).show();
					TrackMeImgBtn.setBackgroundResource(R.drawable.track);
					TextView trackStatus = (TextView) findViewById(R.id.tv_status);
					trackStatus.setText("OFF");
				}
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

				TrackMeImgBtn.setBackgroundResource(R.drawable.stoptrack);
				TextView trackStatus = (TextView) findViewById(R.id.tv_status);
				trackStatus.setText("ON");
				getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);

			}		
			break;

		case R.id.imageRouteBtn://for Route
			TrackMeActivity.this.mWebView.loadUrl("javascript:createRouteToSelectedAddress(\"" + GPSTracker.latitude + "\",\"" + GPSTracker.longitude +"\")");
			break;

		case R.id.imageCamera :
			String timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss").format(new Date());
			ContentValues values = new ContentValues();
			values.put(MediaStore.Images.Media.TITLE, "IMG_" + timeStamp + ".jpg");
			Intent intent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
			fileUri = getContentResolver().insert(MediaStore.Images.Media.EXTERNAL_CONTENT_URI, values); 
			intent.putExtra( MediaStore.EXTRA_OUTPUT,  fileUri);
			startActivityForResult(intent,AppConstant.CAMERA_ACTIVITY_RESULT);

			break;


		default:
			break;
		}


	}

	private void initBottomBar() {
		SOSImgBtn =(ImageView) findViewById(R.id.imageSOSBtn);
		TrackMeImgBtn = (ImageView) findViewById(R.id.imageTrackBtn);
		RoutImgBtn = (ImageView) findViewById(R.id.imageRouteBtn);
		cameraImgBtn = (ImageView) findViewById(R.id.imageCamera);

		SOSImgBtn.setOnClickListener(this);
		TrackMeImgBtn.setOnClickListener(this);
		RoutImgBtn.setOnClickListener(this);
		cameraImgBtn.setOnClickListener(this);
	}

	private void onOffSOS() {
		if(AppPrefs.getBoolean(AppConstant.SOS_on, false))
		{
			SOSImgBtn.setBackgroundResource(R.drawable.stopsos);
		}
		else
		{
			SOSImgBtn.setBackgroundResource(R.drawable.sos);
		}
	}

	private void onOffTrackMe() {
		if(AppPrefs.getBoolean(AppConstant.trackMe_on, false))
		{
			TrackMeImgBtn.setBackgroundResource(R.drawable.stoptrack);
		}
		else
		{
			TrackMeImgBtn.setBackgroundResource(R.drawable.track);
		}
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		getMenuInflater().inflate(R.menu.trackme_menu, menu);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		// TODO Auto-generated method stub
		switch(item.getItemId()){

		case R.id.aerial :
			TrackMeActivity.this.mWebView.loadUrl("javascript:showMapInAerialView(\"" + GPSTracker.latitude + "\",\"" + GPSTracker.longitude +"\")");
			break;

		case R.id.road_view:
			TrackMeActivity.this.mWebView.loadUrl("javascript:showMapInRoadView()");
			break;

		case R.id.focus_user:
			TrackMeActivity.this.mWebView.loadUrl("javascript:takeMeToMyPlace(\"" + GPSTracker.latitude + "\",\"" + GPSTracker.longitude +"\")");
			break;

		}
		return super.onOptionsItemSelected(item);
	}



	@Override
	public void onLocationChanged(Location location) {
		// TODO Auto-generated method stub

	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		// TODO Auto-generated method stub
		super.onActivityResult(requestCode,  resultCode, data);
		if (requestCode == AppConstant.CAMERA_ACTIVITY_RESULT && resultCode == RESULT_OK) {  

			if(AppConstant.check_networkConnectivity(this)!= AppConstant.NO_NETWORK)
			{
				if(AppPrefs.getBoolean(AppConstant.isProfileDone, false)
						&&AppConstant.userProfile.isPostLocationConsent())
				{
					if(AppConstant.isCanPostToServer(this))
					{
						ImageCompressor imageComp = new ImageCompressor(this);
						Bitmap photo = imageComp.compressImage(imageComp.getRealPathFromURI(this,fileUri));
						MakeHTTPServices sendfromServices = new MakeHTTPServices(this , null);
						try {
							showProgressDialog(this.getString(R.string.post_photo_to_server));
							sendfromServices.postMyLocationAsync( AppConstant.convertIntoBase64(photo),AppConstant.DEFAULT_SOS_COMMAND);
						} catch (JSONException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						}
					}
				}
				else{
					Toast.makeText(this, this.getString(R.string.unregistered_captured), Toast.LENGTH_LONG).show();
				}
			}
		}
	}



	private ArrayList<GPSCoordinate> getGeoTagData() {
		coordinateList = new ArrayList<GPSCoordinate>();
		if(GPSTracker.GeoTagList.size()>0)
		{
			for(int i=0;i<GPSTracker.GeoTagList.size();i++)
			{
				GPSCoordinate coordinate = new GPSCoordinate();
				coordinate.setLat(GPSTracker.GeoTagList.get(i).getLat());
				coordinate.setLong(GPSTracker.GeoTagList.get(i).getLong());
				coordinate.setIsSOS(GPSTracker.GeoTagList.get(i).getIsSOS());
				coordinateList.add(coordinate);

			}
		}
		else
		{
			GPSCoordinate coordinate = new GPSCoordinate();
			coordinate.setLat(String.valueOf(GPSTracker.latitude));
			coordinate.setLong(String.valueOf(GPSTracker.longitude));
			if(AppConstant.userProfile.isIsSOSOn()){
				coordinate.setIsSOS(1);
			}else if(AppConstant.userProfile.isIsTrackingOn()){
				coordinate.setIsSOS(0);
			}
			coordinateList.add(coordinate);
		}

		return coordinateList;

	}




	@Override
	public void onStatusChanged(String provider, int status, Bundle extras) {
		// TODO Auto-generated method stub

	}



	@Override
	public void onProviderEnabled(String provider) {
		// TODO Auto-generated method stub

	}



	@Override
	public void onProviderDisabled(String provider) {
		// TODO Auto-generated method stub

	}

	private boolean checkGPS() {
		if(AppConstant.gpsTracker!=null && AppConstant.userProfile!=null && !AppConstant.gpsTracker.checkGPSStatusOn()
				||!AppConstant.userProfile.isLocationConsent())
		{
			showGPSDialog();
			return false;
		}
		else
			return true;
	}
	private void showGPSDialog() {
		View view =  getLayoutInflater().inflate(R.layout.common_dialog, null);
		final AlertDialog gpsDialog = new AlertDialog.Builder(this)
		.setView(view).create();

		TextView title = (TextView) view.findViewById(R.id.tv_title);	
		title.setText(this.getString(R.string.info_txt));

		TextView msgTxt = (TextView) view.findViewById(R.id.tv_msgAleart);	
		msgTxt.setText(this.getText(R.string.enable_loc_settings));
		Button callbtn = (Button) view.findViewById(R.id.btnOK);
		callbtn.setText(this.getString(R.string.dialog_ok_button));

		Button cancelBtn = (Button) view.findViewById(R.id.btnCancel);
		cancelBtn.setText(this.getString(R.string.dialog_cancel_button));
		view.findViewById(R.id.btnOK).setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {
				//startActivityForResult(new Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS),AppConstant.GPS_SETTINGS_ACTIVITY_RESULT);
				if(AppConstant.gpsTracker!=null && !AppConstant.gpsTracker.checkGPSStatusOn()&&AppConstant.userProfile.isLocationConsent())
					startActivityForResult(new Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS),AppConstant.GPS_SETTINGS_ACTIVITY_RESULT);
				else
				{
					Intent gpsInt = new Intent(TrackMeActivity.this,SettingsActivity.class);
					gpsInt.putExtra("GPS_SETTINGS", "gpsSettings");
					startActivity(gpsInt);
				}
				gpsDialog.dismiss();
			}
		});

		view.findViewById(R.id.btnCancel).setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {
				gpsDialog.dismiss();

			}
		});

		gpsDialog.show();
	}

	private BroadcastReceiver receiver = new BroadcastReceiver() {

		@Override
		public void onReceive(Context context, Intent intent) {
			// TODO Auto-generated method stub
			ArrayList<GPSCoordinate> coordinateList = null;
			coordinateList = getGeoTagData();
			latLongArray = new String[coordinateList.size()];
			TrackMeActivity.this.populateLatLongArray(coordinateList);
			latLongArrayStr = AppConstant.arrayToJson(TrackMeActivity.this.latLongArray);

			TrackMeActivity.this.mWebView.loadUrl("javascript:appendPathToExisting(\"" + GPSTracker.latitude + "\",\"" + GPSTracker.longitude +  "\",\"" +GPSTracker.GeoTagList.get(GPSTracker.GeoTagList.size()-1).getIsSOS()+"\")");
			speedTv.setText(GPSTracker.latestSpeed+"".trim());
			recordedTimeTv.setText(AppConstant.getRecordedTimeFromTicks(GPSTracker.recordedTime)+"".trim());

		}
	};

	public class WebAppInterface {

		Context mContext;

		public WebAppInterface(Context paramContext){
			this.mContext = paramContext;
		}

		@JavascriptInterface
		public void mapLoaded(){

			TrackMeActivity.this.mHandler.post(new Runnable() {

				@Override
				public void run() {
					TrackMeActivity.this.mWebView.loadUrl("javascript:createRouteToDestLocArray(\"" + GPSTracker.latitude + "\",\"" + GPSTracker.longitude +  "\",\"" +TrackMeActivity.this.latLongArrayStr+"\")");

				}
			});
		}
		@JavascriptInterface
		public void showEmptyRouteToast(){
			Toast.makeText(TrackMeActivity.this,"Please tap on a location on the map to create route", Toast.LENGTH_LONG).show();
		}

	}

}
