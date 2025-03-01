using System.ComponentModel.DataAnnotations;

namespace Auth.Models
{
    public class UnapprovedUserData 
    {
        public string Id { get; set; }
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string? FirstName { get; set; }
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string? LastName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Username { get; set; }
    }
}
