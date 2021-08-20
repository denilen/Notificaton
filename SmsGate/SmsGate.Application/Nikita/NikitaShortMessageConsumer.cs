using System.Threading.Tasks;

namespace SmsGate.Application.Nikita
{
    public class NikitaShortMessageConsumer : ShortMessageConsumer<NikitaShortMessageQueueDTO>
    {
        private readonly INikitaShortMessageSender _nikitaShortMessageSender;

        public NikitaShortMessageConsumer(INikitaShortMessageSender nikitaShortMessageSender,
                                          RepeatableSendMessageGuard repeatableSendMessageGuard)
            : base(repeatableSendMessageGuard)
        {
            _nikitaShortMessageSender = nikitaShortMessageSender;
        }

        protected override async Task Consume(NikitaShortMessageQueueDTO message)
        {
            await _nikitaShortMessageSender.Send(message);
        }
    }
}
