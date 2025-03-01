using Auth.Models;
using Auth.ViewModels;

namespace Auth.Interfaces
{
    public interface IUserService
    {
        Task<bool> UpdateUser(EditUserViewModel model);
        Task<UnapprovedUserData> GetUser(string id);
    }
}
