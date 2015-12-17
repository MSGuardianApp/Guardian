using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SOS.Service.Interfaces.DataContracts.OutBound
{
    [DataContract]
    public class BasicProfile : IResult
    {
        [DataMember]
        public long ProfileID { get; set; }

        [DataMember]
        public long UserID { get; set; }
        
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string MobileNumber { get; set; }

        [DataMember]
        public List<BasicGeoTag> LastLocs { get; set; }

        [DataMember]
        public bool IsTrackingOn { get; set; }

        [DataMember]
        public bool IsSOSOn { get; set; }

        [DataMember]
        public List<ResultInfo> DataInfo { get; set; }
    }

    [DataContract]
    public class BasicGeoTag
    {
        [DataMember]
        public bool? IsSOS { get; set; }

        [DataMember]
        public string Lat { get; set; }
        [DataMember]
        public string Long { get; set; }

        [DataMember]
        public long TimeStamp { get; set; }        

        [DataMember]
        public string MediaUri { get; set; }

        [DataMember]
        public double Accuracy { get; set; }
    }
}
