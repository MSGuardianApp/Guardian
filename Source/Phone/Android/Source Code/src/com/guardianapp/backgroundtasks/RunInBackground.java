package com.guardianapp.backgroundtasks;

import android.content.Context;
import android.content.SharedPreferences;

import com.google.gson.JsonParseException;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.LogUtils;
import com.guardianapp.webservicecomponents.MakeHTTPServices;

/**
 * @author v-dhmadd
 *
 */
public class RunInBackground {

	private Context context;
	private SharedPreferences appPrefs;

	public RunInBackground(Context argContext){
		this.context = argContext;
		appPrefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
	}

	public void run() {
		if(AppConstant.check_networkConnectivity(context)!= AppConstant.NO_NETWORK)
		{
			if(isCanPostToServer())
			{
				MakeHTTPServices httpService = new MakeHTTPServices(context);
				try {
					httpService.postLocationArray();
				} catch (JsonParseException jse) {
					LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), jse.getCause().getLocalizedMessage());
				}catch(Exception e){
					LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), e.getCause().getLocalizedMessage());
				}
			}
		}
	}
	private boolean isCanPostToServer() {
		if(appPrefs.getBoolean(AppConstant.isProfileDone, false)&&AppConstant.userProfile.isPostLocationConsent())
		{
			if((iSSOSOn()||iSTrackOn()))
				return true;
			else
				return false; 
		}
		else
			return false;

	}

	private boolean iSSOSOn() {
		if(appPrefs.getBoolean(AppConstant.SOS_on, false))
		{
			AppConstant.TokenId = AppConstant.SOS_TOKEN;
			return true;

		}
		else
			return false;
	}
	private boolean iSTrackOn() {
		if(appPrefs.getBoolean(AppConstant.trackMe_on, false))
		{
			if(!appPrefs.getBoolean(AppConstant.SOS_on, false))
				AppConstant.TokenId = AppConstant.TRACKING_TOKEN;
			else
				AppConstant.TokenId = AppConstant.SOS_TOKEN;

			return true;
		}

		else
			return false;
	}

	public boolean stop() {

		MakeHTTPServices httpService = new MakeHTTPServices(context);
		try{
			httpService.stopPosting(AppConstant.TokenId);	
		}catch(JsonParseException jpe){
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), jpe.getCause().getLocalizedMessage());
		}catch(Exception e){
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), e.getCause().getLocalizedMessage());
		}
		return true;


	}
}
