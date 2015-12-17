package com.guardianapp.utilities;

import static android.os.Build.VERSION.SDK_INT;
import static android.os.Build.VERSION_CODES.GINGERBREAD;
import static android.os.Build.VERSION_CODES.GINGERBREAD_MR1;
import static android.os.Build.VERSION_CODES.HONEYCOMB;
import static android.os.Build.VERSION_CODES.HONEYCOMB_MR1;
import static android.os.Build.VERSION_CODES.ICE_CREAM_SANDWICH;
import static android.os.Build.VERSION_CODES.JELLY_BEAN;
import static android.os.Build.VERSION_CODES.JELLY_BEAN_MR1;
import static android.os.Build.VERSION_CODES.JELLY_BEAN_MR2;


public class Versions {
	public static boolean isApiLevelAvailable(int level) {
		return SDK_INT >= level;
	}
	public static boolean hasGingerbreadApi() {
		return isApiLevelAvailable(GINGERBREAD);
	}
	public static boolean hasGingerbreadMr1Api() {
		return isApiLevelAvailable(GINGERBREAD_MR1);
	}
	public static boolean hasHoneycombApi() {
		return isApiLevelAvailable(HONEYCOMB);
	}
	public static boolean hasHoneycombMr1Api() {
		return isApiLevelAvailable(HONEYCOMB_MR1);
	}
	public static boolean hasICSApi() {
		return isApiLevelAvailable(ICE_CREAM_SANDWICH);
	}
	public static boolean hasJellyBeanApi() {
		return isApiLevelAvailable(JELLY_BEAN);
	}
	public static boolean hasJellyBeanMr1Api() {
		return isApiLevelAvailable(JELLY_BEAN_MR1);
	}
	public static boolean hasJellyBeanMr2Api() {
		return isApiLevelAvailable(JELLY_BEAN_MR2);
	}
	
}