using Auth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Auth.Authorization
{
    public class RoleClaimsTransformation : IClaimsTransformation
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public RoleClaimsTransformation(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity is not ClaimsIdentity identity)
                return principal;

            // Prevent duplicate transformation
            if (identity.HasClaim("ClaimsTransformed", "true"))
                return principal;

            var user = await _userManager.GetUserAsync(principal);
            if (user == null) return principal;


            // Add role-based claims
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null) continue;

                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var claim in roleClaims)
                {
                    if (!identity.HasClaim(claim.Type, claim.Value))
                    {
                        identity.AddClaim(claim);
                    }
                }
            }

            identity.AddClaim(new Claim("ClaimsTransformed", "true"));

            return principal;
        }
    }
}