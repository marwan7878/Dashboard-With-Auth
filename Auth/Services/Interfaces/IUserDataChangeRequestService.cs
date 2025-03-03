using Auth.Models;
using Auth.ViewModels;

namespace Auth.Services.Interfaces
{
    public interface IUserDataChangeRequestService
    {
        bool AddChangeRequest(EditUserViewModel model);
        List<UnapprovedUserData> GetAllUnapprovedUserDataAsync();
        void RejectChangeRequest(string id);
        Task ApproveChangeRequestAsync(string id);
        Task<UserDataChangeRequestVM> ShowChangeRequest(string id);
    }
}
