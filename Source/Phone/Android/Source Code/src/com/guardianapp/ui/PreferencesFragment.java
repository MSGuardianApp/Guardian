package com.guardianapp.ui;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Vector;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.xmlpull.v1.XmlPullParserException;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.content.pm.Signature;
import android.os.Bundle;
import android.provider.Settings;
import android.support.v4.app.Fragment;
import android.text.Editable;
import android.text.TextUtils;
import android.text.TextWatcher;
import android.util.Base64;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.AdapterView.OnItemSelectedListener;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.CompoundButton.OnCheckedChangeListener;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.RelativeLayout;
import android.widget.ScrollView;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.ToggleButton;

import com.guardianapp.R;
import com.guardianapp.adapters.CallerListAdapter;
import com.guardianapp.database.DBQueries;
import com.guardianapp.helpercomponents.GpsChangeReceiver;
import com.guardianapp.listener.ScrollViewListener;
import com.guardianapp.model.CountryCodes;
import com.guardianapp.model.GroupDO;
import com.guardianapp.model.MyBuddies;
import com.guardianapp.services.GPSTracker;
import com.guardianapp.services.SMSService;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.LogUtils;
import com.guardianapp.views.ScrollViewExt;
import com.guardianapp.webservicecomponents.MakeHTTPServices;

@SuppressLint("NewApi") 
public class PreferencesFragment extends Fragment implements OnClickListener , OnItemSelectedListener, ScrollViewListener  {
	private ToggleButton tlocSerBtn, tSendLocSerBtn, tphoneLoc;
	private TextView tv_locatonSer,tv_sendLocSer,tv_phoneLocSer,tv_sendLocSer_txt,fbGroupDescText, fbConnectedText;
	private CheckBox startSOSonPB,canSMS,canEmail, canFBpost;
	private ScrollViewExt scrollView;
	private EditText et_Country, et_police, et_fire, et_ambulance,et_defaultCaller, etSosRecurrence;
	private RelativeLayout rel_sendLocSer;
	private ListView callerListView;
	private Button fbLogInButton , fbLogOutButton;
	private ImageView refreshButton;
	public SharedPreferences prefs;
	private SharedPreferences.Editor edit;
	private  String APP_ID = "";
	private Spinner fbGroupSpinner;
	Vector<GroupDO> groupList = null;
	private List<String> groupNamesList = null;
	ArrayList<MyBuddies> callerList = null;
	private Map<String,String> groupIDNameMap = null;
	private boolean isFBGroupsListRefreshed = false;
	private GpsChangeReceiver reciever;


	public View onCreateView(LayoutInflater inflater, ViewGroup container,
			Bundle savedInstanceState) {
		validateAppSignatureForPackage();

		View view = inflater.inflate(R.layout.preferences_fragment, container, false);
		prefs = this.getActivity().getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		APP_ID = this.getActivity().getResources().getString(R.string.Str_faceBookAppId);
		init(view);
		checkPreference();
		registerLocationConsentReceiver();
		initGPSTracker();


		tlocSerBtn.setOnCheckedChangeListener(new OnCheckedChangeListener() {

			@Override
			public void onCheckedChanged(CompoundButton buttonView,
					boolean isChecked) {

				DBQueries updateRecord = new DBQueries(getActivity().getApplicationContext());
				if(isChecked){
					tv_locatonSer.setText("On");
					tlocSerBtn.setBackgroundResource(R.drawable.on);
					AppConstant.userProfile.setLocationConsent(true);
					if((AppConstant.gpsTracker!=null)&&AppConstant.gpsTracker.checkGPSStatusOn())
						AppConstant.gpsTracker.startGPS();
					Intent intent = new Intent(GpsChangeReceiver.ACTION_GPS_CHANGE_RECEIVER);
					PreferencesFragment.this.getActivity().sendBroadcast(intent);
				}else{
					tlocSerBtn.setBackgroundResource(R.drawable.off);
					tv_locatonSer.setText("Off");
					AppConstant.userProfile.setLocationConsent(false);
					if((AppConstant.gpsTracker!=null)&&!AppConstant.gpsTracker.checkGPSStatusOn())
						AppConstant.gpsTracker.stopUsingGPS();
					Intent intent = new Intent(GpsChangeReceiver.ACTION_GPS_CHANGE_RECEIVER);
					PreferencesFragment.this.getActivity().sendBroadcast(intent);
				}

				updateRecord.updateValue("LocationConsent", AppConstant.userProfile.isLocationConsent());
			}


		});

		tSendLocSerBtn.setOnCheckedChangeListener(new OnCheckedChangeListener() {

			@Override
			public void onCheckedChanged(CompoundButton buttonView,
					boolean isChecked) {
				DBQueries updateRecord = new DBQueries(getActivity().getApplicationContext());
				if(isChecked){
					tv_sendLocSer.setText("On");
					tSendLocSerBtn.setBackgroundResource(R.drawable.on);
					AppConstant.userProfile.setPostLocationConsent(true);
				}else{
					tv_sendLocSer.setText("Off");
					AppConstant.userProfile.setPostLocationConsent(false);
					tSendLocSerBtn.setBackgroundResource(R.drawable.off);
				}

				updateRecord.updateValue("PostLocationConsent", AppConstant.userProfile.isPostLocationConsent());
			}


		});



		tphoneLoc.setOnCheckedChangeListener(new OnCheckedChangeListener() {

			@Override
			public void onCheckedChanged(CompoundButton buttonView,
					boolean isChecked) {

				if(isChecked){

					tv_phoneLocSer.setText("On");


				}else{
					tv_phoneLocSer.setText("Off");
				}
				startActivity(new Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS));

			}


		});

		final SharedPreferences.Editor caller_edit = prefs.edit();
		callerListView.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> arg0,  View v, int position, long arg3) {

				if(callerList.get(position)!=null && position > 0){
					et_defaultCaller.setText(" "+callerList.get(position).getName());
					caller_edit.putBoolean(AppConstant.IS_CALLER_BUDDY_SELECTED, true);
					caller_edit.commit();
					AppConstant.callerBuddies = callerList.get(position);
				}
				else{
					et_defaultCaller.setText("None");
					caller_edit.putBoolean(AppConstant.IS_CALLER_BUDDY_SELECTED, false);
					caller_edit.commit();
				}
				callerListView.setVisibility(View.GONE);
			}
		});

		if(prefs.getBoolean(AppConstant.IS_CALLER_BUDDY_SELECTED, false) && AppConstant.callerBuddies!=null&&AppConstant.callerBuddies.getName()!=null)
			et_defaultCaller.setText(" "+AppConstant.callerBuddies.getName());
		else
			et_defaultCaller.setText("None");

		if(prefs.getBoolean(AppConstant.isLiveRagister, false))
		{
			tv_sendLocSer_txt.setVisibility(View.VISIBLE);
			rel_sendLocSer.setVisibility(View.VISIBLE);
		}
		else
		{
			tv_sendLocSer_txt.setVisibility(View.GONE);
			rel_sendLocSer.setVisibility(View.GONE);
		}
		return view;
	}


	@Override
	public void onResume() {
		// TODO Auto-generated method stub
		super.onResume();
		this.checkDeviceLocSettings();
		this.manageCallerList();
		this.manageFBConnectedStatus();

	}

	@Override
	public void onDestroy() {
		// TODO Auto-generated method stub
		super.onDestroy();
		this.getActivity().unregisterReceiver(reciever);
	}


	private void init(View view)
	{
		final ScrollViewExt ScrlView = (ScrollViewExt) view.findViewById(R.id.ScrlView);
		ScrlView.setScrollViewListener(this);
		//ScrlView.fullScroll(ScrollView.FOCUS_UP);
		ScrlView.postDelayed(new Runnable() {
			@Override
			public void run() {
				ScrlView.fullScroll(ScrollView.FOCUS_UP);
			}
		}, 50);

		tlocSerBtn = (ToggleButton) view.findViewById(R.id.toggleLocationServices);
		tSendLocSerBtn = (ToggleButton) view.findViewById(R.id.toggleSendLocToSer);
		tphoneLoc  = (ToggleButton) view.findViewById(R.id.iv_phneLocServices);

		startSOSonPB = (CheckBox)view.findViewById(R.id.sos_on_pb);
		canSMS = (CheckBox) view.findViewById(R.id.cb_sms);
		canEmail = (CheckBox) view.findViewById(R.id.cb_email);
		canFBpost = (CheckBox) view.findViewById(R.id.cb_fbPost);
		etSosRecurrence = (EditText)view.findViewById(R.id.editTxt_default_sos_recurrence);

		if(prefs.getString(AppConstant.SOS_RECURRENCE_DURATION, "10")!=null)
			etSosRecurrence.setText(prefs.getString(AppConstant.SOS_RECURRENCE_DURATION, "10"));

		edit = prefs.edit();

		etSosRecurrence.addTextChangedListener(new TextWatcher() {

			@Override
			public void onTextChanged(CharSequence s, int start, int before, int count) {
				// TODO Auto-generated method stub
				edit.putString(AppConstant.SOS_RECURRENCE_DURATION, s.toString().trim());
				edit.commit();

			}

			@Override
			public void beforeTextChanged(CharSequence s, int start, int count,
					int after) {
				// TODO Auto-generated method stub

			}

			@Override
			public void afterTextChanged(Editable s) {
				// TODO Auto-generated method stub



			}
		});



		startSOSonPB.setOnClickListener(this);
		canSMS.setOnClickListener(this);
		canEmail.setOnClickListener(this);
		canFBpost.setOnClickListener(this);

		tv_locatonSer=(TextView)view.findViewById(R.id.tv_LocatonServicesSettings);
		tv_sendLocSer=(TextView)view.findViewById(R.id.tv_SendLocToSerSettings);
		tv_sendLocSer_txt =(TextView)view.findViewById(R.id.tv_sendLocToSer);

		tv_phoneLocSer=(TextView)view.findViewById(R.id.tv_PhoneLocServices);
		et_Country = (EditText) view.findViewById(R.id.editTxt_phoneNum); 
		et_Country.setOnClickListener(this);
		et_police = (EditText) view.findViewById(R.id.editTxt_policeNumber);
		et_ambulance =  (EditText) view.findViewById(R.id.editTxt_AmbulanceNumber);
		et_fire = (EditText) view.findViewById(R.id.editTxt_FireNumber);
		et_defaultCaller = (EditText) view.findViewById(R.id.editTxt_defaultCaller);
		rel_sendLocSer = (RelativeLayout) view.findViewById(R.id.Rel_sendLocToSer) ;
		callerListView =(ListView) view.findViewById(R.id.lv_callerList);
		et_defaultCaller.setOnClickListener(this);

		fbConnectedText = (TextView) view.findViewById(R.id.tv_fb_connected);
		fbGroupDescText = (TextView)view.findViewById(R.id.tv_fbGrpDesc);
		fbLogOutButton = (Button)view.findViewById(R.id.btn_fbLogout);
		fbLogOutButton.setOnClickListener(this);
		fbLogInButton = (Button) view.findViewById(R.id.btn_fbLogin);
		fbLogInButton.setOnClickListener(this);
		refreshButton = (ImageView) view.findViewById(R.id.iv_refershBox);
		this.manageFBConnectedStatus();
		refreshButton.setOnClickListener(this);
		fbGroupSpinner = (Spinner)view.findViewById(R.id.editTxt_secCode);
		fbGroupSpinner.setOnItemSelectedListener(this);

		groupNamesList = new ArrayList<String>();
		ArrayAdapter<String> dataAdapter = new ArrayAdapter<String>(this.getActivity(),
				android.R.layout.simple_spinner_item, groupNamesList);
		dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		fbGroupSpinner.setAdapter(dataAdapter);
		fbGroupSpinner.setSelection(0);

	}

	private void manageCallerList(){
		callerList = new ArrayList<MyBuddies>();
		callerList.clear();
		callerList.add(0, null);
		for(int i=1;i<=AppConstant.globalBuddies.size();i++){
			callerList.add(i, AppConstant.globalBuddies.get(i-1));
		}

	}

	private void manageFBConnectedStatus(){
		if(prefs.getString("str_token", null)!=null && !prefs.getString("str_token", null).equalsIgnoreCase("")){
			refreshButton.setEnabled(true);
			fbConnectedText.setVisibility(View.VISIBLE);
			fbGroupDescText.setVisibility(View.GONE);
			fbLogOutButton.setVisibility(View.VISIBLE);
			fbLogInButton.setVisibility(View.GONE);
		}else{
			refreshButton.setEnabled(false);
			fbLogInButton.setVisibility(View.VISIBLE);
			fbLogOutButton.setVisibility(View.GONE);
			fbConnectedText.setVisibility(View.GONE);
			fbGroupDescText.setVisibility(View.VISIBLE);
		}
	}

	private void checkPreference() {
		CountryCodes codes = null;
		if(AppConstant.userProfile!=null && AppConstant.userProfile.getCountryCode()!=null && !AppConstant.userProfile.getCountryCode().equalsIgnoreCase(""))
			codes =	AppConstant.getCountryCodes(this.getActivity(),AppConstant.userProfile.getCountryCode());
		else
			codes = AppConstant.getCountryCodes(this.getActivity(),AppConstant.getDialingCode(this.getActivity()));

		et_Country.setText("  "+codes.getCountryName()+" "+"("+codes.getISDCode()+")");
		if(!prefs.getBoolean(AppConstant.Settings_Lounched_firstTime, false))
		{

			et_ambulance.setText(codes.getAmbulance());
			et_fire.setText(codes.getFire());
			et_police.setText(codes.getPolice());


			SharedPreferences.Editor edit = prefs.edit();
			edit.putBoolean(AppConstant.Settings_Lounched_firstTime, true);
			edit.commit();
		}
		else
		{
			et_ambulance.setText(AppConstant.userProfile.getAmbulanceContact());
			et_fire.setText(AppConstant.userProfile.getFireContact());
			et_police.setText(codes.getPolice());
		}

		if(AppConstant.userProfile.isLocationConsent())
		{
			tv_locatonSer.setText("On");
			tlocSerBtn.setBackgroundResource(R.drawable.on);
		}
		else
		{
			tv_locatonSer.setText("Off");
			tlocSerBtn.setBackgroundResource(R.drawable.off);
		}

		if(AppConstant.userProfile.isPostLocationConsent())
		{
			tv_sendLocSer.setText("On");
			tSendLocSerBtn.setBackgroundResource(R.drawable.on);
		}
		else
		{
			tv_sendLocSer.setText("Off");
			tSendLocSerBtn.setBackgroundResource(R.drawable.off);
		}

		if(AppConstant.gpsTracker!=null && AppConstant.gpsTracker.checkGPSStatusOn())
		{
			tv_phoneLocSer.setText("On");
			tphoneLoc.setBackgroundResource(R.drawable.loc_on_image);
		}
		else
		{
			tv_phoneLocSer.setText("Off");
			tphoneLoc.setBackgroundResource(R.drawable.loc_off_image);
		}

		if(AppConstant.userProfile.isCanSMS())
			canSMS.setChecked(true);
		else
			canSMS.setChecked(false);

		if(AppConstant.userProfile.isCanMail())
			canEmail.setChecked(true);
		else
			canEmail.setChecked(false);

		if(AppConstant.userProfile.isCanPost())
			canFBpost.setChecked(true);
		else
			canFBpost.setChecked(false);

		if(prefs.getBoolean(AppConstant.isStartSOSOnPB, false)){
			startSOSonPB.setChecked(true);
		}else{
			startSOSonPB.setChecked(false);
		}


	}

	private void registerLocationConsentReceiver(){
		IntentFilter intentFilter = new IntentFilter(GpsChangeReceiver.ACTION_GPS_CHANGE_RECEIVER);
		reciever = new GpsChangeReceiver();
		this.getActivity().registerReceiver(reciever, intentFilter);
	}

	private void initGPSTracker() {
		if(AppConstant.gpsTracker == null)
			AppConstant.gpsTracker = new GPSTracker(getActivity().getApplicationContext());
	}

	private void checkDeviceLocSettings(){
		if(AppConstant.gpsTracker!=null && AppConstant.gpsTracker.checkGPSStatusOn())
		{
			tv_phoneLocSer.setText("On");
			tphoneLoc.setBackgroundResource(R.drawable.loc_on_image);
			if((AppConstant.gpsTracker!=null)&&!AppConstant.gpsTracker.checkGPSStatusOn())
				AppConstant.gpsTracker.startGPS();
		}
		else
		{
			tv_phoneLocSer.setText("Off");
			tphoneLoc.setBackgroundResource(R.drawable.loc_off_image);
			if((AppConstant.gpsTracker!=null)&&AppConstant.gpsTracker.checkGPSStatusOn())
				AppConstant.gpsTracker.stopUsingGPS();
		}
	}

	@Override
	public void onActivityResult(int requestCode, int resultCode, Intent data) {
		// TODO Auto-generated method stub
		super.onActivityResult(requestCode, resultCode, data);

	}
	
	public void setSelectedCountryCode(Intent data){
		if(data!=null)
		{
			CountryCodes codes = AppConstant.getCountryCodes(this.getActivity(),data.getStringExtra("Country Code")); 
			et_Country.setText("  "+codes.getCountryName()+" "+"("+codes.getISDCode()+")");
			AppConstant.setCountryCode(this.getActivity(), codes.getISDCode());
			AppConstant.userProfile.setCountryCode(codes.getISDCode());
		}
		
	}


	@Override
	public void onPause() {
		super.onPause();

		callerListView.setVisibility(View.GONE);

		DBQueries updateRecord = new DBQueries(getActivity().getApplicationContext());

		if(et_police!=null&&!et_police.getText().toString().equalsIgnoreCase(""))
		{
			AppConstant.userProfile.setPoliceContact(et_police.getText().toString());
			updateRecord.updateValue("PoliceContact", AppConstant.userProfile.getPoliceContact());
		}



		if(et_ambulance!=null&&!et_ambulance.getText().toString().equalsIgnoreCase(""))
		{
			AppConstant.userProfile.setAmbulanceContact(et_ambulance.getText().toString());
			updateRecord.updateValue("AmbulanceContact", AppConstant.userProfile.getAmbulanceContact());
		}


		if(et_fire!=null&&!et_fire.getText().toString().equalsIgnoreCase(""))
		{
			AppConstant.userProfile.setFireContact(et_fire.getText().toString());
			updateRecord.updateValue("FireContact", AppConstant.userProfile.getFireContact());
		}



	}
	@Override
	public void onClick(View v) {
		DBQueries updateRecord = new DBQueries(getActivity().getApplicationContext());
		switch (v.getId()) {
		case R.id.sos_on_pb :
			SharedPreferences.Editor editorTrack = prefs.edit();
			if(startSOSonPB.isChecked()){
				if(!AppConstant.isMyServiceRunning(this.getActivity(),SMSService.class))
					this.getActivity().startService(new Intent(this.getActivity(), SMSService.class));  
				editorTrack.putBoolean(AppConstant.isStartSOSOnPB, true);
				editorTrack.commit();

			}else{
				if(AppConstant.isMyServiceRunning(this.getActivity(),SMSService.class))
					this.getActivity().stopService(new Intent(this.getActivity(), SMSService.class)); 
				editorTrack.putBoolean(AppConstant.isStartSOSOnPB, false);
				editorTrack.commit();
			}
		case R.id.cb_sms:

			if(canSMS.isChecked())
			{
				AppConstant.userProfile.setCanSMS(true);
				updateRecord.updateValue("CanSMS", AppConstant.userProfile.isCanSMS());
				AppConstant.sendDistressText(getActivity()) ;
			}
			else
			{
				AppConstant.userProfile.setCanSMS(false);
				updateRecord.updateValue("CanSMS", AppConstant.userProfile.isCanSMS());
			}

			break;
		case R.id.cb_email:

			if(canEmail.isChecked())
			{
				AppConstant.userProfile.setCanMail(true);
				updateRecord.updateValue("CanEmail", AppConstant.userProfile.isCanMail());
			}
			else
			{
				AppConstant.userProfile.setCanMail(false);
				updateRecord.updateValue("CanEmail", AppConstant.userProfile.isCanMail());
			}
			break;
		case R.id.cb_fbPost:
			if(canFBpost.isChecked())
			{
				AppConstant.userProfile.setCanPost(true);
				updateRecord.updateValue("CanFBPost", AppConstant.userProfile.isCanPost());
				AppConstant.sendDistressText(getActivity());
			}
			else
			{
				AppConstant.userProfile.setCanPost(false);
				updateRecord.updateValue("CanFBPost", AppConstant.userProfile.isCanPost());
			}
			break;

		case R.id.btn_fbLogin:
			this.startActivityForResult(new Intent(this.getActivity() , FaceBookLoginActivity.class), 2);
			break;

		case R.id.btn_fbLogout :
			this.resetFacebookDetails();
			break;
		case R.id.editTxt_defaultCaller:

			if(AppConstant.globalBuddies.size()>0)
			{
				this.manageCallerList();
				callerListView.setVisibility(View.VISIBLE);
				CallerListAdapter callListAdpt= new CallerListAdapter(getActivity(),callerList);
				callerListView.setAdapter(callListAdpt);
			}

			break;
		case R.id.editTxt_phoneNum:
			//startActivityForResult(new Intent(this.getActivity(), CountryListActivity.class),AppConstant.GET_COUNTRY_CODE_ACTIVITY_RESULT);
			break;

		case R.id.iv_refershBox:
			this.refreshFaceBookGroupsList();
			break;
		default:
			break;
		}
	}

	private void validateAppSignatureForPackage() {

		try {
			PackageInfo info = getActivity().getPackageManager().getPackageInfo(
					"com.guardian", 
					PackageManager.GET_SIGNATURES);
			for (Signature signature : info.signatures) {
				MessageDigest md = MessageDigest.getInstance("SHA");
				md.update(signature.toByteArray());
				Log.d("KeyHash:", Base64.encodeToString(md.digest(), Base64.DEFAULT));
			}
		} catch (NameNotFoundException e) {

		} catch (NoSuchAlgorithmException e) {

		}
	}

	public void refreshFaceBookGroupsList(){
		MakeHTTPServices httpServices = new MakeHTTPServices(getActivity(),null);
		((BaseActivity)getActivity()).showProgressDialog("Loading Facebook Groups. Please wait...");
		httpServices.getListOfFBGroups(AppConstant.strGetGroupsUrl+prefs.getString("str_token", null));
	}

	public void resetFacebookDetails(){
		SharedPreferences.Editor editor = prefs.edit();
		editor.putString("FBAccessToken", "");
		editor.commit();
		editor.putString("str_token", "");
		editor.commit();

		refreshButton.setEnabled(false);
		fbConnectedText.setVisibility(View.GONE);
		fbLogOutButton.setVisibility(View.GONE);
		fbGroupDescText.setVisibility(View.VISIBLE);
		fbLogInButton.setVisibility(View.VISIBLE);
		fbGroupSpinner.setAdapter(null);
	}

	public void loadRefreshedFBGroupsList(String result){
		((BaseActivity)getActivity()).dismissProgressDialog();
		try {
			Vector<GroupDO> veGroupDOs = new Vector<GroupDO>();
			JSONObject jsonObjectMain = new JSONObject(result);
			String strFbId = jsonObjectMain.getString("id");
			JSONArray jsonArray = jsonObjectMain.optJSONObject("groups").optJSONArray("data");
			for (int i = 0; i < jsonArray.length(); i++) 
			{
				GroupDO groupDO = new GroupDO();
				JSONObject jsonObject = (JSONObject) jsonArray.get(i);
				groupDO.setStrId(jsonObject.getString("id"));
				groupDO.setStrName(jsonObject.getString("name"));

				JSONObject jObjectOwner = jsonObject.getJSONObject("owner");
				if(TextUtils.equals(strFbId,jObjectOwner.getString("id")))
				{
					veGroupDOs.add(groupDO);
				}
			}
			groupIDNameMap= new HashMap<String, String>();
			groupNamesList.clear();
			for(int i=0; i<veGroupDOs.size();i++){
				groupNamesList.add(veGroupDOs.get(i).getStrName());
				groupIDNameMap.put(veGroupDOs.get(i).getStrName(), veGroupDOs.get(i).getStrId());
			}
			ArrayAdapter<String> dataAdapter = new ArrayAdapter<String>(this.getActivity(),
					android.R.layout.simple_spinner_item, groupNamesList);
			dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
			fbGroupSpinner.setAdapter(dataAdapter);
			fbGroupSpinner.setSelection(0);



		} catch (JSONException je) {
			// TODO Auto-generated catch block
			LogUtils.LOGE(LogUtils.makeLogTag(FaceBookLoginActivity.class), je.getLocalizedMessage());
		} catch (Exception e) {
			// TODO: handle exception
			LogUtils.LOGE(LogUtils.makeLogTag(FaceBookLoginActivity.class), e.getLocalizedMessage());
		}
	}

	public void onFaceBookGroupsListResult(int requestCode, int resultCode, Intent data){
		refreshButton.setEnabled(true);
		fbLogInButton.setVisibility(View.GONE);
		fbConnectedText.setVisibility(View.VISIBLE);
		fbGroupDescText.setVisibility(View.GONE);
		fbLogOutButton.setVisibility(View.VISIBLE);
		LogUtils.LOGE(LogUtils.makeLogTag(PreferencesFragment.class), "data===="+data.toString());
		ArrayList<GroupDO> groupDOList = (ArrayList<GroupDO>)data.getSerializableExtra("fb_groups_list");
		groupIDNameMap= new HashMap<String, String>();
		groupNamesList.clear();
		for(int i=0; i<groupDOList.size();i++){
			groupNamesList.add(groupDOList.get(i).getStrName());
			groupIDNameMap.put(groupDOList.get(i).getStrName(), groupDOList.get(i).getStrId());
		}
		ArrayAdapter<String> dataAdapter = new ArrayAdapter<String>(this.getActivity(),
				android.R.layout.simple_spinner_item, groupNamesList);
		dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		fbGroupSpinner.setAdapter(dataAdapter);
		fbGroupSpinner.setSelection(0);

	}
	@Override
	public void onItemSelected(AdapterView<?> parent, View view, int position,
			long id) {
		// TODO Auto-generated method stub
		AppConstant.selectedFBGroup = parent.getItemAtPosition(position).toString();
		SharedPreferences.Editor edit = prefs.edit();
		edit.putString(AppConstant.FB_Selected_Group, groupIDNameMap.get(AppConstant.selectedFBGroup.trim()));
		edit.commit();

	}
	@Override
	public void onNothingSelected(AdapterView<?> parent) {
		// TODO Auto-generated method stub



	}


	@Override
	public void onScrollChanged(ScrollViewExt scrollView, int x, int y,
			int oldx, int oldy) {
		// TODO Auto-generated method stub
		View view = (View) scrollView.getChildAt(scrollView.getChildCount() - 1);
		int diff = (view.getBottom() - (scrollView.getHeight() + scrollView.getScrollY()));

		// if diff is zero, then the bottom has been reached
		if ((diff <= scrollView.getHeight()/2) && !isFBGroupsListRefreshed && (prefs.getString("str_token", null)!=null) &&  !(prefs.getString("str_token", null).equalsIgnoreCase(""))){
			// do stuff
			isFBGroupsListRefreshed = true;
			this.refreshFaceBookGroupsList();
		}

	}

}
