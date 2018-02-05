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
            users[0].Password = Hasher.PasswordHash(users[0], users[0].Password);
            users[1].Password = Hasher.PasswordHash(users[1], users[1].Password);

            List<Team> teams = new List<Team>()
            {
                new Team()
                {
                    IsActive = true,
                    TeamName = "TestUsers",
                }
            };

            context.Users.AddRange(users);
            context.Teams.AddRange(teams);
            context.AddRange(
                new UserTeam() { Team = teams[0], User = users[0] }
                );

            context.SaveChanges();
        }
    }
}
