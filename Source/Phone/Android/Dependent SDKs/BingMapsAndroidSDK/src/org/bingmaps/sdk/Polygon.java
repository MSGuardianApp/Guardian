package org.bingmaps.sdk;

import java.util.ArrayList;
import java.util.List;

public class Polygon extends BaseEntity {

	public Polygon(){
		super(EntityType.Polygon);
		Locations = new ArrayList<Coordinate>();
	}
	
	public Polygon(List<Coordinate> locations){
		super(EntityType.Polygon);
		Locations = locations;
	}
	
	public List<Coordinate> Locations;
	
	public PolygonOptions Options;

	@Override
	public String toString() {
		StringBuilder sb = new StringBuilder();
		sb.append("{EntityId:");
		sb.append(EntityId);
		
		sb.append(",Entity:new MM.Polygon(");
		sb.append(Utilities.LocationsToString(Locations));
		
		if(Options != null){
			sb.append(",");
			sb.append(Options.toString());
		}
		sb.append(")");
		
		if(!Utilities.isNullOrEmpty(Title)){
			sb.append(",title:'");
			sb.append(Title);
			sb.append("'");
		}
		
		sb.append("}");
		
		return sb.toString();
	}
}
