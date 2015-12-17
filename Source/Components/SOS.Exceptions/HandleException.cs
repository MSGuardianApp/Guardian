using System;

namespace SOS.Service.Exceptions
{
    public static class ExceptionHandler
    {
        public static void HandleException(Exception BaseException)//, ExceptionType type, string messaginfo)
        {
            try
            {
                BaseException exception = (BaseException)BaseException;
                if (exception.InnerException != null)
                {
                    SOSExceptionLogic(exception);
                }
            }
            catch
            {
                StandardExceptionLogic(BaseException);
            } 
         

        }


        private static void SOSExceptionLogic(BaseException exception)
        {
            if (exception.InnerException != null)
            {
                switch (exception.GetType().Name.ToLower())
                {
                    case "serviceexception":
                    case "accessviolationexception":
                        break;
                }
            }
        }

        private static void StandardExceptionLogic(Exception ex)
        {
            //TraceException(ex.GetType().FullName, ex.Message ?? "");
        }


       
    }
}
