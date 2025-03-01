using Auth.Models;
using Auth.Repositories.Interfaces;
using Auth.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Repositories
{
    public class UserDataChangeRequestRepository : IUserDataChangeRequestRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserDataChangeRequestRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> SaveAsync(EditUserViewModel model)
        {
            try
            {
                UnapprovedUserData unApprovedUserData = new UnapprovedUserData
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Username = model.Username
                };
                _context.UnapprovedUsers.Remove(unApprovedUserData);
                await _context.UnapprovedUsers.AddAsync(unApprovedUserData);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<UnapprovedUserData> GetAll()
        {
            return _context.UnapprovedUsers.ToList();
        }
        public bool Delete(string id)
        {
            try
            {
                _context.UnapprovedUsers.Remove(GetById(id));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public UnapprovedUserData GetById(string id)
        {
            return _context.UnapprovedUsers.FirstOrDefault(u => u.Id == id);
        }
    }
}
