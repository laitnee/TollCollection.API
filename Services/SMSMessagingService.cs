using Microsoft.Extensions.Options;
using newNet.Helpers;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace newNet.Services
{
    public class SMSMessagingService : ISMSMessagingService
    {

        public SMSMessagingService(IOptions<TwilioSettings> twilioSettings)
        {
            TwilioClient.Init(twilioSettings.Value.accountSid, twilioSettings.Value.authToken);
        }
        public void sendMessage(string msg, string receiver)
        {
            var message = MessageResource.Create(
            body: msg,
            from: new Twilio.Types.PhoneNumber("+12299994035"),
            to: new Twilio.Types.PhoneNumber($"+234{receiver}")
            );
            string result = message.Sid;
        }
    }
}