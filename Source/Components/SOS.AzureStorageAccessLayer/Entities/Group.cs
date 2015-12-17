using System;
using System.Collections.Generic;

namespace SOS.AzureStorageAccessLayer.Entities
{

    public class Group : StoreEntityBase
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

        public List<string> Tags { get; set; }

        public int? ParentGroupID { get; set; }

        public bool NotifySubgroups { get; set; }

        public string ShapeFileID { get; set; }

        public string SubGroupIdentificationKey { get; set; }

        public string GeoLocation { get; set; }

        public bool ShowIncidents { get; set; }
    }

    public class GroupMemberValidator : StoreEntityBase
    {

        public int GroupID { get { return Convert.ToInt32(base.PartitionKey); } set { base.PartitionKey = value.ToString(); } }
        public string ValidationID { get { return base.RowKey; } set { base.RowKey = value; } }
        public string ProfileID { get; set; }
        public bool IsValidated { get; set; }
        public string NotificationIdentity { get; set; }
        public bool NotificationSent { get; set; }
        public GroupMemberValidator()
        {
            IsValidated = false;
            NotificationSent = false;
        }
    }

    public class GroupMarshalValidator : StoreEntityBase
    {
        public int GroupID { get { return Convert.ToInt32(base.PartitionKey); } set { base.PartitionKey = value.ToString(); } }

        public string ValidationID { get { return base.RowKey; } set { base.RowKey = value; } }

        public bool NotificationSent { get; set; }

        public string NotificationIdentity { get; set; }

        public string ProfileID { get; set; }

        public bool IsValidated { get; set; }
    }
}
