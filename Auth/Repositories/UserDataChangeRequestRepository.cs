using Auth.Models;
using Auth.Models.Contexts;
using Auth.Repositories.Interfaces;
using Auth.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Repositories
{
    public class UserDataChangeRequestRepository : IUserDataChangeRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public UserDataChangeRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveAsync(UnapprovedUserData model)
        {
            try
            {
                var oldRequest = _context.UnapprovedUsers.FirstOrDefaultAsync(u => u.Id == model.Id).Result;
                if (oldRequest != null)
                    _context.UnapprovedUsers.Remove(oldRequest);
                _context.UnapprovedUsers.Add(model);
                _context.SaveChanges();
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
