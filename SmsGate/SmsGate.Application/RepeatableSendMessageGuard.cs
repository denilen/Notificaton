using System;
using System.Diagnostics.CodeAnalysis;
using Commons.Logging;
using Infrastructure.SmsGate.DataTypes;
using JetBrains.Annotations;
using SmsGate.Application.Handlers;
using StackExchange.Redis;

namespace SmsGate.Application
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class SendShortMessageHandlerConfiguration
    {
        public TimeSpan MessageTTL { get; set; }
    }

    public class RepeatableSendMessageGuard
    {
        private static readonly ILogger Logger = LoggerFactory.Create<SendShortMessageHandler>();

        private readonly SendShortMessageHandlerConfiguration _configuration;
        private readonly IConnectionMultiplexer _redis;

        public RepeatableSendMessageGuard(SendShortMessageHandlerConfiguration configuration,
                                          IConnectionMultiplexer redis)
        {
            _configuration = configuration;
            _redis         = redis;
        }

        public bool WasSent(SendShortMessageCommand sendShortMessageCommand)
        {
            var redisDb        = _redis.GetDatabase();
            var tenantId       = TenantId(sendShortMessageCommand);
            var sourceIdExists = redisDb.StringGet(tenantId);

            if (sourceIdExists == RedisValue.Null)
                return false;

            if (Logger.IsInfoEnabled)
            {
                Logger.Info("ShortMessage#{SourceId} was already sent to '{PhoneNumber}', ignored",
                            sendShortMessageCommand.SourceId, sendShortMessageCommand.PhoneNumber);
            }

            return true;
        }

        public void Put(SendShortMessageCommand sendShortMessageCommand)
        {
            var redisDb = _redis.GetDatabase();
            redisDb.StringSetAsync(TenantId(sendShortMessageCommand), true, _configuration.MessageTTL);
        }

        private static string TenantId(SendShortMessageCommand sendShortMessageCommand)
        {
            return $"sms_{sendShortMessageCommand.SourceId}_{sendShortMessageCommand.Tenant}_{sendShortMessageCommand.PhoneNumber}";
        }
    }
}
