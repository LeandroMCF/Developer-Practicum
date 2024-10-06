using Application;
using Microsoft.AspNetCore.Mvc;

namespace GrosvenorDeveloper.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly IServer _server;

        public ServerController(IServer server)
        {
            _server = server;
        }

        [HttpPost("mock/takeorder")]
        public ActionResult<string> TakeOrder([FromBody] string unparsedOrder)
        {
            try
            {
                var result = _server.TakeOrder(unparsedOrder);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
