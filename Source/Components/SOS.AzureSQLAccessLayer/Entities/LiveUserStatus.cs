namespace SOS.AzureSQLAccessLayer
{
    public class LiveUserStatus
    {
        public long ProfileID { get; set; }

        public string Name { get; set; }

        public string SessionID { get; set; }

        public int Status { get; set; }

        public string MobileNumber { get; set; }

        public string DispatchInfo { get; set; }
    }
}
