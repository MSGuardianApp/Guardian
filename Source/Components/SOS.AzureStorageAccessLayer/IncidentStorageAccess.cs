using Guardian.Common.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using SOS.AzureStorageAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SOS.AzureStorageAccessLayer
{
    public class IncidentStorageAccess : StorageAccessBase, IIncidentStorageAccess
    {
        public IncidentStorageAccess(IConfigManager configManager)
            : base(configManager) { }

        public void RecordIncident(Incident report)
        {
            if (base.LoadTableSilent(Constants.IncidentsTableName))
            {
                TableOperation insertTeaser = TableOperation.Insert(report);
                base.EntityTable.Execute(insertTeaser);
            }
        }

        // Get all the Incident Data
        public List<Incident> GetAllIncidents()
        {
            base.LoadTable(Constants.IncidentsTableName);

            TableQuery<Incident> UQuery = new TableQuery<Incident>();
            List<Incident> reports = base.EntityTable.ExecuteQuery(UQuery).ToList();

            return reports != null && reports.Count > 0 ? reports : null;
        }

        public List<Incident> GetAllIncidentsByProfile(string ProfileID)
        {
            base.LoadTable(Constants.IncidentsTableName);

            TableQuery<Incident> UQuery = new TableQuery<Incident>().Where(TableQuery.GenerateFilterCondition(Constants.PartitionKey, QueryComparisons.Equal, ProfileID));
            List<Incident> reports = base.EntityTable.ExecuteQuery(UQuery).ToList();

            return reports != null && reports.Count > 0 ? reports : null;
        }

        public void RemoveIncident(string ProfileID)
        {
            if (!string.IsNullOrEmpty(ProfileID))
            {
                TableQuery<Incident> UQuery = new TableQuery<Incident>().Where(TableQuery.GenerateFilterCondition(Constants.PartitionKey, QueryComparisons.Equal, ProfileID));

                base.LoadTable(Constants.IncidentsTableName);

                List<Incident> qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();
                if (qryReturn != null && qryReturn.Count > 0)
                {
                    foreach (var item in qryReturn)
                    {
                        TableOperation updateOperation = TableOperation.Delete(item);
                        base.EntityTable.Execute(updateOperation);
                    }
                }
            }

        }

        public List<Incident> GetAllIncidentsDataByFilter(string startDate, string endDate)
        {
            long startTicks = Convert.ToInt64(startDate);
            long endTicks = Convert.ToInt64(endDate);

            string filterTimeStamp = TableQuery.CombineFilters(
                                        TableQuery.GenerateFilterConditionForLong("DateTime", QueryComparisons.GreaterThanOrEqual, startTicks),
                                        TableOperators.And,
                                        TableQuery.GenerateFilterConditionForLong("DateTime", QueryComparisons.LessThanOrEqual, endTicks)
                                        );

            TableQuery<Incident> UQuery = new TableQuery<Incident>().Where(filterTimeStamp);

            base.LoadTable(Constants.IncidentsTableName);

            List<Incident> qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            return qryReturn != null && qryReturn.Count > 0 ? qryReturn : null;
        }

        //Incident Report by ID.
        public List<Incident> GetAllIncidentsDataByID(string incidentID)
        {
            //VR , we need to provide full incidentID to get data now, 
            //otherwsise we cant compare as QueryComparisons having only Equal comparison but we discussed, user will provide only end part of ID in UI, 
            //in that case we need to have either contains comparison.Kindly suggest, how I can do.

            string filterTimeStamp = TableQuery.GenerateFilterCondition("ID", QueryComparisons.Equal, incidentID);

            TableQuery<Incident> UQuery = new TableQuery<Incident>().Where(filterTimeStamp);

            base.LoadTable(Constants.IncidentsTableName);

            List<Incident> qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            return qryReturn != null && qryReturn.Count > 0 ? qryReturn : null;
        }

    }

}
