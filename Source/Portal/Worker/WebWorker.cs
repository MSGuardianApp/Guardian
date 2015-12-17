//using Microsoft.WindowsAzure.ServiceRuntime;
//using SOS.ThreadedWorkerRole;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading;
//using System.Web;

//namespace SOS.Web
//{
//    public class WebWorker : ThreadedRoleEntryPoint
//    {
//        public override bool OnStart()
//        {
//            return base.OnStart();
//        }
//        public override void Run()
//        {
//            // Note: Evenhub processing has been moved to worker bcz this is not running in long running

//            Trace.WriteLine("Worker in Web role has started...", "Information");

//            List<WorkerEntryPoint> workers = new List<WorkerEntryPoint>();

//            // Add multiple workers, if you have any.
//            //workers.Add(new EventHubReceivingWorker());
//            //workers.Add(new Worker2());

//            base.Run(workers.ToArray());
//        }
//    }
//}