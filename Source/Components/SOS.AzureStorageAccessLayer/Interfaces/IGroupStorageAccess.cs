using SOS.AzureStorageAccessLayer.Entities;
using System.Collections.Generic;

namespace SOS.AzureStorageAccessLayer
{
    public interface IGroupStorageAccess
    {
        void CreateGroup(Group grp);

        void UpdateGroup(Group grp);

        Group GetGroupByID(int groupID);

        void DeleteGroup(int GrpID);

        Group GetGroupByLocationAndID(int groupID, string Location);

        Dictionary<int, string> GetAllGroupsWithGroupNames();

        Group GetGroupForGroupID(int GroupID);

        List<Group> GetGroupsForNameMatch(string nameSmpl);

        List<Group> GetAllGroups(bool? RetriveOnlyChilds = false);

        List<AdminUser> GetAllGroupAdmins();

        AdminUser GetAdminUser(string LUID);

        bool AddGroupMemberToValidate(string profileID, string groupID, string notificationIdentity);

        GroupMemberValidator GetValidateGroupMemberRec(string validationID, string ProfileID);

        bool DeleteGroupMemberValidatorRec(GroupMemberValidator GMV);

        bool SaveGroupMarshalValidator(GroupMarshalValidator GMV);

        GroupMarshalValidator GetGroupMarshalValidator(string ValidationID, string ProfileID);

        bool DeleteGroupMarshalValidator(GroupMarshalValidator GMV);

        bool ValidateAdminForGroup(string groupID, string adminID);

        void SaveUpdateGroupMemberValidator(GroupMemberValidator GMV);

        List<GroupMemberValidator> GetValidateGroupMembers(string ProfileID);

        AdminUser GetAdminUserByGroupID(int GroupID);

    }
}
