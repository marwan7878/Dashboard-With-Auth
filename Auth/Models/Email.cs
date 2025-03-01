using System.ComponentModel.DataAnnotations;

namespace Auth.Models
{
    public class Email
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string SmtpServer { get; set; }
        [Required]
        public int Port { get; set; }
        [Required]
        public string SenderName { get; set; }

    }
}
