using AuthorizationServer.Configuration;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using System.Linq;

namespace AuthorizationServer.Services
{
    public static class ConfigurationDbContextExtensions
    {
        public static void SeedDataForContext(this ConfigurationDbContext context)
        {
            context.ApiResources.RemoveRange(context.ApiResources);
            context.Clients.RemoveRange(context.Clients);
            context.IdentityResources.RemoveRange(context.IdentityResources);
            context.SaveChanges();

            if (!context.Clients.Any())
            {
                foreach (Client client in Config.Clients())
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (IdentityResource identityResource in Config.IdentityResources())
                {
                    context.IdentityResources.Add(identityResource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (ApiResource apiResource in Config.ApiResources())
                {
                    context.ApiResources.Add(apiResource.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
}
