using Auth.Models;

namespace Auth.ViewModels
{
    public class UserDataChangeRequestVM
    {
        public UnapprovedUserData CurrentData { get; set; }
        public UnapprovedUserData NewData { get; set; }
    }
}
