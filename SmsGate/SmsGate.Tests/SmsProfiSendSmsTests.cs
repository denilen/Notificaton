using System;
using System.Threading.Tasks;
using FluentAssertions;
using Infrastructure.SmsGate.DataTypes;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace SmsGate.Tests
{
    public class SmsProfiSendSmsTests : ApiTestsBase
    {
        [Fact]
        public async Task ItShouldSendSmsViaSmsProfi()
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

            var message = SmsProfiShortMessageSenderSpy.WaitMessage();

            message.Should().BeEquivalentTo(sendShortMessageCommand, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task ItShouldNotSendSameSmsViaSmsProfiTwice()
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
            var message = NikitaShortMessageSenderSpy.WaitMessage();

            message.Should().BeNull();
        }
    }
}
