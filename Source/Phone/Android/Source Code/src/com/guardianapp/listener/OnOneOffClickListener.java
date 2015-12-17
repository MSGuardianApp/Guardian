package com.guardianapp.listener;

import android.os.SystemClock;
import android.view.View;
/**
 * @author v-dhmadd
 *Restricts the multiple clicks on a button. Ensures that multiple activities are not opened even if a button is 
 *clicked multiple no. of times instantaneously . 
 */
public abstract class OnOneOffClickListener implements View.OnClickListener {
    
    private static final long MIN_CLICK_INTERVAL=600;
    private long mLastClickTime;
    
    public abstract void onSingleClick(View v);

    @Override
    public final void onClick(View v) {
        long currentClickTime=SystemClock.uptimeMillis();
        long elapsedTime=currentClickTime-mLastClickTime;
       
        mLastClickTime=currentClickTime;

        if(elapsedTime<=MIN_CLICK_INTERVAL)
            return;

        onSingleClick(v);        
    }

}


