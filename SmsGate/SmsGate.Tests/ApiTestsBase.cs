using System;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Commons.Tests.Extensions;
using Infrastructure.SmsGate.DataTypes;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SmsGate.Api.Controllers;
using SmsGate.Application.MassTransit;
using SmsGate.Application.Nikita;
using SmsGate.Application.SmsProfi;
using SmsGate.Tests.Configuration;
using SmsGate.Tests.Fixtures;
using SmsGate.Tests.TestHelpers;
using StackExchange.Redis;
using Xunit;

namespace SmsGate.Tests
{
    [Trait("Category", "qa")]
    [Collection(nameof(IntegrationTestsCollection))]
    public abstract class ApiTestsBase : IAsyncLifetime,
                                         IClassFixture<LoggerFixture>,
                                         IClassFixture<DatabaseConfigurationFixture>,
                                         IClassFixture<NhSessionFactoryFixture>
    {
        protected readonly AutoMock AutoMock;

        protected readonly NhSessionFactoryFixture NhSessionFactory = new();
        protected readonly NikitaShortMessageSenderSpy NikitaShortMessageSenderSpy = new();
        protected readonly SmsProfiShortMessageSenderSpy SmsProfiShortMessageSenderSpy = new();

        private readonly IContainer _container;
        private readonly IBusControl _bus;

        protected ApiTestsBase()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterAssemblyModules(Assembly.Load("SmsGate.Application"));
            containerBuilder.RegisterAssemblyModules(Assembly.Load("SmsGate.Api"));
            containerBuilder.RegisterControllers(Assembly.Load("SmsGate.Api"));

            containerBuilder.RegisterInstance<INikitaShortMessageSender>(NikitaShortMessageSenderSpy);
            containerBuilder.RegisterInstance<ISmsProfiShortMessageSender>(SmsProfiShortMessageSenderSpy);
            containerBuilder.RegisterConfigurations();

            containerBuilder.AddMassTransit(x => x.ConfigureRabbitMqConsumer(autoDelete: true));

            var tenantCache = ConnectionMultiplexer.Connect("localhost");
            containerBuilder.RegisterInstance<IConnectionMultiplexer>(tenantCache);

            _container = containerBuilder.Build();

            _bus = _container.Resolve<IBusControl>();

            AutoMock = AutoMock.GetLoose(cb => { cb.RegisterInstance(NhSessionFactory.Instance); });
        }

        public async Task InitializeAsync()
        {
            await _bus.StartAsync(TimeSpan.FromSeconds(2));
        }

        public async Task DisposeAsync()
        {
            await _bus.StopAsync();
        }

        protected async Task<IActionResult> SmsGateController_Send([FromBody] SendShortMessageCommand sendShortMessageCommand)
        {
            var controller = _container.Resolve<SmsGateController>();

            return await controller.Send(sendShortMessageCommand);
        }
    }
}
