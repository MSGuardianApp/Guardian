using System.Runtime.Serialization;

namespace SOS.Service.Interfaces.DataContracts
{
    [DataContract]
    public class BuddyAndMarshalRelation
    {
        [DataMember]
        public string GroupID { get; set; }
        [DataMember]
        public string MarshalUserID { get; set; }
        [DataMember]
        public string TargetUserProfileID { get; set; }
        [DataMember]
        public string MarshalName { get; set; }
        [DataMember]
        public string UserName { get; set; }

    }
}
