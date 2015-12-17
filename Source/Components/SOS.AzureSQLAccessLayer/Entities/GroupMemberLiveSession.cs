using SOS.Model;

namespace SOS.AzureSQLAccessLayer
{
    public class GroupMemberLiveSession
    {
        public int GrpId { get; set; }
        public LiveSession LiveSessionObj { get; set; }
    }
}
