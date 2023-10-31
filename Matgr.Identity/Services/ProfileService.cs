using Duende.IdentityServer;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Matgr.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;

namespace Matgr.Identity.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly ILogger _logger;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            //_logger = logger;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // Log or print to console
            Console.WriteLine("GetProfileDataAsync is called.");
            //_logger.LogInformation("GetProfileDataAsync is called.");
            string sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);

            List<Claim> claims = new();
            claims = claims.Where(claim => context.RequestedClaimTypes
            .Contains(claim.Type)).ToList();

            claims.Add(new Claim(JwtClaimTypes.Name, user.UserName));
            claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));

            IList<string> roles = await _userManager.GetRolesAsync(user);
            foreach (var roleName in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, roleName));
            }

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            string sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
