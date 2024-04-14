using API.Authentication;
using API.Authentication.JWT;
using API.Controllers.Queries;
using API.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {

        private JWTTokenFactory jwtTokenFactory;

        public AuthenticationController(JWTTokenFactory jwtTokenFactory)
        {
            this.jwtTokenFactory = jwtTokenFactory;
        }

        [HttpPost("[action]")]
        public IActionResult GenerateJWTToken([FromBody] AuthenticationModel model)
        {
            var token = jwtTokenFactory.GenerateToken(model.UserName);
            return Ok(token);
        }

        [HttpGet("[action]")]
        [AuthorizationFilter(Policy = "googlePolicy")]
        public IActionResult GenerateJWTTokenWithGoogle()
        {
            var token = jwtTokenFactory.GenerateToken(HttpContext.User.Identity.Name);
            return Ok(token);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateCookie([FromBody] AuthenticationModel model)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.Name, model.UserName ?? string.Empty));
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(1)
                });

            return Ok();
        }


        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Unauthorized();
        }

        [HttpGet("forbidden")]
        public IActionResult GetForbidden()
        {
            return Forbid();
        }


    }
}
