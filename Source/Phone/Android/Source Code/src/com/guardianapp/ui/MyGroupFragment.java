package com.guardianapp.ui;

import java.util.ArrayList;
import java.util.Observable;
import java.util.Observer;

import android.content.Intent;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.guardianapp.R;
import com.guardianapp.adapters.SOSGroupsAdapter;
import com.guardianapp.database.DBQueries;
import com.guardianapp.helpercomponents.NetworkChangeReceiver;
import com.guardianapp.model.Group;
import com.guardianapp.utilities.AppConstant;

public class MyGroupFragment extends Fragment implements OnClickListener , Observer{


	private LinearLayout btnAmbulance,btnPolice, btnFirebrigade;
	private SOSGroupsAdapter sosGroupAdapter;
	ArrayList<Group> groupList;
	ListView sosGroupListView;

	@Override
	public View onCreateView(LayoutInflater inflater, ViewGroup container,
			Bundle savedInstanceState) {
		View view;
		view = inflater.inflate(R.layout.sos_groups_fragment, container, false);
		initControls(view);
		createGroupsListView(view);
		return view;
	}
	
	@Override
	public void onResume() {
		// TODO Auto-generated method stub
		super.onResume();
		NetworkChangeReceiver.getObservable().addObserver(this);
	}
	
	@Override
	public void onDestroy() {
		// TODO Auto-generated method stub
		super.onDestroy();
		NetworkChangeReceiver.getObservable().deleteObserver(this);
	}
	
	private void initControls(View v){
		TextView emptyTV = (TextView)v.findViewById(android.R.id.empty);
		emptyTV.setOnClickListener(this);
	}

	private void createGroupsListView(View view){
		groupList = new ArrayList<Group>();
		DBQueries mQuery = new DBQueries(getActivity());
		groupList = mQuery.SelectAllGroups();
		if(groupList!=null){
			sosGroupAdapter = new SOSGroupsAdapter(getActivity(),groupList);
			sosGroupListView = (ListView) view.findViewById(R.id.lv_buddiesList);
			sosGroupListView.setEmptyView(view.findViewById(android.R.id.empty));
			sosGroupListView.setAdapter(sosGroupAdapter);
		}
	}

	@Override
	public void onClick(View v) {
		// TODO Auto-generated method stub
		Intent intent = new Intent(this.getActivity() , SettingsActivity.class);
		intent.putExtra("open_group_fragment", true);
		startActivity(intent);
	}

	@Override
	public void update(Observable observable, Object data) {
		// TODO Auto-generated method stub
		if((Integer)data == AppConstant.NO_NETWORK){
          Toast.makeText(this.getActivity(), this.getString(R.string.internet_unavailable), Toast.LENGTH_LONG).show();
		}
		
	}
}
