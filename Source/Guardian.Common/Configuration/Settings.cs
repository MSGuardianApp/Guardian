namespace Guardian.Common.Configuration
{
    /// <summary>
    /// The object is used for getting application configuration settings
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Settings
    {
        public string AzureSQLConnectionString { get; set; }

        public string AzureStorageConnectionString { get; set; }

        public bool UseEventHubs { get; set; }

        public string EventHubConnectionString { get; set; }

        public string EventHubName { get; set; }

        public string SMSDefaultFromNumber { get; set; }

        public string GuardianPortalUri { get; set; }

        public string TinyServiceUri { get; set; }

        public int SMSPostGap { get; set; } = 15;

        public int EmailPostGap { get; set; } = 15;

        public bool SendSms { get; set; }

        public int SubGroupAllocationIntervalInMinutes { get; set; } = 5;

        public int ArchiveTimeGapInMinutes { get; set; } = 240;

        public int ArchiveRunIntervalInMinutes { get; set; } = 10;

        public string SMSServiceUserID { get; set; }

        public string SMSServicePassword { get; set; }

        public string IntlSMSServiceUserID { get; set; }

        public string IntlSMSServicePassword { get; set; }

        public string SendGridUserID { get; set; }

        public string SendGridPassword { get; set; }

        public string BingKey { get; set; }

        public string LiveAuthClientSecret { get; set; }

        public string LiveAuthAppUri { get; set; }

        public string GoogleClientID { get; set; }

        public string RandomNumberDigits { get; set; }

        public int TimeToResetCacheInMinutes { get; set; } = 240;

        public bool IncludeActiveMembers { get; set; }

        public string DefaultGroupID { get; set; }

        public int BroadcastRunIntervalInSeconds { get; set; } = 60;

        public string AppInsights_InstrumentationKey { get; set; }

        public bool IsEnterpriseBuild { get; set; }

        public string EnterpriseDomain { get; set; }
    }
}