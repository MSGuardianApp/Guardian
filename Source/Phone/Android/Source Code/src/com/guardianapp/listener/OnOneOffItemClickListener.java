package com.guardianapp.listener;

/**
 * 
 */

import android.os.SystemClock;
import android.view.View;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;

/**
 * @author v-dhmadd
 *Restricts the multiple clicks on an individual list item. Ensures that multiple activities are not opened even if a button is 
 *clicked multiple no. of times instantaneously . 
 */

public abstract class OnOneOffItemClickListener implements OnItemClickListener {

	/**
	 * 
	 */
	private static final long MIN_CLICK_INTERVAL=600;
	private long mLastClickTime;
	public abstract void onListItemSingleClick(View parent, View view, int position, long id);

	public OnOneOffItemClickListener() {
		// TODO Auto-generated constructor stub
	}

	/* (non-Javadoc)
	 * @see android.widget.AdapterView.OnItemClickListener#onItemClick(android.widget.AdapterView, android.view.View, int, long)
	 */
	@Override
	public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
		// TODO Auto-generated method stub
		long currentClickTime=SystemClock.uptimeMillis();
        long elapsedTime=currentClickTime-mLastClickTime;
       
        mLastClickTime=currentClickTime;

        if(elapsedTime<=MIN_CLICK_INTERVAL)
            return;

        onListItemSingleClick(parent, view, position, id);        

	}

}
