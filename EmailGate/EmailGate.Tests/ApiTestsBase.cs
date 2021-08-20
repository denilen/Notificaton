using System;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Commons.Tests.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using EmailGate.Api;
using EmailGate.Api.Controllers;
using EmailGate.Application.Handlers;
using EmailGate.Application.Email;
using EmailGate.Application.MassTransit;
using EmailGate.Tests.Configuration;
using EmailGate.Tests.Fixtures;
using EmailGate.Tests.TestHelpers;
using Infrastructure.EmailGate.DataTypes;
using StackExchange.Redis;
using Xunit;

namespace EmailGate.Tests
{
    [Trait("Category", "qa")]
    [Collection(nameof(IntegrationTestsCollection))]
    public abstract class ApiTestsBase : IAsyncLifetime,
                                         IClassFixture<LoggerFixture>
    {
        protected readonly EmailMessageSenderSpy EmailMessageSenderSpy = new();

        private readonly IContainer _container;
        private readonly IBusControl _bus;

        protected ApiTestsBase()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterAssemblyModules(Assembly.Load("EmailGate.Application"));
            containerBuilder.RegisterAssemblyModules(Assembly.Load("EmailGate.Api"));
            containerBuilder.RegisterControllers(Assembly.Load("EmailGate.Api"));

            containerBuilder.RegisterInstance<IEmailMessageSender>(EmailMessageSenderSpy);
            containerBuilder.RegisterConfigurations();

            containerBuilder.AddMassTransit(x => x.ConfigureRabbitMqConsumer(autoDelete: true));

            var tenantCache = ConnectionMultiplexer.Connect("localhost");
            containerBuilder.RegisterInstance<IConnectionMultiplexer>(tenantCache);

            _container = containerBuilder.Build();

            _bus = _container.Resolve<IBusControl>();
        }

        public async Task InitializeAsync()
        {
            await _bus.StartAsync(TimeSpan.FromSeconds(2));
        }

        public async Task DisposeAsync()
        {
            await _bus.StopAsync();
        }

        protected async Task<IActionResult> EmailGateController_Send([FromBody] SendEmailMessageCommand sendEmailMessageCommand)
        {
            var controller = _container.Resolve<EmailGateController>();

            return await controller.Send(sendEmailMessageCommand);
        }
    }
}
