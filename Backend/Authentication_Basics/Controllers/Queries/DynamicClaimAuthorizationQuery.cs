using System.Collections.Generic;
using System.Security.Claims;

namespace Authentication_Basics.Controllers.Queries
{
    public class DynamicClaimAuthorizationQuery
    {
        public string AuthSchema { get; set; }
        public List<ClaimDTO> Claims { get; set; }
    }

    public class ClaimDTO
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
