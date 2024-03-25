using Api.Authentication;
using Elfo.Round.Identity.JWT;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Api.Controllers
{
	public class AuthenticateUserModel
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
	public class TokenResult
	{
		public string AccessToken { get; set; }
	}
	[Route("api/authentication")]
	public class RoundController : ControllerBase
	{
		private readonly IIdentityService identityService;
		private readonly TokenFactory tokenFactory;

		public RoundController(IIdentityService identityService, TokenFactory tokenFactory)
		{
			this.identityService = identityService;
			this.tokenFactory = tokenFactory;
		}

		[HttpPost("[action]")]
		[AllowAnonymous]
		public IActionResult GetAccessToken([FromBody] AuthenticateUserModel userParam)
		{
			if (string.IsNullOrEmpty(userParam.Username) || string.IsNullOrEmpty(userParam.Password))
				return BadRequest();

			if (!identityService.AreValidCredentials(userParam.Username, userParam.Password))
				return BadRequest();

			var result = new TokenResult
			{
				AccessToken = tokenFactory.GenerateAccessToken(userParam.Username)
			};

			return Ok(result);
		}
	}

}
