using Microsoft.WindowsAzure.Storage.Table;
using SOS.AzureStorageAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOS.AzureStorageAccessLayer
{
    public class LocationHistoryStorageAccess : StorageAccessBase
    {
        public async Task SaveToLocationHistoryAsync(LocationHistory locData)
        {
            if (base.LoadTableSilent(Constants.LocationHistoryTableName))
            {
                if (string.IsNullOrEmpty(locData.PartitionKey))
                    locData.PartitionKey = locData.ProfileID;
                locData.RowKey = locData.ClientTimeStamp.ToString();
                locData.ClientDateTime = new DateTime(locData.ClientTimeStamp);

                TableOperation insertLocation = TableOperation.InsertOrReplace(locData);
                await base.EntityTable.ExecuteAsync(insertLocation);
            }
        }


        public List<LocationHistory> GetLocationHistory(string profileID, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrEmpty(profileID)) return null;

            long startTicks = startDate.Ticks;
            long endTicks = endDate.Ticks;

            string filterPartitionKey = TableQuery.GenerateFilterCondition(Constants.PartitionKey, QueryComparisons.Equal, profileID.ToString());
            string filterTimeStamp = TableQuery.CombineFilters(
                                        TableQuery.GenerateFilterConditionForLong("ClientDateTime", QueryComparisons.GreaterThanOrEqual, startTicks),
                                        TableOperators.And,
                                        TableQuery.GenerateFilterConditionForLong("ClientDateTime", QueryComparisons.LessThanOrEqual, endTicks)
                                        );

            TableQuery<LocationHistory> UQuery = new TableQuery<LocationHistory>().Where(TableQuery.CombineFilters(filterPartitionKey, TableOperators.And, filterTimeStamp));

            base.LoadTable(Constants.LocationHistoryTableName);
            List<LocationHistory> qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            return qryReturn;
        }

        public List<SessionHistory> GetSessionHistory(string profileID, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrEmpty(profileID)) return null;

            string filterPartitionKey = TableQuery.GenerateFilterCondition(Constants.PartitionKey, QueryComparisons.Equal, profileID.ToString());
            string filterTimeStamp = TableQuery.CombineFilters(
                                        TableQuery.GenerateFilterConditionForDate("SessionStartTime", QueryComparisons.GreaterThanOrEqual, startDate),
                                        TableOperators.And,
                                        TableQuery.GenerateFilterConditionForDate("SessionStartTime", QueryComparisons.LessThanOrEqual, endDate)
                                        );

            TableQuery<SessionHistory> UQuery = new TableQuery<SessionHistory>().Where(TableQuery.CombineFilters(filterPartitionKey, TableOperators.And, filterTimeStamp));

            base.LoadTable(Constants.SessionHistoryTableName);
            List<SessionHistory> qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            return qryReturn;
        }

        public List<LocationHistory> GetHistoryLocationsBySessionID(string profileID, string sessionID)
        {
            if (string.IsNullOrEmpty(profileID)) return null;

            string filterPartitionKey = TableQuery.GenerateFilterCondition(Constants.PartitionKey, QueryComparisons.Equal, profileID.ToString());
            string filterTimeStamp = TableQuery.GenerateFilterCondition("SessionID", QueryComparisons.Equal, sessionID);

            TableQuery<LocationHistory> UQuery = new TableQuery<LocationHistory>().Where(TableQuery.CombineFilters(filterPartitionKey, TableOperators.And, filterTimeStamp));

            base.LoadTable(Constants.LocationHistoryTableName);

            List<LocationHistory> qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            return qryReturn;
        }


        public List<GeoTag> GetLocationDataFromHistory(string Token)
        {
            if (string.IsNullOrEmpty(Token)) return null;

            TableQuery<LocationHistory> UQuery = new TableQuery<LocationHistory>().Where(TableQuery.GenerateFilterCondition("Identifier", QueryComparisons.Equal, Token.ToString()));

            base.LoadTable(Constants.LocationHistoryTableName);
            List<GeoTag> qryReturn = base.EntityTable.ExecuteQuery(UQuery).Select(s => new GeoTag { SessionID = s.SessionID, IsSOS = s.IsSOS, Lat = s.Lat, Long = s.Long, Alt = s.Alt, Speed = s.Speed, ClientTimeStamp = s.ClientTimeStamp, ProfileID = Convert.ToInt64(s.ProfileID) }).ToList();

            return qryReturn;
        }


        public bool IsEntryThereInHistoryTable(string ProfileID, string Token, long ClientDateTime)
        {
            bool returnVal = false;

            TableQuery<LocationHistory> UQuery = null;

            UQuery = new TableQuery<LocationHistory>().Where(TableQuery.CombineFilters(
                                                    TableQuery.CombineFilters(
                                                    TableQuery.GenerateFilterCondition(Constants.PartitionKey, QueryComparisons.Equal, ProfileID),
                                                    TableOperators.And,
                                                    TableQuery.GenerateFilterCondition("Identifier", QueryComparisons.Equal, Token))
                                                    , TableOperators.And,
                                                    TableQuery.GenerateFilterCondition("IsSOS", QueryComparisons.Equal, "1")));

            base.LoadTable(Constants.LocationHistoryTableName);

            returnVal = base.EntityTable.ExecuteQuery(UQuery).Count() > 0;

            return returnVal;
        }

        public void DeleteHistoryDetails(string ProfileID, string SessionID)
        {
            if (ProfileID != null)
            {
                try
                {
                    List<Task> tasks = new List<Task>();
                    Task locationHistoryTask = DeleteLocationHistoryAsync(ProfileID, SessionID);
                    Task sessionHistoryTask = DeleteSessionHistoryAsync(ProfileID, SessionID);
                    tasks.Add(locationHistoryTask);
                    tasks.Add(sessionHistoryTask);
                    Task.WaitAll(tasks.ToArray());
                }
                catch(Exception e)
                {
                    throw new InvalidOperationException("Unable to delete the history for SessionID: " + SessionID + ", Please contact administrator");
                }
            }
        }

        public async Task DeleteLocationHistoryAsync(string ProfileID, string SessionID)
        {
            if (ProfileID != null)
            {
                base.LoadTable(Constants.LocationHistoryTableName);

                TableQuery<LocationHistory> UQuery = null;
                string filterCondition = TableQuery.CombineFilters(
                                            TableQuery.GenerateFilterCondition(Constants.PartitionKey, QueryComparisons.Equal, ProfileID),
                                            TableOperators.And,
                                            TableQuery.GenerateFilterCondition("SessionID", QueryComparisons.Equal, SessionID)
                                            );
                UQuery = new TableQuery<LocationHistory>().Where(filterCondition);
                List<LocationHistory> locationHistoryData = null;
                locationHistoryData = base.EntityTable.ExecuteQuery(UQuery).ToList();

                TableBatchOperation deleteOperation = new TableBatchOperation();
                foreach (var locationHistoryItem in locationHistoryData)
                {
                    TableOperation item = TableOperation.Delete(locationHistoryItem);
                    deleteOperation.Add(item);
                }
                await base.EntityTable.ExecuteBatchAsync(deleteOperation);
            }
        }

        public async Task DeleteSessionHistoryAsync(string ProfileID, string SessionID)
        {
            if (ProfileID != null)
            {
                base.LoadTable(Constants.SessionHistoryTableName);

                TableQuery<SessionHistory> UQuery = null;
                string filterCondition = TableQuery.CombineFilters(
                                            TableQuery.GenerateFilterCondition(Constants.PartitionKey, QueryComparisons.Equal, ProfileID),
                                            TableOperators.And,
                                            TableQuery.GenerateFilterCondition("SessionID", QueryComparisons.Equal, SessionID)
                                            );
                UQuery = new TableQuery<SessionHistory>().Where(filterCondition);
                List<SessionHistory> sessionHistoryData = null;
                sessionHistoryData = base.EntityTable.ExecuteQuery(UQuery).ToList();

                TableBatchOperation deleteOperation = new TableBatchOperation();
                foreach (var sessionHistoryItem in sessionHistoryData)
                {
                    TableOperation item = TableOperation.Delete(sessionHistoryItem);
                    deleteOperation.Add(item);
                }
                await base.EntityTable.ExecuteBatchAsync(deleteOperation);
            }
        }
    }

}
