using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmsGate.Application;
using SmsGate.Application.MassTransit;

namespace SmsGate.Api.Configuration
{
    public static class ConfigurationFacade
    {
        public static void RegisterConfigurations(this IServiceCollection builder, IConfiguration configuration)
        {
            builder.RegisterConfiguration<MessageServiceBusConfiguration>(configuration, "MessageServiceBus");
            builder.RegisterConfiguration<SendShortMessageHandlerConfiguration>(configuration, "SendShortMessageHandler");
        }

        private static void RegisterConfiguration<T>(this IServiceCollection builder,
                                                     IConfiguration configuration,
                                                     string section) where T : class
        {
            var configurationObject = configuration.GetSection(section).Get<T>();

            builder.AddSingleton(configurationObject);
        }
    }
}
