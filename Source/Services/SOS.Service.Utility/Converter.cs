using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SOS.Service.Utility
{
    public static class Converter
    {

        public static byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
       // private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public static DateTime ToDateTime(string DateTimeStr)
        {
            DateTime vTime = DateTime.MinValue;
            if (!string.IsNullOrEmpty(DateTimeStr))
            {
                //vTime = Convert.ToDateTime(LastUpdateTime);
                long lTime = 0;
                if (long.TryParse(DateTimeStr, out lTime))
                {
                    try
                    {
                        vTime = new DateTime(lTime);
                        //vTime = vTime.AddHours(5);
                        //vTime = vTime.AddMinutes(30);
                        if(vTime == DateTime.MinValue)
                            vTime = GetSOSMinDateTime();
                    }
                    catch
                    {
                        vTime = GetSOSMinDateTime();
                    }
                }
                else if (!DateTime.TryParse(DateTimeStr, out vTime))
                {
                    vTime = GetSOSMinDateTime();
                }
            }
            else
                vTime = GetSOSMinDateTime();

            return vTime;
        }

       

        public static DateTime ToMaxDateTime(string DateTimeStr)
        {
            DateTime vTime = DateTime.MaxValue;
            if (!string.IsNullOrEmpty(DateTimeStr))
            {
                long lTime = 0;
                if (long.TryParse(DateTimeStr, out lTime))
                {
                    try
                    {
                        vTime = new DateTime(lTime);
                        //vTime = vTime.AddHours(5);
                        //vTime = vTime.AddMinutes(30);
                        if (vTime == DateTime.MinValue)
                            vTime = GetSOSMaxDateTime();
                    }
                    catch
                    {
                        vTime = GetSOSMaxDateTime();
                    }
                }
                else if (!DateTime.TryParse(DateTimeStr, out vTime))
                {
                    vTime = GetSOSMaxDateTime();
                }
            }
            else
                vTime = GetSOSMaxDateTime();

            return vTime;
        }

        public static DateTime GetSOSMinDateTime()
        {
            return DateTime.Parse("1/2/1601");
        }

        public static DateTime GetSOSMaxDateTime()
        {
            return DateTime.Parse("12/12/9999");
        }

    }


}
