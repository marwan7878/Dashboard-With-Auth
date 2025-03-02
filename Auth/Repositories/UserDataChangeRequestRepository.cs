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

        public async Task<bool> SaveAsync(UnapprovedUserData model)
        {
            try
            {
                _context.UnapprovedUsers.Remove(model);
                await _context.UnapprovedUsers.AddAsync(model);
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
        public async Task<bool> Delete(string id)
        {
            try
            {
                var model = GetById(id);
                _context.UnapprovedUsers.Remove(model);
                _context.SaveChanges();
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
