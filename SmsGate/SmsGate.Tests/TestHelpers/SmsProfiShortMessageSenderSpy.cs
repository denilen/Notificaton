using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Infrastructure.SmsGate.DataTypes;
using SmsGate.Application.SmsProfi;

namespace SmsGate.Tests.TestHelpers
{
    public class SmsProfiShortMessageSenderSpy : ISmsProfiShortMessageSender
    {
        private readonly BlockingCollection<SmsProfiShortMessageQueueDTO> _receivedMessages = new();

        public Task Send(SmsProfiShortMessageQueueDTO shortMessage)
        {
            _receivedMessages.Add(shortMessage);

            return Task.CompletedTask;
        }

        public SendShortMessageCommand? WaitMessage(TimeSpan? timeout = null)
        {
            timeout ??= TimeSpan.FromSeconds(2);

            return _receivedMessages.TryTake(out var notification, timeout.Value)
                ? notification
                : null;
        }
    }
}
