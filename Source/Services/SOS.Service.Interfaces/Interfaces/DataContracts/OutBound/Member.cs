using System;
using System.Runtime.Serialization;

namespace SOS.Service.Interfaces.DataContracts.OutBound
{
    [DataContract]
    public class Member
    {
        /// <summary>
        /// ProfileID
        /// </summary>
        [DataMember]
        public long P { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public string N { get; set; }

        /// <summary>
        /// Mobile Number
        /// </summary>
        [DataMember]
        public string M { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [DataMember]
        public string E { get; set; }

        /// <summary>
        /// EnterpriseEmail
        /// </summary>
        [DataMember]
        public string EE { get; set; }

        /// <summary>
        /// CreateDate as Joinded Date
        /// </summary>
        [DataMember]
        public DateTime? J { get; set; }
    }
}
