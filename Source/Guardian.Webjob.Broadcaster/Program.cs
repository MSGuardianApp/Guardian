using Microsoft.Azure.WebJobs;
using Microsoft.Practices.Unity;

namespace Guardian.Webjob.Broadcaster
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        static void Main()
        {
            var container = new UnityContainer();

            var config = new JobHostConfiguration
            {
                JobActivator = new UnityJobActivator(container)
            };

            DependencyInjection.Configure(container, config);

            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }

            config.UseTimers();

            var host = new JobHost(config);

            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
