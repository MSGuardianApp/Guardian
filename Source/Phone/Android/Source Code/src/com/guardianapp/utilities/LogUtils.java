package com.guardianapp.utilities;

/**
 * 
 */

import android.util.Log;

import com.guardianapp.BuildConfig;

/**
 * @author dharani
 * 
 * Android advises that a deployed application should not contain logging code. 
 * The Android development tools provide the BuildConfig.DEBUG flag for this purpose. 
 * This flag will be automatically set to false if you export the Android application for deployment. 
 * During development it will be set to true, therefore allowing you to see your logging statements
 * during development.
 *
 */
public class LogUtils {

	private static final String LOG_PREFIX = "Guardian_";
	private static final int LOG_PREFIX_LENGTH = LOG_PREFIX.length();
	private static final int MAX_LOG_TAG_LENGTH = 23;

	/**
	 * 
	 */
	private LogUtils() {
		// TODO Auto-generated constructor stub
	}

	public static String makeLogTag(String str) {
		if (str.length() > MAX_LOG_TAG_LENGTH - LOG_PREFIX_LENGTH) {
			return LOG_PREFIX + str.substring(0, MAX_LOG_TAG_LENGTH - LOG_PREFIX_LENGTH - 1);
		}

		return LOG_PREFIX + str;
	}

	public static String makeLogTag(Class<?> cls) {
		return makeLogTag(cls.getSimpleName());
	}

	public static void LOGD(final String tag, String message) {
		if (Log.isLoggable(tag, Log.DEBUG)) {
			Log.d(tag, message);
		}
	}

	public static void LOGD(final String tag, String message, Throwable cause) {
		if (Log.isLoggable(tag, Log.DEBUG)) {
			Log.d(tag, message, cause);
		}
	}

	public static void LOGV(final String tag, String message) {
		if (BuildConfig.DEBUG && Log.isLoggable(tag, Log.VERBOSE)) {
			Log.v(tag, message);
		}
	}

	public static void LOGV(final String tag, String message, Throwable cause) {
		if (BuildConfig.DEBUG && Log.isLoggable(tag, Log.VERBOSE)) {
			Log.v(tag, message, cause);
		}
	}
	
	public static void LOGE(final String tag, String message) {
		if (Log.isLoggable(tag, Log.ERROR)) {
			Log.e(tag, message);
		}
	}

	public static void LOGE(final String tag, String message, Throwable cause) {
		if (Log.isLoggable(tag, Log.ERROR)) {
			Log.e(tag, message, cause);
		}
	}

}
