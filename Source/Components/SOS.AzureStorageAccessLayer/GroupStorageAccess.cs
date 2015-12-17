using System;
using System.Collections.Generic;
using System.Linq;
using SOS.AzureStorageAccessLayer.Entities;
using Microsoft.WindowsAzure.Storage.Table;
using SOS.Service.Utility;

namespace SOS.AzureStorageAccessLayer
{
    public class GroupStorageAccess : StorageAccessBase
    {
        int zeroValue = 0;
        public void CreateGroup(Group grp)
        {
            if (grp != null && !string.IsNullOrEmpty(grp.GroupName) && !string.IsNullOrEmpty(grp.Location))
            {
                if (base.LoadTableSilent(Constants.GroupTableName))
                {
                    TableOperation insertGroup = TableOperation.Insert(grp);
                    base.EntityTable.Execute(insertGroup);
                }
            }
        }

        public void UpdateGroup(Group grp)
        {
            if (grp != null && !string.IsNullOrEmpty(grp.GroupName) && !string.IsNullOrEmpty(grp.Location) && grp.GroupID > 0)
            {
                if (base.LoadTableSilent(Constants.GroupTableName))
                {
                    TableOperation updateOperation = TableOperation.InsertOrReplace(grp);
                    base.EntityTable.Execute(updateOperation);
                }
            }
        }

        public Group GetGroupByID(int groupID)
        {
          
            if (groupID == 0) return null;

           TableQuery<Group> UQuery = new TableQuery<Group>().Where(TableQuery.GenerateFilterCondition(Constants.RowKey, QueryComparisons.Equal, groupID.ToString()));
            base.LoadTable(Constants.GroupTableName);
            List<Group> qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            return qryReturn != null && qryReturn.Count > 0 ? qryReturn.First() : null;
        }


        public void DeleteGroup(int GrpID )
        {
            TableQuery<Group> UQuery = new TableQuery<Group>().Where(TableQuery.GenerateFilterCondition(Constants.RowKey, QueryComparisons.Equal, GrpID.ToString()));          
            base.LoadTable(Constants.GroupTableName);
            try
            {
                Group grp = base.EntityTable.ExecuteQuery(UQuery).FirstOrDefault();
                if (grp != null)
                {
                    TableOperation DeleteOperation = TableOperation.Delete(grp);
                    base.EntityTable.Execute(DeleteOperation);
                }
                TableQuery<AdminUser> AdminQuery = new TableQuery<AdminUser>().Where(TableQuery.GenerateFilterCondition("GroupIDCSV", QueryComparisons.Equal, GrpID.ToString()));
                base.LoadTable(Constants.GroupAdminsTableName);
                AdminUser adminuser = base.EntityTable.ExecuteQuery(AdminQuery).FirstOrDefault();
                if (adminuser != null)
                {
                    TableOperation DeleteOperation = TableOperation.Delete(adminuser);
                    base.EntityTable.Execute(DeleteOperation);
                }
            }
            catch { }
        }


        public Group GetGroupByLocationAndID(int groupID, string Location)
        {
            Group retGroup = null;

            if (groupID == 0 || string.IsNullOrEmpty(Location)) return null;

            TableQuery<Group> UQuery = null;
            List<Group> qryReturn = null;

            string pkFilter = TableQuery.GenerateFilterCondition(Constants.PartitionKey, QueryComparisons.Equal, Location);
            string rkFilter = TableQuery.GenerateFilterCondition(Constants.RowKey, QueryComparisons.Equal, groupID.ToString());
          
            string combinedFilter = TableQuery.CombineFilters(pkFilter, TableOperators.And, rkFilter);            
            UQuery = new TableQuery<Group>().Where(combinedFilter);
            base.LoadTable(Constants.GroupTableName);
            qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            if (qryReturn != null && qryReturn.Count > 0)
                retGroup = qryReturn.First();

            return retGroup;
        }

       
        public Dictionary<int, string> GetAllGroupsWithGroupNames()
        {
            //TODO: Need to check with VR whether to add below condition
            TableQuery<Group> UQuery = new TableQuery<Group>();
            base.LoadTable(Constants.GroupTableName);
            var list = base.EntityTable.CreateQuery<Group>().ToList();
            Dictionary<int, string> qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList()
                                                .Where(x => (!x.ParentGroupID.HasValue || x.ParentGroupID.Value == 0))                                            
                                                .Select(grp => new
                                                {
                                                    GroupID = grp.GroupID,
                                                    GroupName = grp.GroupName

                                                }).ToDictionary(k => k.GroupID, v => v.GroupName);

            return qryReturn != null && qryReturn.Count > 0 ? qryReturn : null;
        }

       

        public Group GetGroupForGroupID( int GroupID )
        {
            if (GroupID == 0)
                return null;

            TableQuery<Group> UQuery = new TableQuery<Group>().Where(TableQuery.GenerateFilterConditionForInt("GroupID", QueryComparisons.Equal, GroupID));

            base.LoadTable(Constants.GroupTableName);

            List<Group> qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            return qryReturn != null && qryReturn.Count > 0 ? qryReturn.First() : null;
        }

        public List<Group> GetGroupsForNameMatch( string nameSmpl )
        {
            if (string.IsNullOrEmpty(nameSmpl))
                return null;

            //TODO: Need to check with VR whether to add below condition
            TableQuery<Group> UQuery = new TableQuery<Group>();

            base.LoadTable(Constants.GroupTableName);
            var ret = base.EntityTable.ExecuteQuery(UQuery).ToList();
            List<Group> qryReturn = ret.Where(x => (!x.ParentGroupID.HasValue || x.ParentGroupID.Value == 0) && x.GroupName.ToUpper().Contains(nameSmpl.ToUpper())).ToList();

            return qryReturn != null && qryReturn.Count > 0 ? qryReturn : null;
        }

        public List<Group> GetAllGroups( bool? RetriveOnlyChilds = false )
        {
            TableQuery<Group> UQuery = new TableQuery<Group>(); 
            LoadTable(Constants.GroupTableName);
            List<Group> qryReturn = new List<Group>();
            if (RetriveOnlyChilds.HasValue)
            {
                if (RetriveOnlyChilds.Value)
                    qryReturn =  base.EntityTable.ExecuteQuery(UQuery).Where(x=>x.ParentGroupID.HasValue && x.ParentGroupID.Value != 0 ).ToList();
                else
                    qryReturn = base.EntityTable.ExecuteQuery(UQuery).Where(x=>(!x.ParentGroupID.HasValue) || (x.ParentGroupID.Value ==  0)).ToList();
            }
            else
            {
               qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();
            }
           return qryReturn != null && qryReturn.Count > 0 ? qryReturn : null;
        }

        public List<AdminUser> GetAllGroupAdmins()
        {
            TableQuery<AdminUser> UQuery = new TableQuery<AdminUser>();
            base.LoadTable(Constants.GroupAdminsTableName);
            List<AdminUser> qryReturn = new List<AdminUser>();
            qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();
            return qryReturn.ToList();
        }



        public AdminUser GetAdminUser(string LUID)
        {
            if (string.IsNullOrEmpty(LUID))
                return null;

            AdminUser retUser = null;
            TableQuery<AdminUser> UQuery = null;
            List<AdminUser> qryReturn = null;

            UQuery = new TableQuery<AdminUser>().Where(
                    TableQuery.GenerateFilterCondition("LiveUserID", QueryComparisons.Equal, LUID.ToString())
                   );

            base.LoadTable(Constants.GroupAdminsTableName);

            qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            if (qryReturn != null && qryReturn.Count > 0)
                retUser = qryReturn.First();

            return retUser;
        }

        public bool AddGroupMemberToValidate(string profileID, string groupID, string notificationIdentity)
        {
            if (string.IsNullOrEmpty(profileID) || string.IsNullOrEmpty(profileID))
                return false;

            GroupMemberValidator member = new GroupMemberValidator();
            member.ProfileID = profileID;
            member.GroupID = Convert.ToInt32(groupID);
            member.ValidationID = TokenManager.GenerateNewValidationID();
            member.NotificationIdentity = notificationIdentity;

            if (base.LoadTableSilent(Constants.GroupMemberValidatorTableName))
            {
                TableOperation insertOperation = TableOperation.InsertOrReplace(member);
                base.EntityTable.Execute(insertOperation);
                return true;
            }
            return false;
        }
        //ssm
        public GroupMemberValidator GetValidateGroupMemberRec(string validationID, string ProfileID)
        {
            GroupMemberValidator ValidatorRec = null;
            TableQuery<GroupMemberValidator> query = null;

            query = new TableQuery<GroupMemberValidator>().Where(TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("ValidationID", QueryComparisons.Equal, validationID),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("ProfileID", QueryComparisons.Equal, ProfileID))
                    );
            try
            {
                if (base.LoadTableSilent(Constants.GroupMemberValidatorTableName))
                    ValidatorRec = base.EntityTable.ExecuteQuery(query).FirstOrDefault();

                return ValidatorRec;
            }
            catch (Exception ex) { }

            return ValidatorRec;
        }

        public bool DeleteGroupMemberValidatorRec(GroupMemberValidator GMV)
        {
            try
            {
                if (base.LoadTableSilent(Constants.GroupMemberValidatorTableName))
                {
                    TableOperation deleteOperation = TableOperation.Delete(GMV);
                    base.EntityTable.Execute(deleteOperation);
                    return true;
                }
            }
            catch (Exception ex) { }

            return false;
        }

        public bool SaveGroupMarshalValidator(GroupMarshalValidator GMV)
        {
            try
            {
                if (GMV != null && !string.IsNullOrEmpty(GMV.ProfileID))
                {
                    if (base.LoadTableSilent(Constants.GroupMarshalValidatorTableName))
                    {
                        TableOperation InsertOperation = TableOperation.InsertOrReplace(GMV);
                        EntityTable.Execute(InsertOperation);
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch { return false; }
        }

        public GroupMarshalValidator GetGroupMarshalValidator(string ValidationID, string ProfileID)
        {
            try
            {
                if (!string.IsNullOrEmpty(ValidationID) && !string.IsNullOrEmpty(ProfileID))
                {
                    if (base.LoadTableSilent(Constants.GroupMarshalValidatorTableName))
                    {
                        TableQuery<GroupMarshalValidator> query = new TableQuery<GroupMarshalValidator>().Where(
                            TableQuery.CombineFilters(
                                TableQuery.GenerateFilterCondition("ProfileID", QueryComparisons.Equal, ProfileID),
                                TableOperators.And,
                                TableQuery.GenerateFilterCondition("ValidationID", QueryComparisons.Equal, ValidationID)));

                        List<GroupMarshalValidator> GMV = base.EntityTable.ExecuteQuery(query).ToList();

                        return GMV != null && GMV.Count() > 0 ? GMV.FirstOrDefault() : null;
                    }
                }
                return null;
            }
            catch { return null; }
        }

        public bool DeleteGroupMarshalValidator(GroupMarshalValidator GMV)
        {
            try
            {
                if (GMV != null && string.IsNullOrEmpty(GMV.ProfileID))
                {
                    if (base.LoadTableSilent(Constants.GroupMarshalValidatorTableName))
                    {
                        TableOperation DeleteOperation = TableOperation.Delete(GMV);
                        EntityTable.Execute(DeleteOperation);
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch { return false; }
        }

        public bool ValidateAdminForGroup(string groupID, string adminID)
        {
            TableQuery<AdminUser> query = null;
            List<AdminUser> adminUsers = null;

            query = new TableQuery<AdminUser>().Where(TableQuery.GenerateFilterConditionForInt("AdminID", QueryComparisons.Equal, Int32.Parse(adminID)));

            base.LoadTable(Constants.GroupAdminsTableName);

            adminUsers = base.EntityTable.ExecuteQuery(query).ToList();

            if (adminUsers != null && adminUsers.Count == 1)
            {
                foreach (var admin in adminUsers)
                {
                    string[] IDs = admin.GroupIDCSV.Split(',');

                    var selectedIDs = IDs.Select(id => id == groupID).ToList();

                    if (selectedIDs != null && selectedIDs.Count > 0)
                        return true;
                }
            }
            return false;
        }

        public void SaveUpdateGroupMemberValidator(GroupMemberValidator GMV)
        {
            GroupMemberValidator GMVRec = new GroupMemberValidator();

            if (base.LoadTableSilent(Constants.GroupMemberValidatorTableName))
            {
                TableOperation insertOperation = TableOperation.InsertOrReplace(GMV);
                base.EntityTable.Execute(insertOperation);

            }
        }

        public List<GroupMemberValidator> GetValidateGroupMembers(string ProfileID)
        {

            List<GroupMemberValidator> ValidatorRec = null;
            TableQuery<GroupMemberValidator> query = null;

            if (base.LoadTableSilent(Constants.GroupMemberValidatorTableName))
            {
                query = new TableQuery<GroupMemberValidator>().Where(
                        TableQuery.GenerateFilterCondition("ProfileID", QueryComparisons.Equal, ProfileID));

                ValidatorRec = base.EntityTable.ExecuteQuery(query).ToList();
            }

            return ValidatorRec;
        }

        public AdminUser GetAdminUserByGroupID(int GroupID)
        {
            if (GroupID <= 0)
                return null;

            TableQuery<AdminUser> UQuery = null;
            List<AdminUser> qryReturn = null;

            UQuery = new TableQuery<AdminUser>().Where(
                    TableQuery.GenerateFilterConditionForInt("AdminID", QueryComparisons.Equal, GroupID)
                   );

            base.LoadTable(Constants.GroupAdminsTableName);

            qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            return qryReturn != null && qryReturn.Count > 0 ? qryReturn.First() : null;

        }
    }
}
