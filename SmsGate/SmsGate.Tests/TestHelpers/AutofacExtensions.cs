using System.Linq;
using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Mvc;

namespace SmsGate.Tests.TestHelpers
{
    public static class AutofacExtensions
    {
        public static void RegisterControllers(this ContainerBuilder cb, Assembly assembly)
        {
            foreach (var controllerType in assembly.GetTypes().Where(t => t.IsAssignableTo<ControllerBase>()))
            {
                cb.RegisterType(controllerType).SingleInstance();
            }
        }
    }
}
