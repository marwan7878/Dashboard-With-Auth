using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Auth.Authorization
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolePermissionService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<string>> GetPermissionsByRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            var claims = await _roleManager.GetClaimsAsync(role);
            return claims
                .Where(c => c.Type == "Permission")
                .Select(c => c.Value)
                .ToList();
        }

        public async Task<bool> UpdateRolePermissionsAsync(string roleName, List<string> selectedPermissions)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            var existingClaims = await _roleManager.GetClaimsAsync(role);

            // Remove old permissions
            foreach (var claim in existingClaims.Where(c => c.Type == "Permission"))
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            // Add selected ones
            foreach (var permission in selectedPermissions)
            {
                await _roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }

            return true;
        }
    }

}
