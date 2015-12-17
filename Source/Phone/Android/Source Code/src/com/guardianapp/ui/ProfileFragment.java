package com.guardianapp.ui;

import org.json.JSONException;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Typeface;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.guardianapp.R;
import com.guardianapp.model.Profile;
import com.guardianapp.services.LiveAccount;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.webservicecomponents.MakeHTTPServices;
import com.guardianapp.webservicecomponents.OnTaskCompleted;




@SuppressLint("NewApi") 
public class ProfileFragment extends Fragment implements OnClickListener, OnTaskCompleted{
	public SharedPreferences prefs_LiveAttributes;
	private TextView syncDate;
	private Button connectToMsLive;
	EditText et_phoneNo;

	public View onCreateView(LayoutInflater inflater, ViewGroup container, 
			Bundle savedInstanceState){
		View view = null;
		prefs_LiveAttributes = this.getActivity().getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

		if(prefs_LiveAttributes.getBoolean(AppConstant.isLiveRagister, false))
		{
			if(prefs_LiveAttributes.getBoolean(AppConstant.isProfileDone, false))
			{
				view = this.initSettingsProfileFragment(inflater,container);
			}
			else{
				Intent intent = new Intent(this.getActivity(),CreateProfileActivity.class);
				startActivityForResult(intent,AppConstant.CREATE_PROFILE_ACTIVITY_RESULT);

			}
		}else
		{
			view = this.initCreateProfileScreen(inflater, container);
		}
		return view;
	}

	private View initCreateProfileScreen(LayoutInflater inflater , ViewGroup container){
		View view = inflater.inflate(R.layout.create_profie_fragment, container, false);
		TextView profiletext = (TextView) view.findViewById(R.id.tv_createProfHeadding);
		profiletext.setText(getText(R.string.createProf_heading));
		TextView link = (TextView) view.findViewById(R.id.hyperLink);
		link.setOnClickListener(this);
		connectToMsLive = (Button) view.findViewById(R.id.btn_connectToMSLive);
		Typeface face=Typeface.createFromAsset(getActivity().getAssets(), "SEGOEUI.TTF");
		connectToMsLive.setTypeface(face);
		connectToMsLive.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				LiveAccount guardianLive = new LiveAccount(getActivity(),ProfileFragment.this);
				guardianLive.connectToLiveAccount();
			}
		});
		return view;

	}


	private View initSettingsProfileFragment(LayoutInflater inflater , ViewGroup container){
		View view = inflater.inflate(R.layout.settings_profile_fragment, container, false);
		EditText et_liveName = (EditText) view.findViewById(R.id.et_name);
		EditText et_liveId = (EditText) view.findViewById(R.id.et_loggedInLiveAccount);
		TextView syncDate = (TextView) view.findViewById(R.id.tv_lastDate);
		et_phoneNo = (EditText) view.findViewById(R.id.et_phoneNum);
		et_liveId.setText(prefs_LiveAttributes.getString(AppConstant.LiveEmail_id, ""));
		et_liveName.setText(prefs_LiveAttributes.getString(AppConstant.Live_user_name, ""));
		et_phoneNo.setText(prefs_LiveAttributes.getString(AppConstant.Ragister_mobile, ""));
		syncDate.setText(prefs_LiveAttributes.getString(AppConstant.Last_profileSyncTime, ""));

		view.findViewById(R.id.iv_edit).setOnClickListener(this);
		view.findViewById(R.id.iv_synced).setOnClickListener(this);

		et_liveName.addTextChangedListener(new TextWatcher() {
			public void onTextChanged(CharSequence s, int start, int before,
					int count) {
				if(!s.equals("") )
				{ 
					AppConstant.localUserName=s.toString();
					if(!AppConstant.localUserName.equalsIgnoreCase(AppConstant.user.getName())) {
						AppConstant.UpdateIsDataSynced(getActivity(), false);
					}
				}

			}

			@Override
			public void afterTextChanged(Editable arg0) {
				// TODO Auto-generated method stub

			}

			@Override
			public void beforeTextChanged(CharSequence arg0,
					int arg1, int arg2, int arg3) {
				// TODO Auto-generated method stub

			}
		});
		return view;
	}

	@Override
	public void onResume() {
		// TODO Auto-generated method stub
		super.onResume();
		if(prefs_LiveAttributes.getBoolean(AppConstant.isProfileDone, false))
		{
			if(et_phoneNo!=null)
				et_phoneNo.setText(prefs_LiveAttributes.getString(AppConstant.Ragister_mobile, ""));
		}


	}

	@Override
	public void onClick(View v){
		MakeHTTPServices httpServices = new MakeHTTPServices(getActivity(),ProfileFragment.this);
		switch (v.getId()) {

		case R.id.iv_synced:

			try {

				httpServices.updateProfileService();
			} catch (JSONException e) {	
				AppConstant.dismissProgressDialog();
				e.printStackTrace();
			}

			break;
		case R.id.iv_edit:
			AppConstant.isNumberISUpdate =true;
			startActivityForResult(new Intent(this.getActivity(), CreateProfileActivity.class),AppConstant.CREATE_PROFILE_ACTIVITY_RESULT);
			break;

		case R.id.hyperLink:
			Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse(AppConstant.guardianPortalLink));
			startActivity(browserIntent);
			break;
		default:
			break;
		}

	}

	@Override
	public void onActivityResult(int requestCode, int resultCode,
			Intent data) {

		if(requestCode==AppConstant.CREATE_PROFILE_ACTIVITY_RESULT
				||requestCode==AppConstant.UNREGISTER_SERVICE_TAG)
		{
			if(prefs_LiveAttributes.getBoolean(AppConstant.isProfileDone, false))
			{
				if(et_phoneNo!=null)
					et_phoneNo.setText(prefs_LiveAttributes.getString(AppConstant.Ragister_mobile, ""));
			}

			updateUI();
		}


	}

	public void setUpdateMobileNumber(){
		if(et_phoneNo!=null)
			et_phoneNo.setText(prefs_LiveAttributes.getString(AppConstant.Ragister_mobile, ""));
	}


	public void updateUI() {
		Fragment currentFragment = ProfileFragment.this;
		FragmentTransaction fragTransaction = getFragmentManager().beginTransaction();
		fragTransaction.detach(currentFragment);
		fragTransaction.attach(currentFragment);
		fragTransaction.commitAllowingStateLoss();
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB) {
			getActivity().invalidateOptionsMenu(); 
		}else{
			getActivity().supportInvalidateOptionsMenu();
		}
	}
	@Override
	public void onTaskComplete(String result) {
		AppConstant.dismissProgressDialog();
		if(prefs_LiveAttributes.getBoolean(AppConstant.isProfileDone, false))
		{
			if(syncDate!=null)
				syncDate.setText(prefs_LiveAttributes.getString(AppConstant.Last_profileSyncTime, ""));
			if(et_phoneNo!=null)
				et_phoneNo.setText(prefs_LiveAttributes.getString(AppConstant.Ragister_mobile, ""));
		}

		updateUI();
	}


	@Override
	public void onGetObjectResult(Object obj) {
		AppConstant.dismissProgressDialog();
		
		if(obj instanceof NumberFormatException){
			AppConstant.reloadServiceURLs();
			/*MakeHTTPServices httpServices = new MakeHTTPServices(this.getActivity(),ProfileFragment.this);
			httpServices.membershipServiceUrl();*/
			LiveAccount guardianLive = new LiveAccount(getActivity(),ProfileFragment.this);
			guardianLive.connectToLiveAccount();
		}
		
		if(prefs_LiveAttributes.getBoolean(AppConstant.isProfileDone, false))
		{
			if(syncDate!=null)
				syncDate.setText(prefs_LiveAttributes.getString(AppConstant.Last_profileSyncTime, ""));
			if(et_phoneNo!=null)
				et_phoneNo.setText(prefs_LiveAttributes.getString(AppConstant.Ragister_mobile, ""));
		}

		if(AppConstant.service_Tag == AppConstant.CREATE_PROFILE_SERVICE_TAG){
			Profile profObj = (Profile)obj;
			if(profObj.getProfileID()!= null && profObj.getProfileID()!=0){
				updateUI();
			}	
		}else{
			updateUI();
		}



	}

}
