using System.Runtime.Serialization;

namespace SOS.Service.Interfaces.DataContracts
{
    [DataContract]
   public class ActiveSOSReports
    {
        [DataMember]
        public int SNo { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string MobileNumber { get; set; }

        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public string SOSAlertCount { get; set; }

        [DataMember]
        public string StartTime { get; set; }

        [DataMember]
        public string ProfileId { get; set; }
    }
}
