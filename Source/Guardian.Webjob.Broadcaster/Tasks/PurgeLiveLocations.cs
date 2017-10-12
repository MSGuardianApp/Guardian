using Guardian.Common.Configuration;
using SOS.AzureSQLAccessLayer;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Guardian.Webjob.Broadcaster
{
    public class PurgeLiveLocations
    {
        readonly IConfigManager configManager;
        readonly ILocationRepository locationRepository;
        const int minute = 60 * 1000;

        public PurgeLiveLocations(ILocationRepository locationRepository, IConfigManager configManager)
        {
            this.configManager = configManager;
            this.locationRepository = locationRepository;
        }

        public async Task Run()
        {
            Trace.TraceInformation("Purging Live Locations started...", "Information");

            try
            {
                await locationRepository.PurgeStaleLiveLocations();

                Trace.TraceInformation("Purging Live Locations completed. Sleeping for " + configManager.Settings.ArchiveRunIntervalInMinutes.ToString() + " minutes...", "Information");
                await Task.Delay(configManager.Settings.ArchiveRunIntervalInMinutes * minute);
            }
            catch (Exception ex)
            {
                Trace.TraceError("WebJob Error: Purging Live Location failed! " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace, "Error");

                await Task.Delay(5 * minute);
            }
        }
    }
}
