using SOS.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SOS.Service.Utility;
using SOS.AzureSQLAccessLayer.Entities;
using OpstoolEntity = SOS.OPsTools.Entities;
using SOS.AzureSQLAccessLayer;
namespace SOS.OPsTools
{
    public class MainRepository : BaseRepository, IMemberRepository
    {
        private readonly GuardianContext _guardianContext;
        private readonly SqlAzureExecutionStrategy _sqlAzureExecutionStrategy = new SqlAzureExecutionStrategy(3, TimeSpan.FromMilliseconds(500));

        public MainRepository()
            : this(new GuardianContext())
        {
        }

        public MainRepository(GuardianContext guardianContext)
        {
            if (guardianContext == null)
            {
                throw new ArgumentException("guardianContext");
            }
            _guardianContext = guardianContext;
        }


         public async Task InsertUserIDToSQLStorage(string UserID)
        {           
            await _sqlAzureExecutionStrategy.ExecuteAsync(async () =>
            {
                 await _guardianContext.Database
                    .SqlQuery<OpstoolEntity.User>("EXEC [dbo].[InsertUserIDToSQLStorage] @UserID",                   
                    new SqlParameter("@UserID", UserID)).FirstOrDefaultAsync();             

            }, CancellationToken.None);            
        }


         public async Task InsertUserToSQLUserTable(OpstoolEntity.User User)
        {
                    
               await _sqlAzureExecutionStrategy.ExecuteAsync(async () =>
               {
                   _guardianContext.Database
                     .ExecuteSqlCommand("EXEC [migrate].[InsertUserToSQLUserTable] @UserID,@Name,@Email,@MobileNumber,@FBAuthID,@FBID,@LiveID,@CreatedDate",
                          new SqlParameter("@UserID", User.UserID.OrDbNull()),
                          new SqlParameter("@Name", User.Name.OrDbNull()),
                          new SqlParameter("@Email", User.Email.OrDbNull()),
                          new SqlParameter("@MobileNumber", User.MobileNumber.OrDbNull()),
                          new SqlParameter("@FBAuthID", User.FBAuthID.OrDbNull()),
                          new SqlParameter("@FBID", User.FBID.OrDbNull()),
                          new SqlParameter("@LiveID", User.LiveID.OrDbNull()),
                          new SqlParameter("@CreatedDate", User.Timestamp));
               }, CancellationToken.None);                              
        }


        public async Task InsertProfileToSQLProfileTable(OpstoolEntity.Profile Profile, long UserID)
        {

            await _sqlAzureExecutionStrategy.ExecuteAsync(async () =>
            {
                 _guardianContext.Database
                   .ExecuteSqlCommand("EXEC [migrate].[InsertProfileToSQLProfileTable] @ProfileID,@UserID,@MobileNumber,@RegionCode,@CanPost,@CanSMS,@CanEmail,@SecurityToken,@LocationConsent,@IsValid,@CreatedDate",
                        new SqlParameter("@ProfileID", Profile.ProfileID.OrDbNull()),
                        new SqlParameter("@UserID", UserID),
                        new SqlParameter("@MobileNumber", Profile.MobileNumber.OrDbNull()),
                        new SqlParameter("@RegionCode", Profile.RegionCode.OrDbNull()),
                        new SqlParameter("@CanPost", Profile.CanPost),
                        new SqlParameter("@CanSMS", Profile.CanSMS),
                        new SqlParameter("@CanEmail", Profile.CanEmail),
                        new SqlParameter("@LocationConsent", Profile.LocationConsent),
                        new SqlParameter("@SecurityToken", Profile.SecurityToken.OrDbNull()),
                        new SqlParameter("@IsValid", Profile.IsValid),
                        new SqlParameter("@CreatedDate", Profile.Timestamp));
            }, CancellationToken.None);                              
        }

        public async Task InsertProfileToSQLBuddyTable(OpstoolEntity.Buddy buddy, long UserID, long ProfileID)
        {
            await _sqlAzureExecutionStrategy.ExecuteAsync(async () =>
            {
                 _guardianContext.Database
                   .ExecuteSqlCommand("EXEC [migrate].[InsertBuddyToSQLBuddyTable] @ProfileID,@UserID,@BuddyName,@MobileNumber,@Email,@IsPrimeBuddy,@State,@CreatedDate",                        
                        new SqlParameter("@ProfileID", ProfileID),
                        new SqlParameter("@UserID", UserID),
                        new SqlParameter("@BuddyName", buddy.BuddyName.OrDbNull()),
                        new SqlParameter("@MobileNumber", buddy.MobileNumber.OrDbNull()),
                        new SqlParameter("@Email", buddy.Email.OrDbNull()),
                        new SqlParameter("@IsPrimeBuddy", buddy.IsPrimeBuddy),
                        new SqlParameter("@State", Convert.ToInt32(buddy.State)),
                        new SqlParameter("@CreatedDate", buddy.Timestamp));
                   }, CancellationToken.None);  
           
        }

        public async Task InsertProfileToSQGroupMembershipTable(OpstoolEntity.GroupMembership group, long ProfileID)
        {

            await _sqlAzureExecutionStrategy.ExecuteAsync(async () =>
            {
                 _guardianContext.Database
                  .ExecuteSqlCommand("EXEC [migrate].[InsertProfileToSQLGroupMembershipTable] @GroupID,@ProfileID,@UserName,@EnrollmentKeyValue,@IsValidated,@CreatedDate",

                        new SqlParameter("@GroupID", Convert.ToInt32(group.GroupID)),
                        new SqlParameter("@ProfileID", ProfileID),
                        new SqlParameter("@UserName", group.UserName.OrDbNull()),
                        new SqlParameter("@EnrollmentKeyValue", group.EnrollmentKeyValue.OrDbNull()),
                        new SqlParameter("@IsValidated", group.IsValidated),
                        new SqlParameter("@CreatedDate", group.Timestamp));
            }, CancellationToken.None);
        }

        public async Task InsertProfileToSQGroupMarshalTable(OpstoolEntity.GroupMarshalRelation groupMarshal, long ProfileID)
        {

            await _sqlAzureExecutionStrategy.ExecuteAsync(async () =>
            {
                 _guardianContext.Database
                   .ExecuteSqlCommand("EXEC [migrate].[InsertProfileToSQLGroupMarshalTable] @GroupID,@ProfileID,@IsValidated,@CreatedDate",
                        new SqlParameter("@GroupID", Convert.ToInt32(groupMarshal.GroupID)),
                        new SqlParameter("@ProfileID", ProfileID),
                        new SqlParameter("@IsValidated", groupMarshal.IsValidated),
                        new SqlParameter("@CreatedDate", groupMarshal.Timestamp));
            }, CancellationToken.None);
        }

        //public async Task InsertProfileToSQLiveLocationTable(OpstoolEntity.Location location, long ProfileID)
        //{

        //    await _sqlAzureExecutionStrategy.ExecuteAsync(async () =>
        //    {
        //        await _guardianContext.Database
        //           .SqlQuery<OpstoolEntity.Location>("EXEC [dbo].[InsertGeoLocationToSQLLiveLocationTable] @ProfileID,@SessionID,@ClientDateTime,@IsSOS,@Lat,@Long,@Alt,@Speed,@ClientTimeStamp,@MediaUri",

        //                new SqlParameter("@ProfileID", ProfileID),
        //                new SqlParameter("@SessionID", location.PartitionKey),
        //                new SqlParameter("@ClientDateTime", Convert.ToInt64(location.ClientDateTime)),                   
        //                new SqlParameter("@IsSOS", location.IsSOS),
        //                new SqlParameter("@Lat", location.Lat),
        //                new SqlParameter("@Long", location.Long),
        //                new SqlParameter("@Alt", location.Alt),
        //                new SqlParameter("@Speed", location.Speed), 
        //                new SqlParameter("@ClientTimeStamp", location.ClientTimeStamp),
        //                new SqlParameter("@MediaUri", location.MediaUri)

        //           ).FirstOrDefaultAsync();

        //    }, CancellationToken.None);
        //}

        public async Task<List<Model.User>> GetAllUser()
        {
          
                return  _guardianContext.Users.ToList();
           

        }

        public async Task<Dictionary<string, long>> GetAllGroupsWithProfileID()
        {

            return await _sqlAzureExecutionStrategy.ExecuteAsync<Dictionary<string, long>>(async () =>
            {
                return await _guardianContext.GroupMemberships
                                .Select(grpRecord => new
                                {
                                    GroupID = grpRecord.GroupID + ":" + grpRecord.ProfileID,
                                    ProfileID = grpRecord.ProfileID

                                })
                                        .AsNoTracking().ToDictionaryAsync(k => k.GroupID, v => v.ProfileID);



            }, CancellationToken.None);
        }


        public async Task<Dictionary<Guid,long>> GetAllMapUsers()
        {
           
                return _guardianContext.Database
                 .SqlQuery<OpstoolEntity.UserMap>("EXEC [migrate].[GetAllMapUsers]").ToList().ToDictionary(k => k.StorageUserID, v => v.UserID);           

        }

        public async Task<Dictionary<Guid, long>> GetAllMapProfiles()
        {

            return await _sqlAzureExecutionStrategy.ExecuteAsync<Dictionary<Guid, long>>(async () =>
            {
                return  _guardianContext.Database
                    .SqlQuery<OpstoolEntity.ProfileMap>("EXEC [migrate].[GetAllMapProfiles]").ToDictionary(k => k.StorageProfileID, v => v.ProfileID);
                

            }, CancellationToken.None);

            
        }

        public async Task<User> GetUserByMailIDAsync(string EmailID)
        {
            return await _sqlAzureExecutionStrategy.ExecuteAsync<User>(async () =>
            {
                return await _guardianContext.Users
                    .Where(w => w.Email == EmailID)
                    .AsNoTracking().FirstOrDefaultAsync();
            }, CancellationToken.None);
        }

        public async Task<User> GetUserByMobileAsync(string Mobile)
        {
            string encryptedMobile = Security.Encrypt(Mobile);

            return await _sqlAzureExecutionStrategy.ExecuteAsync<User>(async () =>
            {
                return await _guardianContext.Users
                    .Where(w => w.MobileNumber == encryptedMobile)
                    .AsNoTracking().FirstOrDefaultAsync();
            }, CancellationToken.None);
        }

        public async Task<User> GetUserByUserIDAsync(long UserID)
        {
            return await _sqlAzureExecutionStrategy.ExecuteAsync<User>(async () =>
            {
                return await _guardianContext.Users
                    .Where(w => w.UserID == UserID)
                    .AsNoTracking().FirstOrDefaultAsync();
            }, CancellationToken.None);
        }

        public async Task<User> GetUserByProfileID(string profileId)
        {

            return await _sqlAzureExecutionStrategy.ExecuteAsync<User>(async () =>
            {
                return await (from usr in _guardianContext.Users
                              join prf in _guardianContext.Profiles on usr.UserID equals prf.UserID
                              where prf.ProfileID == Convert.ToInt64(profileId)
                              select usr
                               ).AsNoTracking().FirstOrDefaultAsync();
            }, CancellationToken.None);
        }


        public async Task<List<Buddy>> GetBuddiesForProfileIDAsync(long ProfileID)
        {
            return await _sqlAzureExecutionStrategy.ExecuteAsync<List<Buddy>>(async () =>
            {
                return await _guardianContext.Buddies
                    .Where(w => w.ProfileID == ProfileID)
                    .AsNoTracking().OrderBy(b => b.BuddyName).ToListAsync();
            }, CancellationToken.None);
        }

        public async Task<List<Buddy>> GetBuddyProfilesForUserIDAsync(long UserID)
        {
            return await _sqlAzureExecutionStrategy.ExecuteAsync<List<Buddy>>(async () =>
            {
                return await _guardianContext.Buddies
                    .Where(w => w.UserID == UserID)
                    .AsNoTracking().ToListAsync();
            }, CancellationToken.None);
        }

        public async Task<Profile> GetProfileAsync(long ProfileID)
        {
            return await _sqlAzureExecutionStrategy.ExecuteAsync<Profile>(async () =>
            {
                return await _guardianContext.Profiles
                    .Include("User")
                    .Where(w => w.ProfileID == ProfileID)
                    .AsNoTracking().FirstOrDefaultAsync();
            }, CancellationToken.None);
        }

        public async Task<Profile> GetProfileByMobileAsync(string Mobile)
        {
            string encryptedMobile = Security.Encrypt(Mobile);

            return await _sqlAzureExecutionStrategy.ExecuteAsync<Profile>(async () =>
            {
                return await _guardianContext.Profiles
                    .Where(w => w.MobileNumber == encryptedMobile)
                    .AsNoTracking().FirstOrDefaultAsync();
            }, CancellationToken.None);
        }

        public async Task<List<Profile>> GetProfilesForUserIDAsync(long UserID)
        {
            return await _sqlAzureExecutionStrategy.ExecuteAsync<List<Profile>>(async () =>
            {
                return await _guardianContext.Profiles
                    .Where(w => w.UserID == UserID)
                    .AsNoTracking().ToListAsync();
            }, CancellationToken.None);
        }

        public async Task<HealthUpdate> GetPendingUpdatesAsync(long ProfileID)
        {
            return await _sqlAzureExecutionStrategy.ExecuteAsync<HealthUpdate>(async () =>
            {
                HealthUpdate pendingUpdate = new HealthUpdate();

                pendingUpdate.IsGroupModified = false;
                pendingUpdate.IsProfileActive = false;

                Profile updatedProfile = null;
                GroupMembership updatedGroup = null;

                List<Task> tasks = new List<Task>();

                Task<Profile> profileTask = _guardianContext.Profiles
                   .Where(w => w.ProfileID == ProfileID)
                   .AsNoTracking().FirstOrDefaultAsync();

                Task<GroupMembership> groupTask = _guardianContext.GroupMemberships
                   .Where(w => w.ProfileID == ProfileID)
                   .AsNoTracking().FirstOrDefaultAsync();

                tasks.Add(profileTask);
                tasks.Add(groupTask);
                Task.WaitAll(tasks.ToArray());

                updatedProfile = profileTask.Result;
                updatedGroup = groupTask.Result;

                if (updatedProfile != null)
                    pendingUpdate.IsProfileActive = updatedProfile.IsValid;

                if (updatedGroup != null)
                    pendingUpdate.IsGroupModified = true;

                return pendingUpdate;
            }, CancellationToken.None);

        }

        public async Task<List<GroupMembership>> GetAllProfilesAssociatedToGroupAsync(int igroupID)
        {
            return await _sqlAzureExecutionStrategy.ExecuteAsync<List<GroupMembership>>(async () =>
            {
                return await _guardianContext.GroupMemberships
                    .Where(w => w.GroupID == igroupID && w.IsValidated == true)
                    .AsNoTracking().ToListAsync();
            }, CancellationToken.None);
        }

        public async Task<bool> AddBuddyAsync(Buddy buddy)
        {
            return await _sqlAzureExecutionStrategy.ExecuteAsync<bool>(async () =>
            {
                _guardianContext.Buddies.Add(buddy);
                await _guardianContext.SaveChangesAsync();
                return true;
            }, CancellationToken.None);
        }

        public async Task<bool> SaveBuddy(Buddy buddy)
        {
            return await _sqlAzureExecutionStrategy.ExecuteAsync<bool>(async () =>
            {
                //there will be no update of buddy only adding .VR
                if (buddy.BuddyID == 0)
                {
                    _guardianContext.Buddies.Add(buddy);
                    await _guardianContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }, CancellationToken.None);
        }

        public async Task<int> GetAllUserCount()
        {
            return await _sqlAzureExecutionStrategy.ExecuteAsync<int>(async () =>
            {
                return await _guardianContext.Users.CountAsync();

            }, CancellationToken.None);
        }

        public async Task SaveOrUpdateProfileAsync(Profile profile)
        {
            await _sqlAzureExecutionStrategy.ExecuteAsync(async () =>
            {
                if (profile.ProfileID != 0)
                {
                    profile.User = null;//as user already added and in this scenario trying to attach again, hence exception
                    _guardianContext.Entry<Profile>(profile).State = EntityState.Modified;
                    await _guardianContext.SaveChangesAsync();
                }
                else
                {
                    _guardianContext.Profiles.Add(profile);
                    await _guardianContext.SaveChangesAsync();
                }
            }, CancellationToken.None);
        }

        public async Task SaveUserAsync(User user)
        {
            await _sqlAzureExecutionStrategy.ExecuteAsync(async () =>
            {
                if (user.UserID != 0)
                {
                    _guardianContext.Entry<User>(user).State = EntityState.Modified;
                    await _guardianContext.SaveChangesAsync();
                }
                else
                {
                    _guardianContext.Users.Add(user);
                    await _guardianContext.SaveChangesAsync();
                }
            }, CancellationToken.None);
        }

        public async Task<bool> IsProfileValidAsync(long ProfileID)
        {
            return await _sqlAzureExecutionStrategy.ExecuteAsync<bool>(async () =>
            {
                List<Profile> profiles = await _guardianContext.Profiles
                        .Where(w => w.ProfileID == ProfileID)
                        .AsNoTracking().ToListAsync();

                return (profiles.Count > 0) ? true : false;
            }, CancellationToken.None);
        }

        public async Task<User> ValidateAndGetUserAsync(string LiveUserID)
        {
            return await _sqlAzureExecutionStrategy.ExecuteAsync<User>(async () =>
            {
                return await _guardianContext.Users
                    .Where(w => w.LiveID == LiveUserID)
                    .FirstOrDefaultAsync();
            }, CancellationToken.None);
        }

        public async Task<bool> ValidateUser(string LiveUserID)
        {
            return await _sqlAzureExecutionStrategy.ExecuteAsync<bool>(async () =>
            {
                var result = await _guardianContext.Users
                    .Where(w => w.LiveID == LiveUserID)
                    .FirstAsync();
                if (result == null)
                    return false;
                else
                    return true;

            }, CancellationToken.None);
        }

        public async Task DeleteWhileUnregisterUserAsync(long ProfileID)
        {
            await _sqlAzureExecutionStrategy.ExecuteAsync(async () =>
            {
                int res = await _guardianContext.Database
                    .ExecuteSqlCommandAsync("EXEC [dbo].[DeleteWhileUnregisterUser] @ProfileID",
                        new SqlParameter("@ProfileID", ProfileID));
            }, CancellationToken.None);
        }

        public async Task RemoveBuddyRelationAsync(long ProfileID, long BuddyUserID)
        {
            await _sqlAzureExecutionStrategy.ExecuteAsync(async () =>
            {
                int result = await _guardianContext.Database
                    .ExecuteSqlCommandAsync("EXEC [dbo].[RemoveBuddyRelation] @ProfileID,@BuddyUserID",
                        new SqlParameter("@ProfileID", ProfileID),
                        new SqlParameter("@BuddyUserID", BuddyUserID));
            }, CancellationToken.None);
        }


        public async Task<User> SubscribeBuddyForProfileActionAsync(long ProfileID, long UserID, int State)
        {
            User resUser = null;
            await _sqlAzureExecutionStrategy.ExecuteAsync(async () =>
            {
                MiniUser resMiniUser = await _guardianContext.Database
                    .SqlQuery<MiniUser>("EXEC [dbo].[SubscribeBuddyForProfileAction] @ProfileID,@UserID,@State",
                    new SqlParameter("@ProfileID", ProfileID),
                    new SqlParameter("@UserID", UserID),
                    new SqlParameter("@State", State)).FirstOrDefaultAsync();

                resUser = new User
                {
                    Name = resMiniUser.Name,
                    MobileNumber = resMiniUser.MobileNumber
                };

            }, CancellationToken.None);

            return resUser;
        }
        //1
        //public async Task InsertUserIDToSQLStorage(string UserID)
        //{           
        //    await _sqlAzureExecutionStrategy.ExecuteAsync(async () =>
        //    {
        //         await _guardianContext.Database
        //            .SqlQuery("EXEC [dbo].[InsertUserIDToSQLStorage] @UserID",                   
        //            new SqlParameter("@UserID", UserID)).FirstOrDefaultAsync();             

        //    }, CancellationToken.None);            
        //}


        //public async Task InsertUserToSQLUserTable(User User)
        //{

        //    await _sqlAzureExecutionStrategy.ExecuteAsync(async () =>
        //    {
        //        await _guardianContext.Database
        //           .SqlQuery<MiniUser>("EXEC [dbo].[InsertUserToSQLUserTable] @UserID,@Name,@Email,@MobileNumber,@FBAuthID,@FBID,@LiveID,@LiveAuthID,@LiveAccessToken,@LiveRefreshToken",

        //           new SqlParameterCollection(){
        //                new SqlParameter("@UserID", Profile.ProfileID),
        //                new SqlParameter("@Name", Profile.ProfileID),
        //                new SqlParameter("@Email", Profile.ProfileID),                   
        //                new SqlParameter("@MobileNumber", Profile.ProfileID),
        //                new SqlParameter("@FBAuthID", Profile.ProfileID),
        //                new SqlParameter("@FBID", Profile.ProfileID),
        //                new SqlParameter("@LiveID", Profile.ProfileID),
        //                new SqlParameter("@LiveAuthID", Profile.ProfileID),
        //                new SqlParameter("@LiveAccessToken", Profile.ProfileID),
        //                new SqlParameter("@LiveRefreshToken", Profile.ProfileID)}
        //           ).FirstOrDefaultAsync();

        //    }, CancellationToken.None);
        //}


        //public async Task InsertProfileToSQLProfileTable(Profile Profile)
        //{

        //    await _sqlAzureExecutionStrategy.ExecuteAsync(async () =>
        //    {
        //        await _guardianContext.Database
        //           .SqlQuery<MiniUser>("EXEC [dbo].[InsertProfileToSQLProfileTable] @ProfileID,@UserID,@MobileNumber,@RegionCode,@CanPost,@CanSMS,@CanEmail,@SMSText,@SecurityToken,@LocationConsent,@IsValid",

        //           new SqlParameterCollection(){
        //                new SqlParameter("@ProfileID", Profile.ProfileID),
        //                new SqlParameter("@UserID", Profile.ProfileID),
        //                new SqlParameter("@MobileNumber", Profile.ProfileID),                   
        //                new SqlParameter("@RegioanlCode", Profile.ProfileID),
        //                new SqlParameter("@CanPost", Profile.ProfileID),
        //                new SqlParameter("@CanSMS", Profile.ProfileID),
        //                new SqlParameter("@CanEmail", Profile.ProfileID),
        //                new SqlParameter("@LocationConsent", Profile.ProfileID),
        //                new SqlParameter("@SecurityToken", Profile.ProfileID),
        //                new SqlParameter("@SMSText", Profile.ProfileID),
        //                new SqlParameter("@IsValid", Profile.ProfileID)}                                   
                                                                                                                                                                                                         
        //           ).FirstOrDefaultAsync();

        //    }, CancellationToken.None);
        //}

        //public async Task InsertProfileToSQLBuddyTable(Buddy buddy)
        //{

        //    await _sqlAzureExecutionStrategy.ExecuteAsync(async () =>
        //    {
        //        await _guardianContext.Database
        //           .SqlQuery<MiniUser>("EXEC [dbo].[InsertProfileToSQLBuddyTable] @BuddyID,@ProfileID,@UserID,@BuddyName,@MobileNumber,@Email,@IsPrimeBuddy,@State",

        //           new SqlParameterCollection(){
        //                new SqlParameter("@BuddyID", Profile.ProfileID),
        //                new SqlParameter("@ProfileID", Profile.ProfileID),
        //                new SqlParameter("@UserID", Profile.ProfileID),                   
        //                new SqlParameter("@BuddyName", Profile.ProfileID),
        //                new SqlParameter("@MobileNumber", Profile.ProfileID),
        //                new SqlParameter("@Email", Profile.ProfileID),
        //                new SqlParameter("@IsPrimeBuddy", Profile.ProfileID),
        //                new SqlParameter("@State", Profile.ProfileID)}

        //           ).FirstOrDefaultAsync();

        //    }, CancellationToken.None);
        //}

                          

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
