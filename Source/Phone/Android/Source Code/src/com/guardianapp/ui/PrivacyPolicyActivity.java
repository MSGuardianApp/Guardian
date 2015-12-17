package com.guardianapp.ui;

import android.os.Bundle;
import android.webkit.WebView;
import android.webkit.WebViewClient;

import com.guardianapp.R;
import com.guardianapp.utilities.AppConstant;

public class PrivacyPolicyActivity extends BaseActivity {
	
	WebView mWebView;

	@Override
	protected void onCreate(Bundle savedInstance) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstance);
		setContentView(R.layout.activity_privacy_policy);
	}

	@Override
	protected void initFields() {
		// TODO Auto-generated method stub
		 // Get Web view
	       mWebView = (WebView) findViewById( R.id.MyWebview );
	       mWebView.getSettings().setJavaScriptEnabled(true);  
	       mWebView.getSettings().setSupportZoom(true);      
	       mWebView.loadUrl(AppConstant.guardianPortalLink);
	       mWebView.setWebViewClient(new WebViewClient() {
	           @Override
	           public boolean shouldOverrideUrlLoading(WebView view, String url) {
	        	   if(url.equals(url)){
	        	        view.loadUrl(url);  
	        	    }
	        	    return true;
	           }
	       });


	}

	@Override
	protected void initCallBacks() {
		// TODO Auto-generated method stub

	}

}
