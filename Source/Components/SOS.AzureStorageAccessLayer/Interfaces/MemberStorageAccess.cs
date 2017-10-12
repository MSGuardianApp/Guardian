using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SOS.AzureStorageAccessLayer.Entities;
using utility = SOS.Service.Utility;
using Microsoft.WindowsAzure.Storage.Table;
using SOS.Service.Utility;

namespace SOS.AzureStorageAccessLayer
{
    public class MemberStorageAccess : StorageAccessBase
    {
        public void CreateUserTable()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PhValidator"></param>
        /// <returns>True if created, false if already exisiting</returns>
        public PhoneValidation CreatePhoneValidatorEntry(PhoneValidation PhValidator, ref bool IsReused)
        {
            if (PhValidator != null)
            {
                PhoneValidation phValid = LoadPhoneValidation(PhValidator.AuthenticatedLiveID, PhValidator.PhoneNumber);

                if (phValid == null)
                {
                    PhValidator.PartitionKey = PhValidator.RegionCode.Substring(1);//to avoid +
                    PhValidator.RowKey = PhValidator.SecurityToken.ToString() + "_" + DateTime.Now.Ticks.ToString();
                    PhValidator.PhoneNumber = Security.Encrypt(PhValidator.PhoneNumber);
                    SavePhoneValidationRecord(PhValidator);

                    IsReused = false;

                    PhValidator.PhoneNumber = Security.Decrypt(PhValidator.PhoneNumber);
                    return PhValidator;
                }
                else
                {
                    IsReused = true;
                    phValid.PhoneNumber = Security.Decrypt(phValid.PhoneNumber);
                    return phValid;
                }
            }
            return null;
        }

        public void SavePhoneValidationRecord(PhoneValidation PhValidator)
        {
            base.LoadEntityTable(Constants.PhoneValidationTableName);

            if (base.IsTableLoaded)
            {
                TableOperation insertUser = TableOperation.InsertOrReplace(PhValidator);
                base.EntityTable.Execute(insertUser);
            }
        }

        public PhoneValidation LoadPhoneValidation(string LiveID, string PhoneNumber)
        {
            if (!string.IsNullOrEmpty(LiveID) && !string.IsNullOrEmpty(PhoneNumber))
            {

                TableQuery<PhoneValidation> UQuery = null;
                List<PhoneValidation> qryReturn = null;
                string encryptedNumber = Security.Encrypt(PhoneNumber);

                UQuery = new TableQuery<PhoneValidation>().Where(TableQuery.CombineFilters(
                       TableQuery.GenerateFilterCondition("PhoneNumber", QueryComparisons.Equal, encryptedNumber),
                       TableOperators.And,
                        TableQuery.GenerateFilterCondition("AuthenticatedLiveID", QueryComparisons.Equal, LiveID)
                       ));

                base.LoadTable(Constants.PhoneValidationTableName);
                qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

                return (qryReturn != null && qryReturn.Count > 0) ? qryReturn.FirstOrDefault() : null;
            }
            return null;
        }

        public bool DeletePhoneValidationEntry(PhoneValidation PhValidationToDelete)
        {
            bool DeleteState = false;
            if (PhValidationToDelete != null && PhValidationToDelete.IsValiated)
            {
                if (base.LoadTableSilent(Constants.PhoneValidationTableName))
                {
                    TableOperation deleteOperation = TableOperation.Delete(PhValidationToDelete);
                    base.EntityTable.Execute(deleteOperation);
                    DeleteState = true;
                }
            }
            return DeleteState;
        }


        public void SaveGroupAdmin(AdminUser grpAdmin)
        {
            if (grpAdmin != null && !string.IsNullOrEmpty(grpAdmin.AdminID.ToString()) && !string.IsNullOrEmpty(grpAdmin.GroupIDCSV))
            {
                if (base.LoadTableSilent(Constants.GroupAdminsTableName))
                {
                    TableOperation SaveGroupAdmin = TableOperation.InsertOrReplace(grpAdmin);
                    base.EntityTable.Execute(SaveGroupAdmin);
                }
            }
        }



        public List<PhoneValidation> GetAllPhoneValidationData()
        {
            TableQuery<PhoneValidation> UQuery = new TableQuery<PhoneValidation>();

            if (!base.LoadTableSilent(Constants.PhoneValidationTableName)) return null;

            return base.EntityTable.ExecuteQuery(UQuery).ToList();
        }

        public int PhoneValidationCount()
        {
            var PhoneValidationData = GetAllPhoneValidationData();
            if (PhoneValidationData != null)
                return PhoneValidationData.Count();
            else
                return 0;
        }

        public void DeleteHistoryForProfile(string ProfileID)
        {
            if (ProfileID != null)
            {
                List<Task> tasks = new List<Task>();
                Task locationHistoryTask = DeleteLocationHistoryForProfileAsync(ProfileID);
                Task sessionHistoryTask = DeleteSessionHistoryForProfileAsync(ProfileID);
                tasks.Add(locationHistoryTask);
                tasks.Add(sessionHistoryTask);
                Task.WaitAll(tasks.ToArray());
            }
        }

        public async Task DeleteLocationHistoryForProfileAsync(string ProfileID)
        {
            if (ProfileID != null)
            {
                base.LoadTable(Constants.LocationHistoryTableName);

                TableQuery<LocationHistory> UQuery = null;
                UQuery = new TableQuery<LocationHistory>().Where(TableQuery.GenerateFilterCondition(Constants.PartitionKey, QueryComparisons.Equal, ProfileID));
                List<LocationHistory> historyData = null;
                historyData = base.EntityTable.ExecuteQuery(UQuery).ToList();

                foreach (var item in historyData)
                {
                    TableOperation updateOperation = TableOperation.Delete(item);
                    await base.EntityTable.ExecuteAsync(updateOperation);
                }
            }
        }

        public async Task DeleteSessionHistoryForProfileAsync(string ProfileID)
        {
            if (ProfileID != null)
            {
                base.LoadTable(Constants.SessionHistoryTableName);

                TableQuery<SessionHistory> UQuery = null;
                UQuery = new TableQuery<SessionHistory>().Where(TableQuery.GenerateFilterCondition(Constants.PartitionKey, QueryComparisons.Equal, ProfileID));
                List<SessionHistory> historyData = null;
                historyData = base.EntityTable.ExecuteQuery(UQuery).ToList();

                foreach (var item in historyData)
                {
                    TableOperation updateOperation = TableOperation.Delete(item);
                    await base.EntityTable.ExecuteAsync(updateOperation);
                }
            }
        }

        #region Methods for OPS tools

        public List<PhoneValidation> GetAllPendingVerificationProfiles(int days)
        {
            DateTime dt = DateTime.Now.AddDays(-1 * days);
            TableQuery<PhoneValidation> UQuery = new TableQuery<PhoneValidation>().Where(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, dt));

            base.LoadTable(Constants.PhoneValidationTableName);
            List<PhoneValidation> qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            qryReturn = (from item in qryReturn
                         orderby item.Timestamp descending
                         select item).ToList();

            return qryReturn;
        }

        public List<object> GetSecurityTokenByNumber(string number)
        {
            TableQuery<PhoneValidation> UQuery = new TableQuery<PhoneValidation>().Where(TableQuery.GenerateFilterCondition("PhoneNumber", QueryComparisons.Equal, Security.Encrypt(number)));

            base.LoadTable(Constants.PhoneValidationTableName);
            List<PhoneValidation> qryReturn = base.EntityTable.ExecuteQuery(UQuery).ToList();

            var result = (from item in qryReturn
                          orderby item.Timestamp descending
                          select new 
                             {
                                 Name = item.Name,
                                 PhoneNumber = number,
                                 SecurityToken = item.SecurityToken,
                                 RegisteredDate = item.Timestamp.AddHours(5).AddMinutes(30)

                             }).ToList<object>();

            return (result);
        }

        #endregion
    }

}
