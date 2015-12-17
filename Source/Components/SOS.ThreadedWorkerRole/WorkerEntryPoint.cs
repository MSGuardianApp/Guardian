using System;

namespace SOS.ThreadedWorkerRole
{
    /// <summary>
    /// Model for Workers
    /// </summary>
    public class WorkerEntryPoint
    {
        /// <summary>
        /// OnStart method for workers
        /// </summary>
        /// <returns>bool for success</returns>
        public virtual bool OnStart()
        {
            return true;
        }

        /// <summary>
        /// Run method
        /// </summary>
        public virtual void Run()
        {
        }

        /// <summary>
        /// OnStop method
        /// </summary>
        public virtual void OnStop()
        {
        }

        /// <summary>
        /// This method prevents unhandled exceptions from being thrown
        /// from the worker thread.
        /// </summary>
        internal void ProtectedRun()
        {
            try
            {
                // Call the Workers Run() method
                this.Run();
            }
            catch (SystemException)
            {
                // Exit Quickly on a System Exception
                throw;
            }
            catch (Exception)
            {
            }
        }
    }
}