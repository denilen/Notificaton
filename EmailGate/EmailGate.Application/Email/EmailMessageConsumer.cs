using System.Threading.Tasks;
using MassTransit;
using Commons.Logging;
using Infrastructure.EmailGate.DataTypes;

namespace EmailGate.Application.Email
{
    public class EmailMessageConsumer : IConsumer<SendEmailMessageCommand>
    {
        private static readonly ILogger Logger = LoggerFactory.Create<EmailMessageConsumer>();

        private readonly IEmailMessageSender _emailMessageSender;
        private readonly RepeatableSendMessageGuard _repeatableSendMessageGuard;

        public EmailMessageConsumer(IEmailMessageSender emailMessageSender,
                                    RepeatableSendMessageGuard repeatableSendMessageGuard)
        {
            _emailMessageSender              = emailMessageSender;
            _repeatableSendMessageGuard = repeatableSendMessageGuard;
        }

        public async Task Consume(ConsumeContext<SendEmailMessageCommand> context)
        {
            var emailMessage = context.Message;

            using (Logger.PushContext("EmailMessage", new {emailMessage.SourceId, emailMessage.Address}))
            {
                if (_repeatableSendMessageGuard.WasSent(emailMessage))
                {
                    if (Logger.IsInfoEnabled)
                    {
                        Logger.Info("EmailMessage#{SourceId} was already sent to '{Address}', ignored",
                                    emailMessage.SourceId, emailMessage.Address);
                    }

                    return;
                }

                await _emailMessageSender.Send(emailMessage);

                _repeatableSendMessageGuard.Put(emailMessage);
            }
        }
    }
}
