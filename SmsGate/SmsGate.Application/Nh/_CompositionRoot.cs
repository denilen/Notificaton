using Autofac;
using JetBrains.Annotations;

namespace SmsGate.Application.Nh
{
    [UsedImplicitly]
    public class CompositionRoot : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ => SessionFactory.Instance);
        }
    }
}
