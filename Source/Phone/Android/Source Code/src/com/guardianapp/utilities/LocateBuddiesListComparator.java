package com.guardianapp.utilities;

import java.util.Comparator;

import com.guardianapp.model.LocateBuddies;

public class LocateBuddiesListComparator implements Comparator<LocateBuddies> {

	@Override
	public int compare(LocateBuddies lhs, LocateBuddies rhs) {
		// TODO Auto-generated method stub
		 return rhs.getOrderNumber() > lhs.getOrderNumber() ? 1 : (rhs.getOrderNumber() < lhs.getOrderNumber() ? -1 : 0);
	}

}
