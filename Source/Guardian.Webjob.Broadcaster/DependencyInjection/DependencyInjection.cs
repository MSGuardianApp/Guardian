﻿namespace Guardian.Webjob.Broadcaster
{
    using Guardian.Common.Configuration;
    using Guardian.Common.Helpers;
    using Microsoft.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Practices.Unity;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using SOS.AzureSQLAccessLayer;
    using SOS.AzureStorageAccessLayer;
    using SOS.EventHubReceiver;

    /// <summary>
    /// Dependency Injection configuration.
    /// </summary>
    public static class DependencyInjection
    {
        public static void Configure(IUnityContainer container, JobHostConfiguration config)
        {
            #region Read Configuration

            container.RegisterInstance(new KeyVaultAuthConfiguration
            {
                ClientId = CloudConfigurationManager.GetSetting("KeyVault_AuthClientId"),
                CertificateThumbprint = CloudConfigurationManager.GetSetting("KeyVault_AuthCertificateThumbprint"),
                CertificateStoreLocation = CloudConfigurationManager.GetSetting("KeyVault_AuthCertificateStoreLocation")
            });
            container.RegisterType<ICertificateProvider, CertificateProvider>(new ContainerControlledLifetimeManager());
            var certProvider = container.Resolve<ICertificateProvider>();

            container.RegisterType<IKeyVaultWrapper, KeyVaultWrapper>(new ContainerControlledLifetimeManager());

            container.RegisterType<IConfigManager, ConfigManager>(new ContainerControlledLifetimeManager());

            var configManager = container.Resolve<IConfigManager>();

            #endregion // Read Configuration

            container.RegisterType<IReceiver, ReceiverHost>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILiveSessionRepository, LiveSessionRepository>();
            container.RegisterType<ILocationRepository, LocationRepository>();
            container.RegisterType<IGroupRepository, GroupRepository>();

            container.RegisterType<ISessionHistoryStorageAccess, SessionHistoryStorageAccess>();
            container.RegisterType<IGroupStorageAccess, GroupStorageAccess>();

            //container.RegisterType<IGroupService, GroupService>();
            container.RegisterType<IReceiver, ReceiverHost>();

            container.AddNewExtension<EventHubReceiverDependencyInjection>();

            #region Initialize Web Jobs SDK

            config.DashboardConnectionString = configManager.Settings.AzureStorageConnectionString;
            config.StorageConnectionString = configManager.Settings.AzureStorageConnectionString;

            #endregion // Initialize Web Job

            #region Serialization Settings

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                //Converters = new List<JsonConverter>()
                //{
                //    new StringEnumConverter()
                //    {
                //        CamelCaseText = true
                //    }
                //}
            };

            #endregion

            #region Logging/ Telemetry

            //container.RegisterInstance(new TelemetryClient());
            //container.RegisterType<ILogger, TelemetryClient>(new ContainerControlledLifetimeManager());

            #endregion
        }
    }
}