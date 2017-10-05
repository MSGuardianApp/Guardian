
namespace Guardian.Common.Configuration
{
    using Guardian.Common.Helpers;
    using Microsoft.Azure;
    using Microsoft.Azure.KeyVault;
    using System;

    /// <summary>
    /// Class for managing configuration settings for the application.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ConfigManager : IConfigManager
    {
        private readonly IKeyVaultWrapper _keyVault;

        /// <summary>
        /// Initializes a new instance of<see cref= "ConfigManager" /> class.
        /// </summary>
        /// <param name="keyVaultWrapper"></param>
        /// <param name="authConfiguration"></param>
        public ConfigManager(IKeyVaultWrapper keyVaultWrapper, KeyVaultAuthConfiguration authConfiguration)
        {
            if (!string.IsNullOrWhiteSpace(authConfiguration.ClientId) && !string.IsNullOrWhiteSpace(authConfiguration.CertificateThumbprint) && !string.IsNullOrWhiteSpace(authConfiguration.CertificateStoreLocation))
            {
                // Key vault settings configured, use the same to initialize the key vault wrapper.
                _keyVault = keyVaultWrapper;
                _keyVault.Initialize(authConfiguration.ClientId, authConfiguration.CertificateThumbprint, authConfiguration.CertificateStoreLocation);
            }

            LoadSettings();
        }

        /// <summary>
        /// Gets the configuration settings objects
        /// </summary>
        public Settings Settings { get; private set; }

        /// <summary>
        /// Gets configuration setting value given a configuration setting key.
        /// </summary>
        /// <param name="key">The configuration setting key.</param>
        /// <returns>The configuration setting value.</returns>
        private string this[string key] => GetSetting(key);

        /// <summary>
        /// Gets configuration setting value given a confirguration setting key.
        /// </summary>
        /// <param name="key">The configuration setting key.</param>
        /// <returns>The configuration setting value.</returns>
        private string GetSetting(string key)
        {
            string value = CloudConfigurationManager.GetSetting(key);
            Uri tempUri;
            // If key vault is configured in the settings, use the same to retrieve the secret values from key vault using the configured secret identifier uri, if any.
            if (_keyVault != null && Uri.TryCreate(value, UriKind.Absolute, out tempUri) && SecretIdentifier.IsSecretIdentifier(value))
            {
                return ResolveSecretSetting(value);
            }
            return value;
        }

        /// <summary>
        /// Loads the configuration settings data.
        /// </summary>
        private void LoadSettings()
        {
            this.Settings = new Settings
            {
                AzureSQLConnectionString = this["AzureSQLConnectionString"],
                AzureStorageConnectionString = this["AzureStorageConnectionString"],
                
                UseEventHubs = bool.Parse(this["UseEventHubs"]),
                EventHubConnectionString = this["EventHubConnectionString"],
                EventHubName = this["EventHubName"],

                GuardianPortalUri = this["GuardianPortalUri"],
                TinyServiceUri = this["TinyServiceUri"],

                SMSPostGap = int.Parse(this["SMSPostGap"]),
                EmailPostGap = int.Parse(this["EmailPostGap"]),

                BroadcastRunIntervalInSeconds = int.Parse(this["BroadcastRunIntervalInSeconds"]),
                ArchiveTimeGapInMinutes = int.Parse(this["ArchiveTimeGapInMinutes"]),
                ArchiveRunIntervalInMinutes = int.Parse(this["ArchiveRunIntervalInMinutes"]),
                TimeToResetCacheInMinutes = int.Parse(this["TimeToResetCacheInMinutes"]),
                SubGroupAllocationIntervalInMinutes = int.Parse(this["SubGroupAllocationIntervalInMinutes"]),

                SendSms = bool.Parse(this["SendSms"]),
                RandomNumberDigits = this["RandomNumberDigits"],
                SMSDefaultFromNumber = this["SMSDefaultFromNumber"],
                SMSServiceUserID = this["SMSServiceUserID"],
                SMSServicePassword = this["SMSServicePassword"],
                IntlSMSServiceUserID = this["IntlSMSServiceUserID"],
                IntlSMSServicePassword = this["IntlSMSServicePassword"],

                SendGridUserID = this["SendGridUserID"],
                SendGridPassword = this["SendGridPassword"],

                BingKey = this["BingKey"],
                LiveAuthClientSecret = this["LiveAuthClientSecret"],
                LiveAuthAppUri = this["LiveAuthAppUri"],

                GoogleClientID = this["GoogleClientID"],
                
                IncludeActiveMembers = bool.Parse(this["IncludeActiveMembers"]),
                DefaultGroupID = this["DefaultGroupID"],

                IsEnterpriseBuild = bool.Parse(this["IsEnterpriseBuild"]),
                EnterpriseDomain = this["EnterpriseDomain"],

                AppInsights_InstrumentationKey = this["AppInsights_InstrumentationKey"]
            };
        }

        /// <summary>
        /// Resolves secret setting using the key vault.
        /// </summary>
        /// <param name="secretUrl"> The secret URL </param>
        /// <returns> the secret value </returns>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private string ResolveSecretSetting(string secretUrl)
        {
            var secret = _keyVault.GetSecretAsync(secretUrl).ConfigureAwait(false);
            return secret.GetAwaiter().GetResult().Value;
        }
    }
}