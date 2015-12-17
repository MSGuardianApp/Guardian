package com.guardianapp.adapters;

import android.support.v4.app.FragmentStatePagerAdapter;

import com.guardianapp.ui.HelpFragment;

/**
 * @author v-dhmadd
 *
 */
public class HelpPageAdapter extends FragmentStatePagerAdapter {

	 private int[] helpImages;
	 
	public HelpPageAdapter(android.support.v4.app.FragmentManager fm,int[] Images) {
     super(fm);
     helpImages = Images;
	
	}

	@Override
	public android.support.v4.app.Fragment getItem(int index) {
		return new HelpFragment(helpImages[index]);
	}

	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return helpImages.length;
	}
}