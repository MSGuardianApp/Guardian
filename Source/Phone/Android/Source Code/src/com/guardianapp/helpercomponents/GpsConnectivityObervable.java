package com.guardianapp.helpercomponents;

import java.util.Observable;

/**
 * @author v-dhmadd
 * observerable class for GpsConnectivity status.
 *
 */
public class GpsConnectivityObervable extends Observable {

	private static GpsConnectivityObervable instance = null;

	public GpsConnectivityObervable() {

	}

	public void gpsProviderChanged(int connectivityStatus) {
		setChanged();
		notifyObservers(connectivityStatus);
	}

	public static GpsConnectivityObervable getInstance() {
		if (instance == null) {
			instance = new GpsConnectivityObervable();
		}
		return instance;

	}
}
