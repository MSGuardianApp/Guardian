using Guardian.Common.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using SOS.AzureStorageAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SOS.AzureStorageAccessLayer
{
    public class SessionHistoryStorageAccess : StorageAccessBase, ISessionHistoryStorageAccess
    {
        public SessionHistoryStorageAccess(IConfigManager configManager)
            : base(configManager) { }

        public async Task ArchiveSessionDetailAsync(SessionHistory session)
        {
            if (session != null)
            {
                if (base.LoadTableSilent(Constants.SessionHistoryTableName))
                {
                    session.PartitionKey = session.ProfileID;
                    session.RowKey = session.ClientTimeStamp;
                    TableOperation insertSession = TableOperation.Insert(session);
                    await base.EntityTable.ExecuteAsync(insertSession);
                }
            }
        }

        public async Task ArchiveSessionDetailsAsync(List<SessionHistory> sessions)
        {
            if (sessions != null)
            {

                if (base.LoadTableSilent(Constants.SessionHistoryTableName))
                {
                    //Parallel and batch processing
                    //TableBatchOperation insertSessions = null;
                    //var disctinctProfileIDs = sessions.Select(s => s.ProfileID).Distinct();
                    //foreach (var p in disctinctProfileIDs)
                    //{
                    //    insertSessions = new TableBatchOperation();
                    //    List<SessionHistory> sessionsByProfileID = sessions.Where(ses => ses.ProfileID == "6").ToList();
                    //    Parallel.ForEach(sessionsByProfileID, session =>
                    //        {
                    //            TableOperation insertSession = TableOperation.Insert(session);
                    //            insertSessions.Add(insertSession);
                    //        });
                    //    await base.EntityTable.ExecuteBatchAsync(insertSessions);
                    //    break;
                    //}

                    //Sequential and batch processing
                    //var disctinctProfileIDs = sessions.Select(s => s.ProfileID).Distinct();
                    //foreach (var p in disctinctProfileIDs)
                    //{
                    //    insertSessions = new TableBatchOperation();
                    //    List<SessionHistory> sessionsByProfileID = sessions.Where(ses => ses.ProfileID == "6").ToList();
                    //    foreach (var session in sessionsByProfileID)
                    //    {
                    //        TableOperation insertSession = TableOperation.InsertOrReplace(session);
                    //        insertSessions.Add(insertSession);
                    //    }
                    //    await base.EntityTable.ExecuteBatchAsync(insertSessions);
                    //}

                    foreach (var session in sessions)
                    {
                        if (!session.InSOS.HasValue || (bool)session.InSOS)
                        {
                            session.EmailRecipientsList = string.Empty;
                            session.SMSRecipientsList = string.Empty;
                        }
                        TableOperation insertSession = TableOperation.InsertOrReplace(session);
                        await base.EntityTable.ExecuteAsync(insertSession);
                    }
                }
            }
        }

        public List<object> GetAllSessionHistory(long startTicks, long endTicks, bool sosFlag = true)
        {
            try
            {
                int rowCount = 1;
                TableQuery<SessionHistory> UQuery = null;

                string filterInSOS = TableQuery.GenerateFilterConditionForBool("InSOS", QueryComparisons.Equal, sosFlag);
                string filterTimeStamp = TableQuery.CombineFilters(
                                            TableQuery.GenerateFilterConditionForDate("SessionStartTime", QueryComparisons.GreaterThanOrEqual, new DateTime(startTicks)),
                                            TableOperators.And,
                                            TableQuery.GenerateFilterConditionForDate("SessionStartTime", QueryComparisons.LessThanOrEqual, new DateTime(endTicks).AddHours(24))
                                            );

                UQuery = new TableQuery<SessionHistory>().Where(TableQuery.CombineFilters(filterInSOS, TableOperators.And, filterTimeStamp));

                base.LoadTableSilent(Constants.SessionHistoryTableName);

                var qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList()
                                        .Select(sessionHist => new
                                        {
                                            SNo = Convert.ToString(rowCount++),
                                            MobileNumber = sessionHist.MobileNumber,
                                            UserName = sessionHist.Name,
                                            StartDate = Convert.ToString(sessionHist.SessionStartTime),
                                            TotalTimeinSOS = sessionHist.SessionEndTime != null ? Convert.ToString(sessionHist.SessionEndTime.Value.Ticks - sessionHist.SessionStartTime.Ticks) : "UnKnown",
                                            SOSAlerts = Convert.ToString(sessionHist.NoOfSMSSent),
                                            EmailAlerts = Convert.ToString(sessionHist.NoOfEmailsSent),
                                            EmailBuddies = Convert.ToString(sessionHist.NoOfEmailRecipients),
                                            SOSBuddies = Convert.ToString(sessionHist.NoOfSMSRecipients)
                                        });

                return qryReturn.ToList<object>();
            }

            catch (Exception ex) { return null; }

        }

        public List<object> GetAllSessionSOSAndTrackHistory(long startTicks, long endTicks, bool sosFlag = true)
        {
            try
            {
                int rowCount = 1;
                TableQuery<SessionHistory> UQuery = null;

                string filterInSOS = TableQuery.GenerateFilterConditionForBool("InSOS", QueryComparisons.Equal, sosFlag);
                string filterTimeStamp = TableQuery.CombineFilters(
                                            TableQuery.GenerateFilterConditionForDate("SessionStartTime", QueryComparisons.GreaterThanOrEqual, new DateTime(startTicks)),
                                            TableOperators.And,
                                            TableQuery.GenerateFilterConditionForDate("SessionStartTime", QueryComparisons.LessThanOrEqual, new DateTime(endTicks).AddHours(24))
                                            );

                UQuery = new TableQuery<SessionHistory>().Where(TableQuery.CombineFilters(filterInSOS, TableOperators.And, filterTimeStamp));

                base.LoadTableSilent(Constants.SessionHistoryTableName);

                var qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList()
                                        .Select(sessionHist => new
                                        {

                                            MobileNumber = sessionHist.MobileNumber,
                                            UserName = sessionHist.Name,
                                            InSOS = sessionHist.InSOS,
                                            NoOfSMSRecipients = sessionHist.NoOfSMSRecipients,
                                            NoOfEmailRecipients = sessionHist.NoOfEmailRecipients,
                                            NoOfSMSSents = sessionHist.NoOfSMSSent,
                                            NoOfEmailsSents = sessionHist.NoOfEmailsSent

                                        });

                var SOSAndTrackInfo = qryReturn
                                        .GroupBy(qry => new { qry.UserName, qry.MobileNumber })
                                        .Select(g =>
                                            new
                                            {
                                                SNo = Convert.ToString(rowCount++),
                                                UserName = g.Key.UserName,
                                                MobileNumber = g.Key.MobileNumber,
                                                TotalTracks = g.Count(),
                                                TotalSOSs = g.Count(x => x.InSOS.Value),
                                                TotalSMSSent = g.Sum(x => x.NoOfSMSSents != null ? x.NoOfSMSSents : 0) * g.Sum(x => x.NoOfSMSRecipients != null ? x.NoOfSMSRecipients : 0),
                                                TotalEmailSent = g.Sum(x => x.NoOfEmailsSents != null ? x.NoOfEmailsSents : 0) * g.Sum(x => x.NoOfEmailRecipients != null ? x.NoOfEmailRecipients : 0)

                                            });

                return SOSAndTrackInfo.ToList<object>();
            }

            catch (Exception ex) { return null; }

        }
    }
}



