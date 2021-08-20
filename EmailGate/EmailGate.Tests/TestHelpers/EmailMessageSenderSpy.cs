using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using EmailGate.Application.Email;
using Infrastructure.EmailGate.DataTypes;

namespace EmailGate.Tests.TestHelpers
{
    public class EmailMessageSenderSpy : IEmailMessageSender
    {
        private readonly BlockingCollection<SendEmailMessageCommand> _receivedMessages = new();

        public Task Send(SendEmailMessageCommand emailMessage)
        {
            _receivedMessages.Add(emailMessage);

            return Task.CompletedTask;
        }

        public SendEmailMessageCommand? WaitMessage(TimeSpan? timeout = null)
        {
            timeout ??= TimeSpan.FromSeconds(2);

            return _receivedMessages.TryTake(out var notification, timeout.Value)
                ? notification
                : null;
        }
    }
}
