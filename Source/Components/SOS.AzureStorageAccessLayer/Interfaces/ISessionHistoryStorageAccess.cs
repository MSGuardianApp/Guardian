using SOS.AzureStorageAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOS.AzureStorageAccessLayer
{
    public interface ISessionHistoryStorageAccess
    {
        Task ArchiveSessionDetailAsync(SessionHistory session);

        Task ArchiveSessionDetailsAsync(List<SessionHistory> sessions);

        List<object> GetAllSessionHistory(long startTicks, long endTicks, bool sosFlag = true);

        List<object> GetAllSessionSOSAndTrackHistory(long startTicks, long endTicks, bool sosFlag = true);
    }
}



