using System;
using System.Collections.Generic;
using System.Linq;
using SOS.Temp.AzureStorageAccessLayer.Entities;
using Microsoft.WindowsAzure.Storage.Table;


namespace SOS.Temp.AzureStorageAccessLayer
{
    public class GlobalPositionStorageAccess : StorageAccessBase
    {

        public void RemoveLiveLocationData(long dt)
        {
            TableQuery<Location> UQuery = null;

            List<Location> qryReturn = null;
            UQuery = new TableQuery<Location>().Where(
                TableQuery.GenerateFilterConditionForLong("ClientDateTime", QueryComparisons.LessThanOrEqual, dt)).Take(10);

            while (true)
            {
                if (!base.GetTableLoadedState("LiveGeoPosition"))
                    LoadEntityTable("LiveGeoPosition");

                if (base.IsTableLoaded)
                {
                    qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

                    if (qryReturn != null && qryReturn.Count > 0)
                    {
                        foreach (var item in qryReturn)
                        {
                            TableOperation updateOperation = TableOperation.Delete(item);
                            base.EntityTable.Execute(updateOperation);
                        }


                        if (!base.GetTableLoadedState("StaleLiveGeoPosition"))
                            LoadEntityTable("StaleLiveGeoPosition");
                        if (base.IsTableLoaded)
                        {
                            foreach (var item in qryReturn)
                            {
                                TableOperation updateOperation = TableOperation.Insert(item);
                                base.EntityTable.Execute(updateOperation);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    throw new Exception("Cloud Table not loaded");
                }
            }
        }
        public void RemoveSOSTrackingData(long dt)
        {
            TableQuery<Location> UQuery = null;

            List<Location> qryReturn = null;
            UQuery = new TableQuery<Location>().Where(
                TableQuery.GenerateFilterConditionForLong("ClientTimeStamp", QueryComparisons.LessThanOrEqual, dt));

            if (!base.GetTableLoadedState("SOSTracking"))
                LoadEntityTable("SOSTracking");

            if (base.IsTableLoaded)
            {
                qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

                if (qryReturn != null && qryReturn.Count > 0)
                {
                    foreach (var item in qryReturn)
                    {
                        TableOperation updateOperation = TableOperation.Delete(item);
                        base.EntityTable.Execute(updateOperation);
                    }
                }
            }
            else
            {
                throw new Exception("Cloud Table not loaded");
            }
        }

    }

}
