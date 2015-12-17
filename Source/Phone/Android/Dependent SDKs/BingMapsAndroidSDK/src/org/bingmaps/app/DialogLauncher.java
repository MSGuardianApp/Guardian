package org.bingmaps.app;

import java.util.HashMap;

import org.bingmaps.bsds.BingSpatialDataService;
import org.bingmaps.bsds.Record;
import org.bingmaps.rest.BingMapsRestService;
import org.bingmaps.rest.RoutePathOutput;
import org.bingmaps.rest.RouteRequest;
import org.bingmaps.sdk.BingMapsView;
import org.bingmaps.sdk.EntityLayer;
import org.bingmaps.sdk.LocationRect;
import org.bingmaps.sdk.Point;
import org.bingmaps.sdk.Polyline;
import org.bingmaps.sdk.Pushpin;
import org.bingmaps.sdk.PushpinOptions;
import org.bingmaps.sdk.Utilities;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.DialogInterface.OnMultiChoiceClickListener;
import android.content.Intent;
import android.net.Uri;
import android.os.Handler;
import android.os.Message;
import android.view.View;
import android.view.ViewGroup;
import android.view.View.OnClickListener;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.ScrollView;
import android.widget.TextView;

public class DialogLauncher {

	public static void LaunchAboutDialog(final Activity activity){
		final View aboutView = activity.getLayoutInflater().inflate(R.layout.about, (ViewGroup)activity.findViewById(R.id.aboutView));
		
		AlertDialog.Builder aboutAlert = new AlertDialog.Builder(activity)
		.setTitle("About")
		.setIcon(android.R.drawable.ic_dialog_info)
		.setView(aboutView) 
		.setPositiveButton("Ok", new DialogInterface.OnClickListener() {  
			  public void onClick(DialogInterface dialog, int whichButton) {  
				    // Canceled. Do nothing
				  }  
				});
		aboutAlert.show(); 
	}
	
	public static void LaunchLayersDialog(final Activity activity, final BingMapsView bingMapsView, final CharSequence[] dataLayers, boolean[] dataLayerSelections){
		new AlertDialog.Builder(activity)
		   .setIcon(android.R.drawable.ic_menu_slideshow)
	       .setTitle(activity.getString(R.string.layers))
	       .setMultiChoiceItems(dataLayers, dataLayerSelections, new OnMultiChoiceClickListener() {
				public void onClick(DialogInterface arg0, int idx, boolean isChecked) {
					if(dataLayers[idx] == activity.getString(R.string.traffic)){
						bingMapsView.showTraffic(isChecked);
						
						//Sample code of how to handle custom tile layer
						/*if(isChecked){
							//bingMapsView.getLayerManager().addLayer(new TileLayer(activity.getString(R.string.traffic), "http://ecn.t{subdomain}.tiles.virtualearth.net/tiles/dp/content?p=tf&a={quadkey}", 0.5));
						}else{
							//bingMapsView.getLayerManager().clearLayer(activity.getString(R.string.traffic));
						}*/
					}
					//Add support for more map data layers here
				}
			})
	       .setPositiveButton("Close", new DialogInterface.OnClickListener(){
				public void onClick(DialogInterface arg0, int arg1) {
				}
	       })
	       .show();
	}
	
	public static void LaunchSearchDialog(final Activity activity, final BingMapsView bingMapsView, final Handler loadingScreenHandler){
		final View searchView = activity.getLayoutInflater().inflate(R.layout.search_input, (ViewGroup)activity.findViewById(R.id.searchInputView));
		
		AlertDialog.Builder searchAlert = new AlertDialog.Builder(activity)
		.setTitle("Search")
		.setIcon(android.R.drawable.ic_menu_search)
		.setView(searchView) 
		.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {  
		  public void onClick(DialogInterface dialog, int whichButton) {  
		    // Canceled. Do nothing
		  }  
		})
		.setPositiveButton("Search", new DialogInterface.OnClickListener() {  
		public void onClick(DialogInterface dialog, int whichButton) {  
		  EditText input = (EditText)searchView.findViewById(R.id.searchInput);
		  String searchText = input.getText().toString().trim(); 
		  if(!Utilities.isNullOrEmpty(searchText)){
			 Message viewMsg = new Message();
	     	 viewMsg.arg1 = 1;
			 loadingScreenHandler.sendMessage(viewMsg);
			  try {
					BingMapsRestService bmService = new BingMapsRestService(Constants.BingMapsKey, activity.getString(R.string.serviceCulture));
					bmService.GeocodeAsyncCompleted = new Handler(){
						public void handleMessage(Message msg) {
							if(msg.obj != null){
								org.bingmaps.rest.models.Location[] locations = (org.bingmaps.rest.models.Location[])msg.obj;
								
								org.bingmaps.rest.models.Location l = locations[0];
								if(l.Point != null){
									EntityLayer searchLayer = (EntityLayer)bingMapsView.getLayerManager().getLayerByName(Constants.DataLayers.Search);
									
									if(searchLayer == null){
										searchLayer = new EntityLayer(Constants.DataLayers.Search);
									}
									
									searchLayer.clear();
																		
									PushpinOptions po = new PushpinOptions();
									po.Icon = Constants.PushpinIcons.RedFlag;
									po.Width = 20;
									po.Height = 35;
									po.Anchor = new Point(4, 35);
									
									Pushpin location = new Pushpin(l.Point, po);		
									searchLayer.add(location);
									bingMapsView.getLayerManager().addLayer(searchLayer);
									searchLayer.updateLayer();
									bingMapsView.setCenterAndZoom(l.Point, Constants.DefaultSearchZoomLevel);
								
									//Search for nearby locations
									BingSpatialDataService bsds = new BingSpatialDataService(
											Constants.BingSpatialAccessId,
											Constants.BingSpatialDataSourceName,
											Constants.BingSpatialEntityTypeName,
											Constants.BingSpatialQueryKey);
									
									//Perform a nearby search for POI data
									bsds.FindByAreaCompleted = new Handler(){
										public void handleMessage(Message msg) {
											if(msg.obj != null){
												Record[] records = (Record[])msg.obj;
												EntityLayer el = (EntityLayer)bingMapsView.getLayerManager().getLayerByName(Constants.DataLayers.Search);
												double maxLat = -90, minLat = 90, maxLon = -180, minLon = 180;
												
												for(Record r : records){
													Pushpin p = new Pushpin(r.Location);
													p.Title = r.DisplayName;
													
													if(r.Location.Latitude > maxLat){
														maxLat = r.Location.Latitude;
													}
													if(r.Location.Longitude > maxLon){
														maxLon = r.Location.Longitude;
													}
													if(r.Location.Latitude < minLat){
														minLat = r.Location.Latitude;
													}
													if(r.Location.Longitude < minLon){
														minLon = r.Location.Longitude;
													}
													
													HashMap<String, Object> metadata = new HashMap<String, Object>();
													metadata.put("record", r);
													el.add(p, metadata);
												}
												
												bingMapsView.setMapView(new LocationRect(maxLat, maxLon, minLat, minLon));
												
												el.updateLayer();
											}
											
											Message v = new Message();
									     	v.arg1 = 0;
											loadingScreenHandler.sendMessage(v);
										}
									};
									
									bsds.FindByArea(l.Point, Constants.SearchRadiusKM, null);
								}else{
									Message v = new Message();
							     	v.arg1 = 0;
									loadingScreenHandler.sendMessage(v);
								}
							}else{
								Message v = new Message();
						     	v.arg1 = 0;
								loadingScreenHandler.sendMessage(v);
							}
						}
					};
					
					bmService.GeocodeAsync(searchText);
				} catch (Exception e) {
					Message v = new Message();
			     	v.arg1 = 0;
					loadingScreenHandler.sendMessage(v);
				}
			 }
		  }  
		});  
		searchAlert.show(); 
	}

	public static void LaunchDirectionsDialog(final Activity activity, final BingMapsView bingMapsView, final Handler loadingScreenHandler){
		final View directionsView = activity.getLayoutInflater().inflate(R.layout.directions_input, (ViewGroup)activity.findViewById(R.id.directionsInputView));
		
		AlertDialog.Builder directionsAlert = new AlertDialog.Builder(activity)  
		.setTitle("Directions")  						
		.setView(directionsView) 
		.setIcon(android.R.drawable.ic_menu_directions)
		.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {  
			  public void onClick(DialogInterface dialog, int whichButton) {  
			    // Canceled. Do nothing
			  }  
		})
		.setPositiveButton("Get Directions", new DialogInterface.OnClickListener() {  
			public void onClick(DialogInterface dialog, int whichButton) {  
				final EditText fromInput = (EditText)directionsView.findViewById(R.id.directionsFromInput);
				final EditText toInput = (EditText)directionsView.findViewById(R.id.directionsToInput);
				
				String fromText = fromInput.getText().toString().trim(); 
				String toText = toInput.getText().toString().trim(); 
						
			  if(!Utilities.isNullOrEmpty(fromText) && !Utilities.isNullOrEmpty(toText)){
				 Message viewMsg = new Message();
		     	 viewMsg.arg1 = 1;
				 loadingScreenHandler.sendMessage(viewMsg);
				  try {
					  BingMapsRestService bmService = new BingMapsRestService(Constants.BingMapsKey, activity.getString(R.string.serviceCulture));
						bmService.RouteAsyncCompleted = new Handler(){
							public void handleMessage(Message msg) {
								if(msg.obj != null){
									org.bingmaps.rest.models.Route route = (org.bingmaps.rest.models.Route)msg.obj;
									
									EntityLayer routeLayer = (EntityLayer)bingMapsView.getLayerManager().getLayerByName(Constants.DataLayers.Route);
									
									if(routeLayer == null){
										routeLayer = new EntityLayer(Constants.DataLayers.Route);
									}

									routeLayer.clear();
									
									PushpinOptions pOption1 = new PushpinOptions();
									pOption1.Icon = Constants.PushpinIcons.Start;
									pOption1.Width = 33;
									pOption1.Height = 43;
									pOption1.Anchor = new Point(4, 40);
									
									Pushpin start = new Pushpin(route.RouteLegs.get(0).ActualStart, pOption1);		
									routeLayer.add(start);
									
									PushpinOptions pOption2 = pOption1.clone();
									pOption2.Icon = Constants.PushpinIcons.End;
									
									Pushpin end = new Pushpin(route.RouteLegs.get(0).ActualEnd, pOption2);		
									routeLayer.add(end);
									
									Polyline routeLine = new Polyline(route.RoutePath);
									routeLayer.add(routeLine);

									bingMapsView.getLayerManager().addLayer(routeLayer);
									routeLine = null;
									
									routeLayer.updateLayer();
									
									if(route.BoundingBox != null){
										bingMapsView.setMapView(route.BoundingBox);
									}
									
									route = null;
									msg.obj = null;
								}
								
								Message v = new Message();
						     	v.arg1 = 0;
								loadingScreenHandler.sendMessage(v);
							}
						};
						
						RouteRequest rr = new RouteRequest();
						rr.addWaypoint(fromText);
						rr.addWaypoint(toText);
						rr.setRoutePathOutput(RoutePathOutput.Points);
						
						bmService.RouteAsync(rr);
					} catch (Exception e) {
						Message v = new Message();
				     	v.arg1 = 0;
						loadingScreenHandler.sendMessage(v);
					}
				 }
			  }  
			});  
			  
		directionsAlert.show(); 
	}
	
	public static void LaunchEntityDetailsDialog(final Activity activity, final HashMap<String, Object> metadata){
		if(metadata.size() > 0 ){
			if(metadata.containsKey("record") && metadata.get("record").getClass() == Record.class){
				Record record = (Record)metadata.get("record");
				
				String title = Utilities.isNullOrEmpty(record.DisplayName)? 
						activity.getString(R.string.details) : record.DisplayName;
						
				final ScrollView detailsView = (ScrollView)activity.getLayoutInflater().inflate(R.layout.details_view, (ViewGroup)activity.findViewById(R.id.detailsView));
				
				if(record.Address != null){
					TextView addressView = (TextView)detailsView.findViewById(R.id.detailsAddress);
					addressView.setText("Address: " + record.Address.toString());
				}
				
				if(!Utilities.isNullOrEmpty(record.Phone)){
					final String phone = "tel:" + record.Phone;
					
					ImageButton phoneBtn = (ImageButton)detailsView.findViewById(R.id.detailsPhoneBtn);
					phoneBtn.setOnClickListener(new OnClickListener() {
						public void onClick(View v) {
							//Call phone number
							Intent i = new Intent(Intent.ACTION_DIAL, Uri.parse(phone));
							activity.startActivity(i);
						}
					});
					phoneBtn.setVisibility(View.VISIBLE);
				}	
				
				//OPTION Add custom content to view
				LinearLayout ccView = (LinearLayout)detailsView.findViewById(R.id.detailsCustomContent);
				
				if(record.Metadata.containsKey("Manager")){
					String manager = (String) record.Metadata.get("Manager");
					if(!Utilities.isNullOrEmpty(manager)){
						TextView managerView = new TextView(activity);
						managerView.setText("Manager: " + manager);
						ccView.addView(managerView);
					}
				}
				
				if(record.Metadata.containsKey("StoreType")){
					String storeType = (String) record.Metadata.get("StoreType");
					if(!Utilities.isNullOrEmpty(storeType)){
						TextView storeTypeView = new TextView(activity);
						storeTypeView.setText("Store Type: " + storeType);
						ccView.addView(storeTypeView);
					}
				}
				
				AlertDialog.Builder detailsAlert = new AlertDialog.Builder(activity)
				.setTitle(title)
				.setView(detailsView) 
				.setNegativeButton("Close", new DialogInterface.OnClickListener() {  
				  public void onClick(DialogInterface dialog, int whichButton) {  
				    // Canceled. Do nothing
				  }  
				});
				detailsAlert.show();
			}else{
				//OPTION add support for HashMap so that data other than records 
				//from the Bing Spatial Data Services can be rendered.
			}
		}		
	}
}
