using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmsGate.Application;
using SmsGate.Application.MassTransit;
using SmsGate.Application.Nikita;
using SmsGate.Application.SmsProfi;

namespace SmsGate.Worker.Configuration
{
    public static class ConfigurationFacade
    {
        public static NikitaShortMessageSenderConfiguration NikitaShortMessageSender { get; private set; } = null!;
        public static SmsProfiShortMessageSenderConfiguration SmsProfiShortMessageSender { get; private set; } = null!;

        public static void RegisterConfigurations(this IServiceCollection builder, IConfiguration configuration)
        {
            builder.RegisterConfiguration<RedisServiceConfiguration>(configuration, "Redis");
            builder.RegisterConfiguration<MessageServiceBusConfiguration>(configuration, "MessageServiceBus");
            builder.RegisterConfiguration<SendShortMessageHandlerConfiguration>(configuration, "SendShortMessageHandler");

            NikitaShortMessageSender   = builder.RegisterConfiguration<NikitaShortMessageSenderConfiguration>(configuration, "NikitaShortMessageSender");
            SmsProfiShortMessageSender = builder.RegisterConfiguration<SmsProfiShortMessageSenderConfiguration>(configuration, "SmsProfiShortMessageSender");
        }

        private static T RegisterConfiguration<T>(this IServiceCollection builder,
                                                  IConfiguration configuration,
                                                  string section) where T : class
        {
            var configurationObject = configuration.GetSection(section).Get<T>();

            builder.AddSingleton(configurationObject);

            return configurationObject;
        }
    }
}
