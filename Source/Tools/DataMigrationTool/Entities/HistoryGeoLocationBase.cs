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
    class HistoryGeoLocationBase: StoreEntityBase
    {

        public string ProfileID { get; set; }

        public long ClientDateTime { get; set; }        

        public string Alt { get; set; }

        public DateTime ClientTimeStamp { get; set; }

        public string Command { get; set; }

        public string Identifier { get; set; }

        //public string IsSOS { get; set; }

        public string Lat { get; set; }
        public string Long { get; set; }

        public int Speed { get; set; }

        public string MediUri { get; set; }       

    }
}


   
