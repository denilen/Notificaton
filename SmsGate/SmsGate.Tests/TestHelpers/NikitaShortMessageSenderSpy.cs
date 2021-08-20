using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Infrastructure.SmsGate.DataTypes;
using SmsGate.Application.Nikita;

namespace SmsGate.Tests.TestHelpers
{
    public class NikitaShortMessageSenderSpy : INikitaShortMessageSender
    {
        private readonly BlockingCollection<NikitaShortMessageQueueDTO> _receivedMessages = new();

        public Task Send(NikitaShortMessageQueueDTO shortMessage)
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
