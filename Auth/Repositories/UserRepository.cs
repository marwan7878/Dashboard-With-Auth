using Auth.Models;
using Auth.Repositories.Interfaces;
using Auth.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Auth.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public List<UserViewModel> GetAll()
        {
            return _userManager.Users
                .Select(user => new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Username = user.UserName,
                    Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault()
                })
                .ToList();
        }
        public async Task<bool> CreateAsync(AddUserViewModel model)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };
                await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, model.Role);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> Update(EditUserViewModel model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                    return false;

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.Username;
                await _userManager.UpdateAsync(user);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<UnapprovedUserData> GetByIdAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                return new UnapprovedUserData
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Username = user.UserName
                };
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(user);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
