using Guardian.Common.Configuration;
using SOS.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;


namespace SOS.AzureSQLAccessLayer
{
    public class ReportRepository : IReportRepository
    {
        readonly GuardianContext _guardianContext;
        readonly IConfigManager configManager;

        public ReportRepository(IConfigManager configManager)
            : this(new GuardianContext(configManager.Settings.AzureSQLConnectionString))
        {
            this.configManager = configManager;
        }

        public ReportRepository(GuardianContext guardianContext)
        {
            if (guardianContext == null)
            {
                throw new ArgumentException("guardianContext");
            }
            _guardianContext = guardianContext;
        }


        public async Task<string> GetUserNameForProfileID(string profileID)
        {
            return await (from usr in _guardianContext.Users
                          join prf in _guardianContext.Profiles on usr.UserID equals prf.UserID
                          where prf.ProfileID == Convert.ToInt64(profileID)
                          select usr.Name).FirstOrDefaultAsync();

            //Method2
            //await _sqlAzureExecutionStrategy.ExecuteAsync(async () =>
            //{
            //    int result = await _guardianContext.Database
            //        .ExecuteSqlCommandAsync("SELECT [dbo].[[UserInfo]] @profileList",
            //            new SqlParameter("@profileList", profileList));
            //}, CancellationToken.None);
        }

        #region "Sync DB Calls for Reports"
        //we have made this method as sync to work for report 
        public async Task<Dictionary<long, Tuple<long, string>>> GetAllUserwithProfileData(string profileList)
        {
            List<string> ProfileList = profileList.Split(new char[] { ',' }).ToList();

            return (from usr in _guardianContext.Users
                    join prf in _guardianContext.Profiles on usr.UserID equals prf.UserID
                    where ProfileList.Contains(prf.ProfileID.ToString())
                    select new
                    {
                        UserID = usr.UserID,
                        UserAttributes = usr.Name + "," + usr.Email + "," + usr.MobileNumber + "," + usr.FBAuthID,
                        ProfileID = prf.ProfileID

                    }).AsNoTracking().ToDictionary(k => k.UserID, v => Tuple.Create<long, string>(v.ProfileID, v.UserAttributes));

        }

        //we have made this method as sync to work for report 
        public async Task<Dictionary<long, string>> GetAllUserIDWithAttributes()
        {

            return _guardianContext.Users
                                .Select(userRow => new
                                {
                                    UserID = userRow.UserID,
                                    UserAttributes = userRow.Name + "," + userRow.Email + "," + userRow.LiveID + "," + userRow.FBAuthID

                                }).AsNoTracking().ToDictionary(k => k.UserID, v => v.UserAttributes);

        }
        //we have made this method as sync to work for report 
        public async Task<Dictionary<long, long>> GetAllProfileIDWithUserID()
        {

            try
            {
                return _guardianContext.Profiles
                                .Select(profileRow => new
                                {
                                    ProfileID = profileRow.ProfileID,
                                    UserID = profileRow.UserID

                                }).ToDictionary(k => k.ProfileID, v => v.UserID);
            }

            catch (Exception ex) { return null; }

        }
        //we have made this method as sync to work for report 
        public async Task<Dictionary<long, int>> GetAllProfileIDWithCount()
        {

            return _guardianContext.Buddies
                                             .GroupBy(bdy => bdy.ProfileID)
                                .Select(bdyRow => new
                                {
                                    ProfileID = bdyRow.Key,
                                    ProfileCount = bdyRow.Count()

                                }).AsNoTracking().ToDictionary(k => k.ProfileID, v => v.ProfileCount);




        }

        //we have made this method as sync to work for report 
        public async Task<Dictionary<long, int>> GetAllProfileIDWithCountFromGrpMemShip()
        {

            return _guardianContext.GroupMemberships.Where(grp => grp.IsValidated)
                                             .GroupBy(grp => grp.ProfileID)
                                .Select(grpRow => new
                                {
                                    ProfileID = grpRow.Key,
                                    ProfileCount = grpRow.Count()

                                }).AsNoTracking().ToDictionary(k => k.ProfileID, v => v.ProfileCount);


        }

        public Dictionary<long, Tuple<string, string>> GetBasicProfileInfo(int startID, int endID)
        {

            return (from usr in _guardianContext.Users
                    join prf in _guardianContext.Profiles on usr.UserID equals prf.UserID
                    where prf.ProfileID >= startID && prf.ProfileID <= endID && usr.Email != "invalid@invalid.com"
                    select new
                    {
                        Name = usr.Name,
                        Email = usr.Email,
                        ProfileID = prf.ProfileID

                    }).AsNoTracking().ToDictionary(k => k.ProfileID, v => Tuple.Create<string, string>(v.Email, v.Name));

        }

        #endregion

        #region Dispose Section
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _guardianContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
