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

    public class GroupBase : StoreEntityBase
    {
        public int GroupID { get; set; }

        public string GroupName { get; set; }

        //F(X1,X2,X3,..Xn); X=> (lat:x;long:y;alt:z)
        public string GroupFence { get; set; }        

        public string Location { get; set; }

        public bool IsActive { get; set; }

        public int GroupType { get; set; }

        public int EnrollmentType { get; set; }

        public string EnrollmentKey { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }        

        public List<string> Tags { get; set; }


        //public int GroupID { get; set; }
        //public string GroupName { get; set; }                
        //public string GeoLocation { get; set; }
        //public string Location { get; set; }
        //public bool IsActive { get; set; }
        //public bool NotifySubgroups { get; set; }
        //public int ParentGroupID { get; set; }
        //public int GroupType { get; set; }
        //public int EnrollmentType { get; set; }
        //public string EnrollmentKey { get; set; }
        //public string PhoneNumber { get; set; }
        //public string Email { get; set; }
        //public string ShapeFileID { get; set; }
        //public bool ShowIncidents { get; set; }
        //public string SubGroupIdentificationKey { get; set; }       
    }
    public class GroupMembership : StoreEntityBase
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

