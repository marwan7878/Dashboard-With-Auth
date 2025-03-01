using Auth.Models;
using Auth.ViewModels;

namespace Auth.Interfaces
{
    public interface IUserDataChangeRequestService
    {
        bool AddChangeRequest(EditUserViewModel model);
        List<UnapprovedUserData> GetAllUnapprovedUserDataAsync();
        void RejectChangeRequest(string id);
        void ApproveChangeRequest(string id);
        Task<UserDataChangeRequestVM> ShowChangeRequest(string id);
    }
}
