using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Data.Service.Core.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public string SubjectId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(1000)]
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

        public bool IsActive { get; set; } = true;

        public ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();

        public ICollection<UserLogin> Logins { get; set; } = new List<UserLogin>();
    }
}
