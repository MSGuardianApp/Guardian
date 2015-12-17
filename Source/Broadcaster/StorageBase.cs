using System;

namespace SOS.Temp.AzureStorageAccessLayer.Entities
{
    [Serializable]
    public abstract class StoreEntityBase : Microsoft.WindowsAzure.Storage.Table.TableEntity
    {
        public StoreEntityBase() { }
    }
}
