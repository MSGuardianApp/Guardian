package org.bingmaps.sdk;

/**
 * Represents the options for a polyline.
 * @author rbrundritt
 */
public class PolylineOptions {

	/**
	 * The color of the polyline.
	 */
	public Color StrokeColor;
	
	/**
	 * The thickness of the polyline. Defaults to Blue.
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
	 * Returns a JSON representation of the PolylineOption object.
	 * @return A JSON representation of the PolylineOption object.
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
