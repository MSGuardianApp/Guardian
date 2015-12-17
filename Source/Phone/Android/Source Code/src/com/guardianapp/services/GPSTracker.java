package com.guardianapp.services;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;
import java.util.Locale;
import java.util.concurrent.TimeUnit;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.location.Address;
import android.location.Geocoder;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;

import com.guardianapp.interfaces.OnLocationUpdate;
import com.guardianapp.model.GeoTag;
import com.guardianapp.ui.TrackMeActivity;
import com.guardianapp.utilities.AppConstant;



public class GPSTracker
{
	private final Context mContext;
	private boolean isGPSEnabled = false;
	private boolean isNetworkEnabled = false;
	private boolean canGetLocation = false;
	private Location location;
	public static double latitude;
	public static double longitude;
	public static long recordedTime;
	public static int latestSpeed;
	public SharedPreferences gps_prefs;

	private static final long MIN_DISTANCE_CHANGE_FOR_UPDATES = 0;//10; //10 metters
	private static final long MIN_TIME_BW_UPDATES = 100 * 75;//get updates every 7.5 seconds
	private MyLocationListner locationListner;
	protected LocationManager locationManager;

	public static List<GeoTag>  GeoTagList = new ArrayList<GeoTag>();
	public static List<GeoTag> tempGeoTagList = new ArrayList<GeoTag>();
	public static long recentLocCapturedTime = 0;
	public static Location recentCapturedLocation;

	public GPSTracker(Context context) 
	{
		this.mContext = context;
		locationListner = new MyLocationListner();
	}

	public boolean setOnLocationListner(Activity argActivity) {
		if(locationListner!=null)
		{
			locationListner.setOnLocationUpdate(argActivity);
			return true;
		}
		else
			return false;
	}

	public boolean stopLocationUpdate() {
		if(locationListner!=null)
		{
			locationListner.stopLocationUpdate();
			return true;
		}
		else
			return false;
	}

	public boolean checkGPSStatusOn() {

		if(locationManager ==null)
			locationManager = (LocationManager) mContext.getSystemService(Context.LOCATION_SERVICE);

		//getting GPS status
		isGPSEnabled = locationManager.isProviderEnabled(LocationManager.GPS_PROVIDER);

		//getting network status
		isNetworkEnabled = locationManager.isProviderEnabled(LocationManager.NETWORK_PROVIDER);

		if (!isGPSEnabled && !isNetworkEnabled)
		{
			return false;
		}
		else
			return true;  
	}

	public void startGPS()
	{
		try
		{
			if(locationManager==null)
				locationManager = (LocationManager) mContext.getSystemService(Context.LOCATION_SERVICE);

			//getting GPS status
			isGPSEnabled = locationManager.isProviderEnabled(LocationManager.GPS_PROVIDER);

			//getting network status
			isNetworkEnabled = locationManager.isProviderEnabled(LocationManager.NETWORK_PROVIDER);

			if (!isGPSEnabled && !isNetworkEnabled)
			{
				// no network provider is enabled
			}
			else
			{
				this.canGetLocation = true;
				AppConstant.isGPS_On= true;

				Handler handler  = new Handler();
				handler.post(new Runnable() {

					@Override
					public void run() {


						//First get location from Network Provider
						if (isNetworkEnabled)
						{
							locationManager.requestLocationUpdates(
									LocationManager.NETWORK_PROVIDER,
									MIN_TIME_BW_UPDATES,
									MIN_DISTANCE_CHANGE_FOR_UPDATES, locationListner);

							Log.d("Network", "Network");


						}

						//if GPS Enabled get lat/long using GPS Services
						if (isGPSEnabled)
						{
							if (location == null)
							{
								locationManager.requestLocationUpdates(
										LocationManager.GPS_PROVIDER,
										MIN_TIME_BW_UPDATES,
										MIN_DISTANCE_CHANGE_FOR_UPDATES, locationListner);

								Log.d("GPS Enabled", "GPS Enabled");


							}
						}


					}
				});

				if (locationManager != null)
				{
					getLastKnownLocation();
				}
			}
		}
		catch (Exception e)
		{
			//e.printStackTrace();
			Log.e("Error : Location", "Impossible to connect to LocationManager", e);
		}


	}

	private void getLastKnownLocation()
	{
		if (isNetworkEnabled)
		{
			location = locationManager.getLastKnownLocation(LocationManager.NETWORK_PROVIDER);
			updateGPSCoordinates();
		}
		if (isGPSEnabled)
		{
			location = locationManager.getLastKnownLocation(LocationManager.GPS_PROVIDER);
			updateGPSCoordinates();
		}

	}
	public void updateGPSCoordinates()
	{
		if (location != null)
		{
			GPSTracker.latitude = location.getLatitude();
			GPSTracker.longitude = location.getLongitude();

			if(mContext!=null)
			{
				gps_prefs = mContext.getSharedPreferences(
						AppConstant.APP_SHARED_PREFRENCE, Context.MODE_PRIVATE);
				SharedPreferences.Editor edit = gps_prefs.edit();
				edit.putString(AppConstant.User_Prefs_Latitute, ""+GPSTracker.latitude );
				edit.putString(AppConstant.User_prefs_logitute, ""+GPSTracker.longitude);
				edit.commit();
			}

		}
	}



	/**
	 * Stop using GPS listener
	 * Calling this function will stop using GPS in your app
	 */

	public void stopUsingGPS()
	{
		if (locationManager != null)
		{
			locationManager.removeUpdates(locationListner);
		}
		AppConstant.isGPS_On= false;
	}





	/**
	 * Function to check GPS/wifi enabled
	 */
	public boolean canGetLocation()
	{
		return this.canGetLocation;
	}

	/**
	 * Function to show settings alert dialog
	 */
	public void showSettingsAlert()
	{
		AlertDialog.Builder alertDialog = new AlertDialog.Builder(mContext);

		//Setting Dialog Title
		// alertDialog.setTitle(R.string.GPSAlertDialogTitle);

		//Setting Dialog Message
		//alertDialog.setMessage(R.string.GPSAlertDialogMessage);

		//On Pressing Setting button
		/*  alertDialog.setPositiveButton(R.string.settings, new DialogInterface.OnClickListener() 
        {   
            @Override
            public void onClick(DialogInterface dialog, int which) 
            {
                Intent intent = new Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS);
                mContext.startActivity(intent);
            }
        });*/

		//On pressing cancel button
		/* alertDialog.setNegativeButton(R.string.cancel, new DialogInterface.OnClickListener() 
        {   
            @Override
            public void onClick(DialogInterface dialog, int which) 
            {
                dialog.cancel();
            }
        });
		 */
		alertDialog.show();
	}

	/**
	 * Get list of address by latitude and longitude
	 * @return null or List<Address>
	 */
	public List<Address> getGeocoderAddress(Context context)
	{
		if (location != null)
		{
			Geocoder geocoder = new Geocoder(context, Locale.ENGLISH);
			try 
			{
				List<Address> addresses = geocoder.getFromLocation(latitude, longitude, 1);
				return addresses;
			} 
			catch (IOException e) 
			{
				//e.printStackTrace();
				Log.e("Error : Geocoder", "Impossible to connect to Geocoder", e);
			}
		}

		return null;
	}

	/**
	 * Try to get AddressLine
	 * @return null or addressLine
	 */
	public String getAddressLine(Context context)
	{
		List<Address> addresses = getGeocoderAddress(context);
		if (addresses != null && addresses.size() > 0)
		{
			Address address = addresses.get(0);
			String addressLine = address.getAddressLine(0);

			return addressLine;
		}
		else
		{
			return null;
		}
	}

	/**
	 * Try to get Locality
	 * @return null or locality
	 */
	public String getLocality(Context context)
	{
		List<Address> addresses = getGeocoderAddress(context);
		if (addresses != null && addresses.size() > 0)
		{
			Address address = addresses.get(0);
			String locality = address.getLocality();

			return locality;
		}
		else
		{
			return null;
		}
	}

	/**
	 * Try to get Postal Code
	 * @return null or postalCode
	 */
	public String getPostalCode(Context context)
	{
		List<Address> addresses = getGeocoderAddress(context);
		if (addresses != null && addresses.size() > 0)
		{
			Address address = addresses.get(0);
			String postalCode = address.getPostalCode();

			return postalCode;
		}
		else
		{
			return null;
		}
	}

	/**
	 * Try to get CountryName
	 * @return null or postalCode
	 */
	public String getCountryName(Context context)
	{
		List<Address> addresses = getGeocoderAddress(context);
		if (addresses != null && addresses.size() > 0)
		{
			Address address = addresses.get(0);
			String countryName = address.getCountryName();

			return countryName;
		}
		else
		{
			return null;
		}
	}

	public class MyLocationListner implements LocationListener {

		private Activity act;

		public void setOnLocationUpdate(Activity argAct)
		{
			this.act = argAct;
		}

		public void stopLocationUpdate() {
			if(act!=null)
				act =null;

		}

		@Override
		public void onLocationChanged(Location location) {
			if(location.hasAccuracy())
			{ 
				location = refineLocation(location);
				if(location == null)
					return;

				if(AppConstant.userProfile.isIsSOSOn() || AppConstant.userProfile.isIsTrackingOn()){
					recentCapturedLocation = location;
					recentLocCapturedTime = System.currentTimeMillis();
					GeoTag geoTag = new GeoTag();
					geoTag.setLat(""+location.getLatitude());
					geoTag.setLong(""+location.getLongitude());
					geoTag.setAlt(""+location.getAltitude());
					geoTag.setTimeStamp(AppConstant.getTicks_time(location.getTime()));
					geoTag.setSpeed((int)location.getSpeed());

					if(AppConstant.ascGroups!=null&&AppConstant.ascGroup.getGroupID() !=null)
						geoTag.setGroupID(AppConstant.ascGroup.getGroupID());
					else
						geoTag.setGroupID(",0,");
					if(AppConstant.userProfile.isIsSOSOn()){
						geoTag.setIsSOS(1);
					}else
						geoTag.setIsSOS(0);
					GeoTagList.add(geoTag);

					if(act!=null)
					{
						OnLocationUpdate locUpdate = (OnLocationUpdate)act;
						locUpdate.OnLocationUpdate(location);
					}

					GPSTracker.latitude = location.getLatitude();
					GPSTracker.longitude= location.getLongitude();
					GPSTracker.latestSpeed = (int)location.getSpeed();
					GPSTracker.recordedTime = AppConstant.getTicks_time(location.getTime());

					GPSTracker.this.broadcastLocation();
				}
			}
		}

		private Location refineLocation(Location location){
			int locCount = GeoTagList.size();

			if (locCount == 0 || ((System.currentTimeMillis() - recentLocCapturedTime)>5*60*1000)){
				if(location.hasAccuracy() && location.getAccuracy()>150){
					return location;
				}else{
					return location;
				}
			}

			double distance = location.distanceTo(recentCapturedLocation);
			if (location.getAccuracy() > 150 || distance < 25)
			{
				return null;
			}
			return location;
		}

		@Override
		public void onProviderDisabled(String provider) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onProviderEnabled(String provider) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onStatusChanged(String provider, int status, Bundle extras) {


		}


		private boolean checkPositionChanged(Location loc) {
			if(GeoTagList.get(GeoTagList.size()-1).getLat().equalsIgnoreCase(""+loc.getLatitude())
					&&GeoTagList.get(GeoTagList.size()-1).getLong().equalsIgnoreCase(""+loc.getLongitude()))
				return false;
			else
				return true;
		}


	}

	private void broadcastLocation()
	{
		Intent intent = new Intent(TrackMeActivity.ACTION_RECEIVE_LOCATION);
		this.mContext.sendBroadcast(intent);
	}
}