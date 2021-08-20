using EmailGate.Application.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmailGate.Api.Configuration
{
    public static class ConfigurationFacade
    {
        public static void RegisterConfigurations(this IServiceCollection builder, IConfiguration configuration)
        {
            builder.RegisterConfiguration<MessageServiceBusConfiguration>(configuration, "MessageServiceBus");
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
