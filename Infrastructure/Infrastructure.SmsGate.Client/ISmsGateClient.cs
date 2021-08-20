using System.Threading.Tasks;
using Infrastructure.SmsGate.DataTypes;
using Refit;

namespace Infrastructure.SmsGate.Client
{
    public interface ISmsGateClient
    {
        [Post("/api/sms/v1/send")]
        Task SendAsync([Body] SendShortMessageCommand sendShortMessageCommand);
    }
}
