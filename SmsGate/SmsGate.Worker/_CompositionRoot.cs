using System.Reflection;
using Autofac;
using Commons.Logging;
using Commons.Logging.Autofac;

namespace SmsGate.Worker
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
