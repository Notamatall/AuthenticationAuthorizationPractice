using Authentication_Basics.CustomServices.KnightService;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_Basics.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RandomTestsController : ControllerBase
    {
        private readonly IKnightFactory knightFactory;

        public RandomTestsController(IKnightFactory knightFactory)
        {
            this.knightFactory = knightFactory;
        }

        [HttpGet("[action]")]
        public IActionResult TestCustomService()
        {
            var knight = knightFactory.CreateKnight();
            return Ok(knight);
        }

    }
}
