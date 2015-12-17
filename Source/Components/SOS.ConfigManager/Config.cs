using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;

namespace SOS.ConfigManager
{
    public static class Config
    {
        private static ConcurrentDictionary<string, string> configCache = new ConcurrentDictionary<string, string>(); //Local Cache
      
        static Config()
        {             
            if (RoleEnvironment.IsAvailable)
                RoleEnvironment.Changed += delegate(object sender, RoleEnvironmentChangedEventArgs e)
                {
                    foreach (RoleEnvironmentConfigurationSettingChange setting in e.Changes.OfType<RoleEnvironmentConfigurationSettingChange>())
                    {
                        if (configCache.ContainsKey(setting.ConfigurationSettingName))
                        {
                            string tempVal;
                            configCache.TryRemove(setting.ConfigurationSettingName, out tempVal);
                        }
                    }
                };
        }

        public static string Get(string key)
        {
            try
            {
                string value = string.Empty;
                if (configCache.TryGetValue(key, out value))
                    return value;

                if (RoleEnvironment.IsAvailable)
                    value = CloudConfigurationManager.GetSetting(key);
                else
                    value = ConfigurationManager.AppSettings.Get(key);

                configCache.TryAdd(key, value);
                return value;
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading Key - " + key + " Error: " + ex.Message);
            }
        }
        public static string AzureSQLConnectionString
        {
            get { return Get("AzureSQLConnectionString"); }
        }

        public static string QueueConnectionString
        {
            get { return Get("QueueConnection"); }
        }
        public static string TableConnectionString
        {
            get { return Get("TableConnection"); }
        }

        public static string BlobConnectionString
        {
            get { return Get("BlobConnection"); }
        }

        public static string EventHubConnectionString
        {
            get { return Get("EventHubConnectionString"); }
        }

        public static string EventHubName
        {
            get { return Get("EventHubName"); }
        }

        public static string SMSDefaultFromNumber
        {
            get { return Get("SMSDefaultFromnumber"); }
        }
       
        public static string GuardianPortalUri
        {
            get { return Get("GuardianPortalUri"); }
        }

        public static string V1GuardianPortalUri
        {
            get { return Get("V1GuardianPortalUri"); }
        }

        public static string TinyServiceUri
        {
            get { return Get("TinyServiceUri"); }
        }             

        public static int SMSPostGap
        {
            get
            {
                int interval = 15;
                int.TryParse(Get("SMSPostGap"), out interval);
                return interval;
            }
        }

        public static int FacebookPostGap
        {
            get
            {
                int interval = 15;
                int.TryParse(Get("FacebookPostGap"), out interval);
                return interval;
            }
        }

        public static int EmailPostGap
        {
            get
            {
                int interval = 15;
                int.TryParse(Get("EmailPostGap"), out interval);
                return interval;
            }
        }
        public static bool SendSms
        {
            get { return Convert.ToBoolean(Get("SendSms")); }
        }

        public static bool UseEventHubs
        {
            get { return Convert.ToBoolean(Get("UseEventHubs")); }
        }

        public static int SubGroupAllocationIntervalInMinutes
        {
            get
            {
                short interval = 5;
                Int16.TryParse(Get("SubGroupAllocationIntervalInMinutes"), out interval);
                return interval;
            }
        }

        public static int ArchiveTimeGapInMinutes
        {
            get
            {
                int interval = 4 * 60;
                int.TryParse(Get("ArchiveTimeGapInMinutes"), out interval);
                return interval;
            }
        }

        public static int ArchiveRunIntervalInMinutes
        {
            get
            {
                int interval = 10;
                int.TryParse(Get("ArchiveRunIntervalInMinutes"), out interval);
                return interval;
            }
        }

        public static string SMSServiceUserID
        {
            get { return Get("SMSServiceUserID"); }
        }

        public static string SMSServicePassword
        {
            get { return Get("SMSServicePassword"); }
        }

        public static string sendGridUserID
        {
            get { return Get("sendGridUserID"); }
        }
        public static string sendGridPassword
        {
            get { return Get("sendGridPassword"); }
        }

        public static string BingKey
        {
            get { return Get("BingKey"); }
        }

        public static string IntlSMSServiceUserID
        {
            get { return Get("IntlSMSServiceUserID"); }
        }

        public static string IntlSMSServicePassword
        {
            get { return Get("IntlSMSServicePassword"); }
        }
      

        public static string ClientSecret
        {
            get { return Get("ClientSecret"); }
        }

        public static string LiveAppUri
        {
            get { return Get("LiveAppUri"); }
        }

        public static string GoogleClientID
        {
            get { return Get("GoogleClientID"); }
        }

        public static string RandomNumberDigits
        {
            get { return Get("RandomNumberDigits"); }
        }
     

        public static int TimeToResetCacheInMinutes
        {
            get
            {
                int interval = 240;
                int.TryParse(Get("TimeToResetCacheInMinutes"), out interval);
                return interval;
            }
        }


        public static bool IncludeActiveMembers
        {
            get { return Convert.ToBoolean(Get("IncludeActiveMembers")); }
        }

        public static string DefaultGroupID
        {
            get { return Get("DefaultGroupID"); }
        }

        public static int BroadcastRunIntervalInSeconds
        {
            get
            {
                int interval = 60;
                int.TryParse(Get("BroadcastRunIntervalInSeconds"), out interval);
                return interval;
            }
        }

        public static string AppInsightsInstrumentationKey
        {
            get { return Get("AppInsightsKey"); }
        }

        public static string AppInsightsEnvironment
        {
            get { return Get("AppInsightsTag"); }
        }

        public static bool IsEnterpriseBuild
        {
            get { return Convert.ToBoolean(Get("IsEnterpriseBuild")); }
        }

        public static bool HasDataMigrated
        {
            get { return Convert.ToBoolean(Get("HasDataMigrated")); }
        }

        public static string EnterpriseDomain
        {
            get { return Get("EnterpriseDomain"); }
        }        
    }
}
