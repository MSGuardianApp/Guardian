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
    [Serializable]
    public class Profile : StoreEntityBase
    {
        public string ProfileID { get { return base.RowKey; } set { base.RowKey = value; } }

        public string UserID { get; set; }

        //TODO:public string ContactID { get; set; } //if referred
        //public string Email { get; set; }

        public bool IsValid { get; set; }

        public string MobileNumber { get; set; }

        public string RegionCode { get; set; }

        public string SOSToken { get; set; }

        public string TrackingToken { get; set; }

        public string DeviceID { get; set; }    

        public string FBGroup { get; set; }

        public string FBGroupID { get; set; }      

        public bool CanPost { get; set; }

        public bool CanSMS { get; set; }

        public bool CanEmail { get; set; }

        public string ArchiveFolder { get; set; }

        public string Platform { get; set; }

        public string ToastChannel { get; set; }

        public string ToastExpiry { get; set; }

        public string TinyURI { get; set; }

        public string SecurityToken { get; set; }

        public bool LocationConsent { get; set; }
    }

    [Serializable]
    public class Marshal 
    {
        public Profile Profile { get; set; }
        public bool IsValidated { get; set; }
    }
}
