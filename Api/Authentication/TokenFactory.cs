using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Elfo.Round.Identity.JWT
{
	public class JWTCreationOptions
	{
		/// <summary>
		/// It must be a string composed by at list 128bit (16 bytes)
		/// </summary>
		public string Secret { get; set; }
		public string Audience { get; set; }
		public string Issuer { get; set; }
		/// <summary>
		/// Default value is DateTime.UtcNow.AddDays(7)
		/// </summary>
		//public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(7);
		public TimeSpan Expires { get; set; } = TimeSpan.FromDays(7);
	}

	public class TokenFactory
	{
		private readonly JWTCreationOptions options;

		public TokenFactory(IOptions<JWTCreationOptions> optionsAccessor)
		{
			options = optionsAccessor.Value;

			if (string.IsNullOrEmpty(options.Secret))
				throw new ArgumentException($"{nameof(JWTCreationOptions.Secret)} cannot be empty!");
		}

		/// <summary>
		/// It generates an AccessToken for the username.
		/// If Expires date is not defined, it will use DateTime.UtcNow.AddDays(7) as default value
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public string GenerateAccessToken(string username)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(options.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.Name, username)
				}),
				Expires = DateTime.UtcNow.Add(options.Expires),
				Audience = options.Audience,
				Issuer = options.Issuer,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
