package com.guardianapp.ui;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ListView;
import android.widget.Toast;

import com.guardianapp.R;
import com.guardianapp.adapters.VersionAdapter;
import com.guardianapp.model.Version;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.LogUtils;

public class AboutFragment extends Fragment implements OnClickListener{
	int LayoutId;

	public AboutFragment(int i) {
		LayoutId = i;
	}


	public View onCreateView(LayoutInflater inflater, ViewGroup container, 
			Bundle savedInstanceState){
		View view = inflater.inflate(LayoutId, container, false);

		if(LayoutId == R.layout.version_history_layout)
		{
			VersionAdapter vAdapter = new VersionAdapter(getActivity(), getVersionList());
			ListView versionListView = (ListView) view.findViewById(R.id.lv_versionList);
			versionListView.setAdapter(vAdapter);
		}

		if(LayoutId == R.layout.logger_layout)
		{
			Button sendReportButton = (Button) view.findViewById(R.id.sendReportBut);
			sendReportButton.setOnClickListener(this);
		}

		return view;
	}



	@Override
	public void onClick(View view) {
		// TODO Auto-generated method stub
		switch(view.getId()){

		case R.id.sendReportBut :
			this.sendEmailWithLogAttachment();
			break;
		}

	}

	private void sendEmailWithLogAttachment(){
		Intent sendIntent = new Intent(Intent.ACTION_SEND);
		String subject = "Your App crashed! Fix it!";
		StringBuilder body = new StringBuilder("");
		body.append('\n').append('\n');
		body.append(AppConstant.LOG_EMAIL_BODY).append('\n').append('\n');
		sendIntent.putExtra(Intent.EXTRA_EMAIL, new String[] { AppConstant.GUARDIAN_CORE_TEAM_MAIL });
		sendIntent.putExtra(Intent.EXTRA_TEXT, body.toString());
		sendIntent.putExtra(Intent.EXTRA_SUBJECT, subject);
		sendIntent.setType("message/rfc822");
		String path = Environment.getExternalStorageDirectory() + AppConstant.DIRECTORY_SEPARATOR + AppConstant.LOGS_PARENT_DIR + AppConstant.DIRECTORY_SEPARATOR
				+ AppConstant.LOGS_CHILD_DIR + AppConstant.DIRECTORY_SEPARATOR;
		
		String fullName = path + AppConstant.LOG_FILE_NAME;
		File file = new File (fullName);
		if(!file.exists() || !file.canRead()){
			Toast.makeText(this.getActivity(), "File doesn't exists or cannot be read", Toast.LENGTH_LONG).show();
			return;
		}
		
		BufferedReader br = null;
		try {
			br = new BufferedReader(new FileReader(file));
			if (br.readLine() == null ) {
				Toast.makeText(this.getActivity(), "No Errors logged", Toast.LENGTH_LONG).show();
				return;
			}
		} catch (FileNotFoundException ffe) {
			// TODO Auto-generated catch block
			LogUtils.LOGE(this.getActivity().getClass().getName(),ffe.getLocalizedMessage());
		} catch (IOException ioe) {
			// TODO Auto-generated catch block
			LogUtils.LOGE(this.getActivity().getClass().getName(),ioe.getLocalizedMessage());
		} catch (Exception e){
			LogUtils.LOGE(this.getActivity().getClass().getName(),e.getLocalizedMessage());
		} finally{
			try {
				br.close();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				LogUtils.LOGE(this.getActivity().getClass().getName(),e.getLocalizedMessage());
			}
		}
		
		Uri uri = Uri.fromFile(file);
		sendIntent.putExtra(Intent.EXTRA_STREAM, uri);
		startActivity(Intent.createChooser(sendIntent, "Pick an Email Provider"));
	}


	private ArrayList<Version> getVersionList()
	{
		ArrayList<Version> versionList = new ArrayList<Version>();
		for (int i = 0; i < AppConstant.version.length; i++) {
			Version version = new Version();
			version.setVersion(AppConstant.version[i]);
			version.setVersionHistory(AppConstant.versionHIstory[i]);
			versionList.add(version);
		}

		return versionList;

	}


}
