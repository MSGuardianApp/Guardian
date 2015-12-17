using System.Runtime.Serialization;

namespace SOS.Service.Interfaces.DataContracts.OutBound
{
    [DataContract]
    public class TileCounts
    {
        [DataMember]
        public string Buddys { get; set; }

        [DataMember]
        public string OnlineBuddys { get; set; }

        [DataMember]
        public string LocateBuddys { get; set; }

        [DataMember]
        public string OnlineLocateBuddys { get; set; }

        [DataMember]
        public string OnSOS { get; set; }

        [DataMember]
        public string OnTrack { get; set;}
    }
}
