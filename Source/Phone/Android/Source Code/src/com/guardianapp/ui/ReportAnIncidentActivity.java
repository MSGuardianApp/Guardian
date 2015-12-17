package com.guardianapp.ui;

import java.text.SimpleDateFormat;
import java.util.Date;

import org.json.JSONException;

import android.app.AlertDialog;
import android.content.ContentValues;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.graphics.Typeface;
import android.net.Uri;
import android.os.Bundle;
import android.provider.MediaStore;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.TextView;

import com.guardianapp.R;
import com.guardianapp.services.LiveAccount;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.ImageCompressor;
import com.guardianapp.utilities.LogUtils;
import com.guardianapp.webservicecomponents.MakeHTTPServices;
import com.guardianapp.webservicecomponents.OnTaskCompleted;
//phani
public class ReportAnIncidentActivity extends BaseActivity implements OnTaskCompleted, OnClickListener{

	private RadioButton rb_harassment,rb_ragging,rb_accident,rb_robbery,rb_others;
	private String command= "HARASSMENT";
	private Uri fileUri;

	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		SharedPreferences prefs_LiveAttributes = this.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
		getSupportActionBar().setTitle("REPORT INCIDENT");

		if(prefs_LiveAttributes.getBoolean(AppConstant.isLiveRagister, false))
		{
			if( prefs_LiveAttributes.getBoolean(AppConstant.isProfileDone, false))
			{
				setContentView(R.layout.report_incident_activity);
				initializeReportAnIncidentView();
			}
			else
				startActivityForResult(new Intent(this, CreateProfileActivity.class),AppConstant.CREATE_PROFILE_ACTIVITY_RESULT);
		}
		else
		{
			setContentView(R.layout.create_profie_fragment);
			this.initializeCreateProfileView();
		}

	}

	@Override
	protected void initFields() {
		// TODO Auto-generated method stub

	}

	@Override
	protected void initCallBacks() {
		// TODO Auto-generated method stub

	}



	private void initializeReportAnIncidentView()
	{
		rb_harassment = (RadioButton) findViewById(R.id.rb_harassment);
		rb_harassment.setOnClickListener(this);

		//set harassment option as checked by default
		command="HARASSMENT";
		rb_harassment.setChecked(true);
		rb_harassment.setButtonDrawable(R.drawable.circle_check);

		rb_ragging =(RadioButton) findViewById(R.id.rb_ragging);
		rb_ragging.setOnClickListener(this);

		rb_accident = (RadioButton) findViewById(R.id.rb_accident);
		rb_accident.setOnClickListener(this);

		rb_robbery =(RadioButton) findViewById(R.id.rb_robbery);
		rb_robbery.setOnClickListener(this);

		rb_others =(RadioButton) findViewById(R.id.rb_others);
		rb_others.setOnClickListener(this);

		findViewById(R.id.btn_report).setOnClickListener(this);
	}

	private void initializeCreateProfileView(){
		findViewById(R.id.hyperLink).setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v){

				Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse(AppConstant.guardianPortalLink));
				startActivity(browserIntent);

			}
		});

		Button connectToMsLive = (Button)findViewById(R.id.btn_connectToMSLive);
		Typeface face=Typeface.createFromAsset(this.getAssets(), "SEGOEUI.TTF");
		connectToMsLive.setTypeface(face);

		connectToMsLive.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {

				LiveAccount guardianLive = new LiveAccount(ReportAnIncidentActivity.this, null);
				guardianLive.connectToLiveAccount();


			}

		});

	}

	@Override
	public void onClick(View v) {
		// TODO Auto-generated method stub
		switch (v.getId()) {

		case R.id.rb_harassment:
			command="HARASSMENT";
			setSingleCheck(R.id.rb_harassment);
			break;
		case R.id.rb_ragging:
			command="RAGGING";
			setSingleCheck(R.id.rb_ragging);
			break;
		case R.id.rb_accident:
			command="ACCIDENT";
			setSingleCheck(R.id.rb_accident);
			break;
		case R.id.rb_robbery:
			command="ROBBERY";
			setSingleCheck(R.id.rb_robbery);
			break;
		case R.id.rb_others:
			command="OTHERS";
			setSingleCheck(R.id.rb_others);
			break;
		case R.id.btn_report:
			String timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss").format(new Date());
	        ContentValues values = new ContentValues();
	        values.put(MediaStore.Images.Media.TITLE, "IMG_" + timeStamp + ".jpg");
	        Intent intent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
	        fileUri = getContentResolver().insert(MediaStore.Images.Media.EXTERNAL_CONTENT_URI, values); 
	        intent.putExtra( MediaStore.EXTRA_OUTPUT,  fileUri);
			startActivityForResult(intent,AppConstant.CAMERA_ACTIVITY_RESULT);
			break;
		}
	}

	private void setSingleCheck(int id) {
		if(R.id.rb_harassment==id)
		{ 
			rb_harassment.setChecked(true);
			rb_harassment.setButtonDrawable(R.drawable.circle_check);
		}

		else
		{
			rb_harassment.setChecked(false);
			rb_harassment.setButtonDrawable(R.drawable.small_circle);
		}


		if(R.id.rb_ragging==id)
		{
			rb_ragging.setChecked(true);
			rb_ragging.setButtonDrawable(R.drawable.circle_check);
		}

		else
		{
			rb_ragging.setChecked(false);
			rb_ragging.setButtonDrawable(R.drawable.small_circle);
		}	


		if(R.id.rb_accident==id)
		{
			rb_accident.setChecked(true);
			rb_accident.setButtonDrawable(R.drawable.circle_check);
		}

		else
		{
			rb_accident.setChecked(false);
			rb_accident.setButtonDrawable(R.drawable.small_circle);
		}


		if(R.id.rb_robbery==id)
		{
			rb_robbery.setChecked(true);
			rb_robbery.setButtonDrawable(R.drawable.circle_check);
		}

		else
		{
			rb_robbery.setChecked(false);
			rb_robbery.setButtonDrawable(R.drawable.small_circle);
		}


		if(R.id.rb_others==id)
		{
			rb_others.setChecked(true);
			rb_others.setButtonDrawable(R.drawable.circle_check);
		}

		else
		{
			rb_others.setButtonDrawable(R.drawable.small_circle);
			rb_others.setChecked(false);
		}



	}
	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {

		super.onActivityResult(requestCode,  resultCode, data);
		if (requestCode == AppConstant.CAMERA_ACTIVITY_RESULT && resultCode == RESULT_OK) {  
			if(AppConstant.check_networkConnectivity(this)!= AppConstant.NO_NETWORK)
			{
				SharedPreferences AppPrefs = getSharedPreferences(
						AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

				if(AppPrefs.getBoolean(AppConstant.isProfileDone, false)
						&&AppConstant.userProfile.isPostLocationConsent())
				{
					ImageCompressor imageComp = new ImageCompressor(this);
					Bitmap photo = imageComp.compressImage(imageComp.getRealPathFromURI(this,fileUri));
					this.reportTeaseToServer(photo);
				}
			}
		}
		else if(requestCode==AppConstant.CREATE_PROFILE_ACTIVITY_RESULT)
		{
			finish();
			startActivity(getIntent());
		}    
	}

	@Override
	public void onTaskComplete(String result) {
		AppConstant.dismissProgressDialog();
		// TODO Auto-generated method stub
		if(AppConstant.service_Tag == AppConstant.REPORT_AN_INCIDENT_SERVICE_TAG)
			this.showThankYouMessage();

	}

	@Override
	public void onGetObjectResult(Object obj) {
		AppConstant.dismissProgressDialog();
		//this.showThankYouMessage();

		if(AppConstant.service_Tag == AppConstant.REPORT_AN_INCIDENT_SERVICE_TAG){
			this.showThankYouMessage();
		}else{
			finish();
			startActivity(getIntent());
		}


	}

	private void showThankYouMessage() {
		View view =  getLayoutInflater().inflate(R.layout.common_dialog, null);
		final AlertDialog gpsDialog = new AlertDialog.Builder(this)
		.setView(view).create();

		TextView title = (TextView) view.findViewById(R.id.tv_title);	
		title.setText(this.getString(R.string.info_txt));

		TextView msgTxt = (TextView) view.findViewById(R.id.tv_msgAleart);	
		msgTxt.setText(this.getText(R.string.report_an_incident_msg));
		Button callbtn = (Button) view.findViewById(R.id.btnOK);
		callbtn.setText(this.getString(R.string.dialog_ok_button));

		Button cancelBtn = (Button) view.findViewById(R.id.btnCancel);
		cancelBtn.setVisibility(View.GONE);
		view.findViewById(R.id.btnOK).setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {
				//startActivityForResult(new Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS),AppConstant.GPS_SETTINGS_ACTIVITY_RESULT);
				gpsDialog.dismiss();
				finish();
				startActivity(new Intent(ReportAnIncidentActivity.this,HomeScreenActivity.class));
			}
		});

		gpsDialog.show();
	}

	private void reportTeaseToServer(Bitmap photo){
		MakeHTTPServices sendfromServices = new MakeHTTPServices(this , null);
		try {
			String additionalInfo=((EditText)findViewById(R.id.editTxt_incidentDescrip)).getText().toString();
			sendfromServices.ReportTeaseToServer(AppConstant.convertIntoBase64(photo),command,additionalInfo);
		} catch (JSONException jse) {
			// TODO Auto-generated catch block
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), jse.getLocalizedMessage());
		}catch(Exception e){
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), e.getLocalizedMessage());
		}
	}


}
