package com.guardianapp.utilities;
import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;

public class ReadTask extends AsyncTask<String, Void, String> {
	private Context context;
	public ReadTask(Context context){
		this.context = context;
	}
	@Override
	protected String doInBackground(String... url) {
		String data = "";
		try {
			data = AppConstant.readUrl(url[0]);
		} catch (Exception e) {
			Log.d("Background Task", e.toString());
		}
		return data;
	}

	@Override
	protected void onPostExecute(String result) {
		super.onPostExecute(result);
		if(result!=null && !result.equalsIgnoreCase(""))
		new ParserTask(this.context).execute(result);
	}
}