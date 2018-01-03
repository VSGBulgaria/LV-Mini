using System;
using System.Security.Cryptography;
using System.Text;

namespace LVMiniApi.Api.Service
{
    /// <summary>
    /// Provides a password hasher.
    /// </summary>
    internal class Hasher
    {
        /// <summary>
        ///Hashes a given password using SHA512.
        /// </summary>
        public static string PasswordHash(string password)
        {
            using (var sha = SHA512.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }
    }
}
