using Microsoft.WindowsAzure.ServiceRuntime;
using System.Collections.Generic;
using System.Threading;

namespace SOS.ThreadedWorkerRole
{
    /// <summary>
    /// Middle class that sits between WebRole and RoleEntryPoint
    /// </summary>
    public abstract class ThreadedRoleEntryPoint: RoleEntryPoint
    {
        /// <summary>
        /// Threads for workers
        /// </summary>
        private List<Thread> threads = new List<Thread>();

        /// <summary>
        /// Worker array passed in from WebRole
        /// </summary>
        private WorkerEntryPoint[] workers;

        /// <summary>
        /// Initializes a new instance of the ThreadedRoleEntryPoint class
        /// </summary>
        public ThreadedRoleEntryPoint()
        {
            EventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        }

        /// <summary>
        /// Gets or sets WaitHandle to deal with stops and exceptions
        /// </summary>
        protected EventWaitHandle EventWaitHandle { get; set; }

        /// <summary>
        /// Called from WebRole, bringing in workers to add to threads
        /// </summary>
        /// <param name="workers">WorkerEntryPoint[] arrayWorkers</param>
        public void Run(WorkerEntryPoint[] arrayWorkers)
        {
            this.workers = arrayWorkers;

            foreach (WorkerEntryPoint worker in this.workers)
            {
                worker.OnStart();
            }

            foreach (WorkerEntryPoint worker in this.workers)
            {
                this.threads.Add(new Thread(worker.ProtectedRun));
            }

            foreach (Thread thread in this.threads)
            {
                thread.Start();
            }

            while (!EventWaitHandle.WaitOne(0))
            {
                // Restart Dead Threads
                for (int i = 0; i < this.threads.Count; i++)
                {
                    if (!this.threads[i].IsAlive)
                    {
                        this.threads[i] = new Thread(this.workers[i].Run);
                        this.threads[i].Start();
                    }
                }

                EventWaitHandle.WaitOne(1000);
            }
        }

        /// <summary>
        /// OnStart override
        /// </summary>
        /// <returns>book success</returns>
        public override bool OnStart()
        {
            return base.OnStart();
        }

        /// <summary>
        /// OnStop override
        /// </summary>
        public override void OnStop()
        {
            EventWaitHandle.Set();

            foreach (Thread thread in this.threads)
            {
                while (thread.IsAlive)
                {
                    thread.Abort();
                }
            }

            // Check To Make Sure The Threads Are
            // Not Running Before Continuing
            foreach (Thread thread in this.threads)
            {
                while (thread.IsAlive)
                {
                    Thread.Sleep(10);
                }
            }

            // Tell The Workers To Stop Looping
            foreach (WorkerEntryPoint worker in this.workers)
            {
                worker.OnStop();
            }

            base.OnStop();
        }
    }
}