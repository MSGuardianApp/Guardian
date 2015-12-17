using System.Threading.Tasks;

namespace SOS.Service.Interfaces
{
    public interface IAuthorization
    {
        Task<bool> SelfAccess(string LiveUserID, long ProfileID);

        Task<bool> LocateBuddyAccess(string LiveUserID, long ProfileID);

        Task<bool> OwnGroupMembersAccess(string LiveUserID, int GroupID, long ProfileID);

        Task<bool> ValidUserAccess(string LiveUserID, long UserID);
    }
}
