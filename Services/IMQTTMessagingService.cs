using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace newNet.Services
{
    public interface IMQTTMessagingService 
    {
         Task ConnectAsync();
         Task PublishAsync(string topic, string payload, bool retainFlag = true, int qos = 1);
         Task SuscribeAync(string topic, int qos = 1);
         void ConnectHandler(EventArgs e);
         void DisconnectHandler(EventArgs e);
         void ApplicationMessageReceivedHandler(EventArgs e);
    }
}