using SOS.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SOS.Service.Utility;
using SOS.AzureSQLAccessLayer.Entities;

namespace SOS.AzureSQLAccessLayer
{
    public class MemberRepository : BaseRepository, IMemberRepository
    {
        private readonly GuardianContext _guardianContext;

        public MemberRepository()
            : this(new GuardianContext())
        {
        }

        public MemberRepository(GuardianContext guardianContext)
        {
            if (guardianContext == null)
            {
                throw new ArgumentException("guardianContext");
            }
            _guardianContext = guardianContext;
        }

        public async Task<User> GetUserByMailIDAsync(string EmailID)
        {
            return await _guardianContext.Users
                .Where(w => w.Email == EmailID && w.Email != Constants.InvalidEmail)
                .AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByMobileAsync(string Mobile)
        {
            string encryptedMobile = Security.Encrypt(Mobile);

            return await _guardianContext.Users
                .Where(w => w.MobileNumber == encryptedMobile && w.MobileNumber != Constants.InvalidMobileNumber)
                .AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUserIDAsync(long UserID)
        {
            return await _guardianContext.Users
                        .Where(w => w.UserID == UserID)
                        .AsNoTracking().FirstOrDefaultAsync<User>();
        }

        public async Task<User> GetUserByProfileID(string profileId)
        {
            return await (from usr in _guardianContext.Users
                          join prf in _guardianContext.Profiles on usr.UserID equals prf.UserID
                          where prf.ProfileID == Convert.ToInt64(profileId)
                          select usr).AsNoTracking().FirstOrDefaultAsync<User>();
        }

        public async Task<List<Buddy>> GetBuddiesForProfileIDAsync(long ProfileID)
        {
            return await _guardianContext.Buddies
                .Where(w => w.ProfileID == ProfileID)
                .AsNoTracking().OrderBy(b => b.BuddyName).ToListAsync<Buddy>();
        }

        public async Task<List<Buddy>> GetBuddyProfilesForUserIDAsync(long UserID)
        {
            return await _guardianContext.Buddies
                        .Where(w => w.UserID == UserID)
                        .AsNoTracking().ToListAsync<Buddy>();
        }

        public async Task<Profile> GetProfileAsync(long ProfileID)
        {
            return await _guardianContext.Profiles
                .Include("User")
                .Where(w => w.ProfileID == ProfileID)
                .AsNoTracking().FirstOrDefaultAsync<Profile>();
        }

        public async Task<Profile> GetProfileByMobileAsync(string Mobile)
        {
            string encryptedMobile = Security.Encrypt(Mobile);

            return await _guardianContext.Profiles
                   .Where(w => w.MobileNumber == encryptedMobile && w.MobileNumber != Constants.InvalidMobileNumber)
                   .AsNoTracking().FirstOrDefaultAsync<Profile>();
        }

        public async Task<List<Profile>> GetProfilesForUserIDAsync(long UserID)
        {
            return await _guardianContext.Profiles
                 .Where(w => w.UserID == UserID && w.IsValid == true)
                 .AsNoTracking().ToListAsync();
        }

        public async Task<HealthUpdate> GetPendingUpdatesAsync(long ProfileID)
        {
            HealthUpdate pendingUpdate = new HealthUpdate();

            pendingUpdate.IsGroupModified = false;
            pendingUpdate.IsProfileActive = false;

            Profile updatedProfile = null;
            GroupMembership updatedGroup = null;

            updatedProfile = _guardianContext.Profiles
               .Where(w => w.ProfileID == ProfileID)
               .AsNoTracking().FirstOrDefault();

            updatedGroup = await _guardianContext.GroupMemberships
               .Where(w => w.ProfileID == ProfileID)
               .AsNoTracking().FirstOrDefaultAsync<GroupMembership>();

            if (updatedProfile != null)
                pendingUpdate.IsProfileActive = updatedProfile.IsValid;

            if (updatedGroup != null)
                pendingUpdate.IsGroupModified = true;

            return pendingUpdate;
        }



        public async Task<bool> AddBuddyAsync(Buddy buddy)
        {
            buddy.CreatedDate = DateTime.UtcNow;
            buddy.LastModifiedDate = DateTime.UtcNow;
            _guardianContext.Buddies.Add(buddy);
            await _guardianContext.SaveChangesAsync();
            return true;
        }

        public async Task SavePrimeBuddyAsync(long BuddyID, long ProfileID)
        {
            int result = await _guardianContext.Database
               .ExecuteSqlCommandAsync("EXEC [dbo].[UpdatePrimeBuddy] @BuddyID, @ProfileID",
                   new SqlParameter("@BuddyID", BuddyID),
                   new SqlParameter("@ProfileID", ProfileID));
        }


        public async Task SaveOrUpdateProfileAsync(Profile profile)
        {
            if (profile.ProfileID != 0)
            {
                profile.User = null;//as user already added and in this scenario trying to attach again, hence exception
                profile.LastModifiedDate = DateTime.UtcNow;
                _guardianContext.Entry<Profile>(profile).State = EntityState.Modified;
                await _guardianContext.SaveChangesAsync();
            }
            else
            {
                profile.CreatedDate = DateTime.UtcNow;
                profile.LastModifiedDate = DateTime.UtcNow;
                _guardianContext.Profiles.Add(profile);
                await _guardianContext.SaveChangesAsync();
            }
        }

        public async Task SaveUserAsync(User user)
        {
            if (user.UserID != 0)
            {
                user.LastModifiedDate = DateTime.UtcNow;
                _guardianContext.Entry<User>(user).State = EntityState.Modified;
                await _guardianContext.SaveChangesAsync();
            }
            else
            {
                user.CreatedDate = DateTime.UtcNow;
                user.LastModifiedDate = DateTime.UtcNow;
                _guardianContext.Users.Add(user);
                await _guardianContext.SaveChangesAsync();
            }
        }

        public async Task<bool> IsProfileValidAsync(long ProfileID)
        {
            int profilesCount = await _guardianContext.Profiles
                    .Where(w => w.ProfileID == ProfileID)
                    .AsNoTracking().CountAsync();

            return (profilesCount > 0) ? true : false;
        }

        public async Task<User> ValidateAndGetUserAsync(string LiveUserID)
        {
            return await _guardianContext.Users
                .Where(w => w.LiveID == LiveUserID && w.Email != Constants.InvalidEmail && w.MobileNumber != Constants.InvalidMobileNumber)
                .FirstOrDefaultAsync();
        }

        public async Task<string> GetProfileUserIDByLiveIDAsync(string LiveUserID)
        {
            var userdetails = await (from usr in _guardianContext.Users
                                     join prf in _guardianContext.Profiles on usr.UserID equals prf.UserID
                                     where usr.LiveID == LiveUserID && usr.Email != Constants.InvalidEmail && usr.MobileNumber != Constants.InvalidMobileNumber
                                     select new
                                     {
                                         UserID = usr.UserID,
                                         ProfileID = prf.ProfileID
                                     }).FirstOrDefaultAsync();

            if (userdetails != null)
                return userdetails.UserID.ToString() + "," + userdetails.ProfileID.ToString();
            else
                return null;
        }

        public async Task<bool> ValidateUser(string LiveUserID)
        {
            var result = await _guardianContext.Users
                .Where(w => w.LiveID == LiveUserID && w.Email != Constants.InvalidEmail && w.MobileNumber != Constants.InvalidMobileNumber)
                .FirstOrDefaultAsync();
            return (result == null) ? false : true;
        }

        public async Task DeleteWhileUnregisterUserAsync(long ProfileID)
        {
            int res = await _guardianContext.Database
                .ExecuteSqlCommandAsync("EXEC [dbo].[DeleteWhileUnregisterUser] @ProfileID",
                    new SqlParameter("@ProfileID", ProfileID));
        }

        public async Task RemoveBuddyRelationAsync(long ProfileID, long BuddyUserID)
        {
            int result = await _guardianContext.Database
                .ExecuteSqlCommandAsync("EXEC [dbo].[RemoveBuddyRelation] @ProfileID,@BuddyUserID",
                    new SqlParameter("@ProfileID", ProfileID),
                    new SqlParameter("@BuddyUserID", BuddyUserID));
        }


        public async Task<User> SubscribeBuddyForProfileActionAsync(long ProfileID, long UserID, int State, string SubscribtionID)
        {
            MiniUser resMiniUser = _guardianContext.Database
                    .SqlQuery<MiniUser>("EXEC [dbo].[SubscribeBuddyForProfileAction] @ProfileID,@UserID,@State,@SubscribtionID",
                    new SqlParameter("@ProfileID", ProfileID),
                    new SqlParameter("@UserID", UserID),
                    new SqlParameter("@State", State),
                    new SqlParameter("@SubscribtionID", SubscribtionID)).FirstOrDefault();

            User resUser = new User
                    {
                        Name = resMiniUser.Name,
                        MobileNumber = Security.Decrypt(resMiniUser.MobileNumber)
                    };

            return resUser;
        }

        public async Task<int> GetMembersCountForGroup(int GroupID)
        {
            return (await _guardianContext.GroupMemberships
                            .Where(w => w.GroupID == GroupID && w.IsValidated == true)
                            .AsNoTracking().ToListAsync<GroupMembership>()).Count;
        }

        public async Task<int> GetLocateBuddiesCountForUser(long UserID)
        {
            return (await _guardianContext.Buddies
               .Where(w => w.UserID == UserID)
               .AsNoTracking().ToListAsync<Buddy>()).Count;
        }

        public async Task<List<LiveUserStatus>> GetFilteredGroupMembers(int groupID, string searchName)
        {
            return await (from grp in _guardianContext.GroupMemberships
                          join prf in _guardianContext.Profiles on grp.ProfileID equals prf.ProfileID
                          where grp.GroupID == groupID && grp.UserName.StartsWith(searchName)
                          select new LiveUserStatus
                          {
                              ProfileID = grp.ProfileID,
                              Name = grp.UserName,
                              MobileNumber = prf.MobileNumber,
                              Status = 0
                          }).OrderByDescending(o => o.Name).AsNoTracking().Take(50).ToListAsync();
        }

        public async Task<List<LiveUserStatus>> GetFilteredLocateBuddies(long UserID, string searchName)
        {
            return await (from lbdy in _guardianContext.LocateBuddiesViews
                          where lbdy.UserID == UserID && lbdy.Name.StartsWith(searchName)
                          select new LiveUserStatus
                          {
                              ProfileID = lbdy.ProfileID,
                              Name = lbdy.Name,
                              MobileNumber = lbdy.MobileNumber,
                              Status = 0
                          }).OrderByDescending(o => o.Name).AsNoTracking().Take(50).ToListAsync();
        }


        public async Task<List<Buddy>> GetMarshalBuddiesByGroupID(int GroupID)
        {
            var buddies = await (from gm in _guardianContext.GroupMarshals
                                 join gmp in _guardianContext.Profiles on gm.ProfileID equals gmp.ProfileID
                                 join bdy in _guardianContext.Buddies on gmp.UserID equals bdy.UserID
                                 join usrprf in _guardianContext.Profiles on bdy.ProfileID equals usrprf.ProfileID
                                 join usrdetails in _guardianContext.Users on usrprf.UserID equals usrdetails.UserID
                                 where gm.GroupID == GroupID && bdy.State == BuddyState.Marshal
                                 select new
                                 {
                                     BuddyID = bdy.BuddyID,
                                     ProfileID = bdy.ProfileID,
                                     BuddyName = bdy.BuddyName,
                                     MobileNumber = bdy.MobileNumber,
                                     Email = bdy.Email,
                                     BuddyUserID = bdy.UserID,
                                     UserName = usrdetails.Name,
                                     UserID = usrdetails.UserID
                                 }).AsNoTracking().ToListAsync();

            return buddies.Select(bdy =>
                         new Buddy
                         {
                             BuddyID = bdy.BuddyID,
                             ProfileID = bdy.ProfileID,
                             BuddyName = bdy.BuddyName,
                             MobileNumber = bdy.MobileNumber,
                             Email = bdy.Email,
                             UserID = bdy.BuddyUserID,
                             User = new User
                             {
                                 UserID = bdy.UserID,
                                 Name = bdy.UserName
                             }
                         }).ToList();
        }

        public async Task CreateBuddy(Buddy buddy)
        {
            int result = await _guardianContext.Database
               .ExecuteSqlCommandAsync("EXEC [dbo].[CreateBuddy] @ProfileID,@Name,@Email,@MobileNumber,@IsPrimeBuddy,@State",
                   new SqlParameter("@ProfileID", buddy.ProfileID),
                   new SqlParameter("@Name", buddy.BuddyName),
                   new SqlParameter("@Email", buddy.Email),
                   new SqlParameter("@MobileNumber", buddy.MobileNumber),
                   new SqlParameter("@IsPrimeBuddy", buddy.IsPrimeBuddy),
                   new SqlParameter("@State", buddy.State));
        }

        public async Task<long> ManageUser(User user)
        {
            long userId = await _guardianContext.Database
                    .SqlQuery<long>("EXEC [dbo].[ManageProfileUser] @Name,@Email,@MobileNumber,@FBAuthID,@FBID,@LiveID",
                   new SqlParameter("@Name", user.Name),
                   new SqlParameter("@Email", user.Email),
                   new SqlParameter("@MobileNumber", user.MobileNumber),
                   new SqlParameter("@FBAuthID", (String.IsNullOrEmpty(user.FBAuthID) ? DBNull.Value.ToString() : user.FBAuthID)),
                   new SqlParameter("@FBID", (String.IsNullOrEmpty(user.FBID) ? DBNull.Value.ToString() : user.FBID)),
                   new SqlParameter("@LiveID", (String.IsNullOrEmpty(user.LiveID) ? DBNull.Value.ToString() : user.LiveID))).FirstOrDefaultAsync();

            return userId;
        }

        public async Task<bool> SaveDispatchInfo(long ProfileID, string SessionID, string DispatchInfo)
        {
            int result = await _guardianContext.Database
               .ExecuteSqlCommandAsync("EXEC [dbo].[UpdateDispatchInfo] @ProfileID, @SessionID, @DispatchInfo",
                   new SqlParameter("@ProfileID", ProfileID),
                   new SqlParameter("@SessionID", SessionID),
                   new SqlParameter("@DispatchInfo", DispatchInfo));

            return (result > 0) ? true : false;
        }

        public async Task UpdateProfileMap(long UserID)
        {
            int result = await _guardianContext.Database
               .ExecuteSqlCommandAsync("EXEC [dbo].[UpdateProfileMap] @UserID",
                   new SqlParameter("@UserID", UserID));
        }

        #region "Sync DB Calls for Reports"
        //we have made this method to work as sync  for report 

        public async Task<List<GroupMembership>> GetAllProfilesAssociatedToGroupAsync(int igroupID)
        {

            return _guardianContext.GroupMemberships
                  .Where(w => w.GroupID == igroupID && w.IsValidated == true)
                  .AsNoTracking().ToList<GroupMembership>();
        }
        //we have made this method to work as sync  for report 
        public async Task<List<Member>> GetMembersByGroupID(int groupID, string searchName, string startDate, string endDate)
        {
            DateTime sDate = DateTime.MinValue;
            DateTime eDate = DateTime.UtcNow;
            if (!string.IsNullOrWhiteSpace(startDate))
                DateTime.TryParse(startDate, out sDate);
            if (!string.IsNullOrWhiteSpace(endDate))
                DateTime.TryParse(endDate, out eDate);

            return (from grp in _guardianContext.GroupMemberships
                    join prf in _guardianContext.Profiles on grp.ProfileID equals prf.ProfileID
                    join usr in _guardianContext.Users on prf.UserID equals usr.UserID
                    where grp.GroupID == groupID && usr.Name.StartsWith(searchName) && usr.CreatedDate >= sDate && usr.CreatedDate <= eDate
                    select new Member
                    {
                        ProfileID = grp.ProfileID,
                        Name = usr.Name,
                        MobileNumber = prf.MobileNumber,
                        Email = usr.Email,
                        EnterpriseEmail = grp.EnrollmentKeyValue ?? prf.EnterpriseEmailID,
                        CreateDate = usr.CreatedDate
                    }).OrderBy(o => o.Name).AsNoTracking().ToList();
        }

        //we have made this method to work as sync  for report  
        public async Task<int> GetAllUserPorfilesCount()
        {

            return _guardianContext.Profiles.AsNoTracking().Count();
        }

        #endregion

        #region Methods for OPS tools

        public List<Profile> GetAllProfilesOutsideIndia()
        {
            throw new NotImplementedException();

            List<Profile> profiles = null;//Get from LINQ
            List<Profile> resultProfiles = new List<Profile>();
            foreach (var profile in profiles)
            {
                var number = Security.Decrypt(profile.MobileNumber);
                if (!number.StartsWith("+91"))
                {
                    profile.MobileNumber = number;
                    resultProfiles.Add(profile);
                }
            }
            return resultProfiles;
        }
        public List<Profile> GetAllProfilesByRegionCode(string regionCode)
        {
            throw new NotImplementedException();

            List<Profile> profiles = null;//Get from LINQ
            List<Profile> resultProfiles = new List<Profile>();
            foreach (var profile in profiles)
            {
                var number = Security.Decrypt(profile.MobileNumber);
                if (number.StartsWith(regionCode))
                {
                    profile.MobileNumber = number;
                    resultProfiles.Add(profile);
                }
            }
            return resultProfiles;
        }

        public List<Buddy> GetAllBuddiesOutsideIndia()
        {
            throw new NotImplementedException();
            List<Buddy> qryReturn = null;
            List<Buddy> resultBuddies = new List<Buddy>();
            foreach (var buddy in qryReturn)
            {
                var number = Security.Decrypt(buddy.MobileNumber);
                if (!number.StartsWith("+91"))
                {
                    buddy.MobileNumber = number;
                    resultBuddies.Add(buddy);
                }
            }
            return resultBuddies;
        }

        /// <summary>
        /// Tool: Update buddies who have missing Country code in their Mobile Number with +91
        /// </summary>
        public void UpdateBuddiesWithNoCountryCode()
        {
            throw new NotImplementedException();
            List<Buddy> qryReturn = null;
            foreach (var buddy in qryReturn)
            {
                var number = Security.Decrypt(buddy.MobileNumber);
                if (!number.StartsWith("+") && number.Length == 10)
                {
                    buddy.MobileNumber = Security.Encrypt("+91" + number);
                    //AddBuddy(buddy);
                }
            }
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
