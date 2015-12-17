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
    class GroupDest : StoreEntityBase
    {
        public int GroupID { get { return Convert.ToInt32(base.RowKey); } set { base.RowKey = value.ToString(); } }
        public string GroupName { get; set; }
        public string Location { get { return base.PartitionKey; } set { base.PartitionKey = value; } }
        public bool IsActive { get; set; }
        public int GroupType { get; set; }
        public int EnrollmentType { get; set; }
        public string EnrollmentKey { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }                
        public int? ParentGroupID { get; set; }
        public bool NotifySubgroups { get; set; }
        public string ShapeFileID { get; set; }
        public string SubGroupIdentificationKey { get; set; }
        public string GeoLocation { get; set; }
        public bool ShowIncidents { get; set; }
    }

}
