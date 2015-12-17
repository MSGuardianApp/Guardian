package com.guardianapp.ui;

import java.io.UnsupportedEncodingException;
import java.util.ArrayList;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;

import com.guardianapp.R;
import com.guardianapp.adapters.SOSGetLocalHelpAdapter;
import com.guardianapp.model.NearByPlaceDetails;
import com.guardianapp.utilities.AppConstant;
import com.guardianapp.utilities.LogUtils;
import com.guardianapp.webservicecomponents.MakeHTTPServices;
import com.guardianapp.webservicecomponents.OnTaskCompleted;

public class GetLocalHelpFragment extends Fragment implements OnTaskCompleted{

	private ArrayList <NearByPlaceDetails> nearByPlaceDetailsList;
	private SOSGetLocalHelpAdapter sosLocalHelpAdapter;
	View view;
	ListView sosLocalHelpListView;
	@Override
	public View onCreateView(LayoutInflater inflater, ViewGroup container,
			Bundle savedInstanceState) {
		view = inflater.inflate(R.layout.sos_localhelp_fragment, container, false);
		this.invokeGetNearByPlacesService();
		return view;
	}

	private void invokeGetNearByPlacesService(){
		MakeHTTPServices mkh = new MakeHTTPServices(getActivity(), GetLocalHelpFragment.this);
		this.nearByPlaceDetailsList =new ArrayList<NearByPlaceDetails>();
		try {
			mkh.GetNearByHelp();
		} catch (UnsupportedEncodingException uee) {
			// TODO Auto-generated catch block
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), uee.getLocalizedMessage());
		} catch (Exception e) {
			// TODO: handle exception
			LogUtils.LOGE(LogUtils.makeLogTag(this.getClass()), e.getLocalizedMessage());
		}
	}

	//phani
	@Override
	public void onTaskComplete(String result)  {
		// TODO Auto-generated method stub
		AppConstant.dismissProgressDialog();
		JSONObject parsedData;
		try {
			parsedData = new JSONObject(result.replace("microsoftMapsNetworkCallback(", "").replace(".d},'r229');", "}"));
			JSONObject  data= parsedData.getJSONObject("response");
			JSONObject  value= data.getJSONObject("d");
			String category=value.getJSONArray("ParseResults").getJSONObject(0).getString("Keyword");
			JSONArray searchResults= value.getJSONArray("SearchResults");
			for(int i = 0 ; i < searchResults.length() ; i++){
				JSONObject obj=searchResults.getJSONObject(i);
				NearByPlaceDetails nearByPlaceDetails=new  NearByPlaceDetails();
				nearByPlaceDetails.setName(obj.getString("Name"));

				StringBuilder address=new StringBuilder();
				address.append(obj.getString("Address"));
				if(!obj.getString("City").equalsIgnoreCase("null") && !obj.getString("City").equalsIgnoreCase("")){
					address.append(",");
					address.append(obj.getString("City"));
				}
				if(!obj.getString("State").equalsIgnoreCase("null") && !obj.getString("State").equalsIgnoreCase("")){
					address.append(",");
					address.append(obj.getString("State"));
				}
				if(!obj.getString("PostalCode").equalsIgnoreCase("null") && !obj.getString("PostalCode").equalsIgnoreCase("")){
					address.append(",");
					address.append(obj.getString("PostalCode"));
				}
				if(!obj.getString("Country").equalsIgnoreCase("null") && !obj.getString("Country").equalsIgnoreCase("")){
					address.append(",");
					address.append(obj.getString("Country"));
				}
				if(!obj.getString("Phone").equalsIgnoreCase("null") && !obj.getString("Phone").equalsIgnoreCase("")){
					address.append(",");
					address.append(obj.getString("Phone"));
				}

				nearByPlaceDetails.setVicinity(address.toString());
				nearByPlaceDetails.setLatitude(obj.getJSONObject("Location").getString("Latitude"));
				nearByPlaceDetails.setLongitude(obj.getJSONObject("Location").getString("Longitude"));
				nearByPlaceDetails.setCategory(category);
				nearByPlaceDetails.setPhoneNumber(obj.getString("Phone"));
				nearByPlaceDetailsList.add(nearByPlaceDetails);
			}	

			if(nearByPlaceDetailsList!=null){

				sosLocalHelpAdapter = new SOSGetLocalHelpAdapter(getActivity(),nearByPlaceDetailsList);
				sosLocalHelpListView = (ListView) view.findViewById(R.id.lv_buddiesList);
				sosLocalHelpListView.setEmptyView(view.findViewById(android.R.id.empty));
				sosLocalHelpListView.setAdapter(sosLocalHelpAdapter);

			}

		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

	}


	@Override
	public void onGetObjectResult(Object obj) {
		// TODO Auto-generated method stub
		AppConstant.dismissProgressDialog();

	}

}
