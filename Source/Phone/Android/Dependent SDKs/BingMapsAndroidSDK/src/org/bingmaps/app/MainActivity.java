package org.bingmaps.app;

import java.util.HashMap;

import org.bingmaps.app.R;
import org.bingmaps.sdk.BingMapsView;
import org.bingmaps.sdk.Coordinate;
import org.bingmaps.sdk.EntityClickedListener;
import org.bingmaps.sdk.EntityLayer;
import org.bingmaps.sdk.MapLoadedListener;
import org.bingmaps.sdk.MapMovedListener;
import org.bingmaps.sdk.MapStyles;
import org.bingmaps.sdk.Pushpin;
import org.bingmaps.sdk.PushpinOptions;
import android.app.Activity;
import android.app.ProgressDialog;
import android.location.Location;
import android.location.LocationListener;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.widget.ViewFlipper;
import android.widget.ZoomButton;

public class MainActivity extends Activity {
	private BingMapsView bingMapsView;
	private GPSManager _GPSManager;
	private EntityLayer _gpsLayer;
	private ProgressDialog _loadingScreen;
	
	private Activity _baseActivity;

	CharSequence[] _dataLayers;
	boolean[] _dataLayerSelections;
	
    @Override
    public void onCreate(Bundle savedInstanceState) {
    	super.onCreate(savedInstanceState);
    	
    	requestWindowFeature(Window.FEATURE_NO_TITLE);
    	
    	//OPTION Lock map orientation
    	//setRequestedOrientation(1);
        
        setContentView(R.layout.main);
        
        Initialize();
    }
    
    private void Initialize()
    {
    	_baseActivity = this;
    	_GPSManager = new GPSManager((Activity)this, new GPSLocationListener());

		//Add more data layers here
    	_dataLayers = new String[] { getString(R.string.traffic)};
    	_dataLayerSelections =  new boolean[ _dataLayers.length ];
    	
    	_loadingScreen = new ProgressDialog(this);
		_loadingScreen.setCancelable(false);
		_loadingScreen.setMessage(this.getString(R.string.loading) + "...");
    	
    	bingMapsView = (BingMapsView) findViewById(R.id.mapView);
    	
    	//Create handler to switch out of Splash screen mode
    	final Handler viewHandler = new Handler() {
			public void handleMessage(Message msg) {
				((ViewFlipper) findViewById(R.id.flipper)).setDisplayedChild(1);
			}
		};
		
		//Add a map loaded event handler
    	bingMapsView.setMapLoadedListener(new MapLoadedListener() {
			public void onAvailableChecked() {
				// hide splash screen and go to map
				viewHandler.sendEmptyMessage(0);
				
				//Add GPS layer
				_gpsLayer = new EntityLayer(Constants.DataLayers.GPS);
				bingMapsView.getLayerManager().addLayer(_gpsLayer);
				UpdateGPSPin();
			}
		});
    	
    	//Add a entity clicked event handler
    	bingMapsView.setEntityClickedListener(new EntityClickedListener() {
			public void onAvailableChecked(String layerName, int entityId) {
				HashMap<String, Object> metadata = bingMapsView.getLayerManager().GetMetadataByID(layerName, entityId);
				DialogLauncher.LaunchEntityDetailsDialog(_baseActivity, metadata);
			}
		});
    	
    	//Load the map
    	bingMapsView.loadMap(Constants.BingMapsKey, _GPSManager.GetCoordinate(), Constants.DefaultGPSZoomLevel, this.getString(R.string.mapCulture));
    	
    	// Create zoom out button functionality
		final ZoomButton zoomOutBtn = (ZoomButton) findViewById(R.id.zoomOutBtn);
		zoomOutBtn.setOnClickListener(new OnClickListener() {
			public void onClick(View v) {
				bingMapsView.zoomOut();
			}
		});

		// Create zoom button in functionality
		final ZoomButton zoomInBtn = (ZoomButton) findViewById(R.id.zoomInBtn);
		zoomInBtn.setOnClickListener(new OnClickListener() {
			public void onClick(View v) {
				bingMapsView.zoomIn();
			}
		});
    }    
    
    @Override
	public boolean onCreateOptionsMenu(Menu menu) {
		MenuInflater inflater = getMenuInflater();
		inflater.inflate(R.layout.menu, menu);
		return true;
	}
    
    @Override
	public boolean onOptionsItemSelected(MenuItem item) {
		// Handle item selection
		switch (item.getItemId()) {
			//Map Mode menu items
			case R.id.autoBtn:
				bingMapsView.setMapStyle(MapStyles.Auto);
				item.setChecked(!item.isChecked());
				return true;
			case R.id.roadBtn:
				bingMapsView.setMapStyle(MapStyles.Road);
				item.setChecked(!item.isChecked());
				return true;
			case R.id.aerialBtn:
				bingMapsView.setMapStyle(MapStyles.Aerial);
				item.setChecked(!item.isChecked());
				return true;
			case R.id.birdseyeBtn:
				bingMapsView.setMapStyle(MapStyles.Birdseye);
				item.setChecked(!item.isChecked());
				return true;
			//More option items
			case R.id.aboutMenuBtn:
				DialogLauncher.LaunchAboutDialog(this);
				return true;
			case R.id.layersMenuBtn:
				DialogLauncher.LaunchLayersDialog(this, bingMapsView, _dataLayers, _dataLayerSelections);
				return true;
			case R.id.clearMapMenuBtn:
				bingMapsView.getLayerManager().clearLayer(null);
				
				//unselect all layers
				for(int i=0;i<_dataLayerSelections.length;i++){
					_dataLayerSelections[i] = false;
				}
				
				//re-add GPS layer
				bingMapsView.getLayerManager().clearLayer(Constants.DataLayers.GPS);
				UpdateGPSPin();
				return true;
			//GPS Menu Item
			case R.id.gpsMenuBtn:
				Coordinate coord = _GPSManager.GetCoordinate();
				
				if(coord != null){
					//Center on users GPS location
					bingMapsView.setCenterAndZoom(coord, Constants.DefaultGPSZoomLevel);
				}
				return true;
			//Search Menu Item
			case R.id.searchMenuBtn:
				DialogLauncher.LaunchSearchDialog(this, bingMapsView, loadingScreenHandler);
				return true;	
			//Directions Menu Item
			case R.id.directionsMenuBtn:
				DialogLauncher.LaunchDirectionsDialog(this, bingMapsView, loadingScreenHandler);
				return true;
			default:
				return super.onOptionsItemSelected(item);
		}
	}

    private void UpdateGPSPin(){		
    	PushpinOptions opt = new PushpinOptions();
    	opt.Icon = Constants.PushpinIcons.GPS;
		Pushpin p = new Pushpin(_GPSManager.GetCoordinate(), opt);
		if (p.Location != null) {		
			_gpsLayer.clear();
			_gpsLayer.add(p);
			_gpsLayer.updateLayer();
		}
    }
    
    @SuppressWarnings("unused")
	private final MapMovedListener mapMovedListener = new MapMovedListener() {
		public void onAvailableChecked() {
			//OPTION Add logic to Update Layers here. 
			//This will update data layers when the map is moved.
		}
	};
	
	/**
	 * Handler for loading Screen
	 */
	protected Handler loadingScreenHandler = new Handler() {
		public void handleMessage(Message msg) {
			if (msg.arg1 == 0) {
			//	_loadingScreen.hide();
			} else {
			//	_loadingScreen.show();
			}
		}
	};
	
	public class GPSLocationListener implements LocationListener {
		public void onLocationChanged(Location arg0) {
			UpdateGPSPin();
		}

		public void onProviderDisabled(String arg0) {
		}

		public void onProviderEnabled(String arg0) {
		}

		public void onStatusChanged(String arg0, int arg1, Bundle arg2) {
		}	
	}
}