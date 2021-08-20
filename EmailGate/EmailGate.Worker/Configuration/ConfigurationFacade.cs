using EmailGate.Application;
using EmailGate.Application.Email;
using EmailGate.Application.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmailGate.Worker.Configuration
{
    public static class ConfigurationFacade
    {
        public static EmailMessageSenderConfiguration EmailMessageSender { get; private set; } = null!;

        public static void RegisterConfigurations(this IServiceCollection builder, IConfiguration configuration)
        {
            builder.RegisterConfiguration<RedisServiceConfiguration>(configuration, "Redis");
            builder.RegisterConfiguration<MessageServiceBusConfiguration>(configuration, "MessageServiceBus");
            builder.RegisterConfiguration<SendEmailMessageHandlerConfiguration>(configuration, "SendEmailMessageHandler");

            EmailMessageSender = builder.RegisterConfiguration<EmailMessageSenderConfiguration>(configuration, "EmailMessageSender");
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
