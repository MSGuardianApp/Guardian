using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOS.AzureSQLAccessLayer
{
    public interface IReportRepository:IDisposable
    {
        Task<Dictionary<long, Tuple<long, string>>> GetAllUserwithProfileData(string profileList);

        Task<string> GetUserNameForProfileID(string profileID);

        Task<Dictionary<long, string>> GetAllUserIDWithAttributes();

        Task<Dictionary<long, long>> GetAllProfileIDWithUserID();

        Task<Dictionary<long, int>> GetAllProfileIDWithCount();

        Task<Dictionary<long, int>> GetAllProfileIDWithCountFromGrpMemShip();
    }
}
