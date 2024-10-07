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
        public ActionResult<string> TakeOrderMock([FromBody] string unparsedOrder)
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

        [HttpPost("takeOrder")]
        public async Task<IActionResult> TakeOrder([FromBody] string request)
        {
            try
            {
                var result = await _server.TakeOrderFromDb(request);
                return Ok(result);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
