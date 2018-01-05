using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace AuthorizationServer.Configuration
{
    public static class InMemoryConfiguration
    {
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new[]
            {
                new ApiResource("lvminiAPI", "LV Mini API"),
                new ApiResource("lvmini_admin", "LV Mini Admin") 
            };
        }

        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static IEnumerable<Client> Clients()
        {
            return new[]
            {
                new Client
                {
                    ClientName = "LV Mini",
                    ClientId = "lvmini_code",
                    ClientSecrets = new []
                    {
                        new Secret("interns".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowedScopes = new []
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "lvminiAPI",
                        "lvmini_admin"
                    },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new [] { "http://localhost:49649/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:49649/signout-callback-oidc" },
                }
            };
        }
    }
}
