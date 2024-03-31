using System;

namespace Authentication_Basics.Authentication.JWT
{
    public interface ITokenCreationOptions
    {
        public string Secret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public TimeSpan Expires { get; set; }
    }

    public class JWTTokenCreationOptions: ITokenCreationOptions
    {
        public string Secret { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public TimeSpan Expires { get; set; } = TimeSpan.FromDays(7);
    }
}
