package com.guardianapp.interfaces;

import com.guardianapp.Guardian;

/**
 * @author v-dhmadd
 * interface implemented by the BaseActivity
 */
public interface AsyncActivity {
	
	public Guardian getApplicationContext();

	public void showLoadingProgressDialog();

	public void showProgressDialog(CharSequence message);

	public void dismissProgressDialog();

}
