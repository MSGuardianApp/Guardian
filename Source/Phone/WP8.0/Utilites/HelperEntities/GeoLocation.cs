using System;

namespace SOS.Phone
{
    public class GeoLocation
    {
        //public string Identifier { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string Alt { get; set; }
        public int Speed { get; set; }

        //Commented to save the local storage space
        public double Accuracy { get; set; }
        //public double VAccuracy { get; set; }

        public DateTime TimeStamp { get; set; }
        //public System.Windows.Media.Color StrokeColor { get; set; }
        /// <summary>
        /// Track or SOS
        /// </summary>
        public bool IsSOS { get; set; }
    }
}
