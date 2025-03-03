using Auth.Enums;
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
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IUserRepository repository, IRolesService rolesService, IEmailService emailService, RoleManager<IdentityRole> roleManager)
        {
            _repository = repository;
            _rolesService = rolesService;
            _emailService = emailService;
            _roleManager = roleManager;
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
        public EditUserViewModel LoadDataOfEditPage(string id)
        {
            var user = _repository.GetByIdAsync(id).Result;
            var userRole = _rolesService.GetUserRoleByUserId(user.Id).Result;
            return new EditUserViewModel
            {
                Id = id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                Roles = _rolesService.GetRolesInSelectList(userRole.Id)
            };
        }
        public ReadUserViewModel LoadDataOfReadPage(string id)
        {
            var user = _repository.GetByIdAsync(id).Result;
            var roles = _roleManager.Roles.ToList();
            return new ReadUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                Role = roles.Select(r => r.Name).FirstOrDefault()
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
