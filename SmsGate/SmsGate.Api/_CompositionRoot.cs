using System.Reflection;
using Autofac;
using Commons.Logging;
using Commons.Logging.Autofac;

namespace SmsGate.Api
{
    public static class CompositionRoot
    {
        public static void RegisterApplicationComponents(this ContainerBuilder builder)
        {
            builder.RegisterAssemblyModules(Assembly.Load("SmsGate.Application"));

            builder.RegisterLogger(LoggerFactory.Instance);
        }
    }
}
