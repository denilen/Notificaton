using System;
using System.Net.Http;
using System.Threading.Tasks;
using Infrastructure.SmsGate.DataTypes;
using Refit;

namespace Infrastructure.SmsGate.Client.PlayGround
{
    internal static class Program
    {
        private static async Task Main()
        {
            var baseSmsApiAddress = new Uri("http://localhost:29000");
            var refitSettings     = new RefitSettings();

            var httpClientHandler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };

            var httpClient = new HttpClient(httpClientHandler) { BaseAddress = baseSmsApiAddress };

            var smsGate = RestService.For<ISmsGateClient>(httpClient, refitSettings);

            var shortMessage = new SendShortMessageCommand
            {
                SourceId    = 1,
                PhoneNumber = "+79120000000",
                Tenant      = "www.tennisi.kg",
                Message     = "Hello World"
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
