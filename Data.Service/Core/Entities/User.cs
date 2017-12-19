using System.ComponentModel.DataAnnotations;
<<<<<<< HEAD:Data.Service/Entities/User.cs
using Data.Service.Core;
using Data.Service.Core.Entities;

namespace Data.Service.Entities
=======

namespace Data.Service.Core.Entities
>>>>>>> 1f9a590a94d4b24c549adbf39e1db8ab1d0d7d0c:Data.Service/Core/Entities/User.cs
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
