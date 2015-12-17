using System;
using Microsoft.WindowsAzure.Storage.Table;


namespace SOS.OPsTools
{

    public abstract class StorageAccessBase
    {

        private Microsoft.WindowsAzure.Storage.CloudStorageAccount _StorageAccount = null;

        internal Microsoft.WindowsAzure.Storage.CloudStorageAccount StorageAccount
        {
            get { return _StorageAccount; }
            private set { _StorageAccount = value; }
        }

        private CloudTableClient _TableClient = null;

        internal CloudTableClient TableClient
        {
            get { return _TableClient; }
            private set { _TableClient = value; }
        }

        private CloudTable _EntityTable = null;

        internal CloudTable EntityTable
        {
            get { return _EntityTable; }
            private set { _EntityTable = value; }
        }

        private bool _TableLoaded = false;

        internal bool IsTableLoaded
        {
            get { return _TableLoaded; }
            private set { _TableLoaded = value; }
        }

        internal void LoadEntityTable(string EntityType)
        {
            try
            {
                _EntityTable = _TableClient.GetTableReference(EntityType);
                _EntityTable.CreateIfNotExists();
                _TableLoaded = true;
            }
            catch (Exception ex)
            {
            }
        }

        internal bool GetTableLoadedState(string ExpectedEntity)
        {
            bool rValue = false;

            if (this._TableLoaded)
            {
                rValue = EntityTable.Name == ExpectedEntity;
            }

            return rValue;

        }

        internal void LoadTable(string tableName)
        {
            if (!GetTableLoadedState(tableName))
                LoadEntityTable(tableName);

            if (!IsTableLoaded)
                throw new FieldAccessException(string.Format("Cloud Table: {0} not loaded", tableName));
        }
        internal bool LoadTableSilent(string tableName)
        {
            if (!GetTableLoadedState(tableName))
                LoadEntityTable(tableName);

            return IsTableLoaded;
        }

        public StorageAccessBase()
        {
            _StorageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(Config.BaseStorageConnection);
            _TableClient = StorageAccount.CreateCloudTableClient();
        }
    }
}
