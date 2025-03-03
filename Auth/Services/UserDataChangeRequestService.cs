using Auth.Models;
using Auth.Repositories.Interfaces;
using Auth.Services.Interfaces;
using Auth.ViewModels;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using System.Security.Policy;

namespace Auth.Services
{
    public class UserDataChangeRequestService : IUserDataChangeRequestService
    {
        private readonly IUserDataChangeRequestRepository _repository;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        public UserDataChangeRequestService(IUserDataChangeRequestRepository repository, IUserService userService, IEmailService emailService)
        {
            _repository = repository;
            _userService = userService;
            _emailService = emailService;
        }

        public List<UnapprovedUserData> GetAllUnapprovedUserDataAsync()
        {
            return _repository.GetAll();
        }
        public void RejectChangeRequest(string id)
        {
            _repository.Delete(id);
        }
        public async Task ApproveChangeRequestAsync(string id)
        {
            var model = _repository.GetById(id);
            await _userService.UpdateUser(new EditUserViewModel
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Username = model.Username,
            });
            _repository.Delete(id);

            string emailBody = $"<p>Congratulations, Your personal data change request has been approved by the admin !</p>";

            await _emailService.SendEmailAsync(model.Email, "Change Request Approval", emailBody);

        }
        public async Task<UserDataChangeRequestVM> ShowChangeRequest(string id)
        {
            var currentUserData = _userService.GetUser(id).Result;
            var newUserData = _repository.GetById(id);
            var model = new UserDataChangeRequestVM
            {
                CurrentData = currentUserData,
                NewData = newUserData,
            };
            return model;
        }

        public bool AddChangeRequest(EditUserViewModel model)
        {
            try
            {

                UnapprovedUserData unapprovedUser = new UnapprovedUserData
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Username = model.Username,
                };
                _repository.SaveAsync(unapprovedUser);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
