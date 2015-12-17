namespace SOS.AzureStorageAccessLayer.Entities
{
    
    public class Configuration : StoreEntityBase
    {
        public long TimeStamp { get; set; }

        public string ConfigEntity { get; set; }

        public string ItemKey { get; set; }

        public string ItemValue { get; set; }


    }
}

