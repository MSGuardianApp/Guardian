package com.guardianapp.listener;

import com.guardianapp.views.ScrollViewExt;

/**
 * @author v-dhmadd
 * used in the preference screen to identifiy the scroll position to refresh the fb groups when the scrollposition moves to the bottom
 */
public interface ScrollViewListener {
	void onScrollChanged(ScrollViewExt scrollView, 
            int x, int y, int oldx, int oldy);

}
