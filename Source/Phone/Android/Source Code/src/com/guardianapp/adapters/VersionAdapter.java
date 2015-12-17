package com.guardianapp.adapters;

import java.util.ArrayList;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import com.guardianapp.R;
import com.guardianapp.model.Version;

public class VersionAdapter extends BaseAdapter{


	private Context context;
	private ArrayList<Version> vesionList;
	public VersionAdapter(Context argContext,ArrayList<Version> argList) {
		this.context = argContext;
		this.vesionList = argList;

	}

	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return vesionList.size();
	}

	@Override
	public Object getItem(int arg0) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public long getItemId(int arg0) {
		// TODO Auto-generated method stub
		return 0;
	}

	private VersionAdapter getInstant() {

		return this;
	}
	@Override
	public View getView(int count, View convertView, ViewGroup arg2) {
		LayoutInflater inflater = (LayoutInflater) context
				.getSystemService(Context.LAYOUT_INFLATER_SERVICE);

		View gridView = convertView;
		if (gridView == null) {
			gridView = inflater.inflate(R.layout.version_list, null);
			ViewHolder viewHolder = new ViewHolder();
			viewHolder.version = (TextView) gridView.findViewById(R.id.tv_version_text);
			viewHolder.verHistory = (TextView) gridView.findViewById(R.id.tv_VerHistory_text);
			gridView.setTag(viewHolder);
		}

		ViewHolder holder = (ViewHolder) gridView.getTag();

		if(holder!=null && vesionList.get(count)!=null){
			holder.version.setText(vesionList.get(count).getVersion());
			holder.verHistory.setText(vesionList.get(count).getVersionHistory());
		}
		return gridView;

	}


	public class ViewHolder
	{
		private TextView version, verHistory;
	}

}
