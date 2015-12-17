package com.guardianapp.helpercomponents;

import org.json.JSONException;

import com.guardianapp.ui.HomeScreenActivity;
import com.guardianapp.utilities.AppConstant;

import android.annotation.SuppressLint;
import android.app.AlertDialog;
import android.app.DownloadManager;
import android.app.DownloadManager.Request;
import android.content.ActivityNotFoundException;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.net.Uri;
import android.os.Environment;

@SuppressLint("NewApi") 
public class VersionCheckReceiver extends BroadcastReceiver {

	private DownloadManager downloadManager;
	public static long downloadReference;
	public static final String PROCESS_RESPONSE = "com.guardianapp.helpercomponents.action.PROCESS_RESPONSE";
	public static final String LATEST_VERSION = "latest_version";

	@Override
	public void onReceive(Context context, Intent intent) {
		// TODO Auto-generated method stub
		final Context appContext = context;
		String latestVersion = intent.getStringExtra(LATEST_VERSION);

		AlertDialog.Builder builder = new AlertDialog.Builder(context);
		builder.setMessage("There is newer version of this application available, click OK to upgrade now?")
		.setPositiveButton("OK", new DialogInterface.OnClickListener() {
			//if the user agrees to upgrade
			public void onClick(DialogInterface dialog, int id) {
					Uri uri = Uri.parse("market://details?id=" + appContext.getPackageName()+"&hl=en");
					Intent goToMarket = new Intent(Intent.ACTION_VIEW, uri);
					try {
						appContext.startActivity(goToMarket);
					} catch (ActivityNotFoundException e) {
						appContext.startActivity(new Intent(Intent.ACTION_VIEW, Uri.parse(AppConstant.PLAYSTORE_URL + appContext.getApplicationContext().getPackageName()+"&hl=en")));
					}
			}
		})
		.setNegativeButton("Remind Later", new DialogInterface.OnClickListener() {
			public void onClick(DialogInterface dialog, int id) {
				// User cancelled the dialog
			}
		});
		//show the alert message
		builder.create().show();
	}

}
