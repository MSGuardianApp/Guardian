package com.guardianapp;

import android.app.Application;

import com.microsoft.live.LiveAuthClient;
import com.microsoft.live.LiveConnectClient;
import com.microsoft.live.LiveConnectSession;

/**
 * @author v-dhmadd
 * Application class which acts as a singleton 
 */
public class Guardian extends Application {

    private LiveAuthClient mAuthClient;
    private LiveConnectClient mConnectClient;
    private LiveConnectSession mSession;
    private static Guardian appInstance;
    
    @Override
	public void onCreate() {
		// TODO Auto-generated method stub
		super.onCreate();
		appInstance = this;
	}
    
    public static synchronized Guardian getInstance(){
    	return appInstance;
    }

	public LiveAuthClient getAuthClient() {
        return mAuthClient;
    }

    public LiveConnectClient getConnectClient() {
        return mConnectClient;
    }

    public LiveConnectSession getSession() {
        return mSession;
    }

    public void setAuthClient(LiveAuthClient authClient) {
        mAuthClient = authClient;
    }

    public void setConnectClient(LiveConnectClient connectClient) {
        mConnectClient = connectClient;
    }

    public void setSession(LiveConnectSession session) {
        mSession = session;
    }
}
