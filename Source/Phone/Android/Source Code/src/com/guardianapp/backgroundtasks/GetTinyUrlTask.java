package com.guardianapp.backgroundtasks;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.Date;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.AsyncTask;

import com.guardianapp.services.TinyUrl;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.LogUtils;

/**
 * @author v-dhmadd
 *
 */
public class GetTinyUrlTask extends AsyncTask<String, Void,  String> {
	private Context context;
	SharedPreferences  AppPrefs ;
	private String longUrl;

	public GetTinyUrlTask(Context context){
		this.context = context;
		this.AppPrefs = context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
	}

	@Override
	protected String doInBackground(String... params) {
		String mtinyUrl = "";
		longUrl = params[0];
		String urlString = AppConstant.T_URL + longUrl;

		try {
			URL url = new URL(urlString);
			BufferedReader in = new BufferedReader(new InputStreamReader(url.openStream()));
			String str;
			while ((str = in.readLine()) != null) {
				mtinyUrl += str;
			}
			in.close();
		}
		catch(MalformedURLException mue){
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), "Can not create an tinyurl link:"+mue.getCause().getLocalizedMessage());
		}
		catch(IOException ioe){
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), "Can not create an tinyurl link:"+ioe.getCause().getLocalizedMessage());
		}
		catch (Exception e) {
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), "Can not create an tinyurl link:"+e.getCause().getLocalizedMessage());
		}
		LogUtils.LOGD(LogUtils.makeLogTag(this.getClass()), "Tiney Url====="+mtinyUrl);
		TinyUrl.tinyUrl = mtinyUrl;
		return mtinyUrl;
	}
	@Override
	protected void onPostExecute(String result) {
		// TODO Auto-generated method stub
		super.onPostExecute(result);
		String smsText = "";
		if(AppPrefs.getBoolean(AppConstant.isProfileDone, false)){
			smsText  = AppConstant.localUserName+" "+AppConstant.userProfile.getMobileNumber()+" "
					+AppConstant.sendSMSFromPhone;
		}else{
			smsText = AppConstant.MessageTemplateText+" I'm at ";
		}

		smsText = smsText+" "+result;	
		AppConstant.sendDistressMessageToMedium(context, smsText, AppConstant.booleanToInt(true) , false);
		AppConstant.sendDistressMessageToMedium(context, smsText, AppConstant.booleanToInt(false) , false);
		AppConstant.TINY_URL = result;
	}
}
