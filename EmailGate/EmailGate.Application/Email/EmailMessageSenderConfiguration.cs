using JetBrains.Annotations;

namespace EmailGate.Application.Email
{
    [UsedImplicitly]
    public class EmailMessageSenderConfiguration
    {
        public string BaseUrl { get; set; } = null!;

        public int BasePort { get; set; }

        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        public bool UseSsl { get; set; }

        public string Sender { get; set; } = null!;
    }
}
