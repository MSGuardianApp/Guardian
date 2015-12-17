using System;
using System.Diagnostics;
using System.Threading;

namespace SOS.ThreadedWorkerRole
{
    public class EventHubReceivingWorker : WorkerEntryPoint
    {
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        /// Run is the function of an working cycle
        /// </summary>
        public override void Run()
        {
            if (!ConfigManager.Config.UseEventHubs) return;

            Trace.TraceInformation("Worker1:Run begin", "Information");

            try
            {
                string traceInformation = DateTime.UtcNow.ToString() + " Worker1: Run loop thread=" + Thread.CurrentThread.ManagedThreadId.ToString();
                Trace.TraceInformation(traceInformation, "Information");

                // Actual location processing work is in EventProcessor.cs-ProcessEventsAsync
                EventHubReceiver.ReceiverHost.Start().Wait();

                this.runCompleteEvent.WaitOne();
            }
            catch (SystemException se)
            {
                Trace.TraceError("RunWorker1:Run SystemException", se.ToString());
                throw se;
            }
            catch (Exception ex)
            {
                Trace.TraceError("RunWorker1:Run Exception", ex.ToString());
            }

            Trace.TraceInformation("Worker1:Run end", "Information");
        }

        public override void OnStop()
        {
            Trace.TraceInformation("ReceiverRole is stopping");
            this.runCompleteEvent.Set();

            //try
            //{
            //    //Unregister the event processor so other instances will handle the partitions
            //    receiver.UnregisterEventProcessor();
            //}
            //catch (Exception oops)
            //{
            //    Trace.TraceError(oops.Message);
            //}

            base.OnStop();
            Trace.TraceInformation("ReceiverRole has stopped");

        }
    }
}