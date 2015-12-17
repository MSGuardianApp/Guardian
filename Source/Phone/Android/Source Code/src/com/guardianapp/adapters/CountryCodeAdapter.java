package com.guardianapp.adapters;

import java.util.ArrayList;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Filter;
import android.widget.TextView;

import com.guardianapp.R;
import com.guardianapp.model.CountryCodes;
import com.guardianapp.utilities.AppConstant;

/**
 * @author v-dhmadd
 *
 */
public class CountryCodeAdapter extends ArrayAdapter<CountryCodes>{

	private Context context;
	private ArrayList<CountryCodes> codeList;
	private ArrayList<CountryCodes> countryList;
	private String countryName = "",countryCode="";
	private CountryFilter filter;

	public CountryCodeAdapter(Context argContext,ArrayList<CountryCodes> argList) {
		super(argContext,R.layout.country_code,argList);
		this.context = argContext;
		this.countryList = new ArrayList<CountryCodes>();
		this.countryList.addAll(argList);
		this.codeList = new ArrayList<CountryCodes>();
		this.codeList.addAll(argList);

	}

	@Override
	public Filter getFilter() {
		// TODO Auto-generated method stub
		if (filter == null){
			filter  = new CountryFilter();
		}
		return filter;
	}

	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return codeList.size();
	}

	@Override
	public CountryCodes getItem(int arg0) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public long getItemId(int arg0) {
		// TODO Auto-generated method stub
		return 0;
	}

	private CountryCodeAdapter getInstant() {

		return this;
	}
	@Override
	public View getView(int count, View convertView, ViewGroup arg2) {
		LayoutInflater inflater = (LayoutInflater) context
				.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
		View gridView = convertView;
		if (gridView == null) {
			gridView = inflater.inflate(R.layout.country_code, null);
			ViewHolder viewHolder = new ViewHolder();
			viewHolder.countryCode = (TextView) gridView.findViewById(R.id.countryCode);
			gridView.setTag(viewHolder);
		}

		if(codeList.get(count)!=null){
			countryName = codeList.get(count).getCountryName();
			countryCode = codeList.get(count).getISDCode();
		}

		ViewHolder holder = (ViewHolder) gridView.getTag();
		if(holder!=null){
			final int row = count;
			holder.countryCode.setText(countryName+" ("+countryCode+")");
			holder.countryCode.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View arg0) {
					Intent intent = new Intent();
					intent.putExtra("Country Code",codeList.get(row).getISDCode());
					((Activity) context).setResult(AppConstant.GET_COUNTRY_CODE_ACTIVITY_RESULT, intent);
					((Activity) context).finish();
				}
			}); 
		}
		return gridView;

	}

	public class ViewHolder
	{
		private TextView countryCode;

	}

	private class CountryFilter extends Filter{

		@Override
		protected FilterResults performFiltering(CharSequence constraint) {
			// TODO Auto-generated method stub
			constraint = constraint.toString().toLowerCase();
			FilterResults result = new FilterResults();
			if(constraint != null && constraint.toString().length() > 0)
			{
				ArrayList<CountryCodes> filteredItems = new ArrayList<CountryCodes>();
				for(int i = 0, l = countryList.size(); i < l; i++)
				{
					CountryCodes country = countryList.get(i);
					if(country.getCountryName().toString().toLowerCase().contains(constraint))
						filteredItems.add(country);
				}
				result.count = filteredItems.size();
				result.values = filteredItems;
			}
			else
			{
				synchronized(this)
				{
					result.values = countryList;
					result.count = countryList.size();
				}
			}
			return result;
		}

		@Override
		protected void publishResults(CharSequence constraint,
				FilterResults results) {
			// TODO Auto-generated method stub
			codeList = (ArrayList<CountryCodes>)results.values;
			notifyDataSetChanged();
			clear();
			for(int i = 0, l = codeList.size(); i < l; i++)
				add(codeList.get(i));
			notifyDataSetInvalidated();
		}

	}

}


