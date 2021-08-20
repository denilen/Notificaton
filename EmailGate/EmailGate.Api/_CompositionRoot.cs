using System.Reflection;
using Autofac;
using Commons.Logging;
using Commons.Logging.Autofac;

namespace EmailGate.Api
{
    internal static class CompositionRoot
    {
        public static void RegisterApplicationComponents(this ContainerBuilder builder)
        {
            builder.RegisterAssemblyModules(Assembly.Load("EmailGate.Application"));

            builder.RegisterLogger(LoggerFactory.Instance);
        }
    }
}
