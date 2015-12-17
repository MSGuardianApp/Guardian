using System;

namespace SOS.Service.Exceptions
{
    public abstract class BaseException : Exception
    {
        //public Exception OriginalException { get; private set; }

        public string ExceptionInfo { get; private set; }

        public ExceptionType TypeOfException { get; private set; }

        //private Exception _OriginalException;

        //public string ExceptionInfo { get; set; }

        //public ExceptionType TypeOfException { get; set; }

        //public BaseException(Exception original, ExceptionType type, string messaginfo)
        //{
        //    OriginalException = original;
        //    ExceptionInfo = messaginfo;
        //    TypeOfException = type;
        //}

        public BaseException(ExceptionType type, string messaginfo)
        {
            ExceptionInfo = messaginfo;
            TypeOfException = type;
        }

        protected void TraceDetails()
        {
            if (this.InnerException != null)
            {
                if (this.InnerException.InnerException != null)
                {
                    //SOS Exp type, Severity, Info,  Original Exception being caught at some layer, Oexcept Messg , inner excp of orig excep, inner excep msg;
                    ExceptionTracer.TraceException(this.GetType().FullName, TypeOfException.ToString(), ExceptionInfo, this.InnerException.GetType().FullName, (this.InnerException.Message ?? ""), this.InnerException.InnerException.GetType().FullName, (this.InnerException.InnerException.Message ?? ""));
                }
                else
                {
                    // No Inner
                    //SOS Exp type, Severity, Info,  Original Exception being caught at some layer, Oexcept Messg ;
                    ExceptionTracer.TraceException(this.GetType().FullName, TypeOfException.ToString(), ExceptionInfo, this.InnerException.GetType().FullName, (this.InnerException.Message ?? ""));
                }
            }
            else
            {
                //Non system exception raised.
                ExceptionTracer.TraceException(this.GetType().FullName, TypeOfException.ToString(), ExceptionInfo);
            }
        }

    }

   
}
