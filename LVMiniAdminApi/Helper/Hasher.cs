using System;
using System.Security.Cryptography;
using System.Text;

namespace LVMiniAdminApi.Helper
{
    /// <summary>
    /// Provides a password hasher.
    /// </summary>
    public class Hasher
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
