using System;


namespace SOS.AzureStorageAccessLayer.Entities
{

    [Serializable]
    public class SessionHistory : StoreEntityBase
    {
        public string ProfileID { get { return base.PartitionKey; } set { base.PartitionKey = value; } }

        public string ClientTimeStamp { get { return base.RowKey; } set { base.RowKey = value; } }

        public string SessionID { get; set; }

        public bool? IsSOS { get; set; }

        public string Lat { get; set; }

        public string Long { get; set; }

        public DateTime? LastCapturedDate { get; set; }

        public string Command { get; set; }

        public string ExtendedCommand { get; set; }

        public string MobileNumber { get; set; }
        public string Name { get; set; }

        public DateTime SessionStartTime { get; set; }

        public DateTime? SessionEndTime { get; set; }
            
        public bool? InSOS { get; set; }

        public DateTime? LastSMSPostTime { get; set; }

        public DateTime? LastEmailPostTime { get; set; }

        public DateTime? LastFacebookPostTime { get; set; }

        public string SMSRecipientsList { get; set; }

        public string EmailRecipientsList { get; set; }

        public string TinyURI { get; set; }
       
        public int NoOfSMSRecipients { get; set; }

        public int NoOfEmailRecipients { get; set; }

        public int NoOfSMSSent { get; set; }

        public int NoOfEmailsSent { get; set; }

        public string DispatchInfo { get; set; }

        public bool IsEvidenceAvailable { get; set; }

        public DateTime LastModifiedDate { get; set; }

    }
}
