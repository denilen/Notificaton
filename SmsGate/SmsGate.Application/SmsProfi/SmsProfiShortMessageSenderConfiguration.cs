using JetBrains.Annotations;

namespace SmsGate.Application.SmsProfi
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class SmsProfiShortMessageSenderConfiguration
    {
        public string BaseUrl { get; set; } = null!;

        public string Source { get; set; } = null!;

        public int Timeout { get; set; }

        public string Token { get; set; } = null!;

        public bool Test { get; set; }
    }
}
