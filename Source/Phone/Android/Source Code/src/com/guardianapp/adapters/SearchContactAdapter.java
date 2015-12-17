package com.guardianapp.adapters;

import java.util.ArrayList;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import com.guardianapp.R;
import com.guardianapp.model.MyBuddies;

/**
 * @author v-dhmadd
 *
 */
public class SearchContactAdapter extends BaseAdapter {
	private Context context;
	private ArrayList<MyBuddies> contectList;
	
	public SearchContactAdapter(Context argContext,ArrayList<MyBuddies> argList) {
		this.context = argContext;
		this.contectList = argList;
	}
	
	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return contectList.size();
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
	
	@Override
	public View getView(int count, View convertView, ViewGroup arg2) {
		LayoutInflater inflater = (LayoutInflater) context
				.getSystemService(Context.LAYOUT_INFLATER_SERVICE);

		View mView = convertView;
		if (mView == null) {
			ViewHolder viewHolder = new ViewHolder();
			mView = inflater.inflate(R.layout.buddies_details_list, null);
			viewHolder.name =  (TextView) mView.findViewById(R.id.tv_name);
			viewHolder.phoneNum = (TextView)mView.findViewById(R.id.tv_phone_num);
			viewHolder.deleteBtn =   (ImageView) mView.findViewById(R.id.iv_delete);
			viewHolder.groupEmail = (TextView)mView.findViewById(R.id.tv_group_email);
			viewHolder.deleteBtn.setBackgroundResource(R.drawable.add);
			viewHolder.deleteBtn.setVisibility(View.GONE);
			viewHolder.phoneNum.setVisibility(View.GONE);
			viewHolder.groupEmail.setVisibility(View.GONE);
			mView.setTag(viewHolder);
		} 

		ViewHolder holder = (ViewHolder) mView.getTag();
		
		if(holder!=null && contectList.get(count)!=null)
		holder.name.setText(contectList.get(count).getName().toString());
		
		return mView;

	}

	public class ViewHolder
	{

		private ImageView photoImg, deleteBtn;
		private TextView name;
		private TextView phoneNum,groupEmail;

	}
}
