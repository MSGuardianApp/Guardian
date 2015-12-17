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
    class GroupMemberValidatorDest : StoreEntityBase
    {

        public int GroupID { get { return Convert.ToInt32(base.PartitionKey); } set { base.PartitionKey = value.ToString(); } }
        public string ValidationID { get { return base.RowKey; } set { base.RowKey = value; } }

        public string ProfileID { get; set; }

        public bool NotificationSent { get; set; }

        public string NotificationIdentity { get; set; }

        public bool IsValidated { get; set; }

       

    }
}