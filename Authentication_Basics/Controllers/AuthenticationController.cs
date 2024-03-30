using Authentication_Basics.AuthrorizationRequirments;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication_Basics.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet("[action]")]
        public IActionResult GenerateJWTToken([FromQuery] string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = configuration["Authentication:AuthSecret"];
            if (secret == null)
                throw new Exception("Secret is null");

            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(DynamicPolicies.SecurityLevel,"6")
                }),
                Expires = DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
                Audience = "",
                Issuer = "",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(tokenHandler.WriteToken(token));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CreateCookie()
        {

            var grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob"),
            };

            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.DateOfBirth,"11/11/2000"),
                new Claim("DrivingLicense","A+"),
            };

            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");

            var userPrincipal = new ClaimsPrincipal([grandmaIdentity, licenseIdentity]);

            await HttpContext.SignInAsync("cookie", userPrincipal);

            return Ok();
        }

    }
}
