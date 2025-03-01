using Auth.Models;
using Auth.Repositories.Interfaces;
using Auth.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Auth.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
