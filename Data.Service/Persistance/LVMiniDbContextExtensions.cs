using Data.Service.Core.Entities;
using IdentityModel;
using LVMini.Service.Constants;
using System.Collections.Generic;
using System.Linq;

namespace Data.Service.Persistance
{
    public static class LVMiniDbContextExtensions
    {
        public static void SeedDataForContext(this LvMiniDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            List<User> users = new List<User>()
            {
                new User()
                {
                    SubjectId = "1",
                    Username = "simo",
                    Password = "123456789",
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
                    SubjectId = "2",
                    Username = "gosho",
                    Password = "123456789",
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

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
