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
    class HistoryGeoLocationDest : StoreEntityBase
    {
        public string ProfileID
        {
            get { return base.PartitionKey; }
            set { base.PartitionKey = value; }
        }

        public string SessionID { get; set; }

        public bool IsSOS { get; set; }

        public string Lat { get; set; }

        public string Long { get; set; }

        public string Alt { get; set; }

        public int Speed { get; set; }

        public DateTime ClientDateTime { get; set; }

        public string MediaUri { get; set; }
        
        public long ClientTimeStamp { get; set; }

        public double Accuracy { get; set; }

    }
}


   

