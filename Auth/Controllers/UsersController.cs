using Auth.Enums;
using Auth.Interfaces;
using Auth.Models;
using Auth.Services;
using Auth.Services.Interfaces;
using Auth.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Cryptography;

namespace Auth.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRolesService _rolesService;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IUserDataChangeRequestService _changeRequestService;
        private readonly IEmailService _emailService;
        public UsersController(IEmailService emailService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor, IRolesService rolesService, IAuthService authService, IUserService userService, IUserDataChangeRequestService changeRequestService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _rolesService = rolesService;
            _authService = authService;
            _userService = userService;
            _changeRequestService = changeRequestService;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            return View(_userService.GetAll());
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Read(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _roleManager.Roles.ToListAsync();

            var userVM = new ReadUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.UserName,
                Role = roles.Select(r => r.Name).FirstOrDefault()
            };
            return View(userVM);
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            return View(_userService.LoadDataOfCreatePage());
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create(AddUserViewModel userVM)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            string changePasswordUrl = $"{baseUrl}/Identity/Account/Manage/ChangePassword";

            if (!ModelState.IsValid) return View(userVM);

            var result = await _userService.CreateUser(userVM, changePasswordUrl);
            if (!result)
                return View(userVM);
            
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRole = _rolesService.GetUserRoleByUserId(userId).Result;
            var loggedinUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var loggedinUserRole = _rolesService.GetUserRoleByUserId(loggedinUser.Id).Result;
            var roles = await _roleManager.Roles.ToListAsync();

            if (loggedinUserRole.Name == Roles.SuperAdmin.ToString())
            {
                var userVM = new EditUserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Username = user.UserName
                };
                return View(userVM);
            }
            else if (userRole.Name == Roles.Admin.ToString() && loggedinUser.Id == userId)
            {
                var userVM = new EditUserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Username = user.UserName
                };
                return View(userVM);
            }
            else if (userRole.Name == Roles.Employee.ToString() && loggedinUser.Id == userId)
            {
                var userVM = new EditUserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Username = user.UserName
                };
                TempData["PendingRequest"] = true;
                return View(userVM);
            }
            else
            {
                return Forbid();
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            if (TempData["PendingRequest"] != null && (bool)TempData["PendingRequest"] == true)
            {
                if (_changeRequestService.AddChangeRequest(model))
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            if (await _userService.UpdateUser(model))
            {
                return RedirectToAction(nameof(Index));
            } 
            return View(model);
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteAsync(string userId)
        {
            if (userId == _userManager.GetUserId(HttpContext.User))
                return RedirectToAction(nameof(Index));
            await _userService.DeleteUser(userId);
            return RedirectToAction(nameof(Index));

        }


        #region remote attribute
        public async Task<IActionResult> CheckEmail(string email)
        {
            if (await _userManager.FindByEmailAsync(email) == null)
                return Json(true);
            return Json(false);
        }
        public async Task<IActionResult> CheckUsername(string username)
        {
            if (await _userManager.FindByNameAsync(username) == null)
                return Json(true);
            return Json(false);
        }

        public async Task<IActionResult> CheckEmailInEdit(string email, string id)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && user != await _userManager.FindByIdAsync(id))
                return Json(false);
            return Json(true);
        }
        public async Task<IActionResult> CheckUsernameInEdit(string username, string id)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null && user != await _userManager.FindByIdAsync(id))
                return Json(false);
            return Json(true);
        }
        #endregion
    }
}
