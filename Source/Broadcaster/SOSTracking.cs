using System;


namespace SOS.Temp.AzureStorageAccessLayer.Entities
{

    [Serializable]
    public class SOSTracking : StoreEntityBase
    {
        public string ProfileID { get { return base.PartitionKey; } set { base.PartitionKey = value; } }

        public string Token { get; set; }

        public bool InProcess { get; set; }

        public DateTime LastSMSPostTime { get; set; }

        public DateTime LastEmailPostTime { get; set; }

        public DateTime LastFacebookPostTime { get; set; }

        public string LastReportedLatitude { get; set; }

        public string LastReportedLongitude { get; set; }

        public DateTime LastLocationUpdate { get; set; }

        public string Command { get; set; }

        public string ExtendedCommand { get; set; }

        public long ClientTimeStamp { get; set; }

    }
}
