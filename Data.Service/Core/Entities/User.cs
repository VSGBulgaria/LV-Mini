using System.ComponentModel.DataAnnotations;


namespace Data.Service.Core.Entities
{
    public class User : BaseEntity, IUser
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
    }
}
