using SOS.AzureSQLAccessLayer;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Guardian.Webjob.Broadcaster
{
    public class InProcessSession
    {
        public async Task Clear(string roleInstanceId)
        {
            Trace.TraceInformation("Clearing In-process session...", "Information");

            try
            {
                //await new LiveSessionRepository().ClearProcessingAsync(roleInstanceId);
            }
            catch (Exception ex)
            {
                Trace.TraceError("WebJob Error: Clearing In-process session failed " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace, "Error");
            }
        }
    }
}
