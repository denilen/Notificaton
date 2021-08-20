using System;
using System.Threading.Tasks;
using FluentAssertions;
using Infrastructure.SmsGate.DataTypes;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace SmsGate.Tests
{
    [Trait("Category", "integration")]
    public class NikitaSendSmsTests : ApiTestsBase
    {
        [Fact(Skip = "You need switch sms provider for this tenant in sms.tenants table")]
        public async Task ItShouldSendSmsViaNikita()
        {
            //arrange
            var random = new Random(DateTime.Now.Millisecond);

            var sendShortMessageCommand = new SendShortMessageCommand
            {
                SourceId    = random.Next(),
                Tenant      = "www.tennisi.kg",
                PhoneNumber = "996700111222",
                Message     = "Test Message"
            };

            //act
            var actionResult = await SmsGateController_Send(sendShortMessageCommand);

            //assert
            actionResult.Should().BeOfType<OkResult>();

            var nikitaMessage = NikitaShortMessageSenderSpy.WaitMessage();

            nikitaMessage.Should().BeEquivalentTo(sendShortMessageCommand, options => options.ExcludingMissingMembers());
        }

        [Fact(Skip = "You need switch sms provider for this tenant in sms.tenants table")]
        public async Task ItShouldNotSendSameSmsViaNikitaTwice()
        {
            //arrange
            var random = new Random(DateTime.Now.Millisecond);

            var sendShortMessageCommand = new SendShortMessageCommand
            {
                SourceId    = random.Next(),
                Tenant      = "www.tennisi.kg",
                PhoneNumber = "996700111222",
                Message     = "Test Message"
            };

            await SmsGateController_Send(sendShortMessageCommand);
            NikitaShortMessageSenderSpy.WaitMessage();

            //act
            await SmsGateController_Send(sendShortMessageCommand);

            //assert
            var nikitaMessage = NikitaShortMessageSenderSpy.WaitMessage();

            nikitaMessage.Should().BeNull();
        }
    }
}
