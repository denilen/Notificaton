using System;
using Commons.Logging;
using EmailGate.Application.Handlers;
using Infrastructure.EmailGate.DataTypes;
using StackExchange.Redis;

namespace EmailGate.Application
{

    public class SendEmailMessageHandlerConfiguration
    {
        public TimeSpan MessageTTL { get; set; }
    }

    public class RepeatableSendMessageGuard
    {
        private static readonly ILogger Logger = LoggerFactory.Create<SendEmailMessageHandler>();
        private readonly SendEmailMessageHandlerConfiguration _configuration;
        private readonly IConnectionMultiplexer _redis;


        public RepeatableSendMessageGuard(SendEmailMessageHandlerConfiguration configuration,
                           IConnectionMultiplexer redis)
        {
            _configuration = configuration;
            _redis         = redis;
        }

        public bool WasSent(SendEmailMessageCommand sendEmailMessageCommand)
        {
            var redisDb        = _redis.GetDatabase();
            var tenantId       = TenantId(sendEmailMessageCommand);
            var sourceIdExists = redisDb.StringGet(tenantId);

            return sourceIdExists != RedisValue.Null;
        }

        public void Put(SendEmailMessageCommand sendEmailMessageCommand)
        {
            var redisDb = _redis.GetDatabase();
            redisDb.StringSetAsync(TenantId(sendEmailMessageCommand), true, _configuration.MessageTTL);
        }

        private static string TenantId(SendEmailMessageCommand sendEmailMessageCommand)
        {
            return $"sms_{sendEmailMessageCommand.SourceId}_{sendEmailMessageCommand.Tenant}_{sendEmailMessageCommand.Address}";
        }
    }
}
