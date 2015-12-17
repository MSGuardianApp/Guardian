
package com.guardianapp.adapters;



import java.util.ArrayList;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import com.guardianapp.R;
import com.guardianapp.model.MyBuddies;

/**
 * @author v-dhmadd
 * adapter view for displaying list of callers
 */
public class CallerListAdapter extends BaseAdapter{


	private Context context;
	ArrayList<MyBuddies> mybuddies;

	public CallerListAdapter(Context argContext,ArrayList<MyBuddies> argbuddies) {
		this.context = argContext;
		this.mybuddies = argbuddies;
	}

	@Override
	public int getCount() {
		return mybuddies.size();
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
		View gridView = convertView;
		if (gridView == null) {
			gridView = inflater.inflate(R.layout.caller_list, null);
			ViewHolder viewHolder = new ViewHolder();
			viewHolder.callerName = (TextView) gridView.findViewById(R.id.tv_CallerName);
			gridView.setTag(viewHolder);
		}

		ViewHolder holder = (ViewHolder) gridView.getTag();
		if(mybuddies.get(count)!=null)
			holder.callerName.setText(mybuddies.get(count).getName());
		else
			holder.callerName.setText("None");
		return gridView;

	}

	public class ViewHolder
	{
		private TextView callerName;
	}

}
