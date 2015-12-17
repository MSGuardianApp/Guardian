using SOS.AzureSQLAccessLayer;
using SOS.AzureStorageAccessLayer;
using SOS.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using SOS.Mappers;

namespace SOS.EventHubReceiver
{
    public static class LocationProcessor
    {
        public static bool ProcessLocation(LiveLocation loc)
        {
            try
            {
                List<Task> tasks = new List<Task>();
                //Process the location and push it to Data Stores
                //Task 1: Save in LiveSession & LiveLocation SQL tables
                Task liveSessionTask = new LiveSessionRepository().PostMyLocationAsync(loc);

                //Task 2: Save in LocationHistory Storage table
                Task historyTask = new LocationHistoryStorageAccess().SaveToLocationHistoryAsync(loc.ConvertToHistory());

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
