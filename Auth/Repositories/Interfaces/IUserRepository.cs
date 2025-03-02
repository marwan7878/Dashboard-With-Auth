using Auth.Models;
using Auth.ViewModels;

namespace Auth.Repositories.Interfaces
{
    public interface IUserRepository
    {
        List<UserViewModel> GetAll();
        Task<bool> CreateAsync(AddUserViewModel model);
        Task<bool> Update(EditUserViewModel model);
        public Task<UnapprovedUserData> GetByIdAsync(string id);
        public Task<bool> DeleteAsync(string id);

    }
}
