using System;
using SOS.AzureStorageAccessLayer.Entities;

namespace SOS.OPsTools.Entities
{
    [Serializable]
    public class Location : StoreEntityBase
    {
        public string ProfileID
        {
            get { return base.PartitionKey; }
            set
            { base.PartitionKey = value; }
        }

        public string Identifier { get; set; }

        public string Lat { get; set; }

        public string Long { get; set; }

        public string Alt { get; set; }

        public int Speed { get; set; }

        public long ClientDateTime { get; set; }

        public string Command { get; set; }

        public string MediaUri { get; set; }

        public string GroupID { get; set; }

        public DateTime ClientTimeStamp { get; set; }

        public string IsSOS { get; set; }

        public string Group1 { get; set; }
        public string Group2 { get; set; }
        public string Group3 { get; set; }
        public string Group4 { get; set; }
        public string Group5 { get; set; }
        public string Group6 { get; set; }
        public string Group7 { get; set; }
        public string Group8 { get; set; }
        public string Group9 { get; set; }
        public string Group10 { get; set; }
    }

    [Serializable]
    public class GeoTag
    {
        public string Identifier { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Alt { get; set; }
        public int Speed { get; set; }
        public long ClientDateTime { get; set; }
        public string ProfileID { get; set; }
        public string Command { get; set; }
        public string MediaUri { get; set; }
        public string GroupID { get; set; }
        public string IsSOS { get; set; }
    }
}
