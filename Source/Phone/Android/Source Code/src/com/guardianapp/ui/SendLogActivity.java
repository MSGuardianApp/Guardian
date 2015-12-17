package com.guardianapp.ui;

import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStreamReader;

import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.os.Build;
import android.os.Bundle;
import android.os.Environment;
import android.view.View;
import android.view.Window;
import android.widget.Button;

import com.guardianapp.R;

public class SendLogActivity extends BaseActivity{
	
	private Button sendLogButton, cancelButton;
	
	@Override
	protected void onCreate(Bundle savedInstance) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstance);
		requestWindowFeature (Window.FEATURE_NO_TITLE); 
		setContentView(R.layout.activity_send_log);
	}

	@Override
	protected void initFields() {
		// TODO Auto-generated method stub
		sendLogButton = (Button)findViewById(R.id.send_report);
		cancelButton = (Button)findViewById(R.id.cancel);
	}

	@Override
	protected void initCallBacks() {
		// TODO Auto-generated method stub
		sendLogButton.setOnClickListener(new View.OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				
				SendLogActivity.this.sendLogFile();
				SendLogActivity.this.extractLogToFile();
			}
		});
		
		
		cancelButton.setOnClickListener(new View.OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				finish();
				
			}
		});
		

	}
	
	private void sendLogFile(){
		
	}
	
	private String extractLogToFile(){
		PackageManager manager = this.getPackageManager();
		  PackageInfo info = null;
		  try {
		    info = manager.getPackageInfo (this.getPackageName(), 0);
		  } catch (NameNotFoundException e2) {
		  }
		  String model = Build.MODEL;
		  if (!model.startsWith(Build.MANUFACTURER))
		    model = Build.MANUFACTURER + " " + model;

		  // Make file name - file must be saved to external storage or it wont be readable by
		  // the email app.
		  String path = Environment.getExternalStorageDirectory() + "/" + "Guardian/";
		  String fullName = path + "guardian_log_file";

		  // Extract to file.
		  File file = new File (fullName);
		  InputStreamReader reader = null;
		  FileWriter writer = null;
		  try
		  {
		    // For Android 4.0 and earlier, you will get all app's log output, so filter it to
		    // mostly limit it to your app's output.  In later versions, the filtering isn't needed.
		    String cmd = (Build.VERSION.SDK_INT <= Build.VERSION_CODES.ICE_CREAM_SANDWICH_MR1) ?
		                  "logcat -d -v time Guardian:v dalvikvm:v System.err:v *:s" :
		                  "logcat -d -v time";

		    // get input stream
		    Process process = Runtime.getRuntime().exec(cmd);
		    reader = new InputStreamReader (process.getInputStream());

		    // write output stream
		    writer = new FileWriter (file);
		    writer.write ("Android version: " +  Build.VERSION.SDK_INT + "\n");
		    writer.write ("Device: " + model + "\n");
		    writer.write ("App version: " + (info == null ? "(null)" : info.versionCode) + "\n");

		    char[] buffer = new char[10000];
		    do 
		    {
		      int n = reader.read (buffer, 0, buffer.length);
		      if (n == -1)
		        break;
		      writer.write (buffer, 0, n);
		    } while (true);

		    reader.close();
		    writer.close();
		  }
		  catch (IOException e)
		  {
		    if (writer != null)
		      try {
		        writer.close();
		      } catch (IOException e1) {
		      }
		    if (reader != null)
		      try {
		        reader.close();
		      } catch (IOException e1) {
		      }

		    // You might want to write a failure message to the log here.
		    return null;
		  }

		  return fullName;
	}

}
