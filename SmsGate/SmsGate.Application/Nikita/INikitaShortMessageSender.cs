using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Commons.Logging;

namespace SmsGate.Application.Nikita
{
    public interface INikitaShortMessageSender
    {
        Task Send(NikitaShortMessageQueueDTO shortMessageQueue);
    }

    public class NikitaShortMessageSender : INikitaShortMessageSender
    {
        private static readonly ILogger Logger = LoggerFactory.Create<NikitaShortMessageConsumer>();

        private readonly NikitaShortMessageSenderConfiguration _configuration;

        public NikitaShortMessageSender(NikitaShortMessageSenderConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Send(NikitaShortMessageQueueDTO shortMessageQueue)
        {
            if (Logger.IsDebugEnabled)
                Logger.Info("Sending SMS to {PhoneNumber}", shortMessageQueue.PhoneNumber);

            using var client = new HttpClient
            {
                BaseAddress = new Uri(_configuration.BaseUrl)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            var xmlSerializer = new XmlSerializer(typeof(NikitaShortMessage));
            var stringWriter  = new StringWriter();

            var phones = new Phones
            {
                Phone = shortMessageQueue.PhoneNumber
            };

            var nikitaShortMessage = new NikitaShortMessage
            {
                Login  = _configuration.Login,
                Pwd    = _configuration.Password,
                Id     = RandomString(12),
                Sender = _configuration.Sender,
                Text   = shortMessageQueue.Message,
                Phones = phones,
                Time   = DateTimeProvider.UtcNow.AddHours(6).ToString("yyyyMMddHHmmss"), // Бишкек GMT+6
                Test   = _configuration.Test
            };

            xmlSerializer.Serialize(stringWriter, nikitaShortMessage);

            var response = await client.GetAsync("api/message");

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
