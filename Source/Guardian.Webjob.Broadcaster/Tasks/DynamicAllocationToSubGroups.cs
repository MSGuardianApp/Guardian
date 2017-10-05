using Guardian.Common.Configuration;
using Microsoft.WindowsAzure.ServiceRuntime;
using SOS.AzureSQLAccessLayer;
using SOS.Service.Implementation;
using SOS.Service.Utility;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Guardian.Webjob.Broadcaster
{
    public class DynamicAllocationToSubGroups
    {
        readonly IConfigManager configManager;
        const int minute = 60 * 1000;

        public DynamicAllocationToSubGroups(IConfigManager configManager)
        {
            this.configManager = configManager;
        }

        public void Run()
        {
            var allocationInterval = configManager.Settings.SubGroupAllocationIntervalInMinutes;
            var liveSessionRepository = new LiveSessionRepository();
            var grouprepository = new GroupRepository();
            GroupService grpService = new GroupService();
            LocalResource myStorage = RoleEnvironment.GetLocalResource("LocalStorageWorkerRole");

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
                    var DownloadedShapeFilePath = Utility.GetGrpIdGroupKeyAndShapePaths(configManager.Settings).FirstOrDefault(x => x.Item1.Equals(s.GrpId));

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
                Thread.Sleep(allocationInterval * minute);
            }

            catch (Exception ex)
            {
                Trace.TraceError("WebJob Error: DynamicAllocationToSubGroups failed! " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace, "Error");
                Thread.Sleep(5 * minute);
            }
        }
    }
}
