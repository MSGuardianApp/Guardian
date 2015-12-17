using System.Data;

namespace SOS.AzureSQLAccessLayer
{
    public class Parameter
    {
        public string ParamName { get; set; }
        public dynamic ParamValue { get; set; }
        public SqlDbType DbType { get; set; }

        public Parameter(string ParamName, dynamic ParamValue, SqlDbType DbType)
        {
            this.ParamName = ParamName;
            this.ParamValue = ParamValue;
            this.DbType = DbType;
        }
    }
}
