package com.guardianapp.adapters;

import android.support.v4.app.FragmentStatePagerAdapter;

import com.guardianapp.ui.AboutFragment;

/**
 * @author v-dhmadd
 * using v4 library to support right from 2.2 devices . Adapter for view pager used in About Screen.
 */
public class AboutPageAdapter extends FragmentStatePagerAdapter {

	 private int[] screens;
	 private String[] tabs;
	 
	public AboutPageAdapter(android.support.v4.app.FragmentManager fm,int[] pgScreen , String[] tabs) {
     super(fm);
     screens = pgScreen;
     this.tabs = tabs;
	
	}

	@Override
	public android.support.v4.app.Fragment getItem(int index) {
		return new AboutFragment(screens[index]);
	}

	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return screens.length;
	}

	@Override
	public CharSequence getPageTitle(int position) {
		// TODO Auto-generated method stub
		return tabs[position];
	}
}