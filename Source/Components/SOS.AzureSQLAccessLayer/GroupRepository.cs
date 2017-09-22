using SOS.AzureSQLAccessLayer.Entities;
using SOS.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace SOS.AzureSQLAccessLayer
{
    public class GroupRepository : BaseRepository, IGroupRepository
    {
        private readonly GuardianContext _guardianContext;

        public GroupRepository()
            : this(new GuardianContext())
        {
        }

        public GroupRepository(GuardianContext guardianContext)
        {
            if (guardianContext == null)
            {
                throw new ArgumentException("guardianContext");
            }
            _guardianContext = guardianContext;
        }

        public async Task<bool> ValidateGroupMembership(int groupID, long profileID)
        {
            var groups = await _guardianContext.GroupMemberships
            .Where(w => w.GroupID == groupID && w.ProfileID == profileID)
            .CountAsync();

            return (groups == 1) ? true : false;
        }

        public async Task<List<GroupMembership>> GetGroupMembershipForProfile(long profileID)
        {
            return await _guardianContext.GroupMemberships
           .Where(w => w.ProfileID == profileID && ((!w.ParentGrpID.HasValue) || (w.ParentGrpID.Value == 0)))
           .ToListAsync();
        }

        public async Task CreateGroupMembership(GroupMembership gmem)
        {
            gmem.CreatedDate = DateTime.UtcNow;
            gmem.LastModifiedDate = DateTime.UtcNow;
            _guardianContext.GroupMemberships.Add(gmem);
            await _guardianContext.SaveChangesAsync();
        }

        public async Task CreatePublicGroupMembership(int GroupId, long ProfileID, string userName)
        {
            int result = await _guardianContext.Database
           .ExecuteSqlCommandAsync("EXEC [dbo].[CreatePublicGroupMembership] @GroupID,@ProfileID,@UserName",
               new SqlParameter("@GroupID", GroupId),
               new SqlParameter("@ProfileID", ProfileID),
               new SqlParameter("@UserName", userName));
        }

        public async Task<int> AutoSubscribeLiveUserToSubGroup(int groupID, long profileID, string userName, string liveSessionID, int parentGrpID)
        {
                    int result = await _guardianContext.Database
                            .ExecuteSqlCommandAsync("EXEC [dbo].[SubscribeLiveUserToSubGroup] @SubGroupId,@ProfileID,@UserName,@LiveSessionID,@ParentGrpID",
                               new SqlParameter("@SubGroupId", groupID),
                               new SqlParameter("@ProfileID", profileID),
                               new SqlParameter("@UserName", userName),
                               new SqlParameter("@LiveSessionID", liveSessionID),
                               new SqlParameter("@ParentGrpID", parentGrpID));
                    return result;
        }

        public async Task DeleteGroupMembership(int groupID, long profileID)
        {
            int result = await _guardianContext.Database
            .ExecuteSqlCommandAsync("EXEC [dbo].[DeleteGroupMembership] @GroupID,@ProfileID",
                new SqlParameter("@GroupID", groupID),
                new SqlParameter("@ProfileID", profileID));
        }

        public async Task UpdateGroupMembership(int groupID, long profileID)
        {
            int result = await _guardianContext.Database
           .ExecuteSqlCommandAsync("EXEC [dbo].[UpdateGroupMembership] @GroupID,@ProfileID",
               new SqlParameter("@GroupID", groupID),
               new SqlParameter("@ProfileID", profileID));
        }

        //ssm
        public async Task<bool> SaveMarshal(int groupID, long profileID, bool isValidated)
        {
            GroupMarshal grpMarshal = new GroupMarshal() { GroupID = groupID, ProfileID = profileID, IsValidated = isValidated };

            if (!isValidated)
            {
                grpMarshal.CreatedDate = DateTime.UtcNow;
                grpMarshal.LastModifiedDate = DateTime.UtcNow;
                _guardianContext.GroupMarshals.Add(grpMarshal);
                _guardianContext.SaveChanges();
            }
            else
            {
                grpMarshal.LastModifiedDate = DateTime.UtcNow;
                _guardianContext.Entry<GroupMarshal>(grpMarshal).State = EntityState.Modified;
                _guardianContext.SaveChanges();
            }
            return true;
        }

        public async Task<bool> DeleteMarshal(int groupID, long profileID)
        {
            int result = await _guardianContext.Database
           .ExecuteSqlCommandAsync("EXEC [dbo].[DeleteGroupMarshal] @GroupID,@ProfileID",
               new SqlParameter("@GroupID", groupID),
               new SqlParameter("@ProfileID", profileID));
            return (result > 0) ? true : false;
        }
        

        public async Task<List<GroupMemberLiveSession>> GetAllGroupMembershipLite()
        {
            bool includeActiveMembers = ConfigManager.Config.IncludeActiveMembers;

            return await ((from ls in _guardianContext.LiveSessions
                           join pf in _guardianContext.Profiles on ls.ProfileID equals pf.ProfileID
                           join gm in _guardianContext.GroupMemberships on pf.ProfileID equals gm.ProfileID
                           where ls.Command != "STOP"
                                  && (!gm.ParentGrpID.HasValue || gm.ParentGrpID.Value == 0)
                                  && (includeActiveMembers ? true : ls.IsSOS)
                           select new GroupMemberLiveSession()
                           {
                               GrpId = gm.GroupID,
                               LiveSessionObj = ls
                           }).AsNoTracking().ToListAsync());
        }

        public async Task<List<Profile>> GetLiveMarshalsByGroupID(int groupID)
        {
            var result = (from GM in _guardianContext.GroupMarshals
                          join P in _guardianContext.Profiles on GM.ProfileID equals P.ProfileID
                          join U in _guardianContext.Users on P.UserID equals U.UserID
                          join LS in _guardianContext.LiveSessions on GM.ProfileID equals LS.ProfileID
                          where GM.GroupID == groupID && LS.Command != "STOP"
                          select new
                          {
                              ProfileID = P.ProfileID,
                              Email = U.Email,
                              MobileNumber = U.MobileNumber,
                              Name = U.Name,
                              UserID = U.UserID,
                              SessionID = LS.SessionID,
                              IsSOS = LS.IsSOS,
                              Lat = LS.Lat,
                              Long = LS.Long
                          }).ToListAsync();

            var profiles = await result;

            return profiles.Select(p =>
                                    new Profile
                                    {
                                        ProfileID = p.ProfileID,
                                        UserID = p.UserID,
                                        User = new User
                                        {
                                            Email = p.Email,
                                            MobileNumber = p.MobileNumber,
                                            Name = p.Name,
                                            UserID = p.UserID
                                        },
                                        LiveLocations = new List<LiveLocation>(){new LiveLocation
                                                                   {
                                                                       ProfileID = p.ProfileID,
                                                                       SessionID = p.SessionID,
                                                                       IsSOS = p.IsSOS,
                                                                       Lat = p.Lat,
                                                                       Long = p.Long
                                                                   }}
                                    }).ToList<Profile>();
        }

        public async Task<bool> ValidateMarshalForGroup(int groupID, long profileID)
        {
            int groups = await _guardianContext.GroupMarshals
            .Where(w => w.GroupID == groupID && w.ProfileID == profileID)
            .CountAsync();

            return (groups == 1) ? true: false;
        }

        public async Task<MarshalStatusInfo> ValidateAndSaveMarshal(int GroupId, string mailId, string encryptedMobile, bool isValidated)
        {
            MarshalStatusInfo resMarshalInfo = await _guardianContext.Database
            .SqlQuery<MarshalStatusInfo>("EXEC [dbo].[ValidateAndSaveMarshal] @GroupID,@Email,@MobileNumber,@IsValidated",
            new SqlParameter("@GroupID", GroupId),
            new SqlParameter("@Email", mailId),
            new SqlParameter("@MobileNumber", encryptedMobile),
            new SqlParameter("@IsValidated", isValidated)).FirstOrDefaultAsync();

            return resMarshalInfo;
        }

        #region "Sync DB Calls for Reports"
        //we have made this method to work as sync for report 
        public async Task<Dictionary<int, int>> GetAllGroupsWithUserCount()
        {

            return _guardianContext.GroupMemberships.Where(x => ((!x.ParentGrpID.HasValue) || (x.ParentGrpID.Value == 0)))
                                            .GroupBy(grpRecord => grpRecord.GroupID)
                                            .Select(grpRecord => new
                                            {
                                                GroupID = grpRecord.Key,
                                                UsersCount = grpRecord.Count()
                                            })
                                            .AsNoTracking().ToDictionary(k => k.GroupID, v => v.UsersCount);
            ;
        }

        //we have made this method to work as sync for report 
        public async Task<Dictionary<string, long>> GetAllGroupsWithProfileID()
        {


            return (_guardianContext.GroupMemberships.Where(x => ((!x.ParentGrpID.HasValue) || (x.ParentGrpID.Value == 0)))
                       .Select(grpRecord => new
                       {
                           GroupID = grpRecord.GroupID + ":" + grpRecord.ProfileID,
                           ProfileID = grpRecord.ProfileID

                       })
                               .AsNoTracking().ToDictionary(k => k.GroupID, v => v.ProfileID));

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