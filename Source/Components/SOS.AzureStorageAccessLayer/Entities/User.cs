namespace SOS.AzureStorageAccessLayer.Entities
{
    public class AdminUser : StoreEntityBase
    {
        //public string UserID { get; set; }

        public int AdminID { get; set; }

        public string GroupIDCSV { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public string LiveUserID { get; set; }

        public bool AllowGroupManagement { get; set; }
    }
}
