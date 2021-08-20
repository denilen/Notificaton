using System.Threading.Tasks;

namespace SmsGate.Application.SmsProfi
{
    public class SmsProfiShortMessageConsumer : ShortMessageConsumer<SmsProfiShortMessageQueueDTO>
    {
        private readonly ISmsProfiShortMessageSender _smsProfiShortMessageSender;

        public SmsProfiShortMessageConsumer(ISmsProfiShortMessageSender smsProfiShortMessageSender,
                                            RepeatableSendMessageGuard repeatableSendMessageGuard)
            : base(repeatableSendMessageGuard)
        {
            _smsProfiShortMessageSender = smsProfiShortMessageSender;
        }

        protected override async Task Consume(SmsProfiShortMessageQueueDTO message)
        {
            await _smsProfiShortMessageSender.Send(message);
        }
    }
}
