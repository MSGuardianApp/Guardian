package com.guardianapp.ui;

import java.util.ArrayList;
import java.util.Observable;
import java.util.Observer;

import android.app.AlertDialog;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;

import com.guardianapp.R;
import com.guardianapp.adapters.SOSBuddiesAdapter;
import com.guardianapp.database.DBQueries;
import com.guardianapp.helpercomponents.NetworkChangeReceiver;
import com.guardianapp.model.MyBuddies;
import com.guardianapp.utilities.AppConstant;


public class MyBuddiesFragment extends Fragment implements OnClickListener , Observer{

	private LinearLayout btnAmbulance,btnPolice, btnFirebrigade;
	private SOSBuddiesAdapter sosBuddieAdapter;
	private ArrayList<MyBuddies> buddiesList;
	private ListView sosBuddiesListView;
	private TextView unableToReachMsgTV;

	@Override
	public View onCreateView(LayoutInflater inflater, ViewGroup container,
			Bundle savedInstanceState) {
		View view;
		view = inflater.inflate(R.layout.sos_buddies_fragment, container, false);
		this.initControls(view);
		this.createBuddiesListView(view);
		return view;
	}

	private void initControls(View v) {
		btnPolice = (LinearLayout) v.findViewById(R.id.btnPolice);
		btnPolice.setOnClickListener(this);
		btnAmbulance = (LinearLayout) v.findViewById(R.id.btnAmbulance);
		btnAmbulance.setOnClickListener(this);
		btnFirebrigade =(LinearLayout) v.findViewById(R.id.btnFire);
		btnFirebrigade.setOnClickListener(this);
		LinearLayout emer_rel = (LinearLayout) v.findViewById(R.id. rel_Emergency);
		emer_rel.setVisibility(View.VISIBLE);
		unableToReachMsgTV = (TextView)v.findViewById(R.id.unable_to_reach);
		TextView emptyTV = (TextView)v.findViewById(android.R.id.empty);
		emptyTV.setOnClickListener(this);
	}

	@Override
	public void onResume() {
		// TODO Auto-generated method stub
		super.onResume();
		NetworkChangeReceiver.getObservable().addObserver(this);
	}


	@Override
	public void onPause() {
		// TODO Auto-generated method stub
		super.onPause();
		NetworkChangeReceiver.getObservable().deleteObserver(this);
	}

	private void createBuddiesListView(View view){
		buddiesList = new ArrayList<MyBuddies>();
		DBQueries mQuery = new DBQueries(getActivity());
		buddiesList = mQuery.SelectAllActiveBuddies();

		if(buddiesList!=null){
			sosBuddieAdapter = new SOSBuddiesAdapter(getActivity(),buddiesList);
			sosBuddiesListView = (ListView) view.findViewById(R.id.lv_buddiesList);
			sosBuddiesListView.setEmptyView(view.findViewById(android.R.id.empty));
			sosBuddiesListView.setAdapter(sosBuddieAdapter);
		}
	}


	@Override
	public void onClick(View v) {
		Intent i = new Intent(Intent.ACTION_CALL);
		String p = "tel:";
		switch (v.getId()) {
		case R.id.btnPolice:
			p+=AppConstant.userProfile.getPoliceContact();
			i.setData(Uri.parse(p));
			startActivity(i);
			break;
		case R.id.btnAmbulance:
			p+=AppConstant.userProfile.getAmbulanceContact();
			i.setData(Uri.parse(p));
			startActivity(i);
			break;
		case R.id.btnFire:
			p+=AppConstant.userProfile.getFireContact();
			i.setData(Uri.parse(p));
			startActivity(i);
			break;

		case android.R.id.empty:
			Intent intent = new Intent(this.getActivity() , SettingsActivity.class);
			intent.putExtra("open_buddy_fragment", true);
			startActivity(intent);



		default:
			break;
		}

	}

	private void callEmergencyContactDialog(String msg,final String number) {

		View view = getActivity().getLayoutInflater().inflate(R.layout.common_dialog, null);
		final AlertDialog callMsgDialog = new AlertDialog.Builder(getActivity())
		.setView(view).create();

		TextView title = (TextView) view.findViewById(R.id.tv_title);	
		title.setText("Phone");


		TextView msgTxt = (TextView) view.findViewById(R.id.tv_msgAleart);	
		msgTxt.setText(msg);
		Button callbtn = (Button) view.findViewById(R.id.btnOK);
		callbtn.setText("call");

		Button cancelBtn = (Button) view.findViewById(R.id.btnCancel);
		cancelBtn.setText("don't call");
		view.findViewById(R.id.btnOK).setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {


				Intent i = new Intent(Intent.ACTION_CALL);
				String p = "tel:" + number;
				i.setData(Uri.parse(p));
				startActivity(i);
				callMsgDialog.dismiss();
			}
		});

		view.findViewById(R.id.btnCancel).setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {
				callMsgDialog.dismiss();

			}
		});

		callMsgDialog.show();
	}

	@Override
	public void update(Observable observable, Object data) {
		// TODO Auto-generated method stub
		if((Integer)data == AppConstant.NO_NETWORK){
			this.unableToReachMsgTV.setVisibility(View.VISIBLE);
		}else{
			this.unableToReachMsgTV.setVisibility(View.GONE);
		}

	}
}
