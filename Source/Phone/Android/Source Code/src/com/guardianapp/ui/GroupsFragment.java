package com.guardianapp.ui;


import java.util.ArrayList;

import android.annotation.SuppressLint;
import android.app.AlertDialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Typeface;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.guardianapp.R;
import com.guardianapp.adapters.GroupListAdapter;
import com.guardianapp.adapters.SearchGroupAdapter;
import com.guardianapp.database.DBQueries;
import com.guardianapp.model.Group;
import com.guardianapp.model.GroupList;
import com.guardianapp.model.ProfileList;
import com.guardianapp.services.LiveAccount;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.webservicecomponents.MakeHTTPServices;
import com.guardianapp.webservicecomponents.OnTaskCompleted;



@SuppressLint("NewApi") 
public class GroupsFragment extends Fragment implements OnTaskCompleted{

	ArrayList<Group>groupList;
	ArrayList<String>serchedGroupList;
	GroupListAdapter mGroupAdapter;
	ListView mGroupListView;
	GroupList grList;
	Group group;
	AlertDialog serchGroupsDialog, addGroupDialog , addGroupsDialog;
	SharedPreferences prefs_LiveAttributes;

	@Override
	public View onCreateView(LayoutInflater inflater, ViewGroup container,
			Bundle savedInstanceState) {
		View view = null;

		prefs_LiveAttributes = this.getActivity().getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

		if(prefs_LiveAttributes.getBoolean(AppConstant.isLiveRagister, false))
		{

			if(prefs_LiveAttributes.getBoolean(AppConstant.isProfileDone, false))
			{
				view = inflater.inflate(R.layout.groups_fragment, container, false);	

				DBQueries mQuery = new DBQueries(getActivity());
				groupList = new ArrayList<Group>();
				groupList = mQuery.SelectAllGroups(); 


				if(groupList!=null){

					mGroupAdapter = new GroupListAdapter(getActivity(),groupList);
					mGroupListView = (ListView) view.findViewById(R.id.lv_groupsList);
					mGroupListView.setEmptyView(view.findViewById(android.R.id.empty));
					mGroupListView.setAdapter(mGroupAdapter);

				}

				view.findViewById(R.id.rel_JoinBtn).setOnClickListener(new OnClickListener() {

					@Override
					public void onClick(View v) {

						showSearchGroupDealog();

					}
				});
			}
			else{
				Intent intent = new Intent(this.getActivity(),CreateProfileActivity.class);
				startActivityForResult(intent,AppConstant.CREATE_PROFILE_ACTIVITY_RESULT);
			}


		}
		else
		{
			view = inflater.inflate(R.layout.create_profie_fragment, container, false);
			TextView grouptext = (TextView) view.findViewById(R.id.tv_createProfHeadding);
			grouptext.setText(getText(R.string.createProf_GroupHeading));
			view.findViewById(R.id.hyperLink).setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v){

					Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse(AppConstant.guardianPortalLink));
					startActivity(browserIntent);

				}
			});

			Button connectToMsLive = (Button) view.findViewById(R.id.btn_connectToMSLive);
			Typeface face=Typeface.createFromAsset(getActivity().getAssets(), "SEGOEUI.TTF");
			connectToMsLive.setTypeface(face);



			connectToMsLive.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {


					LiveAccount guardianLive = new LiveAccount(getActivity(),GroupsFragment.this);
					guardianLive.connectToLiveAccount();

				}

			});

		}

		return view;
	}





	@Override
	public void onActivityResult(int requestCode, int resultCode, Intent data) {

		super.onActivityResult(requestCode, resultCode, data);

		if(requestCode==AppConstant.CREATE_PROFILE_ACTIVITY_RESULT||
				requestCode==AppConstant.CREATE_PROFILE_ACTIVITY_RESULT
				||requestCode==AppConstant.UNREGISTER_SERVICE_TAG)
		{

			updateUI();
		}
	}



	private void addGroupDialog(final Group mgroup) {
		final View view = getActivity().getLayoutInflater().inflate(R.layout.add_group_dialog, null);
		addGroupsDialog = new AlertDialog.Builder(getActivity()).setView(view).create();

		final Button addBtn = (Button) view.findViewById(R.id.btnAdd);
		group = mgroup;


		addBtn.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				final EditText et_checkEnroll = (EditText) view.findViewById(R.id.editTxt_check);

				if(shouldJoinGroup(group , et_checkEnroll.getText().toString()))
				{
					GroupsFragment.this.saveGroupInfo(et_checkEnroll.getText().toString());
				}


			}
		});
		addGroupsDialog.show();

	}

	private void saveGroupInfo(String enrollmentValue){
		DBQueries dbgroup = new DBQueries(getActivity().getApplicationContext());
		AppConstant.service_Tag = AppConstant.GROUP_SERVICE_TAG;
		if(enrollmentValue!=null)
			group.setEnrollmentValue(enrollmentValue);
		else
			group.setEnrollmentValue("");
		if(dbgroup.storeIntoDB(group).equalsIgnoreCase(DBQueries.EXECUTE_SUCCESS))
		{
			AppConstant.ascGroups = dbgroup.SelectAllAsgGroups();
			if(groupList!=null&&groupList.size()>0)
			{
				groupList.add(group);
				mGroupAdapter.notifyDataSetChanged();
				mGroupListView.setAdapter(mGroupAdapter);
			}
			else
			{
				if(groupList==null)
					groupList = new ArrayList<Group>();
				groupList = dbgroup.SelectAllGroups();
				mGroupAdapter = new GroupListAdapter(getActivity(),groupList);
				mGroupListView.setAdapter(mGroupAdapter);
			}
			//phani
			AppConstant.UpdateIsDataSynced(getActivity(),false);

			if(addGroupsDialog!=null && addGroupsDialog.isShowing())
				addGroupsDialog.dismiss();
			if(serchGroupsDialog!=null && serchGroupsDialog.isShowing())
				serchGroupsDialog.dismiss();
		}

	}

	private boolean shouldJoinGroup(Group group, String groupEnrollmentKey){
		if(group.getEnrollmentType().equalsIgnoreCase(AppConstant.PRIVATE_GROUP_TYPE)){
			if(groupEnrollmentKey.contains(group.getEnrollmentKey())){
				return true;
			}
			Toast.makeText(this.getActivity(), this.getString(R.string.invalid_enrollment_key), Toast.LENGTH_LONG).show();	
			return false;
		}else if(group.getEnrollmentType().equalsIgnoreCase(AppConstant.PUBLIC_GROUP_TYPE)){
			return true;
		}
		return true;
	}
	private void showSearchGroupDealog() {

		View view = getActivity().getLayoutInflater().inflate(R.layout.search_buddies_dialog, null);

		if(serchGroupsDialog != null && serchGroupsDialog.isShowing())
			serchGroupsDialog.dismiss();
		serchGroupsDialog = new AlertDialog.Builder(getActivity())
		.setView(view).create();
		final ImageView searchBtn = (ImageView) view.findViewById(R.id.iv_search);
		final EditText et_search = (EditText) view.findViewById(R.id.et_search);
		et_search.setHint("");
		final ListView searchGroupLV = (ListView) view.findViewById(R.id.lv_Searchbuddies);
		ImageView ivCross = (ImageView)view.findViewById(R.id.ivCross);
		ivCross.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) 
			{
				serchGroupsDialog.dismiss();
			}
		});
		searchBtn.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {
				MakeHTTPServices services = new MakeHTTPServices(getActivity(),GroupsFragment.this);
				((BaseActivity)getActivity()).showProgressDialog("Loading Groups. Please wait...");
				services.getListOfGroups(et_search.getText().toString());
			}
		});


		if(grList!=null)
		{

			if(serchedGroupList!=null)
				serchedGroupList.clear();
			else
				serchedGroupList = new ArrayList<String>();

			for(int i=0;i<grList.getList().size();i++)
			{
				serchedGroupList.add(grList.getList().get(i).getGroupName());
			}

			SearchGroupAdapter searchAdapter = new SearchGroupAdapter(getActivity(),serchedGroupList);
			searchGroupLV.setAdapter(searchAdapter);
		}

		searchGroupLV.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> arg0, View v, int position,
					long arg3) {

				if(checkGroupAvilablity(grList.getList().get(position).getGroupName()))
				{
					Toast.makeText(getActivity(),"The selected group is already exist", Toast.LENGTH_LONG).show();
				}
				else if(grList.getList().get(position).getEnrollmentType().equalsIgnoreCase(AppConstant.PRIVATE_GROUP_TYPE))
				{
					addGroupDialog(grList.getList().get(position));
					serchGroupsDialog.dismiss();
				}else if(grList.getList().get(position).getEnrollmentType().equalsIgnoreCase(AppConstant.PUBLIC_GROUP_TYPE))
				{
					group = grList.getList().get(position);
					serchGroupsDialog.dismiss();
					saveGroupInfo(null);
				}

			}


		});
		serchGroupsDialog.show();
	}



	@Override
	public void onTaskComplete(String result) {
		AppConstant.dismissProgressDialog();
		Log.v("Response===",result);
		updateUI();
	}


	@Override
	public void onGetObjectResult(Object obj) {
		AppConstant.dismissProgressDialog();
		((BaseActivity)getActivity()).dismissProgressDialog();
		if(obj instanceof ProfileList){
			//refreshGroupFragment();
			updateUI();
		}
		if(obj instanceof GroupList){
			grList = (GroupList) obj;
			showSearchGroupDealog() ;
		}


	}
	private boolean checkGroupAvilablity(String name) {


		DBQueries mQuery = new DBQueries(getActivity());
		ArrayList<String> List = mQuery.SelectAllGroupsName(); 

		for(int i=0; i<List.size();i++)
		{
			if(List.get(i).equalsIgnoreCase(name))
			{
				return true;
			}

		}
		return false;

	}
	public void updateUI() {
		Fragment currentFragment = GroupsFragment.this;
		FragmentTransaction fragTransaction = getFragmentManager().beginTransaction();
		fragTransaction.detach(currentFragment);
		fragTransaction.attach(currentFragment);
		fragTransaction.commit();
	}


}
