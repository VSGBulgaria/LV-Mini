using System;
using System.Security.Cryptography;
using System.Text;

namespace AuthorizationServer.Helpers
{
    public class Hasher
    {
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
