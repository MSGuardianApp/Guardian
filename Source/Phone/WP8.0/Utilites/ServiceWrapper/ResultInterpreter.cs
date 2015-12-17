using System.Collections.ObjectModel;
using System.Linq;

namespace SOS.Phone.ServiceWrapper
{
    public static class ResultInterpreter
    {
        public static bool IsSuccess(ObservableCollection<SOS.Phone.MembershipServiceRef.ResultInfo> dataInfo)
        {
            if (dataInfo != null && dataInfo.ToList().Exists(x => x.ResultType == SOS.Phone.MembershipServiceRef.ResultTypeEnum.Success))
                return true;
            return false;
        }
        public static bool IsSuccess(ObservableCollection<SOS.Phone.LocationServiceRef.ResultInfo> dataInfo)
        {
            if (dataInfo != null && dataInfo.ToList().Exists(x => x.ResultType == SOS.Phone.LocationServiceRef.ResultTypeEnum.Success))
                return true;
            return false;
        }
        public static bool IsSuccess(ObservableCollection<SOS.Phone.GroupServiceRef.ResultInfo> dataInfo)
        {
            if (dataInfo != null && dataInfo.ToList().Exists(x => x.ResultType == SOS.Phone.GroupServiceRef.ResultTypeEnum.Success))
                return true;
            return false;
        }

        public static bool IsError(ObservableCollection<SOS.Phone.GroupServiceRef.ResultInfo> dataInfo)
        {
            if (dataInfo != null && dataInfo.ToList().Exists(x => x.ResultType == SOS.Phone.GroupServiceRef.ResultTypeEnum.Error))
                return true;
            return false;
        }

        public static bool IsError(ObservableCollection<SOS.Phone.MembershipServiceRef.ResultInfo> dataInfo, string errorCode = "")
        {
            if (errorCode == string.Empty)
            {
                if (dataInfo != null && dataInfo.ToList().Exists(x => x.ResultType == SOS.Phone.MembershipServiceRef.ResultTypeEnum.Error))
                    return true;
            }
            else
            {
                if (dataInfo != null && dataInfo.ToList().Exists(x => x.ResultType == SOS.Phone.MembershipServiceRef.ResultTypeEnum.Error && x.Message == errorCode))
                    return true;
            }

            return false;
        }

        public static bool IsError(ObservableCollection<SOS.Phone.LocationServiceRef.ResultInfo> dataInfo)
        {
            if (dataInfo != null && dataInfo.ToList().Exists(x => x.ResultType == SOS.Phone.LocationServiceRef.ResultTypeEnum.Error))
                return true;
            return false;
        }

        public static bool IsAuthError(ObservableCollection<SOS.Phone.MembershipServiceRef.ResultInfo> dataInfo)
        {
            if (dataInfo != null && dataInfo.ToList().Exists(x => x.ResultType == SOS.Phone.MembershipServiceRef.ResultTypeEnum.AuthError))
                return true;
            return false;
        }

    }
}
