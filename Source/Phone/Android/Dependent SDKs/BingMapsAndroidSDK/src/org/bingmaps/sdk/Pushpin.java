package org.bingmaps.sdk;

/**
 * This class represents a pushpin object. 
 * @author rbrundritt
 */
public class Pushpin extends BaseEntity {
	/* Constructors */
	
	public Pushpin(){
		super(EntityType.Pushpin);
		
		Location = new Coordinate();
	}
	
	public Pushpin(Coordinate coordinate){
		super(EntityType.Pushpin);
		
		Location = coordinate;
	}
	
	public Pushpin(Coordinate coordinate, PushpinOptions options){
		super(EntityType.Pushpin);
		
		Location = coordinate;
		
		Options = options;
	}
	
	/* Public Properties */
	
	/**
	 * The coordinate where the pushpin is positioned
	 */
	public Coordinate Location;
	
	/**
	 * The pushpin options to be used when rendering the pushpin.
	 */
	public PushpinOptions Options;
	
	/* Public Methods */
	
	@Override
	public String toString() {
		StringBuilder sb = new StringBuilder();
		sb.append("{EntityId:");
		sb.append(EntityId);
		sb.append(",Entity:new MM.Pushpin(");
		sb.append(Location.toString());

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
