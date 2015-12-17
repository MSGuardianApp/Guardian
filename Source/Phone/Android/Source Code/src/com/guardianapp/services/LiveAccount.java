package com.guardianapp.services;

import java.util.Arrays;

import org.json.JSONException;
import org.json.JSONObject;

import android.content.Context;
import android.content.SharedPreferences;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentActivity;
import android.widget.Toast;

import com.guardianapp.Guardian;
import com.guardianapp.ui.BaseActivity;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.LogUtils;
import com.guardianapp.webservicecomponents.MakeHTTPServices;
import com.microsoft.live.LiveAuthClient;
import com.microsoft.live.LiveAuthException;
import com.microsoft.live.LiveAuthListener;
import com.microsoft.live.LiveConnectClient;
import com.microsoft.live.LiveConnectSession;
import com.microsoft.live.LiveOperation;
import com.microsoft.live.LiveOperationException;
import com.microsoft.live.LiveOperationListener;
import com.microsoft.live.LiveStatus;


public class LiveAccount {
	private LiveAuthClient mAuthClient;
	private FragmentActivity act;
	private Fragment callFragment;
	public SharedPreferences prefs_LiveAttributes;

	public LiveAccount(FragmentActivity activity,Fragment frag) {
		this.act = activity;
		this.callFragment =frag;
		prefs_LiveAttributes = this.act.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

		mAuthClient = new LiveAuthClient(Guardian.getInstance(), AppConstant.CLIENT_ID);
		Guardian.getInstance().setAuthClient(mAuthClient);

	}

	public  void connectToLiveAccount() {


		if(AppConstant.check_networkConnectivity(act)==AppConstant.NO_NETWORK)
		{
			Toast.makeText(act, "No Network Connection", Toast.LENGTH_LONG).show();
		}
		else
		{
			((BaseActivity)LiveAccount.this.act).showProgressDialog("Initializing your live account. Please wait...");
			mAuthClient.login(act,
					Arrays.asList(AppConstant.SCOPES),
					new LiveAuthListener() {
				
				
				@Override
				public void onAuthComplete(LiveStatus status,
						LiveConnectSession session,
						Object userState) {
					if (status == LiveStatus.CONNECTED) {
						AppConstant.isScreenRefreshRequired = true;
						launchMainActivity(session);
					} else {
						showToast("Login did not connect. Status is " + status + ".");
						((BaseActivity)LiveAccount.this.act).dismissProgressDialog();
					}
				}

				@Override
				public void onAuthError(LiveAuthException arg0,
						Object arg1) {
					// TODO Auto-generated method stub
					((BaseActivity)LiveAccount.this.act).dismissProgressDialog();

				}
			});
		}

	}

	private void launchMainActivity(LiveConnectSession session) {
		assert session != null;
		if(session!=null)
		{
			Guardian.getInstance().setSession(session);
			Guardian.getInstance().setConnectClient(new LiveConnectClient(session));
			SharedPreferences.Editor editor = prefs_LiveAttributes.edit();
			editor.putString(AppConstant.Authentication_Token, session.getAuthenticationToken());
			editor.putString(AppConstant.Access_Token, session.getAccessToken());
			editor.putString(AppConstant.Referesh_Token, session.getRefreshToken());
			editor.putBoolean(AppConstant.isLiveRagister, true);
			editor.commit();

			LiveConnectClient connectClient = new LiveConnectClient(session);
			connectClient.getAsync("me", new LiveOperationListener() {

				@Override
				public void onComplete(LiveOperation operation) {
					((BaseActivity)LiveAccount.this.act).dismissProgressDialog();
					JSONObject result = operation.getResult();

					if (result.has("error"))
					{
						JSONObject error = result.optJSONObject("error");
						String code = error.optString("code");
						String message = error.optString("message");
					}
					else
					{
						try {

							SharedPreferences.Editor edit = prefs_LiveAttributes.edit();
							if(result.toString().contains("name"))
								edit.putString(AppConstant.Live_user_name,result.getString("name"));
							if(result.toString().contains("emails"))
								edit.putString(AppConstant.LiveEmail_id, result.getJSONObject("emails").getString("account").toString());
							edit.commit();

							MakeHTTPServices httpServices = new MakeHTTPServices(act,callFragment);
							httpServices.membershipServiceUrl();

						} catch (JSONException e) {
							// TODO Auto-generated catch block
							LogUtils.LOGE(LiveAccount.class.getName(), e.getLocalizedMessage());
						}

					}

				}

				@Override
				public void onError(LiveOperationException arg0,
						LiveOperation arg1) {
					// TODO Auto-generated method stub

				}

			});


		}


	}

	private void showToast(String message) {
		Toast.makeText(act, message, Toast.LENGTH_LONG).show();

	}
}
