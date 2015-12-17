using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
using SOS.AzureStorageAccessLayer;
using SOS.ConfigManager;
using System.Threading.Tasks;
using SOS.Mappers;
using SOS.Model;
using SOS.AzureStorageAccessLayer.Entities;
using SOS.AzureSQLAccessLayer;
using System.Xml.Serialization;
using System.IO;
using SOS.Model.DTO;
using System.Xml;
using System.Text;
using SOS.Service.Implementation;
using System.Linq;
using SOS.Service.Utility;

namespace SOS.WorkerRole.Broadcaster
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        static int purgeIntervalInMinutes = Config.ArchiveRunIntervalInMinutes;
        static int runInterval = Config.BroadcastRunIntervalInSeconds;
        private string RoleInstanceId = string.Empty;

        public override void Run()
        {
            Trace.TraceInformation("SOS.WorkerRole.Broadcaster entry point called", "Information");

            RoleInstanceId = RoleEnvironment.IsAvailable ? RoleEnvironment.CurrentRoleInstance.Id : string.Empty;

            Mappers.Mapper.InitializeMappers();

            while (true)
            {
                //1. Broadcast messages - Check every 30 secs(or 1 minute, if two role instances are deployed)
                //   Broadcast SMS, Emails, and FB paralelly
                //      Broadcast by Profile Paralelly
                //2. Purge stale sessions in LiveLocation and LiveSession - every 4 hours
                //   On Purge, push the data to LiveSessionHistory
                //3. Purge session immediately on receive of STOP from LiveLocation
                //   If a old session data is received newly, 
                //      If it was already stopped, will ignore to insert in LiveSession. However, will push to LiveLocation
                //      If not, will insert/ update to LiveSession
                //4. Dynamically associate Live users to SubGroups/ circles 
                //5. TODO: Clear Local cache at every 1 hr

                Task[] Tasks = new Task[]{ 
                         Task.Factory.StartNew(() => BroadcastMessages(), TaskCreationOptions.LongRunning)
                        ,Task.Factory.StartNew(() => PurgeLiveLocations(), TaskCreationOptions.LongRunning)
                        ,Task.Factory.StartNew(() => ArchiveStaleSessions(), TaskCreationOptions.LongRunning)
                        ,Task.Factory.StartNew(() => DynamicAllocationToSubGroups(), TaskCreationOptions.LongRunning)
                       ,Task.Factory.StartNew(() => ProcessEventHub(), TaskCreationOptions.LongRunning)
                      // ,Task.Factory.StartNew(() => ClearLocalCache(), TaskCreationOptions.LongRunning)
                        };

                Task.WaitAll(Tasks);
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            System.Net.ServicePointManager.DefaultConnectionLimit = 10000;
            System.Net.ServicePointManager.UseNagleAlgorithm = false;
            System.Net.ServicePointManager.Expect100Continue = false;

            return base.OnStart();
        }

        public async override void OnStop()
        {
            try
            {
                this.runCompleteEvent.Set();

                //Update not null ProcessKey records of the instance to null.
                await new LiveSessionRepository().ClearProcessingAsync(RoleInstanceId);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error in OnStop method : {0}", ex.Message);
            }
            base.OnStop();
        }

        private void BroadcastMessages()
        {
            //Processing Cycle time 30 secs 
            //1. Process messages from LiveSession - Send Messages. Use intance id as identifier for the batch
            //Parallelly send messages for multiple profiles

            while (true)
            {
                try
                {
                    Trace.TraceInformation("Broadcasting messages has started...", "Information");

                    Guid processKey = Guid.NewGuid();

                    List<LiveSession> sosSessions = new LiveSessionRepository().GetSessionsForNotifications(RoleInstanceId, processKey, Config.SendSms, Config.SMSPostGap, Config.EmailPostGap, Config.FacebookPostGap).Result;

                    if (sosSessions != null && sosSessions.Count > 0)
                    {
                        var processedSessions = PostMessages.SendSOSNotifications(sosSessions).Result;

                        var liteSessions = processedSessions.ConvertToLiveSessionLite();

                        string liteSessionsXML = Serialize<List<LiveSessionLite>>(liteSessions, true, true);

                        new LiveSessionRepository().UpdateNotificationComplete(RoleInstanceId, processKey, liteSessionsXML).Wait();
                    }
                    else
                    {
                        Trace.TraceInformation("Broadcasting messages has completed. Going to sleep for " + runInterval.ToString() + " seconds...", "Information");
                        Thread.Sleep(runInterval * 1000);
                    }
                }
                catch (ThreadAbortException ex)
                {
                    Trace.TraceError("Worker Role for Broadcasting messages failed(ThreadAborted)! " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace, "Error");
                    Thread.ResetAbort();

                    Thread.Sleep(1 * 60 * 1000);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Worker Role for Broadcasting messages failed! " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace, "Error");

                    Thread.Sleep(1 * 60 * 1000);
                }
            }
        }

        private void PurgeLiveLocations()
        {
            Trace.TraceInformation("Purging Live Locations started...", "Information");

            while (true)
            {
                try
                {
                    new LocationRepository().PurgeStaleLiveLocations().Wait();

                    Trace.TraceInformation("Purging Live Locations completed. Going to sleep for " + purgeIntervalInMinutes.ToString() + " minutes...", "Information");
                    Thread.Sleep(purgeIntervalInMinutes * 1000 * 60);
                }
                catch (ThreadAbortException ex)
                {
                    Trace.TraceError("Worker Role for Purging Live Location failed(ThreadAborted)! " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace, "Error");
                    Thread.ResetAbort();

                    Thread.Sleep(5 * 60 * 1000);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Worker Role for Purging Live Location failed! " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace, "Error");

                    Thread.Sleep(5 * 60 * 1000);
                }
            }
        }

        private void ArchiveStaleSessions()
        {
            while (true)
            {
                try
                {
                    Trace.TraceInformation("Archive Stale Sessions started...", "Information");

                    List<LiveSession> sessions = new LiveSessionRepository().GetLiveSessionsAsync().Result;
                    List<SessionHistory> sess = sessions.ConvertToHistory();

                    new SessionHistoryStorageAccess().ArchiveSessionDetailsAsync(sess).Wait();

                    new LiveSessionRepository().PurgeStaleSessionsAsync(sessions).Wait();

                    Trace.TraceInformation("Archive Stale Sessions completed. Going to sleep for " + purgeIntervalInMinutes.ToString() + " minutes", "Information");
                    Thread.Sleep(purgeIntervalInMinutes * 60 * 1000); // 1 hour
                }
                catch (ThreadAbortException ex)
                {
                    Trace.TraceError("Worker Role for Archive Sessions failed(ThreadAborted)! " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace, "Error");
                    Thread.ResetAbort();

                    Thread.Sleep(5 * 60 * 1000);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Worker Role to Archive Sessions failed! " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace, "Error");
                    Thread.Sleep(5 * 60 * 1000);
                }
            }
        }

        private void DynamicAllocationToSubGroups()
        {
            var allocationInterval = Config.SubGroupAllocationIntervalInMinutes;
            var liveSessionRepository = new LiveSessionRepository();
            var grouprepository = new GroupRepository();
            GroupService grpService = new GroupService();
            LocalResource myStorage = RoleEnvironment.GetLocalResource("LocalStorageWorkerRole");

            while (true)
            {
                try
                {
                    Trace.TraceInformation("Dynamic Allocation To Sub Groups started...", "Information");
                    //0. Cache Groups and SubGroups information along with Shape files - Clear cache 1hr or Tool to reset cache.
                    //1. For every 5 minutes, check Active SOS in LiveSession. 
                    //2. Findout the groups and if any groups have SubGroups 
                    //3. If NotifySubGroups is enabled and Shape files are available, find out the nearest Subgroup for the latest lat/long of the user
                    //4. Add record in GroupMembership table with SubGroupID and ProfileID of the user, if not already exist
                    //5. If GroupNotifications are enabled, then notify the Sub Group
                    //6. On SOS stop, Delete all dynamic SubGroup allocations

                    var GrpWithSession = grpService.GetFilteredParentGroupLiveMemberSession().Result;

                    //For debugging, please uncomment below
                    //ParallelOptions opt = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount > 1 ? Environment.ProcessorCount - 1 : 1 };
                    //Parallel.ForEach(GrpWithSession, opt, s =>

                    Parallel.ForEach(GrpWithSession, s =>
                    {
                        var DownloadedShapeFilePath = Utility.GrpIdGroupKeyAndShapePaths.FirstOrDefault(x => x.Item1.Equals(s.GrpId));

                        if (!DownloadedShapeFilePath.Equals(default(Tuple<int, string, string>)))
                        {
                            var dataRow = ShapeFileGISutility.FindIntersection(Convert.ToDouble(s.LiveSessionObj.Lat), Convert.ToDouble(s.LiveSessionObj.Long), DownloadedShapeFilePath.Item3);

                            if (dataRow != null && !String.IsNullOrWhiteSpace(DownloadedShapeFilePath.Item2))
                            {
                                var obj = dataRow[DownloadedShapeFilePath.Item2];
                                string wardName = obj != null ? obj.ToString() : String.Empty;
                                Trace.WriteLine(String.Format("{0} found.", wardName));
                                if (!string.IsNullOrWhiteSpace(wardName))
                                {
                                    var SubGroupObj = GroupService.SubGroups.Where(x => x.SubGroupIdentificationKey.Equals(wardName, StringComparison.OrdinalIgnoreCase) && x.ParentGroupID == DownloadedShapeFilePath.Item1).FirstOrDefault();
                                    if (SubGroupObj != null)
                                    {
                                        try
                                        {
                                            int result = grouprepository.AutoSubscribeLiveUserToSubGroup(SubGroupObj.GroupID, s.LiveSessionObj.ProfileID, s.LiveSessionObj.Name, s.LiveSessionObj.SessionID, DownloadedShapeFilePath.Item1).Result;
                                        }
                                        catch (Exception ex)
                                        {
                                            Trace.WriteLine(ex.Message);
                                        }
                                    }
                                }

                            }
                        }

                    });

                    Trace.TraceInformation("Dynamic Allocation To Sub Groups completed. Going to sleep for " + allocationInterval.ToString() + " minutes", "Information");
                    Thread.Sleep(allocationInterval * 60 * 1000);
                }
                catch (ThreadAbortException ex)
                {
                    Trace.TraceError("Worker Role for DynamicAllocationToSubGroups failed(ThreadAborted)! " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace, "Error");
                    Thread.ResetAbort();

                    Thread.Sleep(5 * 60 * 1000);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Worker Role for DynamicAllocationToSubGroups failed! " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace, "Error");
                    Thread.Sleep(5 * 60 * 1000);
                }
            }
        }

        private void ProcessEventHub()
        {
            if (!Config.UseEventHubs)
            {
                Trace.TraceInformation("Event Hub usage has been disabled!", "Information");
                return;
            }
            while (true)
            {
                try
                {
                    Trace.TraceInformation("Processing Event Hub messages started...", "Information");

                    EventHubReceiver.ReceiverHost.Start().Wait();

                    this.runCompleteEvent.WaitOne();

                    Trace.TraceInformation("Processing Event Hub messages has ended at " + DateTime.Now.ToString(), "Information");
                }
                catch (ThreadAbortException ex)
                {
                    this.runCompleteEvent.Set();
                    Trace.TraceError("Worker Role for Event Hub failed(ThreadAborted)! " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace, "Error");
                    Thread.ResetAbort();

                    Thread.Sleep(5 * 60 * 1000);
                }
                catch (Exception ex)
                {
                    this.runCompleteEvent.Set();
                    Trace.TraceError("Worker Role for Event Hub failed! " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace, "Error");

                    Thread.Sleep(5 * 60 * 1000);
                }
            }
        }

        private void ClearLocalCache()
        {
            return;

            while (true)
            {
                try
                {
                    Trace.TraceInformation("Refresh cache started...", "Information");

                    
                    Trace.TraceInformation("Refresh cache completed. Going to sleep for 1hr ...", "Information");
                    Thread.Sleep(60 * 60 * 1000); //1hr
                }
                catch (ThreadAbortException ex)
                {
                    Trace.TraceError("Worker Role for Refresh cache failed(ThreadAborted)! " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace, "Error");
                    Thread.ResetAbort();

                    Thread.Sleep(5 * 60 * 1000);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Worker Role for Refresh cache failed! " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace, "Error");
                    Thread.Sleep(5 * 60 * 1000);
                }
            }
        }

        private string Serialize(List<LiveSessionLite> liteSessions)
        {
            string liteSessionsXML = string.Empty;
            XmlSerializer serializer = new XmlSerializer(liteSessions.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, liteSessions);
                return textWriter.ToString();
            }
        }
        public static string Serialize<X>(X obj, bool omitXmlDeclaration = false, bool omitNameSpace = false)
        {
            var xSer = new XmlSerializer(typeof(X));

            var settings = new XmlWriterSettings() { OmitXmlDeclaration = omitXmlDeclaration, Indent = true, Encoding = new UTF8Encoding(false) };
            using (var ms = new MemoryStream())
            using (var writer = XmlWriter.Create(ms, settings))
            {
                if (!omitNameSpace)
                {
                    xSer.Serialize(writer, obj);
                }
                else
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    xSer.Serialize(writer, obj, ns);
                }
                ms.Seek(0, 0);
                return new UTF8Encoding(false).GetString(ms.GetBuffer(), 0, (int)ms.Length);
            }
        }
    }
}
