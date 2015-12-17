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

public class SearchGroupAdapter extends BaseAdapter {


	private Context context;

	private ArrayList<String> contectList;
	public SearchGroupAdapter(Context argContext,ArrayList<String> argList) {
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
			viewHolder.tv_phone_num =  (TextView) mView.findViewById(R.id.tv_phone_num);
			viewHolder.tv_group_email =  (TextView) mView.findViewById(R.id.tv_group_email);
			viewHolder.deleteBtn =   (ImageView) mView.findViewById(R.id.iv_delete);
			viewHolder.photoImg =   (ImageView) mView.findViewById(R.id.iv_Pic);
			viewHolder.deleteBtn.setBackgroundResource(R.drawable.add);
			viewHolder.photoImg.setVisibility(View.GONE);
			viewHolder.tv_phone_num.setVisibility(View.GONE);
			viewHolder.tv_group_email.setVisibility(View.GONE);
			mView.setTag(viewHolder);
		} 

		ViewHolder holder = (ViewHolder) mView.getTag();
		
		if(holder!=null && contectList.get(count)!=null)
		holder.name.setText(contectList.get(count));
		return mView;

	}

	public class ViewHolder
	{

		private ImageView photoImg, deleteBtn;
		private TextView name,tv_phone_num,tv_group_email;

	}
}
