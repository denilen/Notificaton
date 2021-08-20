using Autofac;
using JetBrains.Annotations;

namespace SmsGate.Application.SmsProfi
{
    [UsedImplicitly]
    public class CompositionRoot : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SmsProfiShortMessageConsumer>()
                   .SingleInstance();

            builder.RegisterType<SmsProfiShortMessageSender>()
                   .As<ISmsProfiShortMessageSender>()
                   .SingleInstance();
        }
    }
}
