package com.guardianapp.ui;

import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.TimeZone;
import java.util.concurrent.ExecutionException;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.graphics.Typeface;
import android.location.Address;
import android.location.Geocoder;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.ListView;
import android.widget.Toast;

import com.guardianapp.R;
import com.guardianapp.adapters.LocateListAdapter;
import com.guardianapp.database.DBQueries;
import com.guardianapp.model.Locate;
import com.guardianapp.model.LocateBuddies;
import com.guardianapp.model.ProfileListLite;
import com.guardianapp.services.LiveAccount;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.LocateBuddiesListComparator;
import com.guardianapp.utilities.LogUtils;
import com.guardianapp.webservicecomponents.MakeHTTPServices;
import com.guardianapp.webservicecomponents.OnTaskCompleted;

public class LocateActivity extends BaseActivity implements OnTaskCompleted{
	private ListView locateListView;
	private LocateListAdapter locateAdapter;
	private ArrayList<Locate> locateList ;
	private ArrayList<LocateBuddies> buddyLocationList ;
	private boolean isUnRagister = false;
	private LocateBuddies buddy;
	DBQueries db;
	public static final String ACTION_LOCATE_BUDDY = "com.guardianapp.ui.LocateActivity.ACTION_LOCATE_BUDDY";
	public static boolean IS_LOCATE_BUDDIES_LIST_LOADED = false;

	protected void onCreate(Bundle savedInstanceState) {
		SharedPreferences prefs_LiveAttributes = this.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

		super.onCreate(savedInstanceState);
		getSupportActionBar().setTitle("LOCATE");

		if(prefs_LiveAttributes.getBoolean(AppConstant.isLiveRagister, false))
		{
			if(prefs_LiveAttributes.getBoolean(AppConstant.isProfileDone, false))
			{
				setContentView(R.layout.locate_activity_screen);
				locateListView  = (ListView) findViewById(R.id.lv_LocateList);
				db = new DBQueries(this);
				buddyLocationList = db.selectRecordFromLocateBuddy();
				if(buddyLocationList!=null && buddyLocationList.size()>0)
				{
					locateAdapter = new LocateListAdapter(this, buddyLocationList, mListViewDidLoadHanlder);
					locateListView.setAdapter(locateAdapter);
				}else{
					locateMembers(true);
				}
				IS_LOCATE_BUDDIES_LIST_LOADED = true;
				this.registerLocateBuddyReceiver();
				findViewById(R.id.img_refreshBtn).setOnClickListener(new OnClickListener() {

					@Override
					public void onClick(View v) {

						locateMembers(true);
					}
				});

			}

			else
				startActivityForResult(new Intent(this, CreateProfileActivity.class),AppConstant.CREATE_PROFILE_ACTIVITY_RESULT);
		}
		else
		{
			setContentView(R.layout.create_profie_fragment);
			isUnRagister = true;
			this.initializeCreateProfileView();



		}

	}

	private void initializeCreateProfileView(){

		Button connectToMsLive = (Button)findViewById(R.id.btn_connectToMSLive);
		Typeface face=Typeface.createFromAsset(this.getAssets(), "SEGOEUI.TTF");
		connectToMsLive.setTypeface(face);
		connectToMsLive.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				LiveAccount guardianLive = new LiveAccount(LocateActivity.this, null);
				guardianLive.connectToLiveAccount();

			}
		});

		findViewById(R.id.hyperLink).setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v){
				Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse(AppConstant.guardianPortalLink));
				startActivity(browserIntent);

			}
		});

	}

	@Override
	protected void initFields() {
		// TODO Auto-generated method stub

	}

	@Override
	protected void initCallBacks() {
		// TODO Auto-generated method stub
	

	}
	
	private Handler mListViewDidLoadHanlder = new Handler(new Handler.Callback() { 
	    @Override
	    public boolean handleMessage(Message message) {
	        //Do whatever you need here the listview is loaded
	    	if(IS_LOCATE_BUDDIES_LIST_LOADED){
	    		LocateActivity.this.locateMembers(true);
	    		IS_LOCATE_BUDDIES_LIST_LOADED = false;
	    	}
	        return false;
	    }
	});

	private void registerLocateBuddyReceiver(){
		IntentFilter intentFilter = new IntentFilter(LocateActivity.ACTION_LOCATE_BUDDY);
		registerReceiver(receiver, intentFilter);
	}

	@Override
	public void onActivityResult(int requestCode, int resultCode,
			Intent data) {
		super.onActivityResult(requestCode, resultCode, data);
		if(requestCode==AppConstant.CREATE_PROFILE_ACTIVITY_RESULT)
		{

			isUnRagister = false;
			finish();
			startActivity(getIntent());
		}
	}

	private void locateMembers(boolean isFirst){
		MakeHTTPServices services = new MakeHTTPServices(this,null);
		if(isFirst)
			AppConstant.showProgressDialogWithMessage(this, "Locating and loading your buddies..");
		services.GetLocateMembers();
	}

	@Override
	public void onTaskComplete(String result) {
		AppConstant.dismissProgressDialog();
	}

	@Override
	public void onGetObjectResult(Object obj) {
		//AppConstant.dismissProgressDialog();
		if(!isUnRagister)
		{
			if(db==null){
				db = new DBQueries(this);
			}
			ProfileListLite oLocateMembersList=(ProfileListLite)obj;
			if(AppConstant.locateBuddies.size()>0)
				AppConstant.locateBuddies.clear();
			if ((oLocateMembersList == null || oLocateMembersList.getList() == null || oLocateMembersList.getList().size() == 0) )
			{
				db.removeAll(DBQueries.TABLE_LOCATION_BUDDY);
				AppConstant.dismissProgressDialog();
				Toast.makeText(this, "Unable to Locate your buddies..", Toast.LENGTH_LONG).show();
			}
			else if (oLocateMembersList != null && oLocateMembersList.getList() != null)
			{	
				for(int i=0;i<oLocateMembersList.getList().size();i++)
				{

					String backStatusColor = "";
					double locBuddyLat = 0;
					double locBuddyLong = 0;

					if(oLocateMembersList.getList().get(i).isIsSOSOn())
					{
						backStatusColor= AppConstant.SaffronColorCode;
					}
					else if(oLocateMembersList.getList().get(i).isIsTrackingOn())
					{
						backStatusColor= AppConstant.GreenColorCode;
					}
					else
					{
						backStatusColor= AppConstant.WhiteColorCode;
					}
					int order=2;
					if(oLocateMembersList.getList().get(i).isIsSOSOn())
					{
						order=6;
					}
					else if(oLocateMembersList.getList().get(i).isIsTrackingOn())
					{
						order=4;
					}
					else
					{
						order=2;
					}
					buddy = new LocateBuddies();
					//These numbers are used for Border Thickness as well.
					buddy.setBuddyProfileId(oLocateMembersList.getList().get(i).getProfileID());
					buddy.setName(oLocateMembersList.getList().get(i).getName());
					buddy.setEmail(oLocateMembersList.getList().get(i).getEmail());
					buddy.setPhoneNumber(oLocateMembersList.getList().get(i).getMobileNumber());
					buddy.setBuddyStatusColor(backStatusColor);
					buddy.setSessionID(oLocateMembersList.getList().get(i).getSessionID());
					buddy.setOrderNumber(order);
					buddy.setBorderThickness(String.valueOf( order));
					buddy.setLastLocs(oLocateMembersList.getList().get(i).getLastLocs());
					String address="";
					if (oLocateMembersList.getList().get(i).getLastLocs() != null && oLocateMembersList.getList().get(i).getLastLocs().size() > 0){
						Date date = AppConstant.getTimeFromTicks(oLocateMembersList.getList().get(i).getLastLocs().get(0).getTimeStamp());
						SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy HH:mm:ss");
						sdf.setTimeZone(TimeZone.getTimeZone("UTC"));
						String currentDateandTime = sdf.format(date);
						try {
							locBuddyLat = Double.parseDouble(oLocateMembersList.getList().get(i).getLastLocs().get(0).getLat().trim());
							locBuddyLong = Double.parseDouble(oLocateMembersList.getList().get(i).getLastLocs().get(0).getLong().trim());
							address = (new GetAddressTask(this,currentDateandTime)).execute(locBuddyLat , locBuddyLong).get();
						} catch (NumberFormatException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						} catch (InterruptedException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						} catch (ExecutionException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						}
						buddy.setLastLocation("( was at " + address + " - " +currentDateandTime+ ")");
					}
					else
					{
						buddy.setBuddyStatusColor(AppConstant.WhiteColorCode);
					}

					if(db.checkLocateBuddyIntoDB(buddy) )
					{
						db.updateLocateBuddyValue(buddy);
					}
					else
					{
						db.insertLocateBuddyIntoDB(buddy);
					}





					if(AppConstant.locateBuddies!=null)
					{
						if(!checkLocateBuddies(buddy))
							AppConstant.locateBuddies.add(buddy);
					}

					if(AppConstant.locateBuddy!=null && AppConstant.locateBuddy.getBuddyProfileId()!=null){

						if((AppConstant.locateBuddy.getBuddyProfileId() == buddy.getBuddyProfileId())&& (locBuddyLat!=0 && locBuddyLong!=0)){
							Intent intent = new Intent(BuddyLocationMapActivity.ACTION_RECEIVE_LOCATE_BUDDY_LOCATION);
							intent.putExtra("loc_buddy_id", buddy.getBuddyProfileId());
							intent.putExtra("loc_buddy_lat", locBuddyLat);
							intent.putExtra("loc_buddy_long", locBuddyLong);
							intent.putExtra("loc_buddy_issos", oLocateMembersList.getList().get(i).isIsSOSOn());
							this.sendBroadcast(intent);
						}

					}

				}
				db.sanitizeLocateBuddiesList(AppConstant.locateBuddies);
				buddyLocationList = AppConstant.locateBuddies;
				Collections.sort(buddyLocationList, new LocateBuddiesListComparator());
				AppConstant.dismissProgressDialog();
				locateAdapter = new LocateListAdapter(this, buddyLocationList , mListViewDidLoadHanlder);
				locateListView.setAdapter(locateAdapter);
			}
		}

		else
		{
			isUnRagister = false;
			finish();
			startActivity(getIntent());
		}


	}


	private boolean  checkLocateBuddies(LocateBuddies buddies) {

		for(int i=0;i<AppConstant.locateBuddies.size();i++)
		{
			if(AppConstant.locateBuddies.get(i).getBuddyProfileId() == buddies.getBuddyProfileId())
			{
				AppConstant.locateBuddies.set(i, buddies) ;
				return true;
			}


		}
		return false;

	}

	private void ClearLocateBuddies()
	{
		if(buddyLocationList.size()>0)
			buddyLocationList.clear();
	}

	@Override
	protected void onResume() {
		// TODO Auto-generated method stub
		super.onResume();
		//locateMembers();
	}


	@Override
	public void onBackPressed() {
		super.onBackPressed();
		finish();
	}

	@Override
	protected void onDestroy() {
		unregisterReceiver(receiver);
		super.onDestroy();
	}

	private class GetAddressTask extends AsyncTask<Double, Void, String>{
		Context mContext;
		String curAddress = null;
		String currentDateTime = "";
		public GetAddressTask(Context context , String currentDateTime) {
			super();
			mContext = context;
			this.currentDateTime = currentDateTime;
		}

		/*
		 * When the task finishes, onPostExecute() displays the address. 
		 */
		@Override
		protected void onPostExecute(String address) {
			// Display the current address in the UI
			buddy.setLastLocation("( was at " + address + " - " +this.currentDateTime+ ")");
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

						if(addresses.get(0).getAddressLine(3)!=null)
							curAddress = addresses.get(0).getAddressLine(0)+" "+addresses.get(0).getAddressLine(1)+", "+addresses.get(0).getAddressLine(2) +" "+addresses.get(0).getAddressLine(3) ;
						else
							curAddress = addresses.get(0).getAddressLine(0)+" "+addresses.get(0).getAddressLine(1)+", "+addresses.get(0).getAddressLine(2);
					}
				} else
				{
					curAddress = "Lat = "+AppConstant.gpsTracker.latitude+", Long ="+AppConstant.gpsTracker.longitude;
				} 
			}else
				curAddress = LocateActivity.this.getString(R.string.loc_settngs_dis_msg);

			return curAddress;

		}
	}// AsyncTask class

	private BroadcastReceiver receiver = new BroadcastReceiver() {

		@Override
		public void onReceive(Context context, Intent intent) {
			// TODO Auto-generated method stub
			locateMembers(false);

		}
	};



}
