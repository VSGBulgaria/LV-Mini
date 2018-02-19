using Data.Service.Core.Entities;
using Data.Service.Services;
using Data.Service.Services.Constants;
using IdentityModel;
using System.Collections.Generic;
using System.Linq;

namespace Data.Service.Persistance
{
    public static class LVMiniDbContextExtensions
    {
        public static void SeedUsersForContext(this LvMiniDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            List<User> users = new List<User>()
            {
                new User()
                {
                    Username = "simo",
                    Password = "simo",
                    FirstName = "Simeon",
                    LastName = "Banev",
                    Email = "simo@abv.bg",
                    IsActive = true,
                    Claims =
                    {
                        new UserClaim(JwtClaimTypes.Role, Role.Admin),
                        new UserClaim(JwtClaimTypes.GivenName, "Simeon"),
                        new UserClaim(JwtClaimTypes.FamilyName, "Banev"),
                        new UserClaim(JwtClaimTypes.Name, "simo"),
                        new UserClaim(JwtClaimTypes.Email, "simo@abv.bg")
                    }
                },
                new User()
                {
                    Username = "gosho",
                    Password = "gosho",
                    FirstName = "Gosho",
                    LastName = "Petrov",
                    Email = "gosho@abv.bg",
                    IsActive = true,
                    Claims =
                    {
                        new UserClaim(JwtClaimTypes.Role, Role.User),
                        new UserClaim(JwtClaimTypes.GivenName, "Gosho"),
                        new UserClaim(JwtClaimTypes.FamilyName, "Petrov"),
                        new UserClaim(JwtClaimTypes.Name, "gosho"),
                        new UserClaim(JwtClaimTypes.Email, "gosho@abv.bg")
                    }
                }
            };
            users[0].Password = Hasher.PasswordHash(users[0], users[0].Password);
            users[1].Password = Hasher.PasswordHash(users[1], users[1].Password);

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
