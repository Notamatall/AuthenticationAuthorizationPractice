using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Authentication
{

	public interface IRoundUserModel
	{
		string Email { get; set; }
		string FirstName { get; set; }
		int Id { get; set; }
		string LastName { get; set; }	
		List<string> Permissions { get; set; }
		string Username { get; set; }
		bool IsEnabled { get; set; }
	}
	public interface IIdentityService
	{
		IRoundUserModel GetUserInformation(string username);
		bool AreValidCredentials(string username, string password);
		void RefreshUser(string username);
		ClaimsIdentity CreateUserIdentity(IRoundUserModel user);
	}
	public class IdentityOptions
	{
		/// <summary>
		/// With this method you can parse the value HttpContext.User.Identity.Name, filtering it, in order to obtain the userName to check inside
		/// the round tables.
		/// It is useful for example in a Windows Authentication scenario likes: in the AD my name is elfo-2k\mario.rossi but in the database is just mario.rossi.
		/// If you don't specify this property, the entire value of HttpContext.User.Identity.Name will be use to perform the previous check.
		/// </summary>
		public Func<string, string> HowToObtainUsernameFromIdentityName { get; set; } = identityName => identityName;

		/// <summary>
		/// Default: false
		/// </summary>
		public bool AuthenticateDisabledUser { get; set; } = false;
		/// <summary>
		/// Default: true
		/// </summary>
		public bool AuthorizeUsersWithPermissionsOnly { get; set; } = true;
		public bool UseCache { get; set; } = true;
		public TimeSpan? UserCacheExpiration { get; set; } = TimeSpan.FromHours(1);

	}

	public class AuthenticationMiddleware
	{
		private readonly RequestDelegate next;

		public AuthenticationMiddleware(RequestDelegate next) =>
			this.next = next;

		public async Task InvokeAsync(HttpContext context, IIdentityService identityService, IOptions<IdentityOptions> options)
		{
			if (context.User.Identity.IsAuthenticated)
				FinishAuthentication(context, identityService, options.Value);

			await next(context);
		}

		private static void FinishAuthentication(HttpContext context, IIdentityService identityService,
			IdentityOptions options)
		{
			var user = identityService.GetUserInformation(context.User.Identity.Name);

			if (user == default || !user.IsEnabled && !options.AuthenticateDisabledUser)
				context.User = new ClaimsPrincipal(new ClaimsIdentity());
			else
			{
				var userIdentity = identityService.CreateUserIdentity(user);
				context.User.AddIdentity(userIdentity);
			}
		}
	}
}
