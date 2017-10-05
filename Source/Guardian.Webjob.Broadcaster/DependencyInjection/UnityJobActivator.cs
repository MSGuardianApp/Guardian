// -----------------------------------------------------------------------
// <copyright file="UnityJobActivator.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Guardian.Webjob.Broadcaster
{
    using Microsoft.Azure.WebJobs.Host;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Unity Job Activator.
    /// This class is used for resolving the interfaces used by the
    /// job functions.
    /// </summary>
    internal class UnityJobActivator : IJobActivator
    {
        private readonly IUnityContainer _container;

        public UnityJobActivator(IUnityContainer container)
        {
            _container = container;
        }

        public T CreateInstance<T>()
        {
            return _container.Resolve<T>();
        }
    }
}