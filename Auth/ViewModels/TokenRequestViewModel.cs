using System.ComponentModel.DataAnnotations;

namespace Auth.ViewModels
{
    public class TokenRequestViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

