package com.guardianapp.adapters;

import java.util.ArrayList;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.os.Build;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import com.guardianapp.R;
import com.guardianapp.model.Group;
import com.guardianapp.services.GPSTracker;
import com.guardianapp.utilities.AppConstant;

public class SOSGroupsAdapter extends BaseAdapter{

	private Context context;
	private ArrayList<Group> groupsList;
	public SOSGroupsAdapter(Context argContext,ArrayList<Group> argList) {
		this.context = argContext;
		this.groupsList = argList;

	}

	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return groupsList.size();
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

	private SOSGroupsAdapter getInstant() {

		return this;
	}
	@Override
	public View getView(int count, View convertView, ViewGroup arg2) {
		LayoutInflater inflater = (LayoutInflater) context
				.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
		View gridView = convertView;
		if (gridView == null) {
			gridView = inflater.inflate(R.layout.sos_buddies_groups_list, null);
			ViewHolder viewHolder = new ViewHolder();
			viewHolder.smsBtn = (ImageView) gridView.findViewById(R.id.iv_sms);
			viewHolder.emailbtn = (ImageView) gridView.findViewById(R.id.iv_email);
			viewHolder.callBtn = (ImageView) gridView.findViewById(R.id.iv_call);
			viewHolder.name = (TextView) gridView.findViewById(R.id.tv_name);
			viewHolder.phoneNumber = (TextView) gridView.findViewById(R.id.tv_phoneNo);
			gridView.setTag(viewHolder);
		}

		ViewHolder holder = (ViewHolder) gridView.getTag();
		
		if(holder!=null && groupsList.get(count)!=null){
		holder.name.setText(groupsList.get(count).getGroupName());
		holder.phoneNumber.setText(groupsList.get(count).getPhoneNumber());

		final int row = count;
		holder.callBtn.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {
				AppConstant.callOrSendMessageToUser(context, groupsList.get(row).getPhoneNumber(), AppConstant.CALL_USER);
			}
		});

		holder.emailbtn.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {
				emailGroups(groupsList.get(row).getEmail());
			}
		});    

		holder.smsBtn.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {
				smsToGroups(groupsList.get(row).getPhoneNumber());
			}
		});    
		}

		return gridView;

	}


	public class ViewHolder
	{

		private ImageView smsBtn,emailbtn,callBtn;
		private TextView name,phoneNumber;

	}

	private void emailGroups(String emailId) {

		Intent emailInt = new Intent(Intent.ACTION_SEND);
		emailInt.setType("message/rfc822");
		emailInt.putExtra(Intent.EXTRA_EMAIL  , new String[]{emailId});
		emailInt.putExtra(Intent.EXTRA_SUBJECT, AppConstant.MessageTemplateText);
		String msg = AppConstant.MessageTemplateText;

		if(AppConstant.isGPS_On)
			msg= AppConstant.MessageTemplateText+" I'm @ Lat: "+GPSTracker.latitude+" Long: "+GPSTracker.longitude;
		if(AppConstant.TINY_URL!=null && !AppConstant.TINY_URL.equalsIgnoreCase(""))
			msg =msg+" Track Me: "+AppConstant.TINY_URL;

		emailInt.putExtra(Intent.EXTRA_TEXT   , msg);
		((Activity) context).startActivity(Intent.createChooser(emailInt, "Send mail..."));

	}


	private void smsToGroups(String phoneNumber) {
		String msg = AppConstant.MessageTemplateText;
		if(AppConstant.isGPS_On)
			msg= AppConstant.MessageTemplateText+" I'm @ Lat: "+GPSTracker.latitude+" Long: "+GPSTracker.longitude;
		if(AppConstant.TINY_URL!=null && !AppConstant.TINY_URL.equalsIgnoreCase(""))
			msg =msg+" Track Me: "+AppConstant.TINY_URL;
		if (Build.VERSION.SDK_INT > Build.VERSION_CODES.JELLY_BEAN_MR2){
			Intent intent = new Intent(Intent.ACTION_SENDTO);
			intent.setData(Uri.parse("smsto:" + Uri.encode(phoneNumber)));
			intent.putExtra("sms_body",msg);
			((Activity) context).startActivity(intent);

		}else{
			Intent intent = new Intent(Intent.ACTION_VIEW/*, smsUri*/);
			intent.putExtra("sms_body", msg);
			intent.setType("vnd.android-dir/mms-sms"); 
			intent.putExtra("address", phoneNumber);
			((Activity) context).startActivity(intent);

		}

	}

}
