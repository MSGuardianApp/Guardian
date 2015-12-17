using System;
using System.Runtime.Serialization;

namespace SOS.Service.Interfaces.DataContracts
{
    [DataContract]
    public class DemographyReport
    {
        [DataMember]
        public int SNo { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string LiveId { get; set; }

        [DataMember]
        public int BuddiesCount { get; set; }

        [DataMember]
        public int GroupCount { get; set; }

        [DataMember]
        public string GroupValidationPending { get; set; }

        [DataMember]
        public string FacebookLinked { get; set; }
    }

      [DataContract]
      [Serializable]
    public class SOSTrackingReport
    {
        [DataMember]
        public string SNo { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string TotalTimeinSOS { get; set; }
        [DataMember]
        public string SOSAlerts { get; set; }
        [DataMember]
        public string EmailAlerts { get; set; }
        [DataMember]
        public string EmailBuddies { get; set; }
        [DataMember]
        public string SOSBuddies { get; set; }
    }

     [DataContract]
     [Serializable]
      public class SOSAndTrackonReport
      {
          [DataMember]
          public string SNo { get; set; }
          [DataMember]
          public string UserName { get; set; }
          [DataMember]
          public string MobileNumber { get; set; }         
          [DataMember]
          public string TotalTracks { get; set; }
          [DataMember]
          public string TotalSOSs { get; set; }
          [DataMember]
          public string TotalSMSSent { get; set; }
          [DataMember]
          public string TotalEmailSent { get; set; }          
      }

}
