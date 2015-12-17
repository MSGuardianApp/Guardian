using System.Runtime.Serialization;

namespace SOS.Service.Interfaces.DataContracts
{
    [DataContract]
    public class LiveCred
    {
        public string LiveID { get; set; }
    }

    public enum LiveType
    { 
        Live = 0,
        Google = 1,
        Facebook = 2
    }

    [DataContract]
    public enum AccessTypes
    { 
        SystemAdmin = 0,
        Personal = 1,
        OrgGroupAdmin = 2,
        PublicGroupAdmin = 3,
        SocialGroupAdmin = 4,
        SubGroupAdmin = 5,
        

    }

 
}
