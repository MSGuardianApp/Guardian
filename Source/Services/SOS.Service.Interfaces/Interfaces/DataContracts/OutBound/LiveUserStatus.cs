using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SOS.Service.Interfaces.DataContracts.OutBound
{
    [DataContract]
    public class LiveUserStatus
    {
        /// <summary>
        /// Profile ID
        /// </summary>
        [DataMember]
        public string PID { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public string N { get; set; }

        /// <summary>
        /// Session ID
        /// </summary>
        [DataMember]
        public string SID { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember]
        public int S { get; set; }

        /// <summary>
        /// Mobile Number
        /// </summary>
        [DataMember]
        public string M { get; set; }

        /// <summary>
        /// AssignedTo
        /// </summary>
        [DataMember]
        public string AT { get; set; }
    }

    [DataContract]
    public class LiveUserStatusList : IResult
    {
        [DataMember]
        public List<LiveUserStatus> List { get; set; }

        [DataMember]
        public List<ResultInfo> DataInfo { get; set; }
    }
}
