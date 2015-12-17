using System;

namespace SOS.AzureSQLAccessLayer
{
    public static class Extensions
    {
        public static object OrDbNull(this string value)
        {
            return (object)value ?? DBNull.Value;
        }
    }
}
