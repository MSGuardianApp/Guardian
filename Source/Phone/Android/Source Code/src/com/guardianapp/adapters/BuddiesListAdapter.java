package com.guardianapp.adapters;

import java.util.ArrayList;

import android.content.Context;
import android.content.SharedPreferences;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import com.guardianapp.R;
import com.guardianapp.database.DBQueries;
import com.guardianapp.model.MyBuddies;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.LogUtils;

/**
 * @author v-dhmadd
 * Adapter for buddieslist
 */
public class BuddiesListAdapter extends BaseAdapter {
	private Context context;
	private ArrayList<MyBuddies> buddiesList;
	private SharedPreferences preferences;
	private String buddyName = "", buddyMobNum = "", buddyEmail = "";

	public BuddiesListAdapter(Context argContext, ArrayList<MyBuddies> buddiesList) {
		this.context = argContext;
		this.buddiesList = buddiesList;
		this.preferences = this.context.getSharedPreferences(
				AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);

	}

	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return buddiesList.size();
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

	private BuddiesListAdapter getInstant() {
		return this;
	}

	@Override
	public View getView(int count, View convertView, ViewGroup arg2) {
		LayoutInflater inflater = (LayoutInflater) context
				.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
		View gridView = convertView;
		// reuse views
		if (gridView == null) {
			gridView = inflater.inflate(R.layout.buddies_details_list, null);
			ViewHolder viewHolder = new ViewHolder();
			viewHolder.photoImg = (ImageView) gridView
					.findViewById(R.id.iv_Pic);
			viewHolder.deleteBtn = (ImageView) gridView
					.findViewById(R.id.iv_delete);
			viewHolder.deleteBtn.setBackgroundResource(R.drawable.delete);
			viewHolder.name = (TextView) gridView.findViewById(R.id.tv_name);
			viewHolder.phoneNum = (TextView) gridView
					.findViewById(R.id.tv_phone_num);
			viewHolder.buddyEmail = (TextView) gridView
					.findViewById(R.id.tv_group_email);
			gridView.setTag(viewHolder);
		}

		ViewHolder holder = (ViewHolder) gridView.getTag();

		if(buddiesList.get(count)!=null){
			buddyName = buddiesList.get(count).getName();
			if (buddiesList.get(count).getState().equalsIgnoreCase(AppConstant.NORMAL_BUDDY)) { //normal buddy
			} else if (buddiesList.get(count).getState().equalsIgnoreCase(AppConstant.UNSUBSCRIBED_BUDDY)) { // unsubscribed buddy
				buddyName = "*" + buddyName;
			} else if (buddiesList.get(count).getState().equalsIgnoreCase(AppConstant.MARSHALL_BUDDY)) { //marshall buddy
				buddyName = "+" + buddyName;
			}

			buddyMobNum = buddiesList.get(count).getMobileNumber();
			buddyEmail = buddiesList.get(count).getEmail();

		}

		if(holder!=null){
			holder.name.setText(buddyName);
			holder.phoneNum.setText(AppConstant.getDialingCode(this.context)
					+ ""
					+ AppConstant.formatPhoneNumber(buddyMobNum , AppConstant.getRegionCodeForCountryCode(AppConstant.getDialingCode(this.context))));
			holder.buddyEmail.setText(buddyEmail);

			final int row = count;
			holder.deleteBtn.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View arg0) {
					DBQueries query = new DBQueries(context);
					if(buddiesList.get(row).getBuddyID()!=null && buddiesList.get(row).getBuddyID()!=0){
						query.deleteBuddyValue(buddiesList.get(row).getMobileNumber());
						AppConstant.UpdateIsDataSynced(context, false);
					}else{
						query.deleteBuddyContectIntoDB(buddiesList.get(row).getMobileNumber());
					}
					try{
						buddiesList.remove(row);
						AppConstant.globalBuddies.remove(row);
					}catch(IndexOutOfBoundsException ioe){
						LogUtils.LOGE(LogUtils.makeLogTag(BuddiesListAdapter.class), ioe.getLocalizedMessage());
					}catch(Exception e){
						LogUtils.LOGE(LogUtils.makeLogTag(BuddiesListAdapter.class), e.getLocalizedMessage());
					}
					if (AppConstant.globalBuddies != null
							&& AppConstant.globalBuddies.size() > 0)
						AppConstant.callerBuddies = AppConstant.getPrimaryBuddy();
					else
						AppConstant.callerBuddies = new MyBuddies();

					getInstant().notifyDataSetChanged();

				}
			});
		}

		return gridView;

	}

	public class ViewHolder {
		private ImageView photoImg, deleteBtn;
		private TextView name;
		private TextView phoneNum;
		private TextView buddyEmail;
	}

}
