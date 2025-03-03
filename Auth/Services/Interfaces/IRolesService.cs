using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Auth.Services.Interfaces
{
    public interface IRolesService
    {
        Task<IdentityRole> GetUserRoleByUserId(string userId);
        List<SelectListItem> GetRolesInSelectList(string? selected);
    }
}
