using Auth.Enums;
using Auth.Models;
using Auth.Services.Interfaces;
using Auth.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRolesService _rolesService;
        private readonly IUserService _userService;
        private readonly IUserDataChangeRequestService _changeRequestService;
        public UsersController(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IRolesService rolesService, IUserService userService, IUserDataChangeRequestService changeRequestService)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _rolesService = rolesService;
            _userService = userService;
            _changeRequestService = changeRequestService;
        }

        public IActionResult Index()
        {
            return View(_userService.GetAll());
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Read(string userId)
        {
            return View(_userService.LoadDataOfReadPage(userId));
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
            var userRole = _rolesService.GetUserRoleByUserId(userId).Result;
            var loggedinUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var loggedinUserRole = _rolesService.GetUserRoleByUserId(loggedinUser.Id).Result;
            TempData["PendingRequest"] = false;

            if (loggedinUserRole.Name == Roles.SuperAdmin.ToString() && loggedinUser.Id == userId)
            {
                var model = _userService.LoadDataOfEditPage(userId);
                model.Roles = null;
                return View(model);
            }
            else if (loggedinUserRole.Name == Roles.SuperAdmin.ToString())
            {
                return View(_userService.LoadDataOfEditPage(userId));
            }
            else if (userRole.Name == Roles.Admin.ToString() && loggedinUser.Id == userId)
            {
                var model = _userService.LoadDataOfEditPage(userId);
                model.Roles = null;
                return View(model);
            }
            else if (userRole.Name == Roles.Employee.ToString() && loggedinUser.Id == userId)
            {
                TempData["PendingRequest"] = true;
                var model = _userService.LoadDataOfEditPage(userId);
                model.Roles = null;
                return View(model);
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
