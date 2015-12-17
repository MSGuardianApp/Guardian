using System.Runtime.Serialization;

namespace SOS.Service.Interfaces.DataContracts
{
    [DataContract]
    public class DeviceSetting
    {
        [DataMember]
        public string ProfileID {get; set;}
        [DataMember]
        public string DeviceID { get; set; }
        [DataMember]
        public string PlatForm { get; set; }
        [DataMember]
        public bool CanEmail { get; set; }
        [DataMember]
        public bool CanSMS { get; set; }
    }

    [DataContract]
    public class PortalSetting
    {

    }
}
