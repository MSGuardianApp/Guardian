package com.guardianapp.utilities;

import android.os.SystemClock;



public class RetryHandler implements HttpConnectionRetryHandler {

	@Override
	public boolean shouldRetry(Throwable t, int attemptNumber) {
		// TODO Auto-generated method stub
		LogUtils.LOGE(LogUtils.makeLogTag(RetryHandler.class), "Attempt ["+attemptNumber+"]");
		if (attemptNumber > AppConstant.RETRY_MAX_COUNT)
			return false;
		
		SystemClock.sleep(AppConstant.RETRY_SLEEP_TIME_MS);
		return true;
	}


}
