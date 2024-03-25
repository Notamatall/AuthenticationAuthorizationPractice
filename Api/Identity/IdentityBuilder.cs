using Microsoft.AspNetCore.Authentication;
using static Elfo.Round.Identity.BuilderExtensions;

namespace Api.Identity
{
	public class IdentityBuilder : IIdentityBuilder
	{
		public IdentityBuilder(AuthenticationBuilder authBuilder)
		{
			AuthBuilder = authBuilder;
			// IdentityConfigure = identityConfigure;
		}
		public AuthenticationBuilder AuthBuilder { get; }
	}
}
