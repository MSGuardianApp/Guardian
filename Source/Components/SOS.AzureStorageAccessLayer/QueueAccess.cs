//using Guardian.Common.Configuration;
//using Microsoft.WindowsAzure.Storage.Queue;
//using SOS.AzureStorageAccessLayer.Entities;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;

//namespace SOS.AzureStorageAccessLayer
//{
//    public class QueueAccess : IQueueAccess
//    {
//        IConfigManager configManager;
//        public QueueAccess(IConfigManager configManager)
//        {
//            this.configManager = configManager;
//            _StorageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(configManager.Settings.AzureStorageConnectionString);
//            _QueueClient = StorageAccount.CreateCloudQueueClient();
//        }

//        private Microsoft.WindowsAzure.Storage.CloudStorageAccount _StorageAccount = null;

//        internal Microsoft.WindowsAzure.Storage.CloudStorageAccount StorageAccount
//        {
//            get { return _StorageAccount; }
//            private set { _StorageAccount = value; }
//        }

//        private CloudQueueClient _QueueClient = null;

//        internal CloudQueueClient QueueClient
//        {
//            get { return _QueueClient; }
//            private set { _QueueClient = value; }
//        }


//        internal CloudQueue SOSQueue
//        {
//            get;
//            private set;
//        }

//        internal CloudQueue SanityQueue
//        {
//            get;
//            private set;
//        }

//        internal CloudQueue HistoryGeoLocationQueue
//        {
//            get;
//            private set;
//        }

//        internal bool IsSOSQueueLoaded { get; private set; }

//        internal void LoadSOSQueue()
//        {
//            try
//            {
//                SOSQueue = _QueueClient.GetQueueReference("geopositionlivesosqueue");
//                SOSQueue.CreateIfNotExists();
//                IsSOSQueueLoaded = true;
//            }
//            catch (Exception ex)
//            {
//            }
//        }

//        internal bool IsSanityQueueLoaded { get; private set; }
//        internal void LoadSanityQueue()
//        {
//            try
//            {
//                SanityQueue = _QueueClient.GetQueueReference("sanityqueue");
//                SanityQueue.CreateIfNotExists();
//                IsSanityQueueLoaded = true;
//            }
//            catch { }
//        }


//        internal bool IsHistoryGeoLocationLoaded { get; private set; }
//        internal void LoadHistoryGeoLocationQueue()
//        {
//            try
//            {
//                HistoryGeoLocationQueue = _QueueClient.GetQueueReference("locationhistoryqueue");
//                HistoryGeoLocationQueue.CreateIfNotExists();
//                IsHistoryGeoLocationLoaded = true;
//            }
//            catch (Exception ex)
//            {
//            }
//        }

//        private static QueueAccess _thisClass = null;
//        private static object _lockObj = new object();

//        private static QueueAccess thisClass
//        {
//            get
//            {
//                if (_thisClass == null)
//                    lock (_lockObj)
//                    {
//                        if (_thisClass == null)
//                            _thisClass = new QueueAccess();
//                    }
//                return _thisClass;

//            }
//        }

//        public static void EnqueueSanityMessage(GeoTag SanityMessage)
//        {
//            if (!thisClass.IsSanityQueueLoaded)
//                thisClass.LoadSanityQueue();

//            CloudQueueMessage message = new CloudQueueMessage(ByteArraySerializer<GeoTag>.Serialize(SanityMessage));
//            if (thisClass.IsSanityQueueLoaded)
//            {
//                thisClass.SanityQueue.AddMessage(message);
//            }
//        }

//        public static IEnumerable<CloudQueueMessage> GetSanityMessages(TimeSpan visibilityTimeout)
//        {
//            if (!thisClass.IsSanityQueueLoaded)
//                thisClass.LoadSanityQueue();
//            return thisClass.SanityQueue.GetMessages(32, visibilityTimeout);
//        }

//        public static void RemoveSanityMessage(CloudQueueMessage Message)
//        {
//            if (!thisClass.IsSanityQueueLoaded)
//                thisClass.LoadSanityQueue();

//            thisClass.SanityQueue.DeleteMessage(Message);
//        }


//        public static CloudQueueMessage GetSOSPositionLiveMessage(TimeSpan visibilityTimeout)
//        {
//            if (!thisClass.IsSOSQueueLoaded)
//                thisClass.LoadSOSQueue();

//            return thisClass.SOSQueue.GetMessage(visibilityTimeout);

//        }

//        public static IEnumerable<CloudQueueMessage> GetSOSPositionLiveMessages(TimeSpan visibilityTimeout)
//        {
//            if (!thisClass.IsSOSQueueLoaded)
//                thisClass.LoadSOSQueue();
//            return thisClass.SOSQueue.GetMessages(32, visibilityTimeout);
//        }

//        public static void RemoveSOSPositionLiveMessage(CloudQueueMessage Message)
//        {
//            if (!thisClass.IsSOSQueueLoaded)
//                thisClass.LoadSOSQueue();

//            thisClass.SOSQueue.DeleteMessage(Message);
//        }

//        public static void EnqueueSOSPositionLive(GeoTag LiveGeoLocation)
//        {
//            if (!thisClass.IsSOSQueueLoaded)
//                thisClass.LoadSOSQueue();

//            CloudQueueMessage message = new CloudQueueMessage(ByteArraySerializer<GeoTag>.Serialize(LiveGeoLocation));
//            if (thisClass.IsSOSQueueLoaded)
//            {
//                thisClass.SOSQueue.AddMessage(message);
//            }
//        }

//        public static void EnqueueLocationHistory(GeoTag HistoryGeoLocation)
//        {
//            if (!thisClass.IsHistoryGeoLocationLoaded)
//                thisClass.LoadHistoryGeoLocationQueue();


//            CloudQueueMessage message = new CloudQueueMessage(ByteArraySerializer<GeoTag>.Serialize(HistoryGeoLocation));
//            if (thisClass.IsHistoryGeoLocationLoaded)
//            {
//                thisClass.HistoryGeoLocationQueue.AddMessage(message);
//            }
//        }


//        private byte[] ObjectToByteArray(Object obj)
//        {
//            if (obj == null)
//                return null;
//            BinaryFormatter bf = new BinaryFormatter();
//            MemoryStream ms = new MemoryStream();
//            bf.Serialize(ms, obj);
//            return ms.ToArray();
//        }

//    }

//    public static class ByteArraySerializer<T>
//    {
//        public static byte[] Serialize(T m)
//        {
//            var ms = new MemoryStream();
//            try
//            {
//                var formatter = new BinaryFormatter();
//                formatter.Serialize(ms, m);
//                return ms.ToArray();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                ms.Close();
//            }
//        }

//        public static T Deserialize(byte[] byteArray)
//        {
//            var ms = new MemoryStream(byteArray);
//            try
//            {
//                var formatter = new BinaryFormatter();
//                return (T)formatter.Deserialize(ms);
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                ms.Close();
//            }
//        }
//    }


//}
