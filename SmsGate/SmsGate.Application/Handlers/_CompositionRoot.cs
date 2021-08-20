using Autofac;
using JetBrains.Annotations;

namespace SmsGate.Application.Handlers
{
    [UsedImplicitly]
    public class CompositionRoot : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SendShortMessageHandler>()
                   .SingleInstance();
        }
    }
}
