using System;
using System.Text;
using System.Diagnostics;

namespace SOS.Service.Exceptions
{
    internal class ExceptionTracer
    {

        internal static void TraceException(Exception Ex)
        {
            if (Ex.GetType().BaseType.Name.ToLower() == "BaseException".ToLower())
            { 
                var SOSException = Ex as BaseException;

            }
        }

        private string BuildExceptionDetails(BaseException SOSException)
        {
            StringBuilder SB = new StringBuilder();

            SB.Append("SOSType: " + SOSException.TypeOfException.ToString());
            SB.Append("SOSInfo: " + SOSException.ExceptionInfo.ToString());
            SB.Append("Source: " + SOSException.Source ?? "" + "#");

            if(SOSException.InnerException != null)
                SB.Append(BuildExceptionDetails(SOSException.InnerException));

            return SB.ToString();
        }

        private string BuildExceptionDetails(Exception exception)
        {
            StringBuilder SB = new StringBuilder();
            SB.Append("InException: " + exception.Message);
            SB.Append("InSource: " + exception.Source ?? "" + "#");

            if (exception.InnerException != null)
            {
                SB.Append(BuildExceptionDetails(exception));
            }
            
            return SB.ToString();
        }

        internal static void TraceException(string ExceptionDetails)
        {
            Trace.TraceError(ExceptionDetails);
        }

        //With SOSException + Exception 
        internal static void TraceException(string SOSExInfo, string SOSExType, string ExType, string ExInfo)
        {
            Trace.TraceError("Exception SOSExInfo: {0} # SOSExType: {1} # ExType: {2} # ExInfo : {3}",
                      SOSExInfo,
                      SOSExType,
                      ExType,
                      ExInfo
                       );
        }


        //With Plain Exception 
        internal static void TraceException(string ExType, string ExInfo)
        {
            Trace.TraceError("Exception ExType: {0} # ExInfo : {1}",
                      ExType,
                      ExInfo
                       );
        }

       /// <summary>
        /// Simple new SOS Exception type with just the sos exception type name , severity and message
       /// </summary>
       /// <param name="SOSExceptionName">self found</param>
       /// <param name="TypeOfExceptionEnum"></param>
       /// <param name="ExceptionInfo"></param>
        internal static void TraceException(string SOSExceptionName, string TypeOfExceptionEnum, string ExceptionInfo)
        {
            Trace.TraceError("Exception SOSExceptionName: {0} # TypeOfExceptionEnum : {1} # ExceptionInfo : {2}",
                      SOSExceptionName,
                      TypeOfExceptionEnum,
                      ExceptionInfo
                       );
        }

        //SOS Exp type, Severity, Info,  Original Exception being caught at some layer, Oexcept Messg , inner excp of orig excep, inner excep msg;
        /// <summary>
        /// SOS Exp type, Severity, Info,  Original Exception being caught at some layer, Oexcept Messg , inner excp of orig excep, inner excep msg;
        /// </summary>
        /// <param name="SOSExceptionName"></param>
        /// <param name="TypeOfExceptionEnum"></param>
        /// <param name="ExceptionInfo"></param>
        /// <param name="CaughtException"></param>
        /// <param name="CaughtExceptionMsg"></param>
        /// <param name="CaughtInnerException"></param>
        /// <param name="CaughtInnerExceptionMsg"></param>
        internal static void TraceException(string SOSExceptionName, string TypeOfExceptionEnum, string ExceptionInfo, string CaughtException, string CaughtExceptionMsg, string CaughtInnerException, string CaughtInnerExceptionMsg)
        {
            Trace.TraceError("Exception SOSExceptionName: {0} # TypeOfExceptionEnum : {1} # ExceptionInfo : {2} # CaughtException : {3} # " +
            "CaughtExceptionMsg : {4} # CaughtInnerException : {5} # CaughtInnerExceptionMsg : {6}",
                      SOSExceptionName,
                      TypeOfExceptionEnum,
                      ExceptionInfo,
                      CaughtException,
                      CaughtExceptionMsg,
                      CaughtInnerException,
                      CaughtInnerExceptionMsg
                       );
        }



        //SOS Exp type, Severity, Info,  Original Exception being caught at some layer, Oexcept Messg ;
        //Caugh Exception without Inner, but caught by and casted as SOS
        internal static void TraceException(string SOSExceptionName, string TypeOfExceptionEnum, string ExceptionInfo, string CaughtException, string CaughtExceptionMsg)
        {
            Trace.TraceError("Exception SOSExceptionName: {0} # TypeOfExceptionEnum : {1} # ExceptionInfo : {2} # CaughtException : {3} # " +
            "CaughtExceptionMsg : {4} # CaughtInnerException : {5} # CaughtInnerExceptionMsg : {6}",
                      SOSExceptionName,
                      TypeOfExceptionEnum,
                      ExceptionInfo,
                      CaughtException,
                      CaughtExceptionMsg
                       );
        }
    }
}
