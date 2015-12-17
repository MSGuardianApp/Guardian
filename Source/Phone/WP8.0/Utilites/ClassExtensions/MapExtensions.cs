using System;
using System.Device.Location;

namespace SOS.Phone
{
    public static partial class Extensions
    {
        /// <summary>
        /// Allows you to set the center of a Map control using a Windows.Devices.Geolocation.Geocoordinate
        /// </summary>
        /// <param name="map">the Map control instance</param>
        /// <param name="coord">The Geocoordinate object to use to set the center of the map</param>
        public static void SetCenter(this Microsoft.Phone.Maps.Controls.Map map, Windows.Devices.Geolocation.Geocoordinate coord)
        {
            System.Device.Location.GeoCoordinate centre = new System.Device.Location.GeoCoordinate()
            {
                Latitude = coord.Latitude,
                Longitude = coord.Longitude,
            };

            map.Center = centre;
        }

        public static GeoLocation ToGeoLocation(this GeoCoordinate c)
        {
            GeoLocation loc = new GeoLocation()
            {
                Lat = c.Latitude,
                Long = c.Longitude,
                Alt = c.Altitude.ToString(),
                TimeStamp = DateTime.Now,
                //StrokeColor = (Globals.CurrentProfile.IsSOSOn) ? Colors.Orange : Colors.Green,
                IsSOS = Globals.CurrentProfile.IsSOSOn,
                Accuracy = double.IsNaN(c.HorizontalAccuracy) ? 0 : Math.Round(c.HorizontalAccuracy),
                //VAccuracy = double.IsNaN(c.VerticalAccuracy) ? 0 : c.VerticalAccuracy,
                Speed = double.IsNaN(c.Speed) ? (int)Math.Round(Utility.CalculateSpeed(Globals.RecentLocation.Coordinate, c, (DateTime.Now - Globals.RecentLocation.CapturedTime).Seconds)) : Convert.ToInt32(c.Speed)
            };
            return loc;
        }
    }

}
