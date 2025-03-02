using Auth.Interfaces;
using Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Auth.Services
{
    public class RolesService : IRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public RolesService(RoleManager<IdentityRole> roleManager ,UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public List<SelectListItem> GetRolesInSelectList(string? selected)
        {
            var roles = _roleManager.Roles.ToList();
            return roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name,
                Selected = r.Id == selected
            }).ToList();
        }

        public async Task<IdentityRole> GetUserRoleByUserId(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roleName = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            var role = await _roleManager.FindByNameAsync(roleName);
            return role;
        }
    }
}
