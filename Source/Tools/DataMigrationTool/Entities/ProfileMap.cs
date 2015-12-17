using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOS.AzureStorageAccessLayer.Entities;

namespace SOS.OPsTools.Entities
{
   public  class ProfileMap : StoreEntityBase
    {

        public Guid StorageProfileID { get; set; }

        public long ProfileID { get; set; }


    }
}
