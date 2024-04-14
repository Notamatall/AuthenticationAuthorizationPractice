using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace API.Authentication
{
    public class IdentityService : IIdentityService
    {
        private readonly IdentityOptions options;
        private readonly IDbUserRepository userRepository;


        public IdentityService(IDbUserRepository userRepository, IOptions<IdentityOptions> options)
        {
            this.options = options.Value;
            this.userRepository = userRepository;
        }
        public IDBUserModel GetUserInformation(string username)
        {
            return userRepository.GetUserInformation(username);

        }

        public virtual ClaimsIdentity CreateUserIdentity(IDBUserModel user)
        {
            var claims = new List<Claim>();

            Type userType = user.GetType();
            foreach (var prop in userType.GetProperties())
                if (prop.GetIndexParameters().Length == 0 && prop.Name != "Permissions")
                {
                    var value = prop.GetValue(user);
                    claims.Add(new Claim(
                        string.Concat(CustomClaimType.ApplyNamespace(prop.Name)),
                        value == null ? "" : value.ToString()!));
                }

            claims.AddRange(user.Permissions.Select(p => new Claim(CustomClaimType.Permission, p)));

            return new ClaimsIdentity(claims);
        }
    }

    public interface IIdentityService
    {
        public IDBUserModel GetUserInformation(string username);
        public ClaimsIdentity CreateUserIdentity(IDBUserModel user);
    }

    public class IdentityOptions
    {
        public TimeSpan UserCacheExpiration { get; set; } = TimeSpan.FromHours(1);
    }

}
