using System.ComponentModel;

namespace SOS.Phone.MVVM.Model
{
    class MapAddressModel : INotifyPropertyChanged, INotifyPropertyChanging
    {

        public MapAddressModel()
        {
        }

        public string BuildingFloor { get; set; }
        public string BuildingName { get; set; }
        public string BuildingRoom { get; set; }
        public string BuildingZone { get; set; }
        public string City { get; set; }
        public string Continent { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string County { get; set; }
        public string District { get; set; }
        public string HouseNumber { get; set; }
        public string Neighborhood { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string Street { get; set; }
        public string Township { get; set; }
        public string Address { get; set; }
        public System.Device.Location.GeoCoordinate GeoCoordinate { get; set; }      
        
       

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
