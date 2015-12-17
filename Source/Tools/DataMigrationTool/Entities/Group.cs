using System;
using System.Collections.Generic;
using SOS.AzureStorageAccessLayer.Entities;

namespace SOS.OPsTools.Entities
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

        ////UserID
        //public string AdminID { get; set; }

        public List<string> Tags { get; set; }
    }

    public class GroupMembership:StoreEntityBase
    {
        public int GroupID { get { return Convert.ToInt32(base.PartitionKey); } set { base.PartitionKey = value.ToString(); } }

        public string ProfileID { get { return base.RowKey; } set { base.RowKey = value; } }

        public string UserName { get; set; }

        public string EnrollmentKeyValue { get; set; }

        public bool IsValidated { get; set; }

        //public string ValidationID { get; set; }
        
        //public string MailIdentity { get; set; }
        //public bool MailSent { get; set; }

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
    //ssm
    public class GroupMarshalRelation : StoreEntityBase
    {
        public int GroupID { get { return Convert.ToInt32(base.PartitionKey); } set { base.PartitionKey = value.ToString(); } }

        public string ProfileID { get { return base.RowKey; } set { base.RowKey = value; } }

        public bool IsValidated { get; set; }
     
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
