using Auth.Models;
using Auth.ViewModels;

namespace Auth.Repositories.Interfaces
{
    public interface IUserDataChangeRequestRepository
    {
        Task<bool> SaveAsync(UnapprovedUserData model);
        List<UnapprovedUserData> GetAll();
        Task<bool> Delete(string id);
        UnapprovedUserData GetById(string id);
    }
}
