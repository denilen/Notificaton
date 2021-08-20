using System.Threading.Tasks;
using Commons.Logging;
using Infrastructure.EmailGate.DataTypes;
using MassTransit;

namespace EmailGate.Application.Handlers
{
    public class SendEmailMessageHandler
    {
        private static readonly ILogger Logger = LoggerFactory.Create<SendEmailMessageHandler>();

        private readonly IPublishEndpoint _publishEndpoint;

        public SendEmailMessageHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(SendEmailMessageCommand sendEmailMessageCommand)
        {


            await _publishEndpoint.Publish(sendEmailMessageCommand);

            if (Logger.IsDebugEnabled)
                Logger.Debug("EmailMessage#{EmailMessageSourceId} was sent", sendEmailMessageCommand.SourceId);
        }
    }
}
