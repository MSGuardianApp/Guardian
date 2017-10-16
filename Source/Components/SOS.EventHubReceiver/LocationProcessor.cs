using SOS.AzureSQLAccessLayer;
using SOS.AzureStorageAccessLayer;
using SOS.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using SOS.Mappers;
using Guardian.Common.Configuration;

namespace SOS.EventHubReceiver
{
    public class LocationProcessor: ILocationProcessor
    {
        readonly IConfigManager configManager;
        readonly ILiveSessionRepository liveSessionRepository;
        readonly ILocationHistoryStorageAccess locationHistoryStorageAccess;

        public LocationProcessor(ILiveSessionRepository liveSessionRepository, ILocationHistoryStorageAccess locationHistoryStorageAccess, IConfigManager configManager)
        {
            this.configManager = configManager;
            this.liveSessionRepository = liveSessionRepository;
            this.locationHistoryStorageAccess = locationHistoryStorageAccess;
        }

        public bool ProcessLocation(LiveLocation loc)
        {
            try
            {
                List<Task> tasks = new List<Task>();
                //Process the location and push it to Data Stores
                //Task 1: Save in LiveSession & LiveLocation SQL tables
                Task liveSessionTask = liveSessionRepository.PostMyLocationAsync(loc);

                //Task 2: Save in LocationHistory Storage table
                Task historyTask = locationHistoryStorageAccess.SaveToLocationHistoryAsync(loc.ConvertToHistory());

                tasks.Add(liveSessionTask);
                tasks.Add(historyTask);
                Task.WhenAll(tasks.ToArray()).Wait();
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error processing Live Location. " + ex.Message);
                return false;
            }
        }
    }
}
