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
    public class User : StoreEntityBase
    {
             
        public string UserID
        {
            get{return base.RowKey;}
            set
            { base.RowKey = value; }
        }

        public string Name { get; set; }

        //TODO:public string ContactID { get; set; } //if referred
        /// <summary>
        /// Used for Live/auth also.
        /// </summary>
        public string Email { get; set; }
       
        public string MobileNumber { get; set; }
        
        public string FBAuthID { get; set; }
        
        public string FBID { get; set; }
        
        //Use email for live id / auth
        public string LiveID { get; set; }

        public string LiveAuthID { get; set; }
        //In case of mupltiple profiles - to set active profile.
        //public string ActiveProfile {get; set;}

        public string LiveAccessToken { get; set; }

        public string LiveRefreshToken { get; set; }

     //   public string AppVersion { get; set; }
    }

    public class AdminUser : StoreEntityBase
    {
        //public string UserID { get; set; }

        public int AdminID { get; set; }

        public string GroupIDCSV { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public string LiveAuthID { get; set; }

        public string LiveUserID { get; set; }

    }

    public class CompUser : StoreEntityBase
    {
        public string UserID { get; set; }

        public string UserName { get; set; }

        public string phone { get; set; }

        public List<CompProfile> ProfileList { get; set; }
    }

    public class CompProfile : StoreEntityBase
    {
        public string ProfileID { get; set; }
        public string ProfleName { get; set; }
    }
}
