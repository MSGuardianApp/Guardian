package com.guardianapp.utilities;

public interface HttpConnectionRetryHandler {
	
	public boolean shouldRetry(Throwable t, int attemptNumber) ;

}
