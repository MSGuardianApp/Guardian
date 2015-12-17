package com.guardianapp.utilities;

public class VersionComparator implements Comparable<VersionComparator> {
	
	private String version;
	
	public VersionComparator(String version){
		if(version == null)
            throw new IllegalArgumentException("Version can not be null");
        if(!version.matches("[0-9]+(\\.[0-9]+)*"))
            throw new IllegalArgumentException("Invalid version format");
        this.version = version;
	}
	
	public String getVersion() {
		return version;
	}

	public void setVersion(String version) {
		this.version = version;
	}

	@Override
	public int compareTo(VersionComparator another) {
		// TODO Auto-generated method stub
		if(another == null)
            return 1;
        String[] thisParts = this.getVersion().split("\\.");
        String[] thatParts = another.getVersion().split("\\.");
        int length = Math.max(thisParts.length, thatParts.length);
        for(int i = 0; i < length; i++) {
            int thisPart = i < thisParts.length ?
                Integer.parseInt(thisParts[i]) : 0;
            int thatPart = i < thatParts.length ?
                Integer.parseInt(thatParts[i]) : 0;
            if(thisPart < thatPart)
                return 1;
            if(thisPart > thatPart)
                return 0;
        }
        return 0;
	}

	@Override
	public boolean equals(Object obj) {
		// TODO Auto-generated method stub
		if(this == obj)
            return true;
        if(obj == null)
            return false;
        if(this.getClass() != obj.getClass())
            return false;
        return this.compareTo((VersionComparator) obj) == 0;
	}
	
	

}
