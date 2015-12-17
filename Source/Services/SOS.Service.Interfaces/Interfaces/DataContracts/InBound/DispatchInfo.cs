using System.Runtime.Serialization;

namespace SOS.Service.Interfaces.DataContracts.InBound
{
    [DataContract]
    public class DispatchInfo
    {
        [DataMember]
        public string ProfileID { get; set; }
        
        [DataMember]
        public string SessionID { get; set; }

        [DataMember]
        public string GroupID { get; set; }

        [DataMember]
        public string AssignedTo { get; set; }
    }
}
