using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOS.EventHubReceiver
{
    public class EventHubReceiverDependencyInjection : UnityContainerExtension
    {
        /// <summary>
        /// Initilizes dependencies.
        /// </summary>
        protected override void Initialize()
        {
            Container.RegisterType<ILocationProcessor, LocationProcessor>();
        }
    }
}