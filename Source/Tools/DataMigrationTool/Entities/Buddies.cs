using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using SOS.AzureStorageAccessLayer.Entities;

namespace SOS.OPsTools.Entities
{
    public class Buddy : StoreEntityBase
    {
        public string BuddyID { get; set; }

        public string ProfileID { get { return base.PartitionKey; } set { base.PartitionKey = value; } }

        public string UserID { get { return base.RowKey; } set { base.RowKey = value; } }

        public string BuddyName { get; set; }

        public string MobileNumber { get; set; }

        public string Email { get; set; }

        public bool IsPrimeBuddy { get; set; }

        public int? State { get; set; }
    }
}
