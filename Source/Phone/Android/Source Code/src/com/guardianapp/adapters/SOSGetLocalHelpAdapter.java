package com.guardianapp.adapters;

import java.util.ArrayList;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.guardianapp.R;
import com.guardianapp.model.NearByPlaceDetails;
import com.guardianapp.ui.GetHelpLocationMapActivity;
import com.guardianapp.utilities.AppConstant;

public class SOSGetLocalHelpAdapter extends BaseAdapter{

	private Context context;
	private ArrayList<NearByPlaceDetails> localHelpList;
	public SOSGetLocalHelpAdapter(Context argContext,ArrayList<NearByPlaceDetails> argList) {
		this.context = argContext;
		this.localHelpList = argList;

	}

	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return localHelpList.size();
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

	private SOSGetLocalHelpAdapter getInstant() {

		return this;
	}
	@Override
	public View getView(int count, View convertView, ViewGroup arg2) {
		LayoutInflater inflater = (LayoutInflater) context
				.getSystemService(Context.LAYOUT_INFLATER_SERVICE);

		View gridView = convertView;
		if (gridView == null) {
			gridView = inflater.inflate(R.layout.get_local_help_list, null);
			ViewHolder viewHolder = new ViewHolder();
			viewHolder.emer_icon = (ImageView) gridView.findViewById(R.id.iv_emergencyIcon);
			viewHolder.callBtn = (ImageView) gridView.findViewById(R.id.iv_callBtn);
			viewHolder.lhName = (TextView) gridView.findViewById(R.id.tv_Name);
			viewHolder.lhAddress = (TextView) gridView.findViewById(R.id.tv_address);
			viewHolder.llPanel=(LinearLayout) gridView.findViewById(R.id.ll_address);
			gridView.setTag(viewHolder);
		}


		ViewHolder holder = (ViewHolder) gridView.getTag();
		if(holder!=null && localHelpList.get(count)!=null){
			holder.lhName.setText(localHelpList.get(count).getName());
			holder.lhAddress.setText(localHelpList.get(count).getVicinity());
			if(localHelpList.get(count).getCategory().equalsIgnoreCase("police station")){
				holder.emer_icon.setImageResource(R.drawable.police);
			}
			else
			{
				holder.emer_icon.setImageResource(R.drawable.medical);
			}


			final int row = count;
			holder.llPanel.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					Intent getmapInt = new Intent(context, GetHelpLocationMapActivity.class);
					AppConstant.localHelpLat = localHelpList.get(row).getLatitude();
					AppConstant.localHelpLong = localHelpList.get(row).getLongitude();
					AppConstant.localHelpAddress = localHelpList.get(row).getVicinity();
					getmapInt.putExtra("lat", Double.parseDouble(localHelpList.get(row).getLatitude()));
					getmapInt.putExtra("lng", Double.parseDouble(localHelpList.get(row).getLongitude()));
					context.startActivity(getmapInt);
				}
			});
			if(!localHelpList.get(row).getPhoneNumber().equalsIgnoreCase("null")||!localHelpList.get(row).getPhoneNumber().equalsIgnoreCase("")){
				holder.callBtn.setOnClickListener(new OnClickListener() {

					@Override
					public void onClick(View arg) {
						AppConstant.callOrSendMessageToUser(context, localHelpList.get(row).getPhoneNumber(), AppConstant.CALL_USER);

					}
				});
			}
			else
			{
				holder.callBtn.setVisibility(View.GONE);
			}

		}


		return gridView;

	}

	public class ViewHolder
	{
		public LinearLayout llPanel;
		public TextView lhName;
		private ImageView emer_icon,callBtn;
		private TextView lhAddress;

	}
}
