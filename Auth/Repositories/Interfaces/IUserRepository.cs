using Auth.Models;
using Auth.ViewModels;

namespace Auth.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> Update(EditUserViewModel model);
        public Task<UnapprovedUserData> GetByIdAsync(string id);

    }
}
