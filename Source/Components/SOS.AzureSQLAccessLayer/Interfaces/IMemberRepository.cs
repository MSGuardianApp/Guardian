using SOS.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOS.AzureSQLAccessLayer
{
    public interface IMemberRepository : IDisposable
    {
        Task<User> GetUserByMailIDAsync(string EmailID);

        Task<User> GetUserByMobileAsync(string Mobile);

        Task<User> GetUserByUserIDAsync(long UserID);

        Task<User> GetUserByProfileID(string profileId);

        Task<List<Buddy>> GetBuddiesForProfileIDAsync(long ProfileID);

        Task<List<Buddy>> GetBuddyProfilesForUserIDAsync(long UserID);

        Task<Profile> GetProfileAsync(long ProfileID);

        Task<Profile> GetProfileByMobileAsync(string Mobile);

        Task<List<Profile>> GetProfilesForUserIDAsync(long UserID);

        Task<HealthUpdate> GetPendingUpdatesAsync(long ProfileID);

        Task<List<GroupMembership>> GetAllProfilesAssociatedToGroupAsync(int igroupID);

        Task<bool> AddBuddyAsync(Buddy buddy);        

        Task SaveOrUpdateProfileAsync(Profile profile);

        Task SaveUserAsync(User user);

        Task<bool> IsProfileValidAsync(long ProfileID);

        Task<User> ValidateAndGetUserAsync(string LiveUserID);        

        Task<bool> ValidateUser(string LiveUserID);

        Task DeleteWhileUnregisterUserAsync(long ProfileID);

        Task RemoveBuddyRelationAsync(long ProfileID, long BuddyUserID);

        //IMemberRepository is inherited in MainRepository also where these methods are not implemented 
        //Task<string> GetProfileUserIDByLiveIDAsync(string LiveUserID);

        //Task<int> GetAllUserPorfilesCount();

        //Task<User> SubscribeBuddyForProfileActionAsync(long ProfileID, long UserID, int State, string SubscribtionID);

        //Task<int> GetMembersCountForGroup(int GroupID);

        //Task<int> GetLocateBuddiesCountForUser(long UserID);

        //Task<List<LiveUserStatus>> GetFilteredGroupMembers(int groupID, string searchName);

        //Task<List<LiveUserStatus>> GetFilteredLocateBuddies(long UserID, string searchName);

        //Task<List<Member>> GetMembersByGroupID(int groupID, string searchName);

        //Task<List<Buddy>> GetMarshalBuddiesByGroupID(int GroupID);
    }
}
