using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmailGate.Application.Handlers;
using Infrastructure.EmailGate.DataTypes;

namespace EmailGate.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Produces("application/json")]
    [Route("api/email/v{version:apiVersion}/")]
    public class EmailGateController : ControllerBase
    {
        private readonly SendEmailMessageHandler _emailMessageHandler;

        public EmailGateController(SendEmailMessageHandler emailMessageHandler)
        {
            _emailMessageHandler = emailMessageHandler;
        }

        /// <summary>
        /// Отправить смс
        /// </summary>
        /// <param name="sendEmailMessageCommand"></param>
        /// <returns></returns>
        [HttpPost, Route("send")]
        [ProducesResponseType(typeof(SendEmailMessageCommand), StatusCodes.Status200OK)]
        public async Task<IActionResult> Send([FromBody] SendEmailMessageCommand sendEmailMessageCommand)
        {
            await _emailMessageHandler.Handle(sendEmailMessageCommand);

            return Ok();
        }
    }
}
