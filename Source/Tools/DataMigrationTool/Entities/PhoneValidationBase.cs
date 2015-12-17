﻿using System;
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
    class PhoneValidationBase : StoreEntityBase
    {
        public string AuthenticatedLiveID { get; set; }		        		        

        public bool IsValiated { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string RegionCode { get; set; }

        public string SecurityToken { get; set; }

        

    }
}