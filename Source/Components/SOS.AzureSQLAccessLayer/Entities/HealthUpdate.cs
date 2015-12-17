namespace SOS.AzureSQLAccessLayer
{
    public class HealthUpdate
    {
        public bool IsProfileActive { get; set; }

        public bool IsGroupModified { get; set; }

        public string ServerVersion { get; set; }
    }
}
