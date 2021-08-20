using System;
using JetBrains.Annotations;

namespace EmailGate.Application.MassTransit
{
    [UsedImplicitly]
    public class MessageServiceBusConfiguration
    {
        public Uri HostUri { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string EmailQueueName { get; set; } = null!;
    }
}
