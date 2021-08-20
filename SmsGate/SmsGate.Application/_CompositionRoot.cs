using Autofac;
using JetBrains.Annotations;
using Module = Autofac.Module;

namespace SmsGate.Application
{
    [UsedImplicitly]
    public class CompositionRoot : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RepeatableSendMessageGuard>()
                   .SingleInstance();
        }
    }
}
