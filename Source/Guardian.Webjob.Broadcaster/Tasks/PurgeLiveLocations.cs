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
        const int minute = 60 * 1000;

        public PurgeLiveLocations(IConfigManager configManager)
        {
            this.configManager = configManager;
        }

        public async Task Run()
        {
            Trace.TraceInformation("Purging Live Locations started...", "Information");

            try
            {
                await new LocationRepository().PurgeStaleLiveLocations();

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
