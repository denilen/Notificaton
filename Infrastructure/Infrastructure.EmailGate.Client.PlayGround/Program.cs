using System;
using System.Net.Http;
using System.Threading.Tasks;
using Infrastructure.EmailGate.DataTypes;
using Refit;

namespace Infrastructure.EmailGate.Client.PlayGround
{
    internal static class Program
    {
        private static async Task Main()
        {
            var baseSmsApiAddress = new Uri("https://localhost:5001");
            var refitSettings     = new RefitSettings();

            var httpClientHandler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };

            var httpClient = new HttpClient(httpClientHandler) { BaseAddress = baseSmsApiAddress };

            var smsGate = RestService.For<IEmailGateClient>(httpClient, refitSettings);

            var shortMessage = new SendEmailMessageCommand()
            {
                SourceId = 1,
                Tenant   = "test",
                Address  = "tennisi.dev@yandex.ru",
                Message  = "Hello World"
            };

            try
            {
                await smsGate.SendAsync(shortMessage);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
