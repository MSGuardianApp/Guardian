package com.guardianapp.ui;

import java.util.Vector;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.annotation.SuppressLint;
import android.app.ProgressDialog;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.os.Bundle;
import android.text.TextUtils;
import android.view.View;
import android.view.View.OnClickListener;
import android.webkit.CookieManager;
import android.webkit.JavascriptInterface;
import android.webkit.WebView;
import android.webkit.WebViewClient;

import com.guardianapp.R;
import com.guardianapp.model.GroupDO;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.LogUtils;
import com.guardianapp.webservicecomponents.MakeHTTPServices;
import com.guardianapp.webservicecomponents.OnTaskCompleted;

public class FaceBookLoginActivity extends BaseActivity implements OnClickListener , OnTaskCompleted{

	private WebView webPage;
	private String strToken= "";
	private SharedPreferences sharedPreferences;
	private SharedPreferences.Editor editor;
	private boolean hideProgressDialog = false;
	private Vector<GroupDO> veGroupDOs;
	boolean loadingFinished = true;
	boolean redirect = false;
	private ProgressDialog progressDialog;

	@SuppressLint("JavascriptInterface")
	@Override
	protected void onCreate(Bundle savedInstance) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstance);
		setContentView(R.layout.activity_fb_login);
	}


	@Override
	protected void initFields() {
		// TODO Auto-generated method stub
		sharedPreferences = getSharedPreferences(AppConstant.APP_SHARED_PREFRENCE, MODE_PRIVATE);
		editor = sharedPreferences.edit();
		webPage = (WebView)findViewById(R.id.webView);
		
		webPage.getSettings().setUseWideViewPort(true);
		webPage.getSettings().setLoadWithOverviewMode(true);
		webPage.getSettings().setBuiltInZoomControls(true);
		webPage.getSettings().setJavaScriptEnabled(true);
		/* Register a new JavaScript interface called HTMLOUT */
		webPage.addJavascriptInterface(new MyJavaScriptInterface(), "HTMLOUT");
		if(sharedPreferences.getString("FBAccessToken", "")!= null && sharedPreferences.getString("FBAccessToken", "").trim().length()<=0){
			CookieManager.getInstance().removeSessionCookie();
			CookieManager.getInstance().removeAllCookie();
			webPage.clearCache(true);
			webPage.clearHistory();
		}
		webPage.loadUrl(AppConstant.strUrl);

	}

	@Override
	protected void initCallBacks() {
		// TODO Auto-generated method stub
		webPage.setWebViewClient(new WebViewClient() {      
			//If you will not use this method url links are opeen in new brower not in webview
			public boolean shouldOverrideUrlLoading(WebView view, String strLoweredAddress) {
				if (!loadingFinished) {
					redirect = true;
				}

				loadingFinished = false;
				
				if(progressDialog==null)
		        {
					progressDialog =  new ProgressDialog(FaceBookLoginActivity.this);
					progressDialog.setMessage("Loading...");
					progressDialog.setCancelable(false);
					progressDialog.show();

		        }

				if (strLoweredAddress.startsWith("http://www.facebook.com/connect/login_success.html?code="))
				{
					webPage.loadUrl(AppConstant.strTokenUrl+strLoweredAddress.substring(56));
				}
				else if (strLoweredAddress.startsWith("https://m.facebook.com/connect/login_success.html?code="))
				{
					webPage.loadUrl(strLoweredAddress.substring(55));
				}
				else  if (strLoweredAddress.contains("http:\\/\\/www.facebook.com\\/connect\\/login_success.html?code="))
				{
					int start = strLoweredAddress.indexOf("login_success.html?code=");
					int end = strLoweredAddress.indexOf("\";</script>");
					webPage.loadUrl(strLoweredAddress.substring(start, end - start).substring(24));
					return false;
				}
				else
				{
					hideProgressDialog = true;
					view.loadUrl(strLoweredAddress);
				}
				return true;
			}
			
			@Override
			public void onPageStarted(WebView view, String url, Bitmap favicon) {
				// TODO Auto-generated method stub
				if(progressDialog==null)
		        {
					progressDialog =  new ProgressDialog(FaceBookLoginActivity.this);
		        }
				progressDialog.setMessage("Loading...");
				progressDialog.setCancelable(false);
				progressDialog.show();
			}

			public void onPageFinished(WebView view, String url) {
				super.onPageFinished(view, url);
				if(!redirect){
					loadingFinished = true;
				}

				if(loadingFinished && !redirect){
				
				} else{
					redirect = false; 
				}
				if(url.contains("access_token")){
					webPage.loadUrl("javascript:window.HTMLOUT.processHTML('<head>'+document.getElementsByTagName('html')[0].innerHTML+'</head>');");
					view.setVisibility(View.GONE);
				}

				webPage.setHorizontalScrollBarEnabled(false);
				webPage.setScrollBarStyle(View.SCROLLBARS_OUTSIDE_OVERLAY);
				webPage.setBackgroundColor(128); 
				if(hideProgressDialog){
					if(progressDialog != null && progressDialog.isShowing())
						progressDialog.dismiss();
				}

				/*if(progressDialog != null && progressDialog.isShowing())
					progressDialog.dismiss();
*/

			}

		}); 
	}


	class MyJavaScriptInterface
	{
		@JavascriptInterface
		@SuppressWarnings("unused")
		public void processHTML(String html)
		{
			MakeHTTPServices httpServices = new MakeHTTPServices(FaceBookLoginActivity.this,null);
			String str = "access_token=";
			int start = html.indexOf(str);
			int end = html.indexOf("&amp;expires=");
			editor.putString("FBAccessToken", html.subSequence(start+str.length(), end).toString());
			editor.commit();
			strToken = html.subSequence(start+str.length(), end).toString();
			editor.putString("str_token", strToken);
			editor.commit();
			if(strToken != null && !strToken.equalsIgnoreCase(""))
				httpServices.getListOfFBGroups(AppConstant.strGetGroupsUrl+strToken);
		}
	}


	@Override
	public void onTaskComplete(String result) {
		// TODO Auto-generated method stub
		AppConstant.dismissProgressDialog();
		try {
			veGroupDOs = new Vector<GroupDO>();
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

			Intent intent = new Intent();
			intent.putExtra("fb_groups_list", veGroupDOs);
			setResult(2,intent);  
			finish();

		} catch (JSONException je) {
			// TODO Auto-generated catch block
			LogUtils.LOGE(LogUtils.makeLogTag(FaceBookLoginActivity.class), je.getLocalizedMessage());
		}


	}


	@Override
	public void onGetObjectResult(Object obj) {
		// TODO Auto-generated method stub
		AppConstant.dismissProgressDialog();

	}


	@Override
	public void onClick(View v) {
		// TODO Auto-generated method stub

	}

}
