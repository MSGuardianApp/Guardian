using System;
using Guardian.Common;
using SOS.Mappers;

namespace SOS.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Config.AppInsightsInstrumentationKey))
            {
                Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey = Config.AppInsightsInstrumentationKey;

                // Set context properties based on config settings using custom telemetry initializers 
                Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.ContextInitializers.Add(new AppInsightsConfigInitializer());
            }
            Mapper.InitializeMappers();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}