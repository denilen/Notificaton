using JetBrains.Annotations;

namespace EmailGate.Worker.Configuration
{
    [UsedImplicitly]
    public class RedisServiceConfiguration
    {
        public string Host { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int ExpirationTime { get; set; }
    }
}
