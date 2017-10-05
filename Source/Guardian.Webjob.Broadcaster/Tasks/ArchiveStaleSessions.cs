using Guardian.Common.Configuration;
using SOS.AzureSQLAccessLayer;
using SOS.AzureStorageAccessLayer;
using SOS.AzureStorageAccessLayer.Entities;
using SOS.Mappers;
using SOS.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Guardian.Webjob.Broadcaster
{
    public class ArchiveStaleSessions
    {
        readonly IConfigManager configManager;
        const int minute = 60 * 1000;

        public ArchiveStaleSessions(IConfigManager configManager)
        {
            this.configManager = configManager;
        }

        public async Task Run()
        {
            try
            {
                Trace.TraceInformation("Archive Stale Sessions started...", "Information");

                List<LiveSession> liveSessions = await new LiveSessionRepository().GetLiveSessionsAsync();
                List<SessionHistory> historySessions = liveSessions.ConvertToHistory();

                await new SessionHistoryStorageAccess().ArchiveSessionDetailsAsync(historySessions);

                await new LiveSessionRepository().PurgeStaleSessionsAsync(liveSessions);

                Trace.TraceInformation("Archive Stale Sessions completed. Sleepin for " + configManager.Settings.ArchiveRunIntervalInMinutes.ToString() + " minutes", "Information");
                await Task.Delay(configManager.Settings.ArchiveRunIntervalInMinutes * minute); // 1 hour
            }
            catch (Exception ex)
            {
                Trace.TraceError("WebJob Error: Archive Sessions failed! " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace, "Error");
                await Task.Delay(5 * minute);
            }
        }
    }
}
