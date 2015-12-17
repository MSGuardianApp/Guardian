//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using SOS.Service.Interfaces.DataContracts;
//using Microsoft.ApplicationServer.Caching;


//namespace SOS.Service.Utility
//{
//    public static class CacheAgent
//    {
//        private const string _SiteAuthCache = "siteauthcache";

//        public static DataCache GetCache(string CacheName)
//        {
//            DataCache cache = null;
//            try
//            {
//                DataCacheFactory cacheFactory = new DataCacheFactory();

//                cache = cacheFactory.GetCache(CacheName);
//                //if (cache == null)
//                //{
//                //    cache = new DataCache(CacheName);
//                //}
//                //cache = cacheFactory.GetDefaultCache();
//            }
//            catch (Exception ex)
//            {
//            }
//            return cache;
//        }

//        private static DataCache _Cache_SiteAuthCache = null;

//        private static DataCache SiteAuthCache
//        {
//            get
//            {
//                if (_Cache_SiteAuthCache == null)
//                {
//                    _Cache_SiteAuthCache = GetCache(_SiteAuthCache);
//                }

//                return _Cache_SiteAuthCache;
//            }
//        }

//        public static void SaveInCache(string CacheName, string key, object obj)
//        {
//            try
//            {
//                switch (CacheName)
//                {
//                    case _SiteAuthCache:
//                        SiteAuthCache.Add(key, obj);
//                        break;
//                }
//            }
//            catch (Exception ex)
//            { }
//        }

//        public static object LoadFromCache(string CacheName, string Key)
//        {
//            object retVal = null;
//            try
//            {

//                switch (CacheName)
//                {
//                    case _SiteAuthCache:
//                        retVal = SiteAuthCache.Get(Key);
//                        break;
//                }

//            }
//            catch (Exception Ex)
//            {

//            }
//            return retVal;
//        }

//        //End ToBaseCache



//        public static void SaveIntoAuthCache(string AuthID, CacheUser _User)
//        {
//            try
//            {
//                SaveInCache(_SiteAuthCache,AuthID, _User);
//            }
//            catch (Exception ex)
//            {

//            }
//        }

//        public static CacheUser LoadFromAuthCache(string AuthToken)
//        {
//            return LoadFromCache(_SiteAuthCache, AuthToken) as CacheUser;
//        }
//    }

//}
