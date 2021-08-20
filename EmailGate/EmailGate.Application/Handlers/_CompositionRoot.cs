using Autofac;
using JetBrains.Annotations;

namespace EmailGate.Application.Handlers
{
    [UsedImplicitly]
    public class CompositionRoot : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SendEmailMessageHandler>()
                   .SingleInstance();
        }
    }
}
