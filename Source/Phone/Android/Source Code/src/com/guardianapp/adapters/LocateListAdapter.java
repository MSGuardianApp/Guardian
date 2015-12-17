package com.guardianapp.adapters;

import java.util.ArrayList;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.os.Build;
import android.os.Handler;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.guardianapp.R;
import com.guardianapp.model.LocateBuddies;
import com.guardianapp.services.GPSTracker;
import com.guardianapp.ui.BuddyLocationMapActivity;
import com.guardianapp.utilities.AppConstant;

public class LocateListAdapter extends BaseAdapter{

	private Context context;
	private ArrayList<LocateBuddies> locateBuddiesList;
	private Handler listViewHandler;
	public LocateListAdapter(Context argContext,ArrayList<LocateBuddies> argList, Handler listViewHandler) {
		this.context = argContext;
		this.locateBuddiesList = argList;
		this.listViewHandler = listViewHandler;
	}
	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return locateBuddiesList.size();
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

	private LocateListAdapter getInstant() {
		return this;
	}
	@Override
	public View getView(int count, View convertView, ViewGroup arg2) {
		LayoutInflater inflater = (LayoutInflater) context
				.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
		View gridView = convertView;
		if (gridView == null) {
			gridView = inflater.inflate(R.layout.locate_buddies_list, null);
			ViewHolder viewHolder = new ViewHolder();
			viewHolder.smsBtn = (ImageView) gridView.findViewById(R.id.iv_sms);
			viewHolder.emailbtn = (ImageView) gridView.findViewById(R.id.iv_email);
			viewHolder.callBtn = (ImageView) gridView.findViewById(R.id.iv_call);
			viewHolder.gpsbutton = (ImageView) gridView.findViewById(R.id.iv_gps);
			viewHolder.name = (TextView) gridView.findViewById(R.id.tv_name);
			viewHolder.phoneNumber = (TextView) gridView.findViewById(R.id.tv_phoneNo);
			viewHolder.rlBuddy = (RelativeLayout) gridView.findViewById(R.id.rl_locBuddies);
			viewHolder.address = (TextView) gridView.findViewById(R.id.tv_locationAddress);
			gridView.setTag(viewHolder);
		}

		ViewHolder holder = (ViewHolder) gridView.getTag();

		if(holder!=null && locateBuddiesList.get(count)!=null){
			holder.name.setText(locateBuddiesList.get(count).getName());
			holder.phoneNumber.setText(locateBuddiesList.get(count).getPhoneNumber());
			if(locateBuddiesList.get(count).getLastLocation()!=null)
				holder.address.setText(locateBuddiesList.get(count).getLastLocation());
			else
				holder.address.setText("");

			if(locateBuddiesList.get(count).getBuddyStatusColor().equalsIgnoreCase(AppConstant.SaffronColorCode))
				holder.rlBuddy.setBackgroundResource(R.drawable.bg_tanble_orange);
			else if(locateBuddiesList.get(count).getBuddyStatusColor().equalsIgnoreCase(AppConstant.WhiteColorCode))
				holder.rlBuddy.setBackgroundResource(R.drawable.bg_edittext);
			else if(locateBuddiesList.get(count).getBuddyStatusColor().equalsIgnoreCase(AppConstant.GreenColorCode))
				holder.rlBuddy.setBackgroundResource(R.drawable.bg_tanble_green);

			final int row = count;
			holder.callBtn.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View arg0) {
					AppConstant.callOrSendMessageToUser(context, locateBuddiesList.get(row).getPhoneNumber(), AppConstant.CALL_USER);
				}
			});

			holder.emailbtn.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View arg0) {
					emailBuddy(locateBuddiesList.get(row).getEmail());
				}
			});    

			holder.smsBtn.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View arg0) {
					smsToBuddy(locateBuddiesList.get(row).getPhoneNumber());
				}
			});    


			holder.gpsbutton.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View arg0) {
					if(locateBuddiesList.get(row).getLastLocs()!=null && locateBuddiesList.get(row).getLastLocs().size()>0)
					getBuddyLocationMap(locateBuddiesList.get(row));
					else
						Toast.makeText(context, "Tracking is Off for this User", Toast.LENGTH_SHORT).show();
				}
			});   
			
			if(count == (locateBuddiesList.size()-1)){
				listViewHandler.sendEmptyMessage(0);
			}
		}
		return gridView;

	}


	public class ViewHolder
	{

		private ImageView smsBtn,emailbtn,callBtn,gpsbutton;
		private TextView name,phoneNumber,address;
		private RelativeLayout rlBuddy;
	}

	private void emailBuddy(String emailId) {

		Intent emailInt = new Intent(Intent.ACTION_SEND);
		emailInt.setType("message/rfc822");
		emailInt.putExtra(Intent.EXTRA_EMAIL  , emailId);
		emailInt.putExtra(Intent.EXTRA_SUBJECT, AppConstant.MessageTemplateText);
		String msg = AppConstant.MessageTemplateText;

		if(AppConstant.isGPS_On)
			msg= AppConstant.MessageTemplateText+" I'm @ Lat: "+GPSTracker.latitude+" Long: "+GPSTracker.longitude;
		if(AppConstant.TINY_URL!=null && !AppConstant.TINY_URL.equalsIgnoreCase(""))
			msg =msg+" Track Me: "+AppConstant.TINY_URL;

		emailInt.putExtra(Intent.EXTRA_TEXT   , msg);
		((Activity) context).startActivity(Intent.createChooser(emailInt, "Send mail..."));

	}


	private void smsToBuddy(String phoneNumber) {
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

	private void getBuddyLocationMap(LocateBuddies buddy) {

        double latitude = (buddy.getLastLocs()!=null)? Double.parseDouble(buddy.getLastLocs().get(0).getLat()) : 0;
        double longitude = (buddy.getLastLocs()!=null)? Double.parseDouble(buddy.getLastLocs().get(0).getLong()) : 0;

		AppConstant.locateBuddy = buddy;
		Intent intent = new Intent(context, BuddyLocationMapActivity.class);
		intent.putExtra("lat", latitude);
		intent.putExtra("lng", longitude);
		context.startActivity(intent);


	}

}
