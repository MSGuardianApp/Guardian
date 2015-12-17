using System;
using System.Threading.Tasks;

namespace SOS.AzureSQLAccessLayer.Interfaces
{
    public interface IAuthRepository : IDisposable
    {
        Task<bool> SelfAccess(string LiveUserID, long ProfileID);

        Task<bool> LocateBuddyAccess(string LiveUserID, long ProfileID);

        Task<bool> SelfGroupMembersAccess(int GroupID, long ProfileID);

        Task<bool> ValidUserAccess(string LiveUserID, long UserID);
    }
}
