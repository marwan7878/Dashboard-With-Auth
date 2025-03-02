using Auth.Interfaces;
using Auth.Models;
using Auth.Repositories.Interfaces;
using Auth.Services.Interfaces;
using Auth.ViewModels;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace Auth.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IRolesService _rolesService;
        private readonly IEmailService _emailService;
        public UserService(IUserRepository repository, IRolesService rolesService, IEmailService emailService)
        {
            _repository = repository;
            _rolesService = rolesService;
            _emailService = emailService;
        }
        public List<UserViewModel> GetAll()
        {
            return _repository.GetAll();
        }
        public async Task<bool> CreateUser(AddUserViewModel model, string changePasswordUrl)
        {
            var result = _repository.CreateAsync(model);
            if(result.Result)
            {
                string emailBody = $"<h3>Congratulations, you became having a new email in our website.</h3>" +
                    $"<p>Please, Don't share the credentials with anyone.</p>" +
                    $"<p>Takecare!! the password is temperory and valid for one use, so be sure to change it before signout</p>" +
                    $"<p>Email : {model.Email}</p>" +
                    $"<p>Password : {model.Password}</p>"+
                    $"<p>Please click here to Sign in <a href='{changePasswordUrl}'>here</a> </p>";

                await _emailService.SendEmailAsync(model.Email, "Email credentials", emailBody);

            }
            return true;
        }
        public Task<bool> UpdateUser(EditUserViewModel model)
        {
            return _repository.Update(model);
        }
        public AddUserViewModel LoadDataOfCreatePage()
        {
            return new AddUserViewModel
            {
                Roles = _rolesService.GetRolesInSelectList("0")
            };
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
