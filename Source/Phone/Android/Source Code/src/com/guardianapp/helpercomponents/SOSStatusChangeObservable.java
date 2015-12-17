package com.guardianapp.helpercomponents;

import java.util.Observable;

/**
 * @author v-dhmadd
 * Observable for SOSStaus ...used when power button is double clicked and sos is enabled / disabled
 */
public class SOSStatusChangeObservable extends Observable {

	private static SOSStatusChangeObservable instance;

	public SOSStatusChangeObservable(){

	}

	public void sosStatusChanged(boolean status){
		setChanged();
		notifyObservers(status);
	}

	public static SOSStatusChangeObservable getInstance(){
		if(instance == null){
			instance = new SOSStatusChangeObservable();
		}
		return instance;
	}

}
