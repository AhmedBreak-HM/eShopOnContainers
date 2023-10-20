using Microsoft.AspNetCore.Identity;

namespace Matgr.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FristName { get; set; }
        public string? LastName { get; set; }
    }
}
