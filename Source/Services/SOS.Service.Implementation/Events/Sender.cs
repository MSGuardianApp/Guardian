using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using SOS.ConfigManager;
using SOS.Model;

namespace SOS.Service.Implementation
{
    public class EventsSender
    {
        private static readonly EventHubClient client;

        static EventsSender()
        {
            client = EventHubClient.CreateFromConnectionString(Config.EventHubConnectionString, Config.EventHubName);
        }

        public static async Task SendLocationEvent(LiveLocation liveLocation)
        {
            string serializedString = JsonConvert.SerializeObject(liveLocation);
            var data = new EventData(Encoding.UTF8.GetBytes(serializedString))
            {
                PartitionKey = liveLocation.ProfileID.ToString()
            };
            await client.SendAsync(data);
        }

        public static async Task SendLocationEvents(LiveLocation[] liveLocation)
        {
            var events = new List<EventData>();

            foreach (LiveLocation loc in liveLocation)
            {
                string serializedString = JsonConvert.SerializeObject(loc);
                var data = new EventData(Encoding.UTF8.GetBytes(serializedString))
                {
                    PartitionKey = loc.ProfileID.ToString()
                };
                events.Add(data);
                //tasks.Add(client.SendAsync(data));
            }

            await client.SendBatchAsync(events);

            //Task.WaitAll(tasks.ToArray());
        }
    }
}