using System.Collections.Generic;
using System.Runtime.Serialization;
using SOS.Service.Interfaces.DataContracts.OutBound;

namespace SOS.Service.Interfaces.DataContracts
{
    [DataContract]
    public class History : IResult
    {
        [DataMember]
        public List<GeoTag> GeoInstances { get; set; }

        [DataMember]
        public string SessionID { get; set; }

        [DataMember]
        public string PeriodStartDate { get; set; }

        [DataMember]
        public string PeriodEndDate { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public bool IsEvidenceAvailable { get; set; }

        [DataMember]
        public List<Media> MediaInstances { get; set; }

        [DataMember]
        public List<ResultInfo> DataInfo { get; set; }
    }


    [DataContract]
    [KnownType(typeof(List<History>))]
    [KnownType(typeof(History))]
    public class HistoryList:IResult
    {
        List<History> _List;


        [DataMember]
        public List<History> List
        {
            get
            {
                if (_List == null)
                {
                    _List = new List<History>();
                }
                return _List;
            }
            set { _List = value; }
        }

        [DataMember]
        public List<ResultInfo> DataInfo { get; set; }

    }
}
