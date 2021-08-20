using Autofac;
using JetBrains.Annotations;
using Module = Autofac.Module;

namespace SmsGate.Application.Nikita
{
    [UsedImplicitly]
    public class CompositionRoot : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NikitaShortMessageConsumer>()
                   .SingleInstance();

            builder.RegisterType<NikitaShortMessageSender>()
                   .As<INikitaShortMessageSender>()
                   .SingleInstance();
        }
    }
}
