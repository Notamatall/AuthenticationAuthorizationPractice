using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Identity.Data
{
    //identitydbcontext contains all the user tables
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {

        }
    }
}
