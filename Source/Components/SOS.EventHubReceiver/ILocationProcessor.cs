using SOS.Model;

namespace SOS.EventHubReceiver
{
    public interface ILocationProcessor
    {
        bool ProcessLocation(LiveLocation loc);
    }
}
