using Authentication_Basics.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Authentication_Basics.Controllers
{
    [Route("[controller]")]
    public class AuthorizationController : ControllerBase
    {
        [HttpGet("[action]")]
        [AuthorizationFilter]
        public IActionResult GetIsAuthenticatedByJWT()
        {
            return Ok("authorized with jwt");
        }

        [HttpGet("[action]")]
        [AuthorizationFilter(Policy = "cookiePolicy")]
        public IActionResult GetIsAuthenticatedByCookie()
        {
            return Ok("authorized with cookie");
        }

        [HttpGet("getWithAny")]
        [AuthorizationFilter]

        public IActionResult GetWithAny()
        {
            return Ok(new { Message = $"Hello to Code Maze {GetUsername()}" });
        }

        private string? GetUsername()
        {
            return HttpContext.User.Claims
                .Where(x => x.Type == ClaimTypes.Name)
                .Select(x => x.Value)
                .FirstOrDefault();
        }

    }
}
