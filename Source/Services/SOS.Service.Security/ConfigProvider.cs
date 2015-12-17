using System;
using System.Collections.Generic;
using System.Linq;
using SOS.ConfigManager;

namespace SOS.Service.Security
{
    public sealed class ConfigProvider:IDisposable
    {
        public static class ConfigurationStore
        {
            private static List<string> _LiveAuthKeys;
            private static string _LiveAuthAppURI;
            private static string _GoogleClientID;

            public static List<string> LiveAuthKeys
            {
                get
                {
                    if (_LiveAuthKeys == null)
                        ReloadLiveAuthKeys();

                    return _LiveAuthKeys;
                }
            }

            public static string LiveAuthAppURI
            {
                get
                {
                    if (string.IsNullOrEmpty(_LiveAuthAppURI))
                        ReloadLiveAppURI();

                    return _LiveAuthAppURI;
                }
            }

            public static string GoogleClientID
            {
                get
                {
                    if (string.IsNullOrEmpty(_GoogleClientID))
                        ReloadGoogleClientID();

                    return _GoogleClientID;
                }
            }

            public static int LiveAuthKeyCount
            {
                get
                {
                    return (LiveAuthKeys == null) ? 0 : LiveAuthKeys.Count;
                }
            }

            public static void ReloadStoreItem(string ItemName)
            {
                switch (ItemName.ToLower())
                {
                    case "liveauthkeys":
                        ReloadLiveAuthKeys();
                        break;
                    case "liveauthappuri":
                        ReloadLiveAppURI();
                        break;
                    default:
                        ReloadAll();
                        break;
                }
            }

            private static void ReloadAll()
            {
                ReloadLiveAuthKeys();
            }

            private static void ReloadLiveAuthKeys()
            {
                //Read From Config & DECRYPT
                _LiveAuthKeys = Config.ClientSecret.Split(',').ToList();
            }

            private static void ReloadLiveAppURI()
            {
                _LiveAuthAppURI = Config.LiveAppUri;
            }

            private static void ReloadGoogleClientID()
            {
                _GoogleClientID = Config.GoogleClientID;
            }

            public static string CryptKeyForRest
            { 
                get{
                    return "Nr9no3frd3ncUeFYv9IhN316CEP/mkcQ";
                }
            }
        }

        public void Dispose()
        { 
            //Nullify usages
        }
    }
}
