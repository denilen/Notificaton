using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.EmailGate.DataTypes;
using Xunit;

namespace EmailGate.Tests
{
    public class SendEmailTest : ApiTestsBase
    {
        [Fact]
        public async Task ItShouldSendEmail()
        {
            //arrange
            var random = new Random(DateTime.Now.Millisecond);

            var sendEmailMessageCommand = new SendEmailMessageCommand
            {
                SourceId = random.Next(),
                Tenant   = "www.tennisi.kg",
                Address  = "tennisi.dev@yandex.ru",
                Message  = "Test Message"
            };

            //act
            var actionResult = await EmailGateController_Send(sendEmailMessageCommand);

            //assert
            actionResult.Should().BeOfType<OkResult>();

            var emailMessage = EmailMessageSenderSpy.WaitMessage();

            emailMessage.Should().BeEquivalentTo(sendEmailMessageCommand);
        }

        [Fact]
        public async Task ItShouldNotSendSameEmail()
        {
            //arrange
            var random = new Random(DateTime.Now.Millisecond);

            var sendEmailMessageCommand = new SendEmailMessageCommand
            {
                SourceId = random.Next(),
                Tenant   = "www.tennisi.kg",
                Address  = "tennisi.dev@yandex.ru",
                Message  = "Test Message"
            };

            await EmailGateController_Send(sendEmailMessageCommand);
            EmailMessageSenderSpy.WaitMessage();

            //act
            await EmailGateController_Send(sendEmailMessageCommand);

            //assert
            var emailMessage = EmailMessageSenderSpy.WaitMessage();

            emailMessage.Should().BeNull();
        }
    }
}
