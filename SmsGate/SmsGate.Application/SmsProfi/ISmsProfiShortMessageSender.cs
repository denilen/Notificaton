using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Commons.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SmsGate.Application.SmsProfi
{
    public interface ISmsProfiShortMessageSender
    {
        Task Send(SmsProfiShortMessageQueueDTO shortMessageQueue);
    }

    public class SmsProfiShortMessageSender : ISmsProfiShortMessageSender
    {
        private static readonly ILogger Logger = LoggerFactory.Create<SmsProfiShortMessageConsumer>();

        private readonly SmsProfiShortMessageSenderConfiguration _configuration;

        private static readonly JsonSerializerSettings Settings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public SmsProfiShortMessageSender(SmsProfiShortMessageSenderConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Send(SmsProfiShortMessageQueueDTO shortMessageQueue)
        {
            if (Logger.IsDebugEnabled)
                Logger.Info("Sending SMS to {PhoneNumber}", shortMessageQueue.PhoneNumber);

            using var client = new HttpClient
            {
                BaseAddress = new Uri(_configuration.BaseUrl)
            };

            client.DefaultRequestHeaders.Add("X-Token", _configuration.Token);

            var smsProfiShortMessage = new SmsProfiShortMessage
            {
                Messages = new List<Message>
                {
                    new()
                    {
                        Id        = RandomString(12),
                        Source    = _configuration.Source,
                        Recipient = shortMessageQueue.PhoneNumber,
                        Text      = shortMessageQueue.Message,
                        Timeout   = _configuration.Timeout
                    }
                },
                Validate = _configuration.Test
            };

            var jsonString = JsonConvert.SerializeObject(smsProfiShortMessage, Settings);

            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");


            var response = await client.PostAsync("sms/send/text", content);

            if (!response.IsSuccessStatusCode)
            {
                Logger.Warn("Cannot send SMS to '{PhoneNumber}', status: {HttpStatus}",
                            shortMessageQueue.PhoneNumber, response.StatusCode);
                return;
            }

            if (Logger.IsDebugEnabled)
                Logger.Info("SMS to '{PhoneNumber}' were sent", shortMessageQueue.PhoneNumber);
        }

        private static string RandomString(int length)
        {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyz";

            var random = new Random(Environment.TickCount);

            var builder = new StringBuilder(length);

            for (var i = 0; i < length; ++i)
                builder.Append(chars[random.Next(chars.Length)]);

            return builder.ToString();
        }
    }
}
