using System.Collections.Generic;
using System.Runtime.Serialization;
using SOS.Service.Interfaces.DataContracts.OutBound;

namespace SOS.Service.Interfaces.DataContracts
{
    [DataContract]
    public class PhoneValidation : IResult
    {
        [DataMember]
        public string RegionCode { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string EnterpriseEmailID { get; set; }
        [DataMember]
        public string AuthenticatedLiveID { get; set; }
        [DataMember]
        public string Name { get; set; }     

        [DataMember]
        public string SecurityToken { get; set; }
        [DataMember]
        public string EnterpriseSecurityToken { get; set; }

        [DataMember]
        public string DeviceType { get; set; }
        
        [DataMember]
        public List<ResultInfo> DataInfo { get; set; }

        public bool IsFilled
        {
            get
            {
                return (!string.IsNullOrEmpty(RegionCode) && !string.IsNullOrEmpty(PhoneNumber) && !string.IsNullOrEmpty(AuthenticatedLiveID)
                    && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(SecurityToken));
            }
        }

    }

}
