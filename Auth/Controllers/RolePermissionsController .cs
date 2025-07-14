using Auth.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    public class RolePermissionsController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRolePermissionService _service;

        public RolePermissionsController(RoleManager<IdentityRole> roleManager, IRolePermissionService service)
        {
            _roleManager = roleManager;
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public async Task<IActionResult> Manage(string roleName)
        {
            var allPermissions = PermissionScanner.GetAllActionPermissions();
            var currentPermissions = await _service.GetPermissionsByRoleAsync(roleName);

            ViewBag.Role = roleName;

            var model = allPermissions.Select(p => new PermissionViewModel
            {
                Name = p,
                Selected = currentPermissions.Contains(p)
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(string roleName, List<PermissionViewModel> permissions)
        {
            var selected = permissions.Where(p => p.Selected).Select(p => p.Name).ToList();
            await _service.UpdateRolePermissionsAsync(roleName, selected);
            return RedirectToAction("Index");
        }
    }

}
