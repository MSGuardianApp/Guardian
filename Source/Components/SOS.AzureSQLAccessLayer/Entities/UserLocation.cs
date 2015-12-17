using System.Collections.Generic;

namespace SOS.AzureSQLAccessLayer
{
   public class UserLocation
    {
       public long UserID { get; set; }
        public long ProfileID { get; set; }
        public string Name { get; set; }
        //public string Lat { get; set; }
        //public string Long { get; set; }
        public List<GeoTagLocs> LastLocs { get; set; }
        public bool IsSOSOn { get; set; }
        public bool IsTrackingOn { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }

    }

   public class GeoTagLocs
    {
        public string Lat { get; set; }
       
        public string Long { get; set; }

        public long TimeStamp { get; set; }

        public string Accuracy { get; set; }
    }
}
