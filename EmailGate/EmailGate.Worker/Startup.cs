using System;
using Autofac;
using EmailGate.Application.Email;
using EmailGate.Worker.Configuration;
using JetBrains.Annotations;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EmailGate.Application.MassTransit;
using StackExchange.Redis;

namespace EmailGate.Worker
{
    public class Startup
    {
        private static IConfiguration Configuration { get; set; } = null!;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            #region "MassTransit"

            services.AddMassTransit(x => x.ConfigureRabbitMqConsumer());
            services.AddMassTransitHostedService();

            #endregion

            #region "Redis"

            var redisConfiguration = Configuration.GetSection("Redis").Get<RedisServiceConfiguration>();
            var tenantCache        = ConnectionMultiplexer.Connect($"{redisConfiguration.Host},password={redisConfiguration.Password}");
            services.AddSingleton<IConnectionMultiplexer>(tenantCache);

            #endregion

            services.RegisterConfigurations(Configuration);

            services.AddHttpClient<EmailMessageSender>(client =>
            {
                client.BaseAddress = new Uri(ConfigurationFacade.EmailMessageSender.BaseUrl);
            });
        }

        [UsedImplicitly]
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterApplicationComponents();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
