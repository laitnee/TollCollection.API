using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using newNet.Data.Repository;
using newNet.Helpers;

namespace newNet.Services {
    public class MQTTMessagingService : IHostedService, IDisposable {
        private IOptions<MQTTSettings> _MQTTSettings;
        private IManagedMqttClient _client;
        private ILogger _logger;
        // private IProcessTransaction _processTransaction;
        // private ITollCollectionRepository _repo;
        // private ISMSMessagingService _sms;
        public IServiceProvider Services { get; }

        public MQTTMessagingService (ILogger<MQTTMessagingService> logger, IOptions<MQTTSettings> MQTTSettings, IServiceProvider services) {
            _MQTTSettings = MQTTSettings;
            _logger = logger;
            Services = services;
            //_processTransaction = new ProcessTransaction(repo);
        }

        public async Task ConnectAsync () {
            var messageBuilder = new MqttClientOptionsBuilder ()
                .WithClientId (_MQTTSettings.Value.ClientId)
                .WithCredentials (_MQTTSettings.Value.Username, _MQTTSettings.Value.Password)
                .WithTcpServer (_MQTTSettings.Value.Server, _MQTTSettings.Value.Port)
                .WithCleanSession ();

            var options = _MQTTSettings.Value.MqttSecure ?
                messageBuilder
                .WithTls ()
                .Build () :
                messageBuilder
                .Build ();

            var managedOptions = new ManagedMqttClientOptionsBuilder ()
                .WithAutoReconnectDelay (System.TimeSpan.FromSeconds (5))
                .WithClientOptions (options)
                .Build ();

            _client = new MqttFactory ().CreateManagedMqttClient ();

            await _client.StartAsync(managedOptions);
        }

        public async Task PublishAsync (string topic, string payload, bool retainFlag = false, int qos = 2) {
            _logger.LogInformation ("she efe pami ni");
            await _client.PublishAsync (new MqttApplicationMessageBuilder ()
                .WithTopic (topic)
                .WithPayload (payload)
                .WithQualityOfServiceLevel ((MQTTnet.Protocol.MqttQualityOfServiceLevel) qos)
                .WithRetainFlag (retainFlag)
                .Build ());

        }

        public async Task SuscribeAync (string topic, int qos = 2) {
            _logger.LogInformation ("o de bi");
            await _client.SubscribeAsync (new TopicFilterBuilder ()
                .WithTopic (topic)
                .WithQualityOfServiceLevel ((MQTTnet.Protocol.MqttQualityOfServiceLevel) qos)
                .Build ());
        }

        // public void ConnectHandler(EventArgs e) => _logger.LogInformation("connected to broker", e);
        // public void DisconnectHandler(EventArgs e) => _logger.LogInformation("disconnected from broker ", e);
        public async Task MessageRecievedHandler (MqttApplicationMessageReceivedEventArgs arg) {
            try {
                string topic = arg.ApplicationMessage.Topic;
                if (string.IsNullOrWhiteSpace (topic) == false) {
                    string payload = Encoding.UTF8.GetString (arg.ApplicationMessage.Payload);
                    _logger.LogInformation ($"Topic: {topic}. Message Received: {payload}");

                    using (var scope = this.Services.CreateScope())
                    {
                        var _processTransaction = 
                            scope.ServiceProvider
                                .GetRequiredService<IProcessTransaction>();
                        if (await _processTransaction.processTransaction (payload)) {
                                    await PublishAsync ("ASP/TOLLCOLLECTION", "Successful");
                                } else {
                                    await PublishAsync ("ASP/TOLLCOLLECTION", "Unsuccessful");
                                }
                    }
                         
                }
            } catch (Exception ex) {
                Console.WriteLine (ex.Message, ex);
            }
        }

        public async Task StartAsync (CancellationToken cancellationToken) {
            _logger.LogInformation ("MQTT background Service is starting ");

            await this.ConnectAsync();
            
            if (_client.IsStarted) _logger.LogInformation ("client is started");

            _client.UseConnectedHandler (ConnectHandler);
            _client.UseDisconnectedHandler (DisconnectHandler);
            _client.UseApplicationMessageReceivedHandler (MessageRecievedHandler);

            await _client.SubscribeAsync ("ESP/TOLLCOLLECTION");
        }

        private void ConnectHandler (MqttClientConnectedEventArgs arg) {
            _logger.LogInformation ("connected to broker", arg);
        }

        private void DisconnectHandler (MqttClientDisconnectedEventArgs arg) {

            if (_client.IsConnected) _logger.LogWarning ("client is connected ");
            _logger.LogCritical ("disconnected from broker ", arg);
        }

        public async Task StopAsync (CancellationToken cancellationToken) {

            await _client.StopAsync ();
            _logger.LogInformation ("MQTT  background service is ending");
        }

        public void Dispose () {

            _client?.Dispose ();
        }
    }
}