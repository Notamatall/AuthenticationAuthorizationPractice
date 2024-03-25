using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_Basics.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "cookie")]
    public class CookieController : ControllerBase
    {
        [HttpGet("[action]")]
        public IActionResult GetIsAuthenticatedByCookie()
        {
            return Ok("authorized with cookie");
        }

    }
}
