package com.guardianapp.utilities;

import java.util.HashMap;
import java.util.List;

import org.json.JSONObject;

import com.guardianapp.ui.TrackMeActivity;

import android.content.Context;
import android.os.AsyncTask;

public class ParserTask extends
AsyncTask<String, Integer, List<List<HashMap<String, String>>>> {
	private Context context;
	public ParserTask(Context context){
		this.context = context;
	}

@Override
protected List<List<HashMap<String, String>>> doInBackground(
	String... jsonData) {

JSONObject jObject;
List<List<HashMap<String, String>>> routes = null;

try {
	jObject = new JSONObject(jsonData[0]);
	GMapV2Direction parser = new GMapV2Direction();
	routes = parser.parse(jObject);
} catch (Exception e) {
	e.printStackTrace();
}
return routes;
}

@Override
protected void onPostExecute(List<List<HashMap<String, String>>> routes) {
if(this.context instanceof TrackMeActivity){
	((TrackMeActivity)this.context).handleRouteBetweenMultipleMarkers(routes);
}
}
}