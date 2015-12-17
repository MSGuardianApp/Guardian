using System.Collections.Generic;
using SOS.Service.Interfaces.DataContracts.OutBound;

namespace SOS.Service.Utility
{
    public static class ResultsManager
    {
        public static ResultContainer NewResultInstance(string CallingMethod)
        {
            return new ResultContainer();
        }

        public static void AddResultInfo1(ResultContainer container, ResultTypeEnum type, string message)
        {
            if (container.Info == null)
                container.Info = new List<ResultInfo>();

            container.Info.Add(new ResultInfo() { ResultType = type, Message = message });
        }

        public static void AddResultInfo(IResult container, ResultTypeEnum type, string message)
        {
            if (container != null)
            {

                if (container.DataInfo == null)
                    container.DataInfo = new List<ResultInfo>();

                container.DataInfo.Add(new ResultInfo() { ResultType = type, Message = message });
            }
        }


        public static bool CanProceedOnResult(IResult container)
        {
            if (container.DataInfo == null || container.DataInfo.Count == 0)
                return true;

            if (container.DataInfo.Exists(x => (x.ResultType == ResultTypeEnum.Error || x.ResultType == ResultTypeEnum.Exception || x.ResultType == ResultTypeEnum.AuthError)))
                return false;

            if (container.DataInfo.Exists(x => x.ResultType == ResultTypeEnum.Success))
                return true;

            return true;
        }
    }
}
