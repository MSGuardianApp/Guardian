namespace SOS.AzureStorageAccessLayer.Entities
{
    public class Incident : StoreEntityBase
    {
        public string ProfileID { get; set; }

        public string ID { get { return base.PartitionKey; } set { base.PartitionKey = value; } }

        public string IncidentID { get { return base.RowKey; } set { base.RowKey = value; } }

        public string Type { get; set; }

        public string Name { get; set; }

        public string MobileNumber { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Altitude { get; set; }

        public long DateTime { get; set; }

        public string MediaUri { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
