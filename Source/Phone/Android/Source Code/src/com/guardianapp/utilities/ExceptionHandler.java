/**
 * 
 */
package com.guardianapp.utilities;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.io.StringWriter;
import java.lang.Thread.UncaughtExceptionHandler;
import java.text.SimpleDateFormat;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Build;
import android.os.Environment;
import android.os.Looper;
import android.util.Log;
import android.widget.Toast;

import com.guardianapp.R;

public class ExceptionHandler implements UncaughtExceptionHandler {

	private Activity context;
	private String LINE_SEPARATOR = "\n";
	private static final String RECIPIENT = AppConstant.GUARDIAN_CORE_TEAM_MAIL;

	public ExceptionHandler(Activity context) {
		this.context = context;
	}

	/* (non-Javadoc)
	 * @see java.lang.Thread.UncaughtExceptionHandler#uncaughtException(java.lang.Thread, java.lang.Throwable)
	 */
	@Override
	public void uncaughtException(Thread thread, Throwable exception) {
		this.addInformation(thread,exception);
	}

	private void addInformation(Thread thread, Throwable exception){
		StringWriter stackTrace = new StringWriter();
		exception.printStackTrace(new PrintWriter(stackTrace));
		StringBuilder errorReport = new StringBuilder();
		errorReport.append("************ CAUSE OF ERROR ************\n\n");
		errorReport.append(stackTrace.toString());

		errorReport.append("\n************ DEVICE INFORMATION ***********\n");
		errorReport.append("Brand: ");
		errorReport.append(Build.BRAND);
		errorReport.append(LINE_SEPARATOR);
		errorReport.append("Device: ");
		errorReport.append(Build.DEVICE);
		errorReport.append(LINE_SEPARATOR);
		errorReport.append("Model: ");
		errorReport.append(Build.MODEL);
		errorReport.append(LINE_SEPARATOR);
		errorReport.append("Id: ");
		errorReport.append(Build.ID);
		errorReport.append(LINE_SEPARATOR);
		errorReport.append("Product: ");
		errorReport.append(Build.PRODUCT);
		errorReport.append(LINE_SEPARATOR);
		errorReport.append("\n************ FIRMWARE ************\n");
		errorReport.append("SDK: ");
		errorReport.append(Build.VERSION.SDK);
		errorReport.append(LINE_SEPARATOR);
		errorReport.append("Release: ");
		errorReport.append(Build.VERSION.RELEASE);
		errorReport.append(LINE_SEPARATOR);
		errorReport.append("Incremental: ");
		errorReport.append(Build.VERSION.INCREMENTAL);
		errorReport.append(LINE_SEPARATOR);
		this.sendErrorMail(errorReport);
		this.extractLogToFile(errorReport);
	}

	private  void sendErrorMail(final StringBuilder errorContent) {
		final AlertDialog.Builder builder= new AlertDialog.Builder(context);
		new Thread(){
			@Override
			public void run() {
				Looper.prepare();
				builder.setTitle(context.getString(R.string.error_dialog_title));
				builder.create();
				builder.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
					@Override
					public void onClick(DialogInterface dialog, int which) {
						System.exit(0);
					}
				});
				builder.setPositiveButton("Send Report", new DialogInterface.OnClickListener() {
					@Override
					public void onClick(DialogInterface dialog, int which) {
						Intent sendIntent = new Intent(Intent.ACTION_SEND);
						String subject = "Your App crashed! Fix it!";
						StringBuilder body = new StringBuilder("");
						body.append('\n').append('\n');
						body.append(errorContent).append('\n').append('\n');
						sendIntent.putExtra(Intent.EXTRA_EMAIL, new String[] { RECIPIENT });
						sendIntent.putExtra(Intent.EXTRA_TEXT, body.toString());
						sendIntent.putExtra(Intent.EXTRA_SUBJECT, subject);
						sendIntent.setType("message/rfc822");
						context.startActivity(sendIntent);
						System.exit(0);
					}
				});
				builder.setMessage(context.getString(R.string.error_dialog_text));
				builder.show();
				Looper.loop();
			}
		}.start();  
	}

	private String extractLogToFile(final StringBuilder errorContent)
	{
		String timestamp = new SimpleDateFormat("yyyy-MM-dd,HH:mm:ss").format(System
				.currentTimeMillis());
		File dirFile = new File(Environment.getExternalStorageDirectory()
				+ AppConstant.DIRECTORY_SEPARATOR + AppConstant.LOGS_PARENT_DIR + AppConstant.DIRECTORY_SEPARATOR
				+ AppConstant.LOGS_CHILD_DIR + AppConstant.DIRECTORY_SEPARATOR);
		dirFile.mkdirs();
		File file = new File(dirFile, "guardian_log.txt");
		if(!file.exists()){
			try {
				file.createNewFile();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
		try {
			String stackString = errorContent.toString();
			if (stackString.length() > 0) {
				FileOutputStream fOut = new FileOutputStream(file,true);
	            OutputStreamWriter myOutWriter =new OutputStreamWriter(fOut);
	            myOutWriter.append("Exception Report logged at"+timestamp+"\n");
	            myOutWriter.append(stackString);
	            myOutWriter.append("\n \n \n");
	            myOutWriter.close();
	            fOut.close();
			}
		} catch (FileNotFoundException fileNotFoundException) {
			Log.e("TAG", "File not found!", fileNotFoundException);
		} catch (IOException ioException) {
			Log.e("TAG", "Unable to write to file!", ioException);
		}
		return file.getAbsolutePath();
	}

}
