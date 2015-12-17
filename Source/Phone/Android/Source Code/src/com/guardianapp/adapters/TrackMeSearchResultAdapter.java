package com.guardianapp.adapters;

import java.util.ArrayList;

import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.webkit.WebView;
import android.widget.BaseAdapter;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;

import com.guardianapp.R;
import com.guardianapp.model.NearByPlaceDetails;
import com.guardianapp.services.GPSTracker;
import com.guardianapp.utilities.AppConstant;

public class TrackMeSearchResultAdapter extends BaseAdapter{

	private Context context;
	private ArrayList<NearByPlaceDetails> addressList;
	private ListView addressListView;
	private WebView mWebView;
	public TrackMeSearchResultAdapter(Context argContext, WebView webView , ArrayList<NearByPlaceDetails> argList, ListView addressListView) {
		this.context = argContext;
		this.addressList = argList;
		this.addressListView = addressListView;
		this.mWebView = webView;
	}

	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return addressList.size();
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

	private TrackMeSearchResultAdapter getInstant() {

		return this;
	}
	@Override
	public View getView(int count, View convertView, ViewGroup arg2) {
		LayoutInflater inflater = (LayoutInflater) context
				.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
		View gridView = convertView;

		if (gridView == null) {
			gridView = inflater.inflate(R.layout.track_me_search_result, null);
			ViewHolder viewHolder = new ViewHolder();
			viewHolder.lhAddress = (TextView) gridView.findViewById(R.id.tv_searchAddress);
			viewHolder.llPanel=(LinearLayout) gridView.findViewById(R.id.ll_address);
			gridView.setTag(viewHolder);
		}

		ViewHolder holder = (ViewHolder) gridView.getTag();

		if(holder!=null && addressList.get(count)!=null){
			holder.lhAddress.setText(addressList.get(count).getVicinity());
			final int Count = count;
			holder.llPanel.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {

					String callJs = "BingMapsAndroid.CreateRoute('"+addressList.get(Count).getLatitude()+"','"+addressList.get(Count).getLongitude()
							+"','"+addressList.get(Count).getVicinity()+"');";

					TrackMeSearchResultAdapter.this.findDirections(GPSTracker.latitude, GPSTracker.longitude, Double.parseDouble(addressList.get(Count).getLatitude().trim()), Double.parseDouble(addressList.get(Count).getLongitude().trim()));
					AppConstant.isGetDirection = true; 

					((Activity) context).getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_HIDDEN);
					TrackMeSearchResultAdapter.this.addressListView.setVisibility(View.GONE);
				}
			});

		}		   

		return gridView;

	}

	public void findDirections(double fromPositionDoubleLat, double fromPositionDoubleLong, double toPositionDoubleLat, double toPositionDoubleLong)
	{
		String shouldDrawRouteToLocateBuddy = "false";
		this.mWebView.loadUrl("javascript:createRouteToDestLocation(\"" + fromPositionDoubleLat + "\",\"" + fromPositionDoubleLong + "\",\"" +toPositionDoubleLat+"\",\""+toPositionDoubleLong+"\",\""+shouldDrawRouteToLocateBuddy+"\")");
	}


	public class ViewHolder
	{
		public LinearLayout llPanel;
		private TextView lhAddress;

	}




}
