using System;

namespace SOS.Phone
{
    public class GeoCoordinateEx
    {
        public GeoCoordinateEx(System.Device.Location.GeoCoordinate co) { GeoCoordinate = co; }        
        public GeoCoordinateEx() { GeoCoordinate = null; }
        public System.Device.Location.GeoCoordinate GeoCoordinate { get; private set; }

        public static implicit operator GeoCoordinateEx(System.Device.Location.GeoCoordinate co)
        {
            return new GeoCoordinateEx(co);
        }

        public static implicit operator GeoCoordinateEx(Windows.Devices.Geolocation.Geocoordinate coord)
        {
            return new GeoCoordinateEx(
                new System.Device.Location.GeoCoordinate()
                {
                    Altitude = coord.Altitude.HasValue ? coord.Altitude.Value : 0.0,
                    Course = coord.Heading.HasValue ? coord.Heading.Value : 0.0,
                    HorizontalAccuracy = Math.Round(coord.Accuracy),
                    Latitude = coord.Latitude,
                    Longitude = coord.Longitude,
                    Speed = coord.Speed.HasValue ? coord.Speed.Value : 0.0,
                    VerticalAccuracy = coord.AltitudeAccuracy.HasValue ? coord.AltitudeAccuracy.Value : 0.0,
                }
            );
        }

    }
}
