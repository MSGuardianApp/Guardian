package com.guardianapp.helpercomponents;

import java.util.Observable;

/**
 * @author v-dhmadd
 * Observable class for NetworkChange status
 */
public  class NetworkObservable extends Observable{
	private static NetworkObservable instance = null;

	private NetworkObservable() {
		// Exist to defeat instantiation.
	}

	public void connectionChanged(int connectivityStatus){
		setChanged();
		notifyObservers(connectivityStatus);
	}

	public static NetworkObservable getInstance(){
		if(instance == null){
			instance = new NetworkObservable();
		}
		return instance;
	}
}
