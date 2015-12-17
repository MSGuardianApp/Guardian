package com.guardianapp.ui;

import java.util.ArrayList;

import android.annotation.SuppressLint;
import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.widget.EditText;
import android.widget.ListView;

import com.guardianapp.R;
import com.guardianapp.adapters.CountryCodeAdapter;
import com.guardianapp.model.CountryCodes;
import com.guardianapp.utilities.AppConstant;

public class CountryListActivity extends BaseActivity{

	private CountryCodeAdapter countryAdapter;
	private ListView countryCodeListView;
	private ArrayList<CountryCodes> codeList;
	private EditText myFilter;
	@SuppressLint("NewApi") @Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		getSupportActionBar().setTitle(this.getString(R.string.select_country));
		setContentView(R.layout.country_code_list);
		
	}
	@Override
	protected void initFields() {
		// TODO Auto-generated method stub
		codeList = new ArrayList<CountryCodes>();
		countryCodeListView = (ListView) findViewById(R.id.lv_countryCodeList);
		codeList = AppConstant.getAllcountryCodes(this);
		countryAdapter = new CountryCodeAdapter(this, codeList);
		countryCodeListView.setAdapter(countryAdapter);
		myFilter = (EditText) findViewById(R.id.myFilter);

	}


	@Override
	protected void initCallBacks() {
		// TODO Auto-generated method stub
		myFilter.addTextChangedListener(new TextWatcher() {
			
			@Override
			public void onTextChanged(CharSequence s, int start, int before, int count) {
				// TODO Auto-generated method stub
				
			}
			
			@Override
			public void beforeTextChanged(CharSequence s, int start, int count,
					int after) {
				// TODO Auto-generated method stub
				countryAdapter.getFilter().filter(s.toString());
				
			}
			
			@Override
			public void afterTextChanged(Editable s) {
				// TODO Auto-generated method stub
				
			}
		});

	}


}
