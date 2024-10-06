using Application;
using Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrosvenorDeveloper.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishManagerController : ControllerBase
    {
        private readonly IDishManager _dishManager;

        public DishManagerController(IDishManager dishManager)
        {
            _dishManager = dishManager;
        }

        [HttpGet("mock/morningmenu")]
        public ActionResult<string> GetMorningMenu()
        {
            var menu = _dishManager.SeeMorninMenu();
            return Ok(menu);
        }

        // GET: api/dishmanager/eveningmenu
        [HttpGet("mock/eveningmenu")]
        public ActionResult<string> GetEveningMenu()
        {
            var menu = _dishManager.SeeEveningMenu();
            return Ok(menu);
        }

        [HttpPost("mock/order")]
        public ActionResult<List<Dish>> PlaceOrder([FromBody] Order order)
        {
            try
            {
                var dishes = _dishManager.GetDishes(order);
                return Ok(dishes);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
