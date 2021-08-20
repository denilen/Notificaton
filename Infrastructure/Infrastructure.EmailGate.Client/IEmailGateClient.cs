using System.Threading.Tasks;
using Infrastructure.EmailGate.DataTypes;
using Refit;

namespace Infrastructure.EmailGate.Client
{
    public interface IEmailGateClient
    {
        [Post("/api/email/v1/send")]
        Task SendAsync([Body] SendEmailMessageCommand sendEmailMessageCommand);
    }
}
