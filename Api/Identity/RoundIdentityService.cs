using Api.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Elfo.Round.Identity
{

	public class RoundUserModel : IRoundUserModel
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string LanguageCode { get; set; }
		public string CultureCode { get; set; }
		public string Email { get; set; }
		public List<string> Permissions { get; set; } = new List<string>();
		public bool IsEnabled { get; set; }
	}

	public interface IUserRepository
	{
		IRoundUserModel GetUserInformation(string username);
		bool AreValidCredentials(string username, string password);
	}

	/// <summary>
	/// Identity service allows to create indentity and provide it with claims.
	/// Identity is an important essence related to User bounded to HttpContext
	/// </summary>
	public class RoundIdentityService : IIdentityService
	{
		//private const string userInformationCacheKeySuffix = "_userInformation";

		private readonly IUserRepository users;
		private readonly IdentityOptions options;

		public RoundIdentityService(IOptions<IdentityOptions> options, IUserRepository users)
		{
			this.users = users;
			this.options = options.Value;
		}

		protected virtual IRoundUserModel AcquireUserInformation(string username)
		{
			return users.GetUserInformation(username);
		}

        public virtual IRoundUserModel GetUserInformation(string username)
        {
            username = options.HowToObtainUsernameFromIdentityName(username);

			return AcquireUserInformation(username);
			//return options.UseCache
   //             ? cache.Get(GetCacheKey(username), () => AcquireUserInformation(username), options.UserCacheExpiration)
   //             : AcquireUserInformation(username);
        }

        public virtual bool AreValidCredentials(string username, string password) =>
			users.AreValidCredentials(username, password);

        public virtual void RefreshUser(string username)
        {
			if (options.UseCache)
				return;
                //cache.Remove(GetCacheKey(username));
        }

        public virtual ClaimsIdentity CreateUserIdentity(IRoundUserModel user)
		{
			var claims = new List<Claim>();

			Type userType = user.GetType();
			foreach (var prop in userType.GetProperties())
				if (prop.GetIndexParameters().Length == 0 && prop.Name != "Permissions")
				{
					var value = prop.GetValue(user);
					claims.Add(new Claim(
						string.Concat(RoundIdentityClaimType.ApplyNamespace(prop.Name)),
						value == null ? "" : value.ToString()));
				}

			claims.AddRange(user.Permissions.Select(p => new Claim(RoundIdentityClaimType.Permission, p)));

			return new ClaimsIdentity(claims);
		}

		//protected virtual string GetCacheKey(string username) =>
		//	$"{username.ToLower()}{userInformationCacheKeySuffix}";
	}
}
