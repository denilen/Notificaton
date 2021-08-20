using JetBrains.Annotations;

namespace SmsGate.Worker.Configuration
{
    [UsedImplicitly]
    public class RedisServiceConfiguration
    {
        public string Host { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
