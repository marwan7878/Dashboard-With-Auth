using Auth.Interfaces;
using Auth.Models;
using Auth.Repositories.Interfaces;
using Auth.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Auth.Services
{
    public class UserDataChangeRequestService : IUserDataChangeRequestService
    {
        private readonly IUserDataChangeRequestRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        public UserDataChangeRequestService(IUserDataChangeRequestRepository repository, UserManager<ApplicationUser> userManager, IUserService userService)
        {
            _repository = repository;
            _userManager = userManager;
            _userService = userService;
        }

        public List<UnapprovedUserData> GetAllUnapprovedUserDataAsync()
        {
            return _repository.GetAll();
        }
        public void RejectChangeRequest(string id)
        {
            _repository.Delete(id);
        }
        public void ApproveChangeRequest(string id)
        {
            var model = _repository.GetById(id);
            _userService.UpdateUser(new EditUserViewModel
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Username = model.Username,
            });
            _repository.Delete(id);
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

    }
}
