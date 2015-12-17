
using SOS.Phone.LocationServiceRef;
using System;
using System.Collections.Generic;
namespace SOS.Phone
{
    public class TrackingSession
    {
        public string SessionId { get; set; }

        public string ProfileId { get; set; }

        public List<GeoTagLite> GeoTags { get; private set; }

        public static TrackingSession ConvertServerGeoTags(GeoTags geoTags)
        {
            if (geoTags.LocCnt <= 0) return null;

            TrackingSession session = new TrackingSession();
            session.ProfileId = geoTags.PID.ToString();
            session.SessionId = geoTags.Id;
            session.GeoTags = new List<GeoTagLite>();
            for (int i = 0; i < geoTags.LocCnt; i++)
            {
                session.GeoTags.Add(new GeoTagLite()
                {
                    Lat = Convert.ToDouble(geoTags.Lat[i]),
                    Long = Convert.ToDouble(geoTags.Long[i]),
                    Alt = (geoTags.Alt != null) ? Convert.ToDouble(geoTags.Alt[i]) : 0,
                    TimeStamp = geoTags.TS[i],
                    IsSOS = geoTags.IsSOS[i],
                    Accuracy=geoTags.Accuracy[i],
                    Speed = geoTags.Spd!=null?geoTags.Spd[i] != null ? geoTags.Spd[i].ToString() : "0":"0"
                });
            }

            return session;
        }
    }

    public class GeoTagLite
    {
        public double Lat { get; set; }

        public double Long { get; set; }

        public double Alt { get; set; }

        public long TimeStamp { get; set; }

        public bool IsSOS { get; set; }

        public string Speed { get; set; }

        public double Accuracy { get; set; }
       
    }

}
