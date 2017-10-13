using Microsoft.WindowsAzure.Storage.Queue;
using SOS.AzureStorageAccessLayer.Entities;
using System;
using System.Collections.Generic;

namespace SOS.AzureStorageAccessLayer
{
    public interface IQueueAccess
    {
        void EnqueueSanityMessage(GeoTag SanityMessage);

        IEnumerable<CloudQueueMessage> GetSanityMessages(TimeSpan visibilityTimeout);

        void RemoveSanityMessage(CloudQueueMessage Message);

        CloudQueueMessage GetSOSPositionLiveMessage(TimeSpan visibilityTimeout);

        IEnumerable<CloudQueueMessage> GetSOSPositionLiveMessages(TimeSpan visibilityTimeout);

        void RemoveSOSPositionLiveMessage(CloudQueueMessage Message);

        void EnqueueSOSPositionLive(GeoTag LiveGeoLocation);

        void EnqueueLocationHistory(GeoTag HistoryGeoLocation);
    }
}
