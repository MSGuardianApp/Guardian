using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOS.AzureStorageAccessLayer.Entities;

namespace SOS.OPsTools.Entities
{
  public  class UserMap : StoreEntityBase
    {
        public Guid StorageUserID { get; set; }
        
        public long UserID { get; set; }
    }
}
