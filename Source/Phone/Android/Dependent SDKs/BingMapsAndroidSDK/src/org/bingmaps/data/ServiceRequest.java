package org.bingmaps.data;

import java.io.IOException;
import java.io.UnsupportedEncodingException;

import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.ResponseHandler;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.BasicResponseHandler;
import org.apache.http.impl.client.DefaultHttpClient;

public class ServiceRequest {
	public ServiceRequest(){
	}
	
	public ServiceRequest(String url, RequestType requestType, String contentType){
		this.URL = url;
		this.RequestType = requestType;
		this.ContentType = contentType;
	}
	
	public ServiceRequest(String url, RequestType requestType, String contentType, String data){
		this.URL = url;
		this.RequestType = requestType;
		this.ContentType = contentType;
		this.Data = data;
	}
	
	public String ContentType;
	public RequestType RequestType;
	public String URL;
	public String Data;
	
	public String execute(){
		String result = "";
		HttpClient httpClient = new DefaultHttpClient(); 
		
		if(this.RequestType == org.bingmaps.data.RequestType.POST)
		{
			HttpPost postRequest = new HttpPost(this.URL);
			postRequest.setHeader("Accept", this.ContentType);
			postRequest.setHeader("Content-type", this.ContentType);

	        try {
	        	if(this.Data != null){
			       	StringEntity entity = new StringEntity(this.Data);	
			       	postRequest.setEntity(entity);
	        	}
		
		        ResponseHandler<String> handler = new BasicResponseHandler();
		        result = httpClient.execute(postRequest, handler); 
		        
	        } catch (UnsupportedEncodingException e) {
				e.printStackTrace();
	        } catch (ClientProtocolException e) {
				e.printStackTrace();
			} catch (IOException e) {
				e.printStackTrace();
			}	
		}else if(this.RequestType == org.bingmaps.data.RequestType.GET)
		{ 
			HttpGet getRequest = new HttpGet(this.URL);  
			getRequest.setHeader("Accept", this.ContentType);
			getRequest.setHeader("Content-type", this.ContentType);

			try {
				ResponseHandler<String> handler = new BasicResponseHandler();
		        result = httpClient.execute(getRequest, handler);  	        
			} catch (ClientProtocolException e) {
				e.printStackTrace();
			} catch (IOException e) {
				e.printStackTrace();
			}  
		}
		
		httpClient.getConnectionManager().shutdown();  
		return result;
	}
}
