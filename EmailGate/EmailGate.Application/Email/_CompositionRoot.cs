using Autofac;
using JetBrains.Annotations;

namespace EmailGate.Application.Email
{
    [UsedImplicitly]
    public class CompositionRoot : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmailMessageConsumer>()
                   .SingleInstance();

            builder.RegisterType<EmailMessageSender>()
                   .As<IEmailMessageSender>()
                   .SingleInstance();
        }
    }
}
