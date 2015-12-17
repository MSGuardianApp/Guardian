using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOS.AzureStorageAccessLayer.Entities;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System.Data.Services.Common;
using System.Data.SqlClient;
using SOS.AzureStorageAccessLayer;
using SOS.OPsTools.Entities;
using System.Threading;
using Tools;

namespace SOS.OPsTools
{
    public class MainStorageAccess : StorageAccessBase
    {

        StorageAccessDestination storageAccessDestination = new StorageAccessDestination();
        MainRepository _Repository = new MainRepository();
        Tools.OPsLogger opsLogger = Tools.OPsLogger.GetInstance("Logger_Migration.txt");

        private HistoryGeoLocationDest ConvertSessionToDest(Entities.HistoryGeoLocationBase session)
        {
            return AutoMapper.Mapper.Map<HistoryGeoLocationDest>(session);
        }

        private void MapHistoryGeoLocation()
        {
            AutoMapper.Mapper.CreateMap<HistoryGeoLocationBase, HistoryGeoLocationDest>()
               .ForMember(d => d.ClientDateTime, opt => opt.MapFrom(s => s.ClientTimeStamp))
               .ForMember(d => d.ClientTimeStamp, opt => opt.MapFrom(s => s.ClientDateTime));
               //.ForMember(d => d.IsSOS, opt => opt.MapFrom(s => s.IsSOS == "1" ? true : false));
        }

        private IncidentDest ConvertSessionToDest(Entities.IncidentBase session)
        {
            return AutoMapper.Mapper.Map<IncidentDest>(session);

        }
        private void MapIncident()
        {
            AutoMapper.Mapper.CreateMap<IncidentBase, IncidentDest>()
               .ForMember(d => d.IncidentID, opt => opt.MapFrom(s => s.TeaseID));
        }

        private PhoneValidationDest ConvertSessionToDest(Entities.PhoneValidationBase session)
        {
            return AutoMapper.Mapper.Map<PhoneValidationDest>(session);
        }

        private void MapPhoneValidation()
        {
            AutoMapper.Mapper.CreateMap<PhoneValidationBase, PhoneValidationDest>();
        }

        private GroupDest ConvertSessionToDest(Entities.GroupBase session)
        {
            return AutoMapper.Mapper.Map<GroupDest>(session);
        }

        private void MapGroup()
        {
            AutoMapper.Mapper.CreateMap<GroupBase, GroupDest>()
                .ForMember(d => d.GeoLocation, opt => opt.MapFrom(s => s.GroupFence));
        }
        private GroupAdminsDest ConvertSessionToDest(Entities.GroupAdminsBase session)
        {
            return AutoMapper.Mapper.Map<GroupAdminsDest>(session);
        }

        private void MapGroupAdmin()
        {
            AutoMapper.Mapper.CreateMap<GroupAdminsBase, GroupAdminsDest>();
        }
        private GroupMemberValidatorDest ConvertSessionToDest(Entities.GroupMemberValidatorBase session)
        {
            return AutoMapper.Mapper.Map<GroupMemberValidatorDest>(session);
        }

        private void MapGroupMemberValidator()
        {
            AutoMapper.Mapper.CreateMap<GroupMemberValidatorBase, GroupMemberValidatorDest>();
        }
        //HistoryGeoLocation
        public void MigrateHistoryGeoLocationtoDest(DateTime LastRunTime)
        {
            TableQuery<SOS.OPsTools.Entities.HistoryGeoLocationBase> UQuery = null;
            if (base.LoadTableSilent(ConstantOps.HistoryGeoLocationSrcTableName))
            {

                UQuery = new TableQuery<SOS.OPsTools.Entities.HistoryGeoLocationBase>().Where(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, LastRunTime));
                var qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

                Dictionary<Guid, long> MapProfiles = _Repository.GetAllMapProfiles().Result;

                SOS.OPsTools.Entities.HistoryGeoLocationDest destRecord = new HistoryGeoLocationDest();

                List<string> RetryMigrateProfiles = new List<string>();
                

                if (storageAccessDestination.LoadTableSilent(ConstantOps.HistoryGeoLocationDestTableName))
                {
                    //Parallel and batch processing
                    TableBatchOperation insertSessions = null;
                    var disctinctProfileIDs = qryReturn.Select(s => s.ProfileID).Distinct();

                    MapHistoryGeoLocation();

                    foreach (var p in disctinctProfileIDs)
                    {
                        try
                        {
                            string newProfileID = MapProfiles[Guid.Parse(p)].ToString();
                            insertSessions = new TableBatchOperation();
                            List<Entities.HistoryGeoLocationBase> sessionsByProfileID = qryReturn.Where(ses => ses.ProfileID == p).ToList();

                            Parallel.ForEach(sessionsByProfileID, session =>
                            {
                                if (session.Identifier.Length > 2)
                                {
                                    destRecord = ConvertSessionToDest(session);                                    
                                    destRecord.ProfileID = newProfileID;
                                    destRecord.SessionID = session.Identifier.Substring(2);
                                    destRecord.IsSOS = Convert.ToBoolean(Convert.ToInt32(session.Identifier.Substring(0, 1)));
                                    destRecord.Accuracy = 0;
                                    TableOperation insertSession = TableOperation.Insert(destRecord);
                                    insertSessions.Add(insertSession);
                                }
                            });

                            storageAccessDestination.EntityTable.ExecuteBatch(insertSessions);
                        }
                        catch (Exception ex)
                        {
                            opsLogger.WriteLog("HistoryGeoLocation Error Message: " + ex.Message + Environment.NewLine + "ProfileID: " + p);
                            if (!string.IsNullOrEmpty(p))
                                RetryMigrateProfiles.Add(p);
                        }
                    }




                    //Sequential and batch processing
                    if (RetryMigrateProfiles.Count > 0)
                    {

                        //disctinctProfileIDs = qryReturn.Select(s => s.ProfileID).Distinct();
                        foreach (var p in RetryMigrateProfiles)
                        {

                            List<HistoryGeoLocationBase> sessionsByProfileID = qryReturn.Where(ses => ses.ProfileID == p).ToList();
                            foreach (var session in sessionsByProfileID)
                            {
                                try
                                {
                                    if (session.Identifier.Length > 2)
                                    {
                                        destRecord = ConvertSessionToDest(session);// Use automapper                                                                        
                                        destRecord.ProfileID = MapProfiles[Guid.Parse(destRecord.ProfileID)].ToString();
                                        destRecord.SessionID = session.Identifier.Substring(2);
                                        destRecord.IsSOS = Convert.ToBoolean(Convert.ToInt32(session.Identifier.Substring(0, 1)));
                                        destRecord.Accuracy = 0;
                                        TableOperation insertSession = TableOperation.InsertOrReplace(destRecord);
                                        storageAccessDestination.EntityTable.Execute(insertSession);
                                    }
                                }
                                catch (Exception ex) { opsLogger.WriteLog("HistoryGeoLocation Error Message: " + ex.Message + Environment.NewLine + "ProfileID: " + p); }
                            }

                        }

                    }

                }
                else
                {

                    opsLogger.WriteLog("HistoryGeoLocation Table  not loaded, Please check settings");
                }

            }  //EndIf          
        }

        //Tease
        public void MigrateIncidenttoDest(DateTime LastRunTime)
        {
            TableQuery<SOS.OPsTools.Entities.IncidentBase> UQuery = null;
            if (base.LoadTableSilent(ConstantOps.TeaseSrcTableName))
            {

                UQuery = new TableQuery<SOS.OPsTools.Entities.IncidentBase>().Where(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, LastRunTime));
                var qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();
                Dictionary<Guid, long> MapProfiles = _Repository.GetAllMapProfiles().Result;
                SOS.OPsTools.Entities.IncidentDest destRecord = new IncidentDest();

                List<string> RetryMigrateProfiles = new List<string>();
                
                if (storageAccessDestination.LoadTableSilent(ConstantOps.TeaseDestTableName))
                {
                    //Parallel and batch processing
                    //TableBatchOperation insertSessions = null;
                    var disctinctProfileIDs = qryReturn.Select(s => s.ProfileID).Distinct();
                    MapIncident();

                    //foreach (var p in disctinctProfileIDs)
                    //{
                    //    try
                    //    {
                    //        string newProfileID = MapProfiles[Guid.Parse(p)].ToString();
                    //        insertSessions = new TableBatchOperation();
                    //        List<Entities.IncidentBase> sessionsByProfileID = qryReturn.Where(ses => ses.ProfileID == p).ToList();

                    //        Parallel.ForEach(sessionsByProfileID, session =>
                    //        {

                    //            destRecord = ConvertSessionToDest(session);// Use automapper                                
                    //            destRecord.ProfileID = newProfileID;
                    //            destRecord.MobileNumber = string.Empty;
                    //            destRecord.Name = string.Empty;
                    //            destRecord.ID = destRecord.IncidentID.Substring(destRecord.IncidentID.LastIndexOf("-") + 1);

                    //            TableOperation insertSession = TableOperation.Insert(destRecord);
                    //            insertSessions.Add(insertSession);

                    //        });

                    //        storageAccessDestination.EntityTable.ExecuteBatch(insertSessions);

                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        opsLogger.WriteLog("Tease Error Message: " + ex.Message + Environment.NewLine + "ProfileID: " + p);
                    //        if (!string.IsNullOrEmpty(p))
                    //            RetryMigrateProfiles.Add(p);
                    //    }
                    //}

                    //Sequential and batch processing
                    if (disctinctProfileIDs.ToList().Count > 0)
                    {

                        //disctinctProfileIDs = qryReturn.Select(s => s.ProfileID).Distinct();
                        foreach (var p in disctinctProfileIDs)
                        {

                            List<IncidentBase> sessionsByProfileID = qryReturn.Where(ses => ses.ProfileID == p).ToList();
                            foreach (var session in sessionsByProfileID)
                            {
                                try
                                {

                                    destRecord = ConvertSessionToDest(session);// Use automapper                                    
                                    destRecord.ProfileID = MapProfiles[Guid.Parse(destRecord.ProfileID)].ToString();
                                    destRecord.MobileNumber = string.Empty;
                                    destRecord.Name = string.Empty;
                                    destRecord.ID = destRecord.IncidentID.Substring(destRecord.IncidentID.LastIndexOf("-") + 1);
                                    TableOperation insertSession = TableOperation.InsertOrReplace(destRecord);
                                    storageAccessDestination.EntityTable.Execute(insertSession);
                                    //insertSessions.Add(insertSession);


                                }
                                catch (Exception ex) { opsLogger.WriteLog("Tease Error Message: " + ex.Message + Environment.NewLine + "ProfileID: " + p); }
                            }

                        }
                    }

                }
                else
                {

                    opsLogger.WriteLog("Tease Table  not loaded, Please check settings");
                }


            }  //EndIf          
        }

        //Phone Validation
        public void MigratePhoneValidationtoDest(DateTime LastRunTime)
        {
            TableQuery<SOS.OPsTools.Entities.PhoneValidationBase> UQuery = null;
            if (base.LoadTableSilent(ConstantOps.PhoneValidationSrcTableName))
            {

                UQuery = new TableQuery<SOS.OPsTools.Entities.PhoneValidationBase>().Where(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, LastRunTime));
                var qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();
                SOS.OPsTools.Entities.PhoneValidationDest destRecord = new PhoneValidationDest();

                List<string> RetryMigratePhoneValidation = new List<string>();

                if (storageAccessDestination.LoadTableSilent(ConstantOps.PhoneValidatorDestTableName))
                {
                    //Parallel and batch processing
                    TableBatchOperation insertSessions = null;
                    var disctinctPartitionKeys = qryReturn.Select(s => s.PartitionKey).Distinct();

                    MapPhoneValidation();

                    foreach (var p in disctinctPartitionKeys)
                    {
                        try
                        {
                            insertSessions = new TableBatchOperation();
                            List<Entities.PhoneValidationBase> sessionsByProfileID = qryReturn.Where(ses => ses.PartitionKey == p).ToList();

                            Parallel.ForEach(sessionsByProfileID, session =>
                            {

                                destRecord = ConvertSessionToDest(session);// Use automapper                                                               
                                TableOperation insertSession = TableOperation.Insert(destRecord);
                                insertSessions.Add(insertSession);

                            });

                            storageAccessDestination.EntityTable.ExecuteBatch(insertSessions);

                        }
                        catch (Exception ex)
                        {
                            opsLogger.WriteLog("Phone Validation Error Message: " + ex.Message + Environment.NewLine + "PartitionKey: " + p);
                            if (!string.IsNullOrEmpty(p))
                                RetryMigratePhoneValidation.Add(p);
                        }
                    }

                    //Sequential and batch processing
                    if (RetryMigratePhoneValidation.Count > 0)
                    {

                        foreach (var p in RetryMigratePhoneValidation)
                        {

                            List<PhoneValidationBase> sessionsByProfileID = qryReturn.Where(ses => ses.PartitionKey == p).ToList();
                            foreach (var session in sessionsByProfileID)
                            {
                                try
                                {

                                    destRecord = ConvertSessionToDest(session);// Use automapper                                    
                                    TableOperation insertSession = TableOperation.InsertOrReplace(destRecord);
                                    storageAccessDestination.EntityTable.Execute(insertSession);

                                }
                                catch (Exception ex) { opsLogger.WriteLog("Phone Validation Error Message: " + ex.Message + Environment.NewLine + "PartitionKey: " + p); }
                            }

                        }
                    }

                }
                else
                {

                    opsLogger.WriteLog("Phone Validation  not loaded, Please check settings");
                }


            }  //EndIf          
        }

        //Group
        public void MigrateGrouptoDest(DateTime LastRunTime)
        {
            TableQuery<SOS.OPsTools.Entities.GroupBase> UQuery = null;
            if (base.LoadTableSilent(ConstantOps.GroupSrcTableName))
            {

                UQuery = new TableQuery<SOS.OPsTools.Entities.GroupBase>().Where(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, LastRunTime));
                var qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();
                SOS.OPsTools.Entities.GroupDest destRecord = new GroupDest();

                List<int> RetryMigrateGroupIDs = new List<int>();
                int RetryMigrateGroupID = 0;
                if (storageAccessDestination.LoadTableSilent(ConstantOps.GroupDestTableName))
                {
                    //Parallel and batch processing
                    TableBatchOperation insertSessions = null;
                    var disctinctGroupIDs = qryReturn.Select(s => s.GroupID).Distinct();
                    MapGroup();

                    foreach (var p in disctinctGroupIDs)
                    {
                        try
                        {

                            insertSessions = new TableBatchOperation();
                            List<Entities.GroupBase> sessionsByGroupID = qryReturn.Where(ses => ses.GroupID == p).ToList();

                            Parallel.ForEach(sessionsByGroupID, session =>
                            {

                                destRecord = ConvertSessionToDest(session);// Use automapper                                                                             
                                destRecord.ParentGroupID = 0;
                                destRecord.NotifySubgroups = false;
                                destRecord.ShapeFileID = string.Empty;
                                destRecord.ShowIncidents = false;
                                TableOperation insertSession = TableOperation.Insert(destRecord);
                                insertSessions.Add(insertSession);

                            });

                            storageAccessDestination.EntityTable.ExecuteBatch(insertSessions);

                        }
                        catch (Exception ex)
                        {
                            opsLogger.WriteLog("Group Error Message: " + ex.Message + Environment.NewLine + "GroupID: " + RetryMigrateGroupID);
                            RetryMigrateGroupIDs.Add(p);
                        }

                    }


                    //Sequential and batch processing

                    if (RetryMigrateGroupIDs.Count > 0)
                    {
                        // disctinctProfileIDs = qryReturn.Select(s => s.GroupID).Distinct();
                        foreach (var p in RetryMigrateGroupIDs)
                        {


                            List<GroupBase> sessionsByGroupID = qryReturn.Where(ses => ses.GroupID == p).ToList();
                            foreach (var session in sessionsByGroupID)
                            {
                                try
                                {
                                    destRecord = ConvertSessionToDest(session);// Use automapper                                                                                  
                                    destRecord.ParentGroupID = 0;
                                    destRecord.NotifySubgroups = false;
                                    destRecord.ShapeFileID = string.Empty;
                                    destRecord.ShowIncidents = false;
                                    TableOperation insertSession = TableOperation.InsertOrReplace(destRecord);
                                    storageAccessDestination.EntityTable.Execute(insertSession);

                                }
                                catch (Exception ex) { opsLogger.WriteLog("Group Error Message: " + ex.Message + Environment.NewLine + "GroupID: " + p); }
                            }


                        }//end for

                    }
                }
                else
                {

                    opsLogger.WriteLog("Group Table  not loaded, Please check settings");
                }
            }  //EndIf          
        }
        //GroupAdmins
        public void MigrateGroupAdminstoDest(DateTime LastRunTime)
        {
            TableQuery<SOS.OPsTools.Entities.GroupAdminsBase> UQuery = null;
            if (base.LoadTableSilent(ConstantOps.GroupAdminsSrcTableName))
            {

                UQuery = new TableQuery<SOS.OPsTools.Entities.GroupAdminsBase>().Where(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, LastRunTime));
                var qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

                SOS.OPsTools.Entities.GroupAdminsDest destRecord = new GroupAdminsDest();

                List<int> RetryMigrateAdminIDs = new List<int>();
                

                if (storageAccessDestination.LoadTableSilent(ConstantOps.GroupAdminsDestTableName))
                {
                    //Parallel and batch processing
                    TableBatchOperation insertSessions = null;
                    var disctinctAdminIDs = qryReturn.Select(s => s.AdminID).Distinct();

                    MapGroupAdmin();

                    foreach (var p in disctinctAdminIDs)
                    {
                        try
                        {

                            insertSessions = new TableBatchOperation();
                            List<Entities.GroupAdminsBase> sessionsByAdminID = qryReturn.Where(ses => ses.AdminID == p).ToList();

                            Parallel.ForEach(sessionsByAdminID, session =>
                            {

                                destRecord = ConvertSessionToDest(session);// Use automapper                                                      
                                destRecord.AllowGroupManagement = false;
                                TableOperation insertSession = TableOperation.Insert(destRecord);
                                insertSessions.Add(insertSession);

                            });


                            storageAccessDestination.EntityTable.ExecuteBatch(insertSessions);

                        }
                        catch (Exception ex)
                        {
                            opsLogger.WriteLog("GroupAdmins Error Message: " + ex.Message + Environment.NewLine + "GroupID: " + p);
                            RetryMigrateAdminIDs.Add(p);
                        }

                    }


                    //Sequential and batch processing

                    if (RetryMigrateAdminIDs.Count > 0)
                    {

                        //disctinctProfileIDs = qryReturn.Select(s => s.AdminID).Distinct();
                        foreach (var p in RetryMigrateAdminIDs)
                        {

                            List<GroupAdminsBase> sessionsByAdminID = qryReturn.Where(ses => ses.AdminID == p).ToList();
                            foreach (var session in sessionsByAdminID)
                            {
                                try
                                {
                                    destRecord = ConvertSessionToDest(session);// Use automapper                            
                                    destRecord.AllowGroupManagement = false;
                                    TableOperation insertSession = TableOperation.InsertOrReplace(destRecord);
                                    storageAccessDestination.EntityTable.Execute(insertSession);

                                }
                                catch (Exception ex) { opsLogger.WriteLog("GroupAdmins Error Message: " + ex.Message + Environment.NewLine + "GroupID: " + p); }
                            }

                        }

                    }//end if


                }
                else
                {

                    opsLogger.WriteLog("GroupAdmins Table  not loaded, Please check settings");
                }
            }  //EndIf          
        }

        //GroupMemberValidator
        public void MigrateGroupMemberValidatortoDest(DateTime LastRunTime)
        {
            TableQuery<SOS.OPsTools.Entities.GroupMemberValidatorBase> UQuery = null;
            if (base.LoadTableSilent(ConstantOps.GroupMemberValidatorSrcTableName))
            {

                UQuery = new TableQuery<SOS.OPsTools.Entities.GroupMemberValidatorBase>().Where(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, LastRunTime));
                var qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

                SOS.OPsTools.Entities.GroupMemberValidatorDest destRecord = new GroupMemberValidatorDest();
                Dictionary<Guid, long> MapProfiles = _Repository.GetAllMapProfiles().Result;

                List<string> RetryMigrateProfiles = new List<string>();
                

                if (storageAccessDestination.LoadTableSilent(ConstantOps.GroupMemberValidatorDestTableName))
                {
                    //Parallel and batch processing
                    //TableBatchOperation insertSessions = null;
                    var disctinctProfileIDs = qryReturn.Select(s => s.ProfileID).Distinct();

                    MapGroupMemberValidator();

                    //foreach (var p in disctinctProfileIDs)
                    //{
                    //    try
                    //    {
                    //        string newProfileID = MapProfiles[Guid.Parse(p)].ToString();
                    //        insertSessions = new TableBatchOperation();
                    //        List<Entities.GroupMemberValidatorBase> sessionsByProfileID = qryReturn.Where(ses => ses.ProfileID == p).ToList();

                    //        Parallel.ForEach(sessionsByProfileID, session =>
                    //        {

                    //            destRecord = ConvertSessionToDest(session);// Use automapper                                
                    //            destRecord.ProfileID = newProfileID;
                    //            TableOperation insertSession = TableOperation.Insert(destRecord);
                    //            insertSessions.Add(insertSession);

                    //        });

                    //        storageAccessDestination.EntityTable.ExecuteBatch(insertSessions);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        opsLogger.WriteLog("GroupMemberValidator Error Message: " + ex.Message + Environment.NewLine + "ProfileID: " + p);
                    //        if (!string.IsNullOrEmpty(p)) RetryMigrateProfiles.Add(p);
                    //    }
                    //}


                    //Sequential and batch processing
                    if (disctinctProfileIDs.ToList().Count > 0)
                    {

                        // disctinctProfileIDs = qryReturn.Select(s => s.ProfileID).Distinct();
                        foreach (var p in disctinctProfileIDs)
                        {

                            List<GroupMemberValidatorBase> sessionsByProfileID = qryReturn.Where(ses => ses.ProfileID == p).ToList();
                            foreach (var session in sessionsByProfileID)
                            {
                                try
                                {
                                    destRecord = ConvertSessionToDest(session);// Use automapper                                    
                                    destRecord.ProfileID = MapProfiles[Guid.Parse(destRecord.ProfileID)].ToString();
                                    TableOperation insertSession = TableOperation.InsertOrReplace(destRecord);
                                    storageAccessDestination.EntityTable.Execute(insertSession);
                                }
                                catch (Exception ex) { opsLogger.WriteLog("GroupMemberValidator Error Message: " + ex.Message + Environment.NewLine + "ProfileID: " + p); }
                            }

                        }

                    }


                }
                else
                {

                    opsLogger.WriteLog("GroupMemberValidator Table  not loaded, Please check settings");
                }

            }  //EndIf          
        }

        public List<User> GetAllStorageUsers(DateTime LastRunTime)
        {
            TableQuery<User> UQuery = null;
            List<User> qryReturn = null;
            base.LoadTable(ConstantOps.UserTableName);
            UQuery = new TableQuery<User>().Where(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, LastRunTime));

            qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            return qryReturn;
        }

        public List<Profile> GetAllStorageProfiles(DateTime LastRunTime)
        {
            TableQuery<Profile> UQuery = null;
            List<Profile> qryReturn = null;
            base.LoadTable(ConstantOps.ProfileTableName);
            UQuery = new TableQuery<Profile>().Where(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, LastRunTime));

            qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            return qryReturn;
        }

        public List<Buddy> GetAllStorageBuddies(DateTime LastRunTime)
        {
            TableQuery<Buddy> UQuery = null;
            List<Buddy> qryReturn = null;
            base.LoadTable(ConstantOps.BuddyTableName);
            UQuery = new TableQuery<Buddy>().Where(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, LastRunTime));

            qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            return qryReturn;
        }


        public List<GroupMembership> GetAllStorageGroupMemberships(DateTime LastRunTime)
        {
            TableQuery<GroupMembership> UQuery = null;
            List<GroupMembership> qryReturn = null;
            base.LoadTable(ConstantOps.GroupMembershipTableName);
            UQuery = new TableQuery<GroupMembership>().Where(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, LastRunTime));

            qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            return qryReturn;
        }


        public List<GroupMarshalRelation> GetAllStorageGroupMarshals(DateTime LastRunTime)
        {
            TableQuery<GroupMarshalRelation> UQuery = null;
            List<GroupMarshalRelation> qryReturn = null;
            base.LoadTable(ConstantOps.GroupMarshalRelationTableName);
            UQuery = new TableQuery<GroupMarshalRelation>().Where(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, LastRunTime));

            qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            return qryReturn;
        }

        public List<OPsTools.Entities.Profile> GetProfilesByUserID(string UserID)
        {
            TableQuery<OPsTools.Entities.Profile> UQuery = null;
            List<OPsTools.Entities.Profile> qryReturn = null;

            UQuery = new TableQuery<OPsTools.Entities.Profile>().Where(TableQuery.GenerateFilterCondition("UserID", QueryComparisons.Equal, UserID));

            base.LoadTable(ConstantOps.ProfileTableName);
            qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            return qryReturn;
        }

        public List<OPsTools.Entities.Buddy> GetBuddiesByProfileID(string ProfileID)
        {
            TableQuery<OPsTools.Entities.Buddy> UQuery = null;
            List<OPsTools.Entities.Buddy> qryReturn = null;

            UQuery = new TableQuery<OPsTools.Entities.Buddy>().Where(TableQuery.GenerateFilterCondition("ProfileID", QueryComparisons.Equal, ProfileID));

            base.LoadTable(ConstantOps.BuddyTableName);
            qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            return qryReturn;
        }


    }
}