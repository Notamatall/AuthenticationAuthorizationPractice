using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Authentication_Basics.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;
        public HomeController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }
        [HttpGet("[action]")]
        public IActionResult Home()
        {
            return this.Ok("home");
        }

        [HttpGet("[action]")]
        [Authorize]
        public IActionResult Secret()
        {
            return this.Ok("secret");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Authenticate()
        {
            return this.Ok("Authenticate");
        }

    }
}
