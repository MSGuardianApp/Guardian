using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SOS.Service.Interfaces.DataContracts.OutBound
{
    [DataContract]
    [KnownType(typeof(ResultInfo))]
    public class ResultContainer
    {
        //List<ResultInfo> _Info = null;   

        [DataMember]
        public object Result { get; set; }

        [DataMember]
        public List<ResultInfo> Info
        {
            get;
            set;
            //{ 
            //if(_Info == null)
            //    _Info = new List<ResultInfo>();  

            //    return _Info; 

            //} 
        }

        [DataMember]
        public bool IsResultLoaded
        {
            get { return !(Result == null); }
        }

        //public void Add(ResultTypeEnum type, string message)
        //{
        //    if (Info == null)
        //        Info = new List<ResultInfo>();

        //    Info.Add(new ResultInfo() { ResultType = type, Message = message });
        //}

        [DataMember]
        public ResultTypeEnum ResultType { get; set; }
    }

    [DataContract]
    public class ResultInfo
    {
        [DataMember]
        public ResultTypeEnum ResultType { get; set; }

        [DataMember]
        public string Message { get; set; }
    }

    //[DataContract]
    public enum ResultTypeEnum
    {
        Success = 0,
        Information=1,
        Warning=2,
        Error=3,
        Exception=4,
        AuthError=5
    }

    public interface IResult {
        List<ResultInfo> DataInfo { get; set; }
    }

}
