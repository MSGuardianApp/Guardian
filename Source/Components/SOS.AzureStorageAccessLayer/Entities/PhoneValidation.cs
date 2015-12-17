namespace SOS.AzureStorageAccessLayer.Entities
{
    public class PhoneValidation : StoreEntityBase
    {
        public string RegionCode { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string AuthenticatedLiveID { get; set; }

        public string Name { get; set; }

        public string SecurityToken { get; set; }

        public string EnterpriseSecurityToken { get; set; }

        public string EnterpriseEmailID { get; set; } 

        public bool IsValiated { get; set; }

        public string DeviceType { get; set; }
    }
}
