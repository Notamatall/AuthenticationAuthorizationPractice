using Authentication_Basics.Authentication;
using Authentication_Basics.Authentication.JWT;
using Authentication_Basics.Controllers.Queries;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authentication_Basics.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private IIdentityService identityService;
        private JWTTokenFactory jwtTokenFactory;

        public AuthenticationController(IIdentityService identityService, JWTTokenFactory jwtTokenFactory)
        {
            this.identityService = identityService;
            this.jwtTokenFactory = jwtTokenFactory;
        }

        [HttpPost("[action]")]
        public IActionResult GenerateJWTToken([FromBody] AuthenticationModel model)
        {
            var token = jwtTokenFactory.GenerateToken(model.UserName);
            return Ok(token);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateCookie([FromBody] AuthenticationModel model)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.Name, model.UserName ?? string.Empty));
            var principal = new ClaimsPrincipal(identity);
            //var dbIdentity = GetIdentityFromDB(model.UserName!);
            //principal.AddIdentity(dbIdentity);

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

        private ClaimsIdentity GetIdentityFromDB(string username)
        {
            var user = identityService.GetUserInformation(username);
            var identity = identityService.CreateUserIdentity(user);
            return identity;
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
