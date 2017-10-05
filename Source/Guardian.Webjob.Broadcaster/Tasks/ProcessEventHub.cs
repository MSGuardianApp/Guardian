using Guardian.Common.Configuration;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Guardian.Webjob.Broadcaster
{
    public class ProcessEventHub
    {
        // readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        readonly IConfigManager configManager;
        const int minute = 60 * 1000;

        public ProcessEventHub(IConfigManager configManager)
        {
            this.configManager = configManager;
        }

        public async Task Run()
        {
            if (!configManager.Settings.UseEventHubs)
            {
                Trace.TraceInformation("Event Hub usage has been disabled!", "Information");
                return;
            }

            try
            {
                Trace.TraceInformation("Processing Event Hub messages started...", "Information");

                await SOS.EventHubReceiver.ReceiverHost.Start();

                //this.runCompleteEvent.WaitOne();

                Trace.TraceInformation("Processing Event Hub messages has ended at " + DateTime.Now.ToString(), "Information");
            }

            catch (Exception ex)
            {
                //this.runCompleteEvent.Set();
                Trace.TraceError("WebJob Error: Event Hub message processing failed! " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace, "Error");

                await Task.Delay(5 * minute);
            }
        }
    }
}
