using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Authentication.JWT
{
    public interface ITokenFactory
    {
        public string GenerateToken(string username);
    }

    public class JWTTokenFactory : ITokenFactory
    {
        private JWTTokenCreationOptions options;

        public JWTTokenFactory(IOptions<JWTTokenCreationOptions> options)
        {
            this.options = options.Value;
        }

        public string GenerateToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = options.Secret;
            if (secret == null)
                throw new Exception("Secret is null");

            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
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
