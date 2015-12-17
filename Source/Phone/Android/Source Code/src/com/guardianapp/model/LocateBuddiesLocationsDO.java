package com.guardianapp.model;

import java.io.Serializable;

public class LocateBuddiesLocationsDO implements Serializable {

	/**
	 * 
	 */
	private static final long serialVersionUID = 93443844350683903L;
	
	private boolean success;
	
	private LocateBuddiesLocations buddiesLoc;

	public LocateBuddiesLocations getBuddiesLoc() {
		return buddiesLoc;
	}

	public void setBuddiesLoc(LocateBuddiesLocations buddiesLoc) {
		this.buddiesLoc = buddiesLoc;
	}
	
	


}
