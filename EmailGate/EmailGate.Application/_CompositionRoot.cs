using Autofac;
using JetBrains.Annotations;
using Module = Autofac.Module;

namespace EmailGate.Application
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
