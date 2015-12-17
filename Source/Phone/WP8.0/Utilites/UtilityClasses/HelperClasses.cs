using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;

namespace SOS.Phone
{
    public class GeoTagPlus
    {
        public GeoCoordinate Coordinate { get; set; }
        public DateTime CapturedTime { get; set; }
        public string Address { get; set; }
        public DateTime AddressCapturedTime { get; set; }
    }

    public class FBGroupOwner
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class FBGroup
    {
        public string id { set; get; }
        public string name { set; get; }
        public FBGroupOwner owner { get; set; }
    }
    public class FBGroups
    {
        public List<FBGroup> data { get; set; }
    }
    public class FBGroupJsonWrapper
    {
        public string id { get; set; }
        public FBGroups groups { get; set; }
        
    }


    public class GeoPoint
    {
       public List<double> coordinates { get; set;}
    }
    public class Address
    {
       public string adminDistrict { get; set; }
       public string adminDistrict2 { get; set; }
       public string countryRegion { get; set; }
       public string formattedAddress { get; set; }
       public string locality { get; set; }
    }

    public class GeocodePoint
    {
       public List<double> coordinates { get; set; }
    }

    public class Resource
    {
        public GeoPoint point { get; set; }
      public Address address { get; set; }
      public List<GeocodePoint> geocodePoints { get; set; }
    }

    public class ResourceSet
    {
       public List<Resource> resources { get; set; }
    }

    public class RootObject
    {
        public List<ResourceSet> resourceSets { get; set; }
    }

    /// <summary>
    /// Map Mode
    /// </summary>
    public enum MapMode
    {
        /// <summary>
        /// Stores are displayed in the map
        /// </summary>
        Stores,

        /// <summary>
        /// Map is showing directions using a Windows Phone Task
        /// </summary>
        Directions,

        /// <summary>
        /// Map is showing a route in the map
        /// </summary>
        Route
    }

    public class Helpline 
    {
        public string Name { get; set; }
        public string PhoneNumber{ get; set; }
    }

    public class CountryCodes : INotifyPropertyChanged
    {
        private string _countryName ;
        public string CountryName
        {
            get
            {
                return _countryName;
            }
            set
            {
                if (value != _countryName)
                {
                    NotifyPropertyChanging("CountryName");
                    _countryName = value;
                    NotifyPropertyChanged("CountryName");
                }
            }
        }

        private string _isdCode;
        public string IsdCode
        {
            get
            {
                return _isdCode;
            }
            set
            {
                if (value != _isdCode)
                {
                    NotifyPropertyChanging("IsdCode");
                    _isdCode = value;
                    NotifyPropertyChanged("IsdCode");
                }
            }
        }

        public string MaxPhoneDigits { get; set; }

        public string Police { get; set; }

        public string Fire { get; set; }

        public string Ambulance { get; set; }


          #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

          #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }


}
