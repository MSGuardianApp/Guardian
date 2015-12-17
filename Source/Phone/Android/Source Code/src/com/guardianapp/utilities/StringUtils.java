package com.guardianapp.utilities;


/**
 * @author dharani
 *
 */
public class StringUtils {

  public static boolean invalidString(String s){
    return !validString(s);
  }

  public static boolean validString(String s){
    return s != null && !s.trim().equalsIgnoreCase("");
  }
}
