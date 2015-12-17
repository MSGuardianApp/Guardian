namespace SOS.Service.Exceptions
{
    public class SecurityException : BaseException
    {
        //public SecurityException(Exception original, ExceptionType type, string messaginfo)
        //    : base(original, type, messaginfo)
        //{

        //}

        public SecurityException( ExceptionType type, string messaginfo)
            : base(type, messaginfo)
        {

        }
    }

    public class ServiceException : BaseException
    {
        //public ServiceException(Exception original, ExceptionType type, string messaginfo)
        //    : base(original, type, messaginfo)
        //{

        //}

        public ServiceException(ExceptionType type, string messaginfo)
            : base(type, messaginfo)
        {

        }
    }


    public class DataAccessException : BaseException
    {
        //public DataAccessException(Exception original, ExceptionType type, string messaginfo)
        //    : base(original, type, messaginfo)
        //{

        //}


        public DataAccessException(ExceptionType type, string messaginfo)
            : base(type, messaginfo)
        {

        }
    }


    public class ConnectionException : BaseException
    {
        //public ConnectionException(Exception original, ExceptionType type, string messaginfo)
        //    : base(original, type, messaginfo)
        //{

        //}
        public ConnectionException(ExceptionType type, string messaginfo)
            : base(type, messaginfo)
        {

        }
    }

    public class ParserException : BaseException
    {
        //public ParserException(Exception original, ExceptionType type, string messaginfo)
        //    : base(original, type, messaginfo)
        //{

        //}
        public ParserException(ExceptionType type, string messaginfo)
            : base(type, messaginfo)
        {

        }
    }

    public class CommonException : BaseException
    {
        //public CommonException(Exception original, ExceptionType type, string messaginfo)
        //    : base(original, type, messaginfo)
        //{

        //}
        public CommonException(ExceptionType type, string messaginfo)
            : base(type, messaginfo)
        {

        }
    }


}
