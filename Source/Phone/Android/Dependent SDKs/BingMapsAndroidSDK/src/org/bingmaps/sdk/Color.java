package org.bingmaps.sdk;

/**
 * A class that is used to represent colors to be used in the Bing Maps View.
 * @author rbrundritt
 */
public class Color {
	/* Constructors */
	
	/**
	 * Constructor
	 */
	public Color(){	
	}
	
	/**
	 * Constructor
	 * @param r Red byte value
	 * @param g Green byte value
	 * @param b Blue byte value
	 */
	public Color(int r, int g, int b){
		this.A = 1;
		this.R = r;
		this.G = g;
		this.B = b;
	}
	
	/**
	 * Constructor
	 * @param a Alpha transparency byte value
	 * @param r Red byte value
	 * @param g Green byte value
	 * @param b Blue byte value
	 */
	public Color(int a, int r, int g, int b){
		this.A = a;
		this.R = r;
		this.G = g;
		this.B = b;
	}
	
	/* Public Properties */
	
	/**
	 * Alpha (opacity) between 0 and 1
	 */
	public int A;
	
	/**
	 * Red color - between 0 and 255
	 */
	public int R;
	
	/**
	 * Green color - between 0 and 255
	 */
	public int G;
	
	/**
	 * Blue color - between 0 and 255
	 */
	public int B;
	
	/* Public Methods */
	
	/**
	 * Generates a JSON string representation of the color for use with Bing Maps.
	 */
	public String toString(){
		A = (A > 255) ? 255:((A < 0)? 0 : A);
		R = (R > 255) ? 255:((R < 0)? 0 : R);
		G = (G > 255) ? 255:((G < 0)? 0 : G);
		B = (B > 255) ? 255:((B < 0)? 0 : B);
		
		return String.format("new MM.Color(%s, %s, %s, %s)", A, R, G, B);
	}
}
