using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOS.AzureStorageAccessLayer.Entities;

namespace SOS.OPsTools.Entities
{
    
    public class Configuration : StoreEntityBase
    {
        public long TimeStamp { get; set; }

        public string ConfigEntity { get; set; }

        public string ItemKey { get; set; }

        public string ItemValue { get; set; }


    }
}

