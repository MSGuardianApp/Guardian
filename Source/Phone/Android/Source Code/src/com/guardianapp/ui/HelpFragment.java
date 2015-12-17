package com.guardianapp.ui;

import android.annotation.SuppressLint;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.ImageView;

import com.guardianapp.R;

@SuppressLint({ "NewApi", "ValidFragment" }) 
public class HelpFragment extends Fragment implements OnClickListener{
	int imageResourceId;


	public HelpFragment(int i) {
		imageResourceId = i;
	}


	public View onCreateView(LayoutInflater inflater, ViewGroup container, 
			Bundle savedInstanceState){
		View view = inflater.inflate(R.layout.help_fragment, container, false);
		ImageView screenShots = (ImageView) view.findViewById(R.id.helpImages);
		screenShots.setBackgroundResource(imageResourceId);
		return view;
	}

	@Override
	public void onClick(View arg0) {
		// TODO Auto-generated method stub

	}






}
