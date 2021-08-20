using System;
using JetBrains.Annotations;

namespace SmsGate.Application.MassTransit
{
    [UsedImplicitly]
    public class MessageServiceBusConfiguration
    {
        public Uri HostUri { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string NikitaQueueName { get; set; } = null!;

        public string SmsProfiQueueName { get; set; } = null!;
    }
}
