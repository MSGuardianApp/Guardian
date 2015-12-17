// Responsible for Tease Report

namespace SOS.Service.Interfaces.DataContracts
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using SOS.Service.Interfaces.DataContracts.OutBound;

    [DataContract]
    public class Incident
    {
        [DataMember]
        public string IncidentID { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string MobileNumber { get; set; }

        [DataMember]
        public double Lat { get; set; }

        [DataMember]
        public double Long { get; set; }

        [DataMember]
        public double Alt { get; set; }

        [DataMember]
        public string ProfileID { get; set; }

        [DataMember]
        public long DateTime { get; set; }

        [DataMember]
        public string MediaUri { get; set; }

        [DataMember]
        public string AdditionalInfo { get; set; }
    }

    [DataContract]
    public class IncidentList : IResult
    {
        [DataMember]
        public List<Incident> List { get; set; }

        [DataMember]
        public List<ResultInfo> DataInfo { get; set; }
    }
}