using System.Threading.Tasks;

namespace newNet.Services
{
    public interface ISMSMessagingService
    {
        void sendMessage(string msg, string receiver);
    }
}