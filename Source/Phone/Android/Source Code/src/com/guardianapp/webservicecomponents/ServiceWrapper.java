package com.guardianapp.webservicecomponents;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.UnsupportedEncodingException;
import java.util.ArrayList;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.params.BasicHttpParams;
import org.apache.http.params.HttpConnectionParams;
import org.apache.http.params.HttpParams;
import org.json.JSONObject;

import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.LogUtils;
import com.guardianapp.utilities.RetryHandler;

public class ServiceWrapper {

	private  InputStream is = null;
	private RetryHandler retryHandler = null;
	private DefaultHttpClient httpClient = null;

	public ServiceWrapper() { }

	public String[] makeHttpRequest(String url, String method, ArrayList<HttpParam> params, JSONObject jObject) {
		HttpParams httpParameters = new BasicHttpParams();
		HttpConnectionParams.setConnectionTimeout(httpParameters, AppConstant.TIME_OUT_CONNECTION);
		HttpConnectionParams.setSoTimeout(httpParameters, AppConstant.TIME_OUT_SOCKET);
		httpClient = new DefaultHttpClient(httpParameters);
		retryHandler = new RetryHandler();
		
		String[] result = new String[2];
		boolean status = true;
		int attemptNumber = 0;
        
		
		while(status){
		try {
			if(method == "POST"){
				attemptNumber++;
				result = this.processPostRequest(url, params, jObject);
				status = false;
				
			}
			else if(method == "GET"){
				attemptNumber++;
				result = this.processGetRequest(url, params, jObject);
				status = false;
			}          
		}catch (UnsupportedEncodingException e) {
			if (retryHandler != null) 
				status = retryHandler.shouldRetry(e, attemptNumber);
			    result[AppConstant.HTTP_RESULT]=e.toString();
		}catch (ClientProtocolException e) {
			if (retryHandler != null) 
				status = retryHandler.shouldRetry(e, attemptNumber);
			result[AppConstant.HTTP_RESULT]=e.toString();
		}catch (IOException e) {
			if (retryHandler != null) 
				status = retryHandler.shouldRetry(e, attemptNumber);
			result[AppConstant.HTTP_RESULT]=e.toString();
		}
		}
		
		if(result[AppConstant.HTTP_RESULT]=="true"){
			try {
				BufferedReader reader = new BufferedReader(new InputStreamReader(is, "iso-8859-1"), 8);
				StringBuilder sb = new StringBuilder();
				String line = null;
				while ((line = reader.readLine()) != null) 
				{
					sb.append(line + "\n");
				}
				is.close();
				result[AppConstant.HTTP_RESPONSE]= sb.toString();
				return result;
			}catch (Exception e) {
				result[AppConstant.HTTP_RESULT]= e.toString();
				return result;
			}
		}else 
			return result;
	}

	public String[] processPostRequest(String url,ArrayList<HttpParam> params, JSONObject jObject) throws IOException{
		String[] result = new String[2];
		HttpPost httpPost = new HttpPost(url);

		if(params !=null)
		{
			for(int i=0;i<params.size();i++)
			{
				httpPost.setHeader(params.get(i).getKey(),params.get(i).getValue() );
			}
		}
		if(jObject!=null)
		{
			LogUtils.LOGD(LogUtils.makeLogTag(this.getClass()),"Json String=="+jObject.toString());
			StringEntity se = new StringEntity( jObject.toString());
			httpPost.setEntity(se);
		}
		HttpResponse httpResponse = httpClient.execute(httpPost);
		if(httpResponse.getStatusLine().getStatusCode()==200)
		{
			HttpEntity httpEntity = httpResponse.getEntity();
			is = httpEntity.getContent();
			result[AppConstant.HTTP_RESULT]="true";
		}
		else
		{
			result[AppConstant.HTTP_RESULT]="false";
			result[AppConstant.HTTP_RESPONSE]= httpResponse.getStatusLine().toString();

		}
		return result;	
	}

	public String[] processGetRequest(String url,ArrayList<HttpParam> params, JSONObject jObject) throws IOException{
		String[] result = new String[2];
		HttpGet httpGet = new HttpGet(url);

		if(params !=null)
		{
			for(int i=0;i<params.size();i++)
			{
				httpGet.setHeader(params.get(i).getKey(),params.get(i).getValue() );
			}

		}

		HttpResponse httpResponse = httpClient.execute(httpGet);
		if(httpResponse.getStatusLine().getStatusCode()==200)
		{
			HttpEntity httpEntity = httpResponse.getEntity();
			is = httpEntity.getContent();
			result[AppConstant.HTTP_RESULT]="true";
		}
		else
		{
			result[AppConstant.HTTP_RESULT]="false";
			result[AppConstant.HTTP_RESPONSE]= httpResponse.getStatusLine().toString();
		}
		return result;
	}

}
