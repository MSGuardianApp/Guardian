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
    public class GroupAdminsDest : StoreEntityBase
    {
        public int AdminID { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string LiveUserID { get; set; }
        public string LiveAuthID { get; set; }
        public string GroupIDCSV { get; set; }
        public string Email { get; set; }
        public bool AllowGroupManagement { get; set; }        
       
    }
}
