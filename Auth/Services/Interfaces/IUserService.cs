using Auth.Models;
using Auth.ViewModels;

namespace Auth.Services.Interfaces
{
    public interface IUserService
    {
        List<UserViewModel> GetAll();
        Task<bool> CreateUser(AddUserViewModel model, string changePasswordUrl);
        Task<bool> UpdateUser(EditUserViewModel model);
        AddUserViewModel LoadDataOfCreatePage();
        EditUserViewModel LoadDataOfEditPage(string id);
        ReadUserViewModel LoadDataOfReadPage(string id);
        Task<UnapprovedUserData> GetUser(string id);
        Task<bool> DeleteUser(string id);
        Task<bool> CheckEmail(string email);
        Task<bool> CheckUsername(string username);
        Task<bool> CheckEmailInEdit(string email, string id);
        Task<bool> CheckUsernameInEdit(string username, string id);

    }
}
