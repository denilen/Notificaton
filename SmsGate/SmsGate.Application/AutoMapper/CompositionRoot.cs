using Autofac;
using AutoMapper;
using JetBrains.Annotations;

namespace SmsGate.Application.AutoMapper
{
    [UsedImplicitly]
    public class CompositionRoot : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.PropertyType.IsPublic;
                cfg.AddMaps("SmsGate.Application");
            });

            builder.RegisterInstance(configuration.CreateMapper());
        }
    }
}
