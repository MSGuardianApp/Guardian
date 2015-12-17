using System;


namespace SOS.AzureStorageAccessLayer.Entities
{
    [Serializable]
    public class LocationHistory : StoreEntityBase
    {
        public string ProfileID
        {
            get { return base.PartitionKey; }
            set { base.PartitionKey = value; }
        }

        public string SessionID { get; set; }

        public bool IsSOS { get; set; }

        public string Lat { get; set; }

        public string Long { get; set; }

        public string Alt { get; set; }

        public int Speed { get; set; }

        public DateTime ClientDateTime { get; set; }

        public long ClientTimeStamp { get; set; }

        public string MediaUri { get; set; }

        public double Accuracy { get; set; }
    }

    [Serializable]
    public class GeoTag
    {
        public string SessionID { get; set; }
        public bool? IsSOS { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Alt { get; set; }
        public int Speed { get; set; }
        public long ClientTimeStamp { get; set; }
        public long ProfileID { get; set; }
        public string MediaUri { get; set; }
        public string GroupID { get; set; }
        public double Accuracy { get; set; }
    }
}
