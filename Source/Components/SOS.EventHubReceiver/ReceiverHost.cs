using Guardian.Common.Configuration;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SOS.EventHubReceiver
{
    public class ReceiverHost: IReceiver
    {
        const string hostName = "LocationReceiver";

        string consumerGroupName = EventHubConsumerGroup.DefaultGroupName;

        EventProcessorHost host;

        EventProcessorFactory factory;

        IConfigManager configManager;

        public ReceiverHost(IConfigManager configManager)
        {
            this.configManager = configManager;
        }

        public async Task Start()
        {
            var eventHubConnectionString = GetEventHubConnectionString();
            var storageConnectionString = configManager.Settings.AzureStorageConnectionString;
            var eventHubName = configManager.Settings.EventHubName;

            // here it's using eventhub as lease name. but it can be specified as any you want.
            // if the host is having same lease name, it will be shared between hosts.
            // by default it is using eventhub name as lease name.
            host = new EventProcessorHost(
                hostName,
                eventHubName,
                consumerGroupName,
                eventHubConnectionString,
                storageConnectionString, 
                eventHubName.ToLowerInvariant());

            factory = new EventProcessorFactory(hostName);

            try
            {
                Trace.WriteLine(string.Format("{0} > Registering host: {1}", DateTime.Now.ToString(), hostName), "Information");
                var options = new EventProcessorOptions();
                options.ExceptionReceived += OptionsOnExceptionReceived;
                
                await host.RegisterEventProcessorFactoryAsync(factory);
            }
            catch (Exception exception)
            {
                Trace.WriteLine(string.Format("{0} > Exception: {1}", DateTime.Now.ToString(), exception.Message), "Error");
                Console.ResetColor();
            }
        }

        //public void UnregisterEventProcessor()
        //{
        //    host.UnregisterEventProcessorAsync().Wait();
        //}

        private void OptionsOnExceptionReceived(object sender, ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            // by receiving host exception, you could respond to the error, e.g. restart the host.
            Trace.WriteLine(string.Format("Received exception, action: {0}, messae： {1}.", exceptionReceivedEventArgs.Action, exceptionReceivedEventArgs.Exception.Message), "Error");
        }

        private string GetEventHubConnectionString()
        {
            var connectionString = configManager.Settings.EventHubConnectionString;
            try
            {
                var builder = new ServiceBusConnectionStringBuilder(connectionString);
                builder.TransportType = TransportType.Amqp;
                return builder.ToString();
            }
            catch (Exception exception)
            {
                Trace.WriteLine("Error while building Evenhub Connection String." + exception.Message, "Error");
            }

            return string.Empty;
        }
    }
}