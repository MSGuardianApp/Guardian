using Guardian.Common.Configuration;
using Microsoft.Azure.WebJobs;
using SOS.AzureSQLAccessLayer;
using SOS.AzureStorageAccessLayer;
using SOS.EventHubReceiver;
using SOS.Service.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Guardian.Webjob.Broadcaster
{
    public class Functions
    {
        readonly ILiveSessionRepository liveSessionRepository;
        readonly ISessionHistoryStorageAccess sessionHistoryStorageAccess;
        readonly IGroupRepository groupRepository;
        readonly ILocationRepository locationRepository;

        readonly IGroupStorageAccess groupStorageAccess;
        readonly IReceiver eventHubReceiverHost;

        readonly IConfigManager configManager;
        public Functions(ILiveSessionRepository liveSessionRepository,
                    ISessionHistoryStorageAccess sessionHistoryStorageAccess,
                    IGroupRepository groupRepository,
                    ILocationRepository locationRepository,
                    IGroupStorageAccess groupStorageAccess,
                    IReceiver eventHubReceiverHost,
                    IConfigManager configManager)
        {
            this.liveSessionRepository = liveSessionRepository;
            this.sessionHistoryStorageAccess = sessionHistoryStorageAccess;
            this.groupRepository = groupRepository;
            this.locationRepository = locationRepository;

            this.groupStorageAccess = groupStorageAccess;
            this.eventHubReceiverHost = eventHubReceiverHost;

            this.configManager = configManager;
            SOS.Mappers.Mapper.InitializeMappers();
        }
        //1. Broadcast messages - Check every 30 secs(or 1 minute, if two role instances are deployed)
        //   Broadcast SMS, Emails paralelly
        //      Broadcast by Profile Paralelly
        //2. Purge stale sessions in LiveLocation and LiveSession - every 4 hours
        //   On Purge, push the data to LiveSessionHistory
        //3. Purge session immediately on receive of STOP from LiveLocation
        //   If a old session data is received newly, 
        //      If it was already stopped, will ignore to insert in LiveSession. However, will push to LiveLocation
        //      If not, will insert/ update to LiveSession
        //4. Dynamically associate Live users to SubGroups/ circles 
        //5. TODO: Clear Local cache at every 1 hr

        public async Task MessageBroadcaster([TimerTrigger("00:00:30", RunOnStartup = true)] TimerInfo timer, TextWriter log)
        {
            log.WriteLine("Broadcasting messages has started..." + DateTime.Now.ToLongTimeString());
            await new MessageBroadcaster(liveSessionRepository, configManager).Run();
        }

        public async Task PurgeLiveLocations([TimerTrigger("00:10:00", RunOnStartup = true)] TimerInfo timer, TextWriter log)
        {
            log.WriteLine("PurgeLiveLocations has started..." + DateTime.Now.ToLongTimeString());
            await new PurgeLiveLocations(locationRepository, configManager).Run();
        }

        public async Task ArchiveStaleSessions([TimerTrigger("00:10:00", RunOnStartup = true)] TimerInfo timer, TextWriter log)
        {
            log.WriteLine("ArchiveStaleSessions has started..." + DateTime.Now.ToLongTimeString());
            await new ArchiveStaleSessions(liveSessionRepository, sessionHistoryStorageAccess, configManager).Run();
        }

        public void DynamicAllocationToSubGroups([TimerTrigger("00:05:00", RunOnStartup = true)] TimerInfo timer, TextWriter log)
        {
            log.WriteLine("DynamicAllocationToSubGroups has started..." + DateTime.Now.ToLongTimeString());
            new DynamicAllocationToSubGroups(liveSessionRepository, groupRepository, groupStorageAccess, configManager).Run();
        }

        public async Task ProcessEventHub([TimerTrigger("00:00:30", RunOnStartup = true)] TimerInfo timer, TextWriter log)
        {
            log.WriteLine("ProcessEventHub has started..." + DateTime.Now.ToLongTimeString());
            await new ProcessEventHub(eventHubReceiverHost, configManager).Run();
        }
    }
}
