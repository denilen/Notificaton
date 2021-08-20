using System;
using System.Threading.Tasks;
using AutoMapper;
using Commons.Logging;
using Commons.Nh;
using Infrastructure.SmsGate.DataTypes;
using MassTransit;
using NHibernate;

namespace SmsGate.Application.Handlers
{
    public class SendShortMessageHandler
    {
        private static readonly ILogger Logger = LoggerFactory.Create<SendShortMessageHandler>();

        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly ISessionFactory _sessionFactory;

        public SendShortMessageHandler(IPublishEndpoint publishEndpoint,
                                       IMapper mapper,
                                       ISessionFactory sessionFactory)
        {
            _publishEndpoint = publishEndpoint;
            _mapper          = mapper;
            _sessionFactory  = sessionFactory;
        }

        public async Task Handle(SendShortMessageCommand sendShortMessageCommand)
        {
            using (NhDatabaseSession.Bind(_sessionFactory))
            {
                Type dtoType = await QueueMessageTypeProvider.GetType(sendShortMessageCommand.Tenant);

                var dto = _mapper.Map(sendShortMessageCommand, typeof(SendShortMessageCommand), dtoType);
                await _publishEndpoint.Publish(dto, dtoType);

                if (Logger.IsInfoEnabled)
                {
                    Logger.Info("ShortMessage#{SourceId} {Tenant} was registered for sending to '{PhoneNumber}'",
                                sendShortMessageCommand.SourceId,
                                sendShortMessageCommand.Tenant,
                                sendShortMessageCommand.PhoneNumber);
                }
            }
        }
    }
}
