using Guardian.Common.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using SOS.AzureStorageAccessLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SOS.AzureStorageAccessLayer
{
    public class ConfigurationStorageAccess : StorageAccessBase, IConfigurationStorageAccess
    {
        public ConfigurationStorageAccess(IConfigManager configManager)
            : base(configManager) { }

        public string GetLatestAppVersion()
        {
            TableQuery<Configuration> UQuery = null;
            List<Configuration> qryReturn = null;

            UQuery = new TableQuery<Configuration>().Take(1);

            LoadTable(Constants.ConfigurationTableName);

            qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            if (qryReturn != null && qryReturn.Count > 0)
                return qryReturn.First().ItemValue;

            if (qryReturn != null && qryReturn.Count > 0)
                return qryReturn.First().ItemValue;
            return "";
        }

    }
}

