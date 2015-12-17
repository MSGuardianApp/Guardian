package com.guardianapp.ui;

import java.util.ArrayList;
import java.util.List;

import android.annotation.SuppressLint;
import android.app.AlertDialog;
import android.content.ContentResolver;
import android.database.Cursor;
import android.database.CursorIndexOutOfBoundsException;
import android.os.Build;
import android.os.Bundle;
import android.provider.ContactsContract;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.AdapterView.OnItemSelectedListener;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.Spinner;
import android.widget.Toast;

import com.guardianapp.R;
import com.guardianapp.adapters.BuddiesListAdapter;
import com.guardianapp.adapters.SearchContactAdapter;
import com.guardianapp.database.DBQueries;
import com.guardianapp.model.MyBuddies;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.LogUtils;

@SuppressLint("NewApi") 
public class BuddiesFragment extends Fragment implements OnItemSelectedListener{
	private BuddiesListAdapter mBuddiesAdapter;
	private AlertDialog serchBuddiesDialog, addBuddiesDialog;
	private ListView mbuddiesListView;
	private ArrayList<MyBuddies> contactList;
	private ArrayList<MyBuddies> searchContactList;
	private DBQueries mQuery ;
	private String selectedPhoneNumber = "";
	@Override
	public View onCreateView(LayoutInflater inflater, ViewGroup container,
			Bundle savedInstanceState) {
		View view = inflater.inflate(R.layout.buddies_fragment, container, false);
		contactList = new ArrayList<MyBuddies>();
		searchContactList =  new ArrayList<MyBuddies>();
		mQuery = new DBQueries(getActivity());
		contactList = mQuery.SelectAllActiveBuddies();

		if(contactList!=null){
			mBuddiesAdapter = new BuddiesListAdapter(getActivity(),contactList);
			mbuddiesListView = (ListView) view.findViewById(R.id.lv_buddiesList);
			mbuddiesListView.setAdapter(mBuddiesAdapter);
		}

		view.findViewById(R.id.rel_AddButtons).setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				showSearchBuddiesDealog();
			}
		});

		return view;
	}


	@Override
	public void onResume() {
		super.onResume();

	}

	@Override
	public void onPause() {
		// TODO Auto-generated method stub
		super.onPause();

	}


	private void showSearchBuddiesDealog() {
		View view = getActivity().getLayoutInflater().inflate(R.layout.search_buddies_dialog, null);

		if(serchBuddiesDialog != null && serchBuddiesDialog.isShowing())
			serchBuddiesDialog.dismiss();
		serchBuddiesDialog = new AlertDialog.Builder(getActivity()).setView(view).create();
		final ImageView searchBtn = (ImageView) view.findViewById(R.id.iv_search);
		final EditText et_search = (EditText) view.findViewById(R.id.et_search);
		final ListView searchContactLV = (ListView) view.findViewById(R.id.lv_Searchbuddies);
		ImageView ivCross = (ImageView)view.findViewById(R.id.ivCross);
		ivCross.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) 
			{
				serchBuddiesDialog.dismiss();
			}
		});
		searchBtn.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {

				if(!et_search.getText().toString().trim().equalsIgnoreCase(""))
				{
					if(searchContactList!=null)
						searchContactList.clear();

					searchContactList = searchContacts(et_search.getText().toString().trim());
					if(searchContactList.size()>0){
						SearchContactAdapter searchAdapter = new SearchContactAdapter(getActivity(),searchContactList);
						searchContactLV.setAdapter(searchAdapter);
					}
					else
						Toast.makeText(getActivity(), BuddiesFragment.this.getString(R.string.no_contacts_msg) , Toast.LENGTH_LONG).show();
				}
				else
					Toast.makeText(getActivity(),BuddiesFragment.this.getString(R.string.no_contacts_msg), Toast.LENGTH_LONG).show();
			}
		});

		searchContactLV.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> arg0, View v, int position,
					long arg3) {

				if(searchContactList!=null)
					setDataFromName(searchContactList.get(position));
				serchBuddiesDialog.dismiss();
			}
		});
		serchBuddiesDialog.show();
	}


	private void setDataFromName(MyBuddies mbuddy)
	{
		List<String> phoneNumberList = new ArrayList<String>();
		Cursor phones = getActivity().getContentResolver().query(
				ContactsContract.CommonDataKinds.Phone.CONTENT_URI,
				null,
				ContactsContract.Contacts.DISPLAY_NAME
				+ " = " + "'"+mbuddy.getName()+"'", null, null);
		while (phones.moveToNext()) {
			mbuddy.setMobileNumber(phones
					.getString(phones
							.getColumnIndex(ContactsContract.CommonDataKinds.Phone.NUMBER)));
			phoneNumberList.add(AppConstant.formatPhoneNumber(phones
					.getString(phones
							.getColumnIndex(ContactsContract.CommonDataKinds.Phone.NUMBER)),AppConstant.getRegionCodeForCountryCode(AppConstant.getDialingCode(getActivity()))));
		}
		phones.close();
		Cursor eMail = getActivity().getContentResolver().query(
				ContactsContract.CommonDataKinds.Email.CONTENT_URI,
				null,
				ContactsContract.Contacts.DISPLAY_NAME
				+ " = " + "'"+mbuddy.getName()+"'", null, null);

		while (eMail.moveToNext()) {
			mbuddy.setEmail(eMail
					.getString(eMail
							.getColumnIndex(ContactsContract.CommonDataKinds.Phone.NUMBER)));
		}

		eMail.close();
		if(phoneNumberList!=null && phoneNumberList.size() > 0){
			this.selectedPhoneNumber = phoneNumberList.get(0);	
		}
		showAddBuddiesDealog(mbuddy , phoneNumberList);
	}

	private ArrayList<MyBuddies> searchContacts(String searchKey) {

		Cursor cur = getContactsByName(searchKey);
		ArrayList<MyBuddies> contactList = new ArrayList<MyBuddies>();
		try {
			if (cur!=null && cur.moveToFirst()) {
				do {

					MyBuddies buddy= new MyBuddies();
					buddy.setName(cur.getString(cur.getColumnIndex(ContactsContract.Contacts.DISPLAY_NAME)));
					contactList.add(buddy);

				} while (cur.moveToNext());
			}
		} catch (CursorIndexOutOfBoundsException ciobe) {
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), ciobe.getLocalizedMessage());
		} catch (IllegalArgumentException iae){
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), iae.getLocalizedMessage());
		} catch (Exception ex){
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), ex.getLocalizedMessage());
		}finally{
			cur.close();
		}
		return contactList;

	}

	private Cursor getContactsByName(String temp) {
		String selection = ContactsContract.Contacts.DISPLAY_NAME + " like '%"
				+ temp + "%'";
		String sortOrder = ContactsContract.Contacts.DISPLAY_NAME
				+ " COLLATE LOCALIZED ASC";
		ContentResolver cr = this.getActivity().getApplicationContext().getContentResolver();
		return cr.query(ContactsContract.Contacts.CONTENT_URI, new String[] { ContactsContract.Contacts._ID,
				ContactsContract.Contacts.DISPLAY_NAME, }, selection, null,sortOrder);
	}

	private void showAddBuddiesDealog(MyBuddies buddies , final List<String> phoneNumbersList) {

		View view = getActivity().getLayoutInflater().inflate(R.layout.add_buddies_dialog, null);
		addBuddiesDialog = new AlertDialog.Builder(getActivity())
		.setView(view).create();

		final EditText etName = (EditText) view.findViewById(R.id.editTxt_name);	
		final Spinner etPhoneNo = (Spinner) view.findViewById(R.id.phone_spinner);
		final EditText etCode = (EditText)view.findViewById(R.id.editTxt_country_code);
		etPhoneNo.setOnItemSelectedListener(this);
		ArrayAdapter<String> dataAdapter = new ArrayAdapter<String>(this.getActivity(),
				android.R.layout.simple_spinner_item, phoneNumbersList);
		dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		etPhoneNo.setAdapter(dataAdapter);
		etPhoneNo.setSelection(0);
		final EditText etEmailId = (EditText) view.findViewById(R.id.editTxt_email);	

		etName.setText(buddies.getName());
		etEmailId.setText(buddies.getEmail());
		
		if(buddies.getRegionCode()!=null && !buddies.getRegionCode().equalsIgnoreCase(""))
			etCode.setText(buddies.getRegionCode());
		else
			etCode.setText(AppConstant.userProfile.getCountryCode());

		view.findViewById(R.id.btnAdd).setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				if(!BuddiesFragment.this.validRequiredFields(phoneNumbersList , etName.getText().toString().trim(), etEmailId.getText().toString().trim())){
					mBuddiesAdapter.notifyDataSetChanged();
					mBuddiesAdapter = new BuddiesListAdapter(getActivity(),contactList);
					mbuddiesListView.setAdapter(mBuddiesAdapter);
					return;
				}
				if(!AppConstant.isValidPhoneNumber(etCode.getText().toString().trim()+BuddiesFragment.this.selectedPhoneNumber.trim())){
					Toast.makeText(BuddiesFragment.this.getActivity(), "The number is either not a valid one or belongs to a different country", Toast.LENGTH_LONG).show();
					mBuddiesAdapter.notifyDataSetChanged();
					mBuddiesAdapter = new BuddiesListAdapter(getActivity(),contactList);
					mbuddiesListView.setAdapter(mBuddiesAdapter);
					return;
				}
				if(contactList.size()<AppConstant.NO_OF_BUDDIES)
				{
					MyBuddies mBuddies = new MyBuddies();
					DBQueries query = new DBQueries(getActivity());
					mBuddies.setName(etName.getText().toString());
					mBuddies.setMobileNumber(etCode.getText().toString()+BuddiesFragment.this.selectedPhoneNumber);
					mBuddies.setEmail(etEmailId.getText().toString());
					mBuddies.setRegionCode(etCode.getText().toString());
					mBuddies.setState("1");

					if(contactList.contains(mBuddies)){
						addBuddiesDialog.dismiss();
						Toast.makeText(getActivity(),BuddiesFragment.this.getString(R.string.buddy_already_exists), Toast.LENGTH_LONG).show();
						return;
					}
					query.insertBuddyIntoDB(mBuddies);
					AppConstant.globalBuddies.add(mBuddies);
					AppConstant.callerBuddies = AppConstant.getPrimaryBuddy();

					if(contactList!=null&&contactList.size()>0)
					{
						contactList.add(mBuddies);
						mBuddiesAdapter.notifyDataSetChanged();
						mBuddiesAdapter = new BuddiesListAdapter(getActivity(),contactList);
						mbuddiesListView.setAdapter(mBuddiesAdapter);
					}
					else
					{
						if(contactList == null)
						contactList = new ArrayList<MyBuddies>();
						contactList = query.SelectAllActiveBuddies();
						mBuddiesAdapter.notifyDataSetChanged();
						mBuddiesAdapter = new BuddiesListAdapter(getActivity(),contactList);
						mbuddiesListView.setAdapter(mBuddiesAdapter);
					}
					AppConstant.UpdateIsDataSynced(getActivity(),false);
				}
				else
				{
					Toast.makeText(getActivity(), BuddiesFragment.this.getString(R.string.max_five), Toast.LENGTH_LONG).show();
					mBuddiesAdapter.notifyDataSetChanged();
					mBuddiesAdapter = new BuddiesListAdapter(getActivity(),contactList);
					mbuddiesListView.setAdapter(mBuddiesAdapter);
				}
				addBuddiesDialog.dismiss();
			}
		});


		addBuddiesDialog.show();
	}
	
	private boolean validRequiredFields(List<String> phoneNumbersList, String validateName, String validateEmail){
		if(phoneNumbersList.size()<=0){
			Toast.makeText(this.getActivity(), this.getString(R.string.contact_must_to_add_buddy), Toast.LENGTH_LONG).show();
			return false;
		}
		if(validateName.equalsIgnoreCase("")){
		 Toast.makeText(this.getActivity(), this.getString(R.string.buddy_name_empty), Toast.LENGTH_LONG).show();
		 return false;
		}
		if(validateEmail.length()>0 && !AppConstant.validEmail(validateEmail)){
			Toast.makeText(this.getActivity(), this.getString(R.string.buddy_invalid_email), Toast.LENGTH_LONG).show();
			return false;
		}
		return true;
	}
	
	private boolean isValidBuddyFromSameCountry(){
		return false;
	}


	@Override
	public void onItemSelected(AdapterView<?> parent, View view, int position,
			long id) {
		// TODO Auto-generated method stub
		this.selectedPhoneNumber = parent.getItemAtPosition(position).toString();

	}


	@Override
	public void onNothingSelected(AdapterView<?> parent) {
		// TODO Auto-generated method stub

	}
	
	public void updateUI() {
		Fragment currentFragment = BuddiesFragment.this;
		FragmentTransaction fragTransaction = getFragmentManager().beginTransaction();
		fragTransaction.detach(currentFragment);
		fragTransaction.attach(currentFragment);
		fragTransaction.commitAllowingStateLoss();
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB) {
			getActivity().invalidateOptionsMenu(); 
		}else{
			getActivity().supportInvalidateOptionsMenu();
		}
	}


}
