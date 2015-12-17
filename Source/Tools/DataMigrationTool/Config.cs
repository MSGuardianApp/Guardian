using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace SOS.OPsTools
{
    public static class Config
    {
        
        public static string Get(string key)
        {

            return ConfigurationManager.AppSettings.Get(key);
            
        }
        public static string BaseStorageConnection
        {
            get { return Get("BaseStorageConnection"); }
        }
        public static string DestStorageConnection
        {
            get { return Get("DestStorageConnection"); }
        }
        
       
    }
}
