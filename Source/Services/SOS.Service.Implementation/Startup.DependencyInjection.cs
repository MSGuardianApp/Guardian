using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOS.Service.Implementation
{
    public class ImplementationDependencyInjection : UnityContainerExtension
    {
        /// <summary>
        /// Initilizes dependencies.
        /// </summary>
        protected override void Initialize()
        {
            //Container.RegisterType<ILicenseRepository, LicenseRepository>();
            //Container.RegisterType<ILicenseCacheProvider, LicenseCacheProvider>(new ContainerControlledLifetimeManager());

            //Container.AddNewExtension<RepositoryLayerDependencyInjection>();
            //Container.AddNewExtension<ProviderLayerDependencyInjection>();
        }
    }
}