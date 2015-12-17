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
    class GroupMemberValidatorBase : StoreEntityBase
    {
        public int GroupID { get; set; }
        public string ValidationID { get; set; }
        public string ProfileID { get; set; }
        public bool IsValidated { get; set; }
        public string NotificationIdentity { get; set; }
        public bool NotificationSent { get; set; }
    }
}
