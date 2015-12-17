package com.guardianapp.adapters;

import android.support.v4.app.FragmentStatePagerAdapter;

import com.guardianapp.ui.GetLocalHelpFragment;
import com.guardianapp.ui.MyBuddiesFragment;
import com.guardianapp.ui.MyGroupFragment;

public class SOSFragmentStatePageAdapter extends FragmentStatePagerAdapter {
	
	final int PAGE_COUNT = 3;
	String[] tabs; 

	public SOSFragmentStatePageAdapter(android.support.v4.app.FragmentManager fm , String[] tabs) {
     super(fm);
     this.tabs = tabs;
 }

	@Override
	public android.support.v4.app.Fragment getItem(int index) {
		
		   switch (index) {
	        case 0:
	            return new MyBuddiesFragment();
	        case 1:
	            return new MyGroupFragment();
	        case 2:
	            return new GetLocalHelpFragment();
	            
	        }

		
		return null;
	}

	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return PAGE_COUNT;
	}

	@Override
	public CharSequence getPageTitle(int position) {
		// TODO Auto-generated method stub
		return this.tabs[position];
	}
	
	
}