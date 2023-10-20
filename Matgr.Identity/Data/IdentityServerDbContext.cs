using Matgr.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Matgr.Identity.Data
{
    public class IdentityServerDbContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityServerDbContext(DbContextOptions options) : base(options) { }



    }
}
