package com.guardianapp.adapters;

import java.util.ArrayList;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import com.guardianapp.R;
import com.guardianapp.database.DBQueries;
import com.guardianapp.model.AscGroups;
import com.guardianapp.model.Group;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.LogUtils;

/**
 * @author v-dhmadd
 *
 */
public class GroupListAdapter extends BaseAdapter{
	private Context context;
	private ArrayList<Group> groupList;
	public GroupListAdapter(Context argContext,ArrayList<Group> argList) {
		this.context = argContext;
		this.groupList = argList;
	}
	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return groupList.size();
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

	private GroupListAdapter getInstant() {

		return this;
	}
	@Override
	public View getView(int count, View convertView, ViewGroup arg2) {
		LayoutInflater inflater = (LayoutInflater) context
				.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
		View gridView = convertView;
		if (gridView == null) {
			gridView = inflater.inflate(R.layout.buddies_details_list, null);
			ViewHolder viewHolder = new ViewHolder();
			viewHolder.photoImg = (ImageView) gridView.findViewById(R.id.iv_Pic);
			viewHolder.deleteBtn = (ImageView) gridView.findViewById(R.id.iv_delete);
			viewHolder.deleteBtn.setBackgroundResource(R.drawable.delete);
			viewHolder.name = (TextView) gridView.findViewById(R.id.tv_name);
			viewHolder.phNumber = (TextView)gridView.findViewById(R.id.tv_phone_num);
			viewHolder.groupEmail = (TextView)gridView.findViewById(R.id.tv_group_email);
			gridView.setTag(viewHolder);
		}

		String groupName = groupList.get(count).getGroupName();
		if(!groupList.get(count).isIsValidated())
			groupName = "*"+groupName;

		ViewHolder holder = (ViewHolder) gridView.getTag();
		holder.name.setText(groupName);
		holder.phNumber.setText(groupList.get(count).getPhoneNumber());
		holder.groupEmail.setText(groupList.get(count).getEmail());
		final int row = count;
		holder.deleteBtn.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {
				DBQueries query = new DBQueries(context);
				if(groupList.get(row).getGroupID()!=null){
					query.deleteGroupValue(groupList.get(row).getGroupName());
					AppConstant.UpdateIsDataSynced(context,false);
				}
				try{
					groupList.remove(row);
					AppConstant.ascGroups.remove(row);
				}catch(IndexOutOfBoundsException ioe){
					LogUtils.LOGE(LogUtils.makeLogTag(GroupListAdapter.class), ioe.getLocalizedMessage());
				}catch(Exception e){
					LogUtils.LOGE(LogUtils.makeLogTag(GroupListAdapter.class), e.getLocalizedMessage());
				}
				getInstant().notifyDataSetChanged();
			}
		});

		return gridView;

	}

	public class ViewHolder
	{
		private ImageView photoImg, deleteBtn;
		private TextView name;
		private TextView phNumber;
		private TextView groupEmail;

	}

}
