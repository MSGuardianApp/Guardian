package com.guardianapp.ui;

import java.util.Timer;
import java.util.TimerTask;

import org.json.JSONException;

import android.app.AlertDialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Typeface;
import android.os.Bundle;
import android.text.InputType;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.inputmethod.InputMethodManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.guardianapp.R;
import com.guardianapp.database.DBQueries;
import com.guardianapp.model.CountryCodes;
import com.guardianapp.model.Profile;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.webservicecomponents.MakeHTTPServices;
import com.guardianapp.webservicecomponents.OnTaskCompleted;

public class CreateProfileActivity extends BaseActivity implements OnClickListener, OnTaskCompleted{
	private EditText et_phoneNo,et_liveId,et_liveName,et_countryCode,et_secCode;
	private TextView createProfileTV;
	public SharedPreferences pref_AuthToken;
	public Button phValidate,codeConfirm;
	private boolean isCountryCodeUpdate = false;
	private DBQueries mQuery;


	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.ragister_profie_fragment);
	}


	@Override
	protected void initFields() {
		// TODO Auto-generated method stub
		pref_AuthToken = getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		phValidate = (Button) findViewById(R.id.btn_validate);
		codeConfirm= (Button)findViewById(R.id.btn_Confirm);
		et_liveName = (EditText) findViewById(R.id.editTxt_name);
		et_liveId = (EditText)findViewById(R.id.editTxt_loggedInLiveAccount);
		et_phoneNo = (EditText) findViewById(R.id.editTxt_phoneNum);
		et_countryCode = (EditText)findViewById(R.id.editTxt_Countcode);
		et_secCode  = (EditText)findViewById(R.id.editTxt_secCode);
		et_secCode.setInputType(InputType.TYPE_CLASS_NUMBER);
		createProfileTV = (TextView)findViewById(R.id.tv_createProfile);
		mQuery = new DBQueries(this);


		if(AppConstant.isNumberISUpdate){
			createProfileTV.setText(this.getString(R.string.update_profile));
		}else{
			createProfileTV.setText(this.getString(R.string.create_profile));
		}

		String str =  pref_AuthToken.getString(AppConstant.Ragister_mobile, "");
		String mobNo = str.replace( pref_AuthToken.getString(AppConstant.Ragister_countryCode, ""),"");
		et_phoneNo.setText(mobNo);

		Typeface face=Typeface.createFromAsset(this.getAssets(), "SEGOEUI.TTF");
		phValidate.setTypeface(face);
		codeConfirm.setTypeface(face);

		et_liveId.setText(pref_AuthToken.getString(AppConstant.LiveEmail_id, ""));
		et_liveName.setText(pref_AuthToken.getString(AppConstant.Live_user_name, ""));
		if(AppConstant.userProfile!=null && AppConstant.userProfile.getCountryCode()!=null && AppConstant.userProfile.getCountryCode().equalsIgnoreCase(""))
			et_countryCode.setText(AppConstant.userProfile.getCountryCode());
		else
			et_countryCode.setText(AppConstant.getDialingCode(this));
	}

	@Override
	public void onBackPressed() {
		// TODO Auto-generated method stub
		if(AppConstant.isNumberISUpdate)
			super.onBackPressed();
	}

	@Override
	protected void initCallBacks() {
		// TODO Auto-generated method stub
		et_countryCode.setOnClickListener(this);
		phValidate.setOnClickListener(this);
		codeConfirm.setOnClickListener(this);
	}


	@Override
	public void onClick(View v) {
		switch (v.getId()) {
		case R.id.btn_validate:
			MakeHTTPServices httpServices = new MakeHTTPServices(this,null);
			try {
				String Mno = et_countryCode.getText().toString()+et_phoneNo.getText().toString();
				if (et_phoneNo != null) {
					InputMethodManager imm = (InputMethodManager)this.getSystemService(Context.INPUT_METHOD_SERVICE);
					imm.hideSoftInputFromWindow(et_phoneNo.getWindowToken(), 0);
				}

				if(!AppConstant.isValidPhoneNumber(et_countryCode.getText().toString().trim()+et_phoneNo.getText().toString().trim())||et_phoneNo.getText().toString().equalsIgnoreCase(""))
				{

					Toast.makeText(this, "Please provide a valid mobile number", Toast.LENGTH_LONG).show();
				}
				else
				{ 
					phValidate.setFocusable(false);
					httpServices.validateMobileNumberSyrice(et_liveName.getText().toString(),
							et_countryCode.getText().toString(), et_phoneNo.getText().toString());

				}


			} catch (JSONException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			break;

		case R.id.btn_Confirm:
			if(this.isCountryCodeUpdate){
				showConfirmationDialog("confirm", this.getString(R.string.country_code_changed));
			}else{
				this.createProileForUser();
			}
			break;
		case R.id.editTxt_Countcode:
			startActivityForResult(new Intent(this, CountryListActivity.class),AppConstant.GET_COUNTRY_CODE_ACTIVITY_RESULT);
			break;
		default:
			break;
		}

	}

	private void createProileForUser(){
		MakeHTTPServices httpServices = new MakeHTTPServices(this,null);
		try {
			if(et_secCode.getText().toString().length()>0){
				SharedPreferences.Editor edit = pref_AuthToken.edit();
				edit.putString(AppConstant.Ragister_mobile, et_countryCode.getText().toString()+ et_phoneNo.getText().toString() );
				edit.putString(AppConstant.Ragister_countryCode, et_countryCode.getText().toString());
				edit.commit();
				AppConstant.setCountryCode(this,et_countryCode.getText().toString());
				this.setEmergencyContactNumbers(et_countryCode.getText().toString());
				if(this.isCountryCodeUpdate){
					AppConstant.isScreenRefreshRequired = true;
					mQuery.markAllBuddiesAsDelete();
				}
				httpServices.createProfileServices(et_liveName.getText().toString(), 
						et_countryCode.getText().toString(),  et_phoneNo.getText().toString(), et_secCode.getText().toString());
			}else{
				Toast.makeText(this, this.getString(R.string.provide_sec_code), Toast.LENGTH_LONG).show();
			}


		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}


	}

	private void enableConfirmButton(){
		Timer buttonTimer = new Timer();
		buttonTimer.schedule(new TimerTask() {

			@Override
			public void run() {
				runOnUiThread(new Runnable() {

					@Override
					public void run() {
						codeConfirm.setEnabled(true);
					}
				});
			}
		}, 1000);
	}



	@Override
	public void onTaskComplete(String result) {

		AppConstant.dismissProgressDialog();

		phValidate.setFocusable(true);

		if((AppConstant.service_Tag==AppConstant.CREATE_PROFILE_SERVICE_TAG)&&
				AppConstant.isNumberISUpdate&&(pref_AuthToken.getBoolean(AppConstant.isProfileDone, false)))
		{
			AppConstant.isNumberISUpdate = false;
			Intent resInt = new Intent();
			setResult(AppConstant.CREATE_PROFILE_ACTIVITY_RESULT, resInt);
			finish();
		}

	}

	@Override
	public void onGetObjectResult(Object obj) {
		AppConstant.dismissProgressDialog();
		Profile profileObj = (Profile)obj;
		phValidate.setFocusable(true);
		if((AppConstant.service_Tag==AppConstant.CREATE_PROFILE_SERVICE_TAG)
				&&(pref_AuthToken.getBoolean(AppConstant.isProfileDone, false))&& (profileObj.getDataInfo().get(0).getResultType()!=3 && profileObj.getDataInfo().get(0).getResultType()!=5))
		{
			AppConstant.isNumberISUpdate = false;
			Intent resInt = new Intent();
			this.isCountryCodeUpdate = false;
			setResult(AppConstant.CREATE_PROFILE_ACTIVITY_RESULT, resInt);
			finish();
		}else if(profileObj!=null && (profileObj.getDataInfo().get(0).getResultType()==3 || profileObj.getDataInfo().get(0).getResultType()==5)){
			Toast.makeText(this,this.getString(R.string.incorrect_security_code), Toast.LENGTH_LONG).show();
			et_secCode.setText("");

		}


	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {

		super.onActivityResult(requestCode, resultCode, data);

		if(requestCode==AppConstant.GET_COUNTRY_CODE_ACTIVITY_RESULT)
		{
			if(data!=null)
			{
				et_countryCode.setText(data.getStringExtra("Country Code"));
				if(!(data.getStringExtra("Country Code")).equalsIgnoreCase(AppConstant.userProfile.getCountryCode())){
					this.isCountryCodeUpdate = true;
				}else{
					this.isCountryCodeUpdate = false;
				}

			}

		}
	}

	private void setEmergencyContactNumbers(String countryCode){
		CountryCodes code = AppConstant.getCountryCodes(this, countryCode);
		AppConstant.userProfile.setCountryCode(code.getISDCode());
		AppConstant.userProfile.setAmbulanceContact(code.getAmbulance());
		AppConstant.userProfile.setFireContact(code.getFire());
		AppConstant.userProfile.setPoliceContact(code.getPolice());
	}

	private void showConfirmationDialog(String dialogTitle, String message) {
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
		cancelBtn.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				gpsDialog.dismiss();  	
			}
		});
		view.findViewById(R.id.btnOK).setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View arg0) {
				gpsDialog.dismiss();
				createProileForUser();
			}

		});

		view.findViewById(R.id.btnCancel).setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View arg0) {


			}
		});

		gpsDialog.show();
	}

}
