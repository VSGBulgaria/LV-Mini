using IdentityModel;
using System;

namespace LVMini.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public string OTAC { get; set; }
        public DateTime? OTACExpires { get; set; }

        public string GenerateOTAC(TimeSpan validFor)
        {
            var otac = CryptoRandom.CreateUniqueId();

            OTAC = otac; // This should be hashed in the future!
            OTACExpires = DateTime.UtcNow.Add(validFor);

            return otac;
        }
    }
}
