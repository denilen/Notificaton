using System.Threading.Tasks;
using Commons.Logging;
using Infrastructure.SmsGate.DataTypes;
using MassTransit;

namespace SmsGate.Application
{
    public abstract class ShortMessageConsumer<TMessage> : IConsumer<TMessage>
        where TMessage : SendShortMessageCommand
    {
        private static readonly ILogger Logger = LoggerFactory.Create<ShortMessageConsumer<TMessage>>();

        private readonly RepeatableSendMessageGuard _repeatableSendMessageGuard;

        protected ShortMessageConsumer(RepeatableSendMessageGuard repeatableSendMessageGuard)
        {
            _repeatableSendMessageGuard = repeatableSendMessageGuard;
        }

        public async Task Consume(ConsumeContext<TMessage> context)
        {
            var shortMessage = context.Message;

            using (Logger.PushContext("ShortMessage", new { shortMessage.SourceId, shortMessage.PhoneNumber }))
            {
                if (_repeatableSendMessageGuard.WasSent(shortMessage))
                {
                    if (Logger.IsInfoEnabled)
                    {
                        Logger.Info("ShortMessage#{SourceId} was already sent to '{PhoneNumber}', ignored",
                                    shortMessage.SourceId, shortMessage.PhoneNumber);
                    }

                    return;
                }

                await Consume(shortMessage);

                _repeatableSendMessageGuard.Put(shortMessage);
            }
        }

        protected abstract Task Consume(TMessage message);
    }
}
