using Data.Service.Core.Entities;
using System.Collections.Generic;

namespace LVMiniApi.Models
{
    /// <summary>
    /// Model for registering a user in the database.
    /// </summary>
    public class RegisterUserDto
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();
    }
}
