using Data.Service.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LVMiniApi.Models
{
    /// <summary>
    /// The model for registering a user. Everything except the Claims is required.
    /// </summary>
    public class RegisterUserDto
    {
        /// <summary>
        /// The unique username of the user.
        /// </summary>
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

        /// <summary>
        /// The password of the user.
        /// </summary>
        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// The user's email.
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// The user's real first name.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// The user's real last name.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        /// <summary>
        /// A collection of claims for the given user.
        /// </summary>
        public ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();
    }
}
