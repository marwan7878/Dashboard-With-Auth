using Auth.Models;
using Auth.ViewModels;

namespace Auth.Repositories.Interfaces
{
    public interface IUserDataChangeRequestRepository
    {
        Task<bool> SaveAsync(EditUserViewModel model);
        List<UnapprovedUserData> GetAll();
        bool Delete(string id);
        UnapprovedUserData GetById(string id);
    }
}
