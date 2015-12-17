package org.bingmaps.sdk;

/**
 * Represents the options for a polygon.
 * @author rbrundritt
 */
public class PolygonOptions {

	/**
	 * The color of the inside of the polygon. Defaults to Green.
	 */
	public Color FillColor;
	/**
	 * The color of the outline of the polygon. Defaults to Blue.
	 */
	public Color StrokeColor;
	
	/**
	 * The thickness of the polygon.
	 */
	public int StrokeThickness;
	
	/**
	 * A string representing the stroke/gap sequence to use to draw the polyline. 
	 * The string must be in the format S, G, S*, G*, where S represents the stroke 
	 * length and G represents gap length. Stroke lengths and gap lengths can be 
	 * separated by commas or spaces. For example, a stroke dash string of “1 4 2 1” 
	 * would draw the polyline with a dash, four spaces, two dashes, one space, and 
	 * then repeated.
	 */
	public String StrokeDashArray; 

	/**
	 * Returns a JSON representation of the PolygonOption object.
	 * @return A JSON representation of the PolygonOption object.
	 */
	public String toString(){
		StringBuilder sb = new StringBuilder();
		sb.append("{");
		
		if(StrokeColor != null){
			sb.append("strokeColor:");
			sb.append(StrokeColor.toString());
		}else{
			//default polyline color to blue.
			sb.append("strokeColor:new MM.Color(200,0,0,200)");
		}
		
		if(StrokeColor != null){
			sb.append(",fillColor:");
			sb.append(FillColor.toString());
		}else{
			//default polygon color to blue.
			sb.append(",fillColor:new MM.Color(200,0,200,0)");
		}
		
		if(!Utilities.isNullOrEmpty(StrokeDashArray)){
			sb.append(",strokeDashArray:");
			sb.append(StrokeDashArray);
		}
		
		if(StrokeThickness > 0){
			sb.append(",strokeThickness:");
			sb.append(StrokeThickness);
		}
		
		sb.append("}");
		return sb.toString();
	}
}
