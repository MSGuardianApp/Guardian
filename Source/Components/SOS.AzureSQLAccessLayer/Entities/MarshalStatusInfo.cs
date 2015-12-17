namespace SOS.AzureSQLAccessLayer.Entities
{
    public class MarshalStatusInfo
    {
        public long ProfileID { get; set; }
        public long UserID { get; set; }
        public int Code { get; set; }
        public string MessageInfo { get; set; }
    }
}
