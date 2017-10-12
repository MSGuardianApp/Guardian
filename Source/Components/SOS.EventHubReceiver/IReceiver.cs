using System.Threading.Tasks;

namespace SOS.EventHubReceiver
{
    public interface IReceiver
    {
        Task Start();
    }
}
