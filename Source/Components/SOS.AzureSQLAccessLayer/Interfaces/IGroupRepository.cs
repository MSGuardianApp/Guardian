using SOS.AzureSQLAccessLayer.Entities;
using SOS.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOS.AzureSQLAccessLayer
{
    public interface IGroupRepository : IDisposable
    {
        Task<bool> ValidateGroupMembership(int groupID, long profileID);

        Task<List<GroupMembership>> GetGroupMembershipForProfile(long profileID);

        Task CreateGroupMembership(GroupMembership gmem);

        Task<int> AutoSubscribeLiveUserToSubGroup(int groupID, long profileID, string userName, string liveSessionID, int parentGrpID);

        Task DeleteGroupMembership(int groupID, long profileID);

        Task UpdateGroupMembership(int groupID, long profileID);

        Task<bool> SaveMarshal(int groupID, long profileID, bool isValidated);

        Task<bool> DeleteMarshal(int groupID, long profileID);

        Task<Dictionary<int, int>> GetAllGroupsWithUserCount();

        Task<Dictionary<string, long>> GetAllGroupsWithProfileID();

        Task<List<GroupMemberLiveSession>> GetAllGroupMembershipLite();

        Task<List<Profile>> GetLiveMarshalsByGroupID(int groupID);

        Task<bool> ValidateMarshalForGroup(int groupID, long profileID);

        Task<MarshalStatusInfo> ValidateAndSaveMarshal(int GroupId, string mailId, string encryptedMobile, bool isValidated);
    }
}
