namespace newNet.Helpers
{
    public class MQTTSettings
    {
        public string Server { get; set; }
        public string ApiKey {get; set;}
        public int Port {get; set;}
        public string ClientId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool MqttSecure { get; set; }
    }
}