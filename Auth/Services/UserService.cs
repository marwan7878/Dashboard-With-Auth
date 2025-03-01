using Auth.Interfaces;
using Auth.Models;
using Auth.Repositories.Interfaces;
using Auth.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> UpdateUser(EditUserViewModel model)
        {
            return _repository.Update(model);
        }
        public Task<UnapprovedUserData> GetUser(string id)
        {
            return _repository.GetByIdAsync(id);
        }
        public async Task<bool> DeleteUser(string id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
