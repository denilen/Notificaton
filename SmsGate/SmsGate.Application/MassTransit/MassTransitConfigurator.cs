using Custom.MassTransit.OpenTracing;
using MassTransit;
using SmsGate.Application.Nikita;
using SmsGate.Application.SmsProfi;

namespace SmsGate.Application.MassTransit
{
    public static class MassTransitConfigurator
    {
        public static void ConfigureRabbitMqConsumer(this IBusRegistrationConfigurator x,
                                                     bool autoDelete = false)
        {
            x.AddConsumer<NikitaShortMessageConsumer>();
            x.AddConsumer<SmsProfiShortMessageConsumer>();

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

                cfg.ReceiveEndpoint(busConfiguration.NikitaQueueName, ec =>
                {
                    ec.AutoDelete = autoDelete;

                    ec.ConfigureConsumer<NikitaShortMessageConsumer>(context);
                });

                cfg.ReceiveEndpoint(busConfiguration.SmsProfiQueueName, ec =>
                {
                    ec.AutoDelete = autoDelete;

                    ec.ConfigureConsumer<SmsProfiShortMessageConsumer>(context);
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
