using System;
using System.Diagnostics.CodeAnalysis;
using Autofac.Extensions.DependencyInjection;
using Commons.Logging;
using Commons.Logging.Serilog;
using Helpers.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace EmailGate.Api
{
    public static class Program
    {
        [ExcludeFromCodeCoverage]
        public static void Main(string[] args)
        {
            var configuration = HostBuilderEx.CreateDefaultConfiguration(args).Build();

            LoggerFactory.Instance = CreateLoggerFactory(configuration);

            try
            {
                var host = CreateHostBuilder(args)
                           .UseWindowsService()
                           .Build();

                host.Run();
                LoggerFactory.Instance.CloseAndFlush();
            }
            catch (Exception ex)
            {
                LoggerFactory.Instance.Logger.Fatal(ex, "Host terminated unexpectedly");
                LoggerFactory.Instance.CloseAndFlush();
                Environment.Exit(-1);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return HostBuilderEx.CreateDefaultWebHost(args)
                                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                                .ConfigureWebHost(b => b.UseStartup<Startup>());
        }

        private static ILoggerFactory CreateLoggerFactory(IConfiguration configuration)
        {
            var loggerConfiguration = new LoggerConfiguration()
                                      .WithDefaults<Startup>()
                                      .ReadFrom.Configuration(configuration);

            return new SerilogLoggerFactory(loggerConfiguration);
        }
    }
}
