using System;

namespace SOS.AzureSQLAccessLayer
{
    public class Member
    {
        public long ProfileID { get; set; }

        public string Name { get; set; }

        public string MobileNumber { get; set; }

        public string Email { get; set; }

        public string EnterpriseEmail { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
