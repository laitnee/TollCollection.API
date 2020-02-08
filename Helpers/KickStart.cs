using newNet.Services;

namespace newNet.Helpers
{
    public class KickStart
    {
        private IMQTTMessagingService _mqttMessagingService;
        public KickStart(IMQTTMessagingService mqttMessagingService)
        {
            _mqttMessagingService = mqttMessagingService;
        }
    }
}