using Microsoft.ApplicationInsights.Extensibility;
using SOS.ConfigManager;
namespace SOS.RESTService
{
    public class AppInsightsConfigInitializer : IContextInitializer
    {
        public void Initialize(Microsoft.ApplicationInsights.DataContracts.TelemetryContext context)
        {
            // Configure app version 
            //context.Component.Version = typeof(SOS.RESTService.Global).Assembly.GetName().Version.ToString();

            // Set arbitrary tags  - Removed from build 13 to build 14
            context.Properties["Environment"] = Config.AppInsightsEnvironment + "Service";
        }
    }
}