using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using SOS.Model;
using Newtonsoft.Json;

namespace SOS.EventHubReceiver
{
    public class EventProcessor : IEventProcessor
    {
        #region Private Members and Properties

        private int totalMessages = 0;

        public EventProcessor()
        {
            Mappers.Mapper.InitializeMappers();
        }

        public event EventHandler ProcessorClosed;

        public bool IsInitialized { get; private set; }

        public bool IsClosed { get; private set; }

        public int TotalMessages
        {
            get
            {
                return this.totalMessages;
            }
        }

        public CloseReason CloseReason { get; private set; }

        public PartitionContext Context { get; private set; }

        #endregion

        public Task OpenAsync(PartitionContext context)
        {
            Trace.WriteLine(string.Format("{0} > Processor Initializing for PartitionId '{1}'; Offset '{2}' and Owner: {3}.",
                DateTime.Now.ToString(), context.Lease.PartitionId, context.Lease.Offset, context.Lease.Owner ?? string.Empty), "Information");
            this.Context = context;
            this.IsInitialized = true;

            return Task.FromResult<object>(null);
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            //keep it simple fast and reliable.
            try
            {
                foreach (EventData message in messages)
                {
                    LiveLocation loc = this.DeserializeEventData(message);
                    if (loc != null)
                    {
                        Trace.WriteLine(string.Format("{0} > received message: {1} at partition {2}, offset: {3}",
                        DateTime.Now.ToString(), loc.ProfileID.ToString() + "-" + loc.SessionID + "-" + loc.ClientTimeStamp.ToString(),
                        context.Lease.PartitionId, message.Offset), "Information");

                        LocationProcessor.ProcessLocation(loc);
                    }
                    // increase the total events count.
                    Interlocked.Increment(ref this.totalMessages);

                }

                lock (this) { return context.CheckpointAsync(); }

            }
            catch (Exception exp)
            {
                Trace.TraceError("Error in processing EventHub message: " + exp.Message);
            }

            return Task.FromResult<object>(null);
        }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            // where you close the processor.
            Trace.WriteLine(string.Format("{0} > Close called for processor with PartitionId '{1}' and Owner: {2} with reason '{3}'.", DateTime.Now.ToString(), context.Lease.PartitionId, context.Lease.Owner ?? string.Empty, reason), "Information");
            this.IsClosed = true;
            this.CloseReason = reason;
            this.OnProcessorClosed();
            return context.CheckpointAsync();
        }

        protected virtual void OnProcessorClosed()
        {
            EventHandler handler = this.ProcessorClosed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        LiveLocation DeserializeEventData(EventData eventData)
        {
            return JsonConvert.DeserializeObject<LiveLocation>(Encoding.UTF8.GetString(eventData.GetBytes()));
        }

    }
}
