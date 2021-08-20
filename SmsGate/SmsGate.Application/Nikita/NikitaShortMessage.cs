using JetBrains.Annotations;

namespace SmsGate.Application.Nikita
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class NikitaShortMessage
    {
        public string Login { get; set; } = null!;

        public string Pwd { get; set; } = null!;

        public string Id { get; set; } = null!;

        public string Sender { get; set; } = null!;

        public string Text { get; set; } = null!;

        public string Time { get; set; } = null!;

        public Phones Phones { get; set; } = null!;

        public int Test { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Phones
    {
        public string Phone { get; set; } = null!;
    }
}
