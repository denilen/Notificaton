using Custom.MassTransit.OpenTracing;
using EmailGate.Application.Email;
using MassTransit;

namespace EmailGate.Application.MassTransit
{
    public static class MassTransitConfigurator
    {
        public static void ConfigureRabbitMqConsumer(this IBusRegistrationConfigurator x,
                                                     bool autoDelete = false)
        {
            x.AddConsumer<EmailMessageConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var busConfiguration = context.GetService<MessageServiceBusConfiguration>();

                cfg.UseSerilogEnricher();
                cfg.PropagateOpenTracingContext();

                cfg.Host(busConfiguration.HostUri, h =>
                {
                    h.Username(busConfiguration.Username);
                    h.Password(busConfiguration.Password);
                });

                cfg.ReceiveEndpoint(busConfiguration.EmailQueueName, ec =>
                {
                    ec.AutoDelete = autoDelete;

                    ec.ConfigureConsumer<EmailMessageConsumer>(context);
                });
            });
        }

        public static void ConfigureRabbitMqPublish(this IBusRegistrationConfigurator x)
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var busConfiguration = context.GetService<MessageServiceBusConfiguration>();

                cfg.UseSerilogEnricher();
                cfg.PropagateOpenTracingContext();

                cfg.Host(busConfiguration.HostUri, h =>
                {
                    h.Username(busConfiguration.Username);
                    h.Password(busConfiguration.Password);
                });
            });
        }
    }
}
