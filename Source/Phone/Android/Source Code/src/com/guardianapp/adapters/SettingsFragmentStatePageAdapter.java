/**
 * 
 */
package com.guardianapp.adapters;

import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.app.FragmentStatePagerAdapter;

import com.guardianapp.ui.BuddiesFragment;
import com.guardianapp.ui.GroupsFragment;
import com.guardianapp.ui.MyFragment;
import com.guardianapp.ui.PreferencesFragment;
import com.guardianapp.ui.ProfileFragment;
import com.guardianapp.utilities.AppConstant;

/**
 * @author v-dhmadd
 *
 */
public class SettingsFragmentStatePageAdapter extends FragmentPagerAdapter {

	final int PAGE_COUNT = 4;
	String[] tabs; 

	public SettingsFragmentStatePageAdapter(FragmentManager fm , String[] tabs) {
		super(fm);
		// TODO Auto-generated constructor stub
		this.tabs = tabs;
	}

	/* (non-Javadoc)
	 * @see android.support.v4.app.FragmentStatePagerAdapter#getItem(int)
	 */
	@Override
	public Fragment getItem(int index) {
		// TODO Auto-generated method stub\
		switch(index){
		case 0:
			return new ProfileFragment();
		case 1:
			return new  BuddiesFragment();
		case 2:
			return  new GroupsFragment();
		case 3:
			return new PreferencesFragment();

		}
		return null;
	}

	/* (non-Javadoc)
	 * @see android.support.v4.view.PagerAdapter#getCount()
	 */
	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return PAGE_COUNT;
	}

	@Override
	public CharSequence getPageTitle(int position) {
		// TODO Auto-generated method stub
		return tabs[position];
	}

	@Override
	public int getItemPosition(Object object) {
		if(AppConstant.isScreenRefreshRequired) {
			if(object instanceof BuddiesFragment)
			AppConstant.isScreenRefreshRequired = true;
			else if(object instanceof GroupsFragment)
				AppConstant.isScreenRefreshRequired = false;
			return POSITION_NONE;
		}else{
			return POSITION_UNCHANGED;
		}


	}


}
