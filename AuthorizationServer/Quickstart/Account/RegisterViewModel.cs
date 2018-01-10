using System.ComponentModel.DataAnnotations;

namespace AuthorizationServer.Quickstart.Account
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
