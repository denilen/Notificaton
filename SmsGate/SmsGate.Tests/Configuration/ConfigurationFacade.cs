using Autofac;
using Microsoft.Extensions.Configuration;
using SmsGate.Application;
using SmsGate.Application.MassTransit;
using SmsGate.Worker.Configuration;

namespace SmsGate.Tests.Configuration
{
    public static class ConfigurationFacade
    {
        public static IConfigurationRoot Configuration { get; }

        static ConfigurationFacade()
        {
            Configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
        }

        public static void RegisterConfigurations(this ContainerBuilder builder)
        {
            builder.RegisterConfiguration<MessageServiceBusConfiguration>("MessageServiceBus");
            builder.RegisterConfiguration<RedisServiceConfiguration>("Redis");
            builder.RegisterConfiguration<SendShortMessageHandlerConfiguration>("SendShortMessageHandler");
        }

        private static void RegisterConfiguration<T>(this ContainerBuilder builder,
                                                     string section) where T : class
        {
            var sectionConfiguration = Configuration.GetSection(section).Get<T>();

            builder.RegisterInstance(sectionConfiguration);
        }
    }
}
