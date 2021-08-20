using System;
using System.Threading.Tasks;
using Commons.Nh;
using Dapper;
using SmsGate.Application.Nikita;
using SmsGate.Application.SmsProfi;

namespace SmsGate.Application
{
    public static class SmsProviderNames
    {
        public const string Nikita = "Nikita";
        public const string SmsProfi = "SmsProfi";
    }

    public static class QueueMessageTypeProvider
    {
        public static async Task<Type> GetType(string tenant)
        {
            const string sql = @"
select p.name
  from sms.tenants t
inner join sms.providers p on p.id = t.provider_id
where t.tenant = :tenant
";

            var connection = NhDatabaseSession.Current.Connection;

            var parameters = new
            {
                tenant
            };

            var providerName = await connection.QueryFirstOrDefaultAsync<string>(sql, parameters);

            return providerName switch
            {
                SmsProviderNames.Nikita => typeof(NikitaShortMessageQueueDTO),
                SmsProviderNames.SmsProfi => typeof(SmsProfiShortMessageQueueDTO),
                _ => throw new ArgumentOutOfRangeException($"Unknown sms provider '{providerName}' for tenant '{tenant}'")
            };
        }
    }
}
