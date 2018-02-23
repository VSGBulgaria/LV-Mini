using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Data.Service.Services
{
    /// <summary>
    /// Provides a password hasher.
    /// </summary>
    public class Hasher
    {
        private static readonly PasswordHasher<User> PasswordHasher = new PasswordHasher<User>();

        /// <summary>
        /// Hashes the password of a given user with the Identity PasswordHasher.
        /// </summary>
        public static string PasswordHash(User user, string password)
        {
            return PasswordHasher.HashPassword(user, password);
        }

        /// <summary>
        /// Verifies if a hashed and a given password match.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="hashedPassword"></param>
        /// <param name="providedPassword"></param>
        /// <returns>A password verification result indicating the comparison.</returns>
        public static PasswordVerificationResult VerifyHashPassword(User user, string hashedPassword, string providedPassword)
        {
            return PasswordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        }
    }
}
