package com.guardianapp.ui;

import org.json.JSONException;

import android.annotation.SuppressLint;
import android.app.AlertDialog;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager.NameNotFoundException;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.view.ViewPager;
import android.support.v4.view.ViewPager.OnPageChangeListener;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.guardianapp.R;
import com.guardianapp.adapters.SettingsFragmentStatePageAdapter;
import com.guardianapp.helpercomponents.VersionCheckReceiver;
import com.guardianapp.model.ProfileList;
import com.guardianapp.model.VersionUpdate;
import com.guardianapp.services.LiveAccount;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.LogUtils;
import com.guardianapp.utilities.VersionComparator;
import com.guardianapp.webservicecomponents.MakeHTTPServices;
import com.guardianapp.webservicecomponents.OnTaskCompleted;

@SuppressLint("NewApi") 
public class SettingsActivity extends BaseActivity implements OnTaskCompleted{

	private String[] tabs = { "Profile", "Buddies", "Groups", "Preferences" };
	private ViewPager mViewPager;
	private SettingsFragmentStatePageAdapter pagerAdapter;
	private SharedPreferences prefs_LiveAttributes;
	private int versionCode = 0;
	private VersionCheckReceiver vReceiver;

	@SuppressLint("NewApi") @Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		getSupportActionBar().setTitle("SETTINGS"); 
		setContentView(R.layout.settings_screen);
	}

	@Override
	protected void initFields() {
		// TODO Auto-generated method stub
		AppConstant.localUserName=AppConstant.user.getName();
		prefs_LiveAttributes = this.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		
		mViewPager = (ViewPager) findViewById(R.id.settingsFragmentLayout);
		pagerAdapter = new SettingsFragmentStatePageAdapter(getSupportFragmentManager(),tabs);
		mViewPager.setAdapter(pagerAdapter);
		mViewPager.setOffscreenPageLimit(pagerAdapter.getCount());
		
		if(this.getIntent().getStringExtra("GPS_SETTINGS")!= null && this.getIntent().getStringExtra("GPS_SETTINGS").equalsIgnoreCase("gpsSettings")){
			mViewPager.setCurrentItem(3);
		}
		if(this.getIntent().getBooleanExtra("open_buddy_fragment", false))
			mViewPager.setCurrentItem(1);
		if(this.getIntent().getBooleanExtra("open_group_fragment", false))
			mViewPager.setCurrentItem(2);
		
	if(prefs_LiveAttributes.getBoolean(AppConstant.isProfileDone, false)){
		this.registerDownloadReceivers();
		this.getPendingUpdatesFromServer();
		}
	}

	@Override
	protected void initCallBacks() {
		// TODO Auto-generated method stub
		mViewPager.setOnPageChangeListener(new OnPageChangeListener() {

			@Override
			public void onPageSelected(int position) {
				// TODO Auto-generated method stub
				switch(position){

				case 0 : if((prefs_LiveAttributes.getBoolean(AppConstant.isProfileDone, false)) && (AppConstant.isScreenRefreshRequired))
					pagerAdapter.notifyDataSetChanged();
				break;

				case 1 : if((prefs_LiveAttributes.getBoolean(AppConstant.isProfileDone, false)) && (AppConstant.isScreenRefreshRequired))
					pagerAdapter.notifyDataSetChanged();
				break;

				case 2: if((prefs_LiveAttributes.getBoolean(AppConstant.isProfileDone, false)) && (AppConstant.isScreenRefreshRequired))
					pagerAdapter.notifyDataSetChanged();
				break;

				}

			}

			@Override
			public void onPageScrolled(int arg0, float arg1, int arg2) {
				// TODO Auto-generated method stub

			}

			@Override
			public void onPageScrollStateChanged(int arg0) {
				// TODO Auto-generated method stub

			}
		});
	}

	@Override
	public void onBackPressed() {
		Fragment currentFragment = (Fragment) mViewPager.getAdapter().instantiateItem(mViewPager, mViewPager.getCurrentItem());
		SharedPreferences prefs_LiveAttributes;
		prefs_LiveAttributes = this.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

		if((prefs_LiveAttributes.getBoolean(AppConstant.isProfileDone, false)) || (AppConstant.globalBuddies!=null&& AppConstant.globalBuddies.size()>0)){
			if(prefs_LiveAttributes.getBoolean(AppConstant.isProfileDone, false) && !AppConstant.userProfile.isIsDataSynced())
			{

				this.showConfirmationDialog("Information",this.getString(R.string.updated_info_msg),AppConstant.UPDATE_PROFILE_SERVICE_TAG);
			}else{
				super.onBackPressed();	
			}}
		else{
			Toast.makeText(this, "Please login using your Microsoft Account / add buddies", Toast.LENGTH_LONG).show();
		}

	}



	@Override
	protected void onDestroy() {
		// TODO Auto-generated method stub
		try{
		this.unregisterReceiver(vReceiver);
		}catch(IllegalArgumentException iae){
			LogUtils.LOGE(LogUtils.makeLogTag(SettingsActivity.class), "Trying to unregistering receiver , without registering it...bypassing...");
		}
		super.onDestroy();
	}

	private void invokeUpdateProfileService(){
		MakeHTTPServices httpServices = new MakeHTTPServices(this,null);
		try {
			httpServices.updateProfileService();
		} catch (JSONException jse) {	
			AppConstant.dismissProgressDialog();
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), jse.getLocalizedMessage());
		} catch (Exception ex){
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), ex.getLocalizedMessage());
		}
	}

	private void invokeUnregisterProfileService(){
		MakeHTTPServices httpServices = new MakeHTTPServices(this,null);
		try {
			httpServices.UnRegisterUser();
		} catch (Exception ex){
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), ex.getLocalizedMessage());
		}
	}

	private void getPendingUpdatesFromServer(){
		MakeHTTPServices httpServices = new MakeHTTPServices(this,null);
		try {
			httpServices.checkPendingUpdatesFromServer();
		} catch (Exception ex){
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), ex.getLocalizedMessage());
		}
	}

	private void registerDownloadReceivers(){
		IntentFilter filter = new IntentFilter(VersionCheckReceiver.PROCESS_RESPONSE);
		filter.addCategory(Intent.CATEGORY_DEFAULT);
		vReceiver = new VersionCheckReceiver();
		registerReceiver(vReceiver, filter);
	}

	@Override
	public void onTaskComplete(String result) {
		AppConstant.dismissProgressDialog();
		Fragment currentFragment = (Fragment) mViewPager.getAdapter().instantiateItem(mViewPager, mViewPager.getCurrentItem());
		if(AppConstant.service_Tag == AppConstant.UPDATE_PROFILE_SERVICE_TAG)
			finish();
		else if(AppConstant.service_Tag == AppConstant.UNREGISTER_SERVICE_TAG){
			Intent a = new Intent(this,HomeScreenActivity.class);
	        a.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
	        startActivity(a);
		}else if(AppConstant.service_Tag == AppConstant.FB_GROUPS_SERVICE_TAG){
			((PreferencesFragment)currentFragment).loadRefreshedFBGroupsList(result);
		}

	}


	@Override
	public void onGetObjectResult(Object obj) {
		AppConstant.dismissProgressDialog();
		if(AppConstant.service_Tag == AppConstant.UPDATE_PROFILE_SERVICE_TAG){
			finish();
		}else if(AppConstant.service_Tag == AppConstant.CHECK_UPDATES_FROM_SERVER_TAG){
			this.checkForPendingUpdatesFromServer((VersionUpdate)obj);
		}else if(AppConstant.service_Tag == AppConstant.UNREGISTER_SERVICE_TAG){
			Fragment currentFragment = (Fragment) mViewPager.getAdapter().instantiateItem(mViewPager, mViewPager.getCurrentItem());
			Intent a = new Intent(this,HomeScreenActivity.class);
	        a.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
	        startActivity(a);
		}

	}


	@Override
	public boolean onCreateOptionsMenu(Menu menu) {

		super.onCreateOptionsMenu(menu);
		getMenuInflater().inflate(R.menu.settings_menu, menu);
		SharedPreferences prefs_LiveAttributes = getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

		MenuItem menuItem= menu.findItem(R.id.unregisterSettings);


		if(!prefs_LiveAttributes.getBoolean(AppConstant.isProfileDone, false))
			menuItem.setTitle("Register");
		else
			menuItem.setTitle("Unregister");

		return true;

	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {

		switch (item.getItemId()) {

		case R.id.unregisterSettings:
			SharedPreferences prefs_LiveAttributes = getSharedPreferences(
					AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

			if(!prefs_LiveAttributes.getBoolean(AppConstant.isProfileDone, false))
			{

				LiveAccount guardianLive = new LiveAccount(SettingsActivity.this,null);
				guardianLive.connectToLiveAccount();
			}
			else
			{
				this.showConfirmationDialog("Confirmation!",this.getString(R.string.unregister_info_msg), AppConstant.UNREGISTER_SERVICE_TAG);
			}
			break;

		default:
			break;
		}
		return super.onOptionsItemSelected(item);
	}

	private void showConfirmationDialog(String dialogTitle, String message, final int source) {

		View view = this.getLayoutInflater().inflate(R.layout.common_dialog, null);
		final AlertDialog callMsgDialog = new AlertDialog.Builder(this)
		.setView(view).create();

		TextView title = (TextView) view.findViewById(R.id.tv_title);	
		title.setText(dialogTitle);

		TextView msgTxt = (TextView) view.findViewById(R.id.tv_msgAleart);	
		msgTxt.setText(message);
		Button callbtn = (Button) view.findViewById(R.id.btnOK);
		callbtn.setText("ok");

		Button cancelBtn = (Button) view.findViewById(R.id.btnCancel);
		cancelBtn.setText("cancel");
		view.findViewById(R.id.btnOK).setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View arg0) {
				callMsgDialog.dismiss();
				switch(source){
				case AppConstant.UPDATE_PROFILE_SERVICE_TAG :
					SettingsActivity.this.invokeUpdateProfileService();
					break;
				case AppConstant.UNREGISTER_SERVICE_TAG:
					SettingsActivity.this.invokeUnregisterProfileService();
					break;
				}

			}
		});

		view.findViewById(R.id.btnCancel).setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View arg0) {
				callMsgDialog.dismiss();
				switch(source){
				case AppConstant.UPDATE_PROFILE_SERVICE_TAG :
					finish();
					break;
				case AppConstant.UNREGISTER_SERVICE_TAG:
					break;
				}

			}
		});
		callMsgDialog.show();
	}

	public void checkForPendingUpdatesFromServer(VersionUpdate versionObj){
		PackageInfo pInfo = null;
		try {
			pInfo = getPackageManager().getPackageInfo(getPackageName(), 0);
		}
		catch (NameNotFoundException e) {
			LogUtils.LOGE(LogUtils.makeLogTag(SettingsActivity.class), e.getLocalizedMessage());
		}
		
		if(!versionObj.isIsProfileActive()){
			Toast.makeText(this, this.getString(R.string.profile_invalid), Toast.LENGTH_LONG).show();
			AppConstant.userProfile.setMobileNumber("+0000000000");
		}
		
		VersionComparator serverVersion = new VersionComparator(versionObj.getServerVersion());
		VersionComparator appVersion = new VersionComparator(pInfo.versionName);

		if(AppConstant.intToBoolean(appVersion.compareTo(serverVersion))){
			Intent broadcastIntent = new Intent();
			broadcastIntent.setAction(VersionCheckReceiver.PROCESS_RESPONSE);
			broadcastIntent.addCategory(Intent.CATEGORY_DEFAULT);
			broadcastIntent.putExtra(VersionCheckReceiver.LATEST_VERSION, versionObj.getServerVersion());
			sendBroadcast(broadcastIntent);
		}
	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		// TODO Auto-generated method stub
		Fragment currentFragment = (Fragment) mViewPager.getAdapter().instantiateItem(mViewPager, mViewPager.getCurrentItem());
		if(resultCode==AppConstant.CREATE_PROFILE_ACTIVITY_RESULT)
		{
			if(currentFragment instanceof ProfileFragment){
				((ProfileFragment)currentFragment).updateUI();
				((ProfileFragment)currentFragment).setUpdateMobileNumber();
			}

		}else if(resultCode==AppConstant.UNREGISTER_SERVICE_TAG){
			if(currentFragment instanceof ProfileFragment)
				((ProfileFragment)currentFragment).updateUI();
		}else if(resultCode==AppConstant.GET_COUNTRY_CODE_ACTIVITY_RESULT)
		{
			if(currentFragment instanceof PreferencesFragment)
				((PreferencesFragment)currentFragment).setSelectedCountryCode(data);
		}
		else if(resultCode == 2){ 		

			((PreferencesFragment)currentFragment).onFaceBookGroupsListResult(requestCode, resultCode, data);
		}
	}


}
