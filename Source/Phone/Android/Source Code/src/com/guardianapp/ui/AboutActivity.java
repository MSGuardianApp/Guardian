package com.guardianapp.ui;

import android.app.ActionBar;
import android.app.Activity;
import android.content.ActivityNotFoundException;
import android.content.Intent;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.support.v4.view.ViewPager;
import android.view.View;
import android.view.View.OnClickListener;

import com.guardianapp.R;
import com.guardianapp.adapters.AboutPageAdapter;
import com.guardianapp.utilities.AppConstant;

public class AboutActivity extends BaseActivity implements OnClickListener{


	private ActionBar actionBar;
	private String[] tabs = { "About", "HowItWorks", "Features", "Version History", "Logger" };
	private ViewPager mViewPager;
	private int AboutLayouts[] = new int[] { R.layout.about_layout,  R.layout.how_it_work_layout,  R.layout.features_layout,
			R.layout.version_history_layout,  R.layout.logger_layout,};
	private AboutPageAdapter aboutPageAdapter;

	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.about_screen);
	}

	@Override
	protected void initFields() {
		// TODO Auto-generated method stub
		aboutPageAdapter=new AboutPageAdapter(getSupportFragmentManager(), AboutLayouts, tabs);
		mViewPager = (ViewPager) findViewById(R.id.aboutFragmentLayout);
		mViewPager.setAdapter(aboutPageAdapter);
		mViewPager.setOffscreenPageLimit(aboutPageAdapter.getCount());

	}

	@Override
	protected void initCallBacks() {
		// TODO Auto-generated method stub
		findViewById(R.id.imageMailBtn).setOnClickListener(this);
		findViewById(R.id.imageSMSBtn).setOnClickListener(this);
		findViewById(R.id.imageShareBtn).setOnClickListener(this);
		findViewById(R.id.imageLikeBtn).setOnClickListener(this);

	}


	@Override
	public void onClick(View v) {
		switch (v.getId()) {
		case R.id.imageMailBtn:
			Intent emailInt = new Intent(Intent.ACTION_SEND);
			emailInt.setType("message/rfc822");
			String emailText=AppConstant.aboutEmailText+" "+AppConstant.PLAYSTORE_URL+ AboutActivity.this.getApplicationContext().getPackageName()+"&hl=en";
			emailInt.putExtra(Intent.EXTRA_EMAIL  ,AppConstant.aboutEmailId);
			emailInt.putExtra(Intent.EXTRA_SUBJECT, AppConstant.aboutEmailSubject);
			emailInt.putExtra(Intent.EXTRA_TEXT   , emailText);
			startActivity(Intent.createChooser(emailInt, "Send mail..."));
			break;

		case R.id.imageSMSBtn:
			//Uri smsUri = Uri.parse("tel:"+phoneNumber);
			String smsText = AppConstant.aboutSMSText+" "+AppConstant.PLAYSTORE_URL+ AboutActivity.this.getApplicationContext().getPackageName()+"&hl=en";
			
			if (Build.VERSION.SDK_INT > Build.VERSION_CODES.JELLY_BEAN_MR2){
				Intent intent = new Intent(Intent.ACTION_SENDTO);
				intent.setData(Uri.parse("smsto:"));
				intent.putExtra("sms_body",smsText);
				startActivity(intent);

			}else{
				Intent intent = new Intent(Intent.ACTION_VIEW);
				intent.putExtra("sms_body", smsText);
				intent.setType("vnd.android-dir/mms-sms"); 
				intent.putExtra("address", "");
				startActivity(intent);

			}
			break;

		case R.id.imageShareBtn:
			//call facebook and other social network
			Intent shareInt = new Intent(Intent.ACTION_SEND);
			shareInt.setType("text/plain");
			String aboutShareLink = AppConstant.PLAYSTORE_URL + AboutActivity.this.getApplicationContext().getPackageName()+"&hl=en";
			shareInt.putExtra(Intent.EXTRA_TEXT   , AppConstant.aboutShareText +"\n"+ aboutShareLink);
			startActivity(Intent.createChooser(shareInt, "Share with"));
			break;

		case R.id.imageLikeBtn:
			//CALL play store to ratings
			Uri uri = Uri.parse("market://details?id=" + AboutActivity.this.getApplicationContext().getPackageName()+"&hl=en");
			Intent goToMarket = new Intent(Intent.ACTION_VIEW, uri);
			try {
				startActivity(goToMarket);
			} catch (ActivityNotFoundException e) {
				startActivity(new Intent(Intent.ACTION_VIEW, Uri.parse(AppConstant.PLAYSTORE_URL + AboutActivity.this.getApplicationContext().getPackageName()+"&hl=en")));
			}
			break;

		default:
			break;
		}

	}






}
