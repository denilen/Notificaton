using System.Threading.Tasks;
using Commons.Logging;
using Infrastructure.SmsGate.DataTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmsGate.Application.Handlers;

namespace SmsGate.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Produces("application/json")]
    [Route("api/sms/v{version:apiVersion}")]
    public class SmsGateController : ControllerBase
    {
        private readonly ILogger _logger = LoggerFactory.Create<SmsGateController>();

        private readonly SendShortMessageHandler _shortMessageHandler;

        public SmsGateController(SendShortMessageHandler shortMessageHandler)
        {
            _shortMessageHandler = shortMessageHandler;
        }

        /// <summary>
        /// Отправить смс
        /// </summary>
        /// <param name="sendShortMessageCommand"></param>
        /// <returns></returns>
        [HttpPost, Route("send")]
        [ProducesResponseType(typeof(SendShortMessageCommand), StatusCodes.Status200OK)]
        public async Task<IActionResult> Send([FromBody] SendShortMessageCommand sendShortMessageCommand)
        {
            var logContext = new
            {
                sendShortMessageCommand.Tenant,
                sendShortMessageCommand.SourceId,
                sendShortMessageCommand.PhoneNumber
            };

            using (_logger.PushContext("ShortMessage", logContext))
            {
                await _shortMessageHandler.Handle(sendShortMessageCommand);
            }

            return Ok();
        }
    }
}
