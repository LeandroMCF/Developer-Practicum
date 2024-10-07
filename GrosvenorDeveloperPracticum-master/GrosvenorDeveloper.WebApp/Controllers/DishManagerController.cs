using Application;
using Application.Inputs;
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
        public ActionResult<string> GetMorningMenuMock()
        {
            var menu = _dishManager.SeeMorninMenuMock();
            return Ok(menu);
        }

        // GET: api/dishmanager/eveningmenu
        [HttpGet("mock/eveningmenu")]
        public ActionResult<string> GetEveningMenuMock()
        {
            var menu = _dishManager.SeeEveningMenuMock();
            return Ok(menu);
        }

        [HttpGet("seeMenu")]
        public async Task<IActionResult> SeeMenu()
        {
            var menu = await _dishManager.GetFullMenu();
            return Ok(menu);
        }

        [HttpPost("addDish")]
        public async Task<IActionResult> AddDish([FromBody] AddDish request)
        {
            await _dishManager.AddDishToMenu(request);
            return Ok("Dish added successfully");
        }
    }
}
