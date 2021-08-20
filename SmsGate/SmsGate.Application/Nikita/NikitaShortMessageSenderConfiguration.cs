using JetBrains.Annotations;

namespace SmsGate.Application.Nikita
{
    [UsedImplicitly]
    public class NikitaShortMessageSenderConfiguration
    {
        public string BaseUrl { get; set; } = null!;

        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Sender { get; set; } = null!;

        public int Test { get; set; }
    }
}
