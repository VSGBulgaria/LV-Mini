using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace AuthorizationServer.Configuration
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new ApiResource[]
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
                new IdentityResource("roles", "Your role(s)", new []{"role"}),
            };
        }

        public static IEnumerable<Client> Clients()
        {
            return new Client[]
            {
                new Client
                {
                    ClientName = "LV Mini",
                    ClientId = "lvmini_code",

                    ClientSecrets = { new Secret("interns".Sha256())},
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    AllowedScopes = new []
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "lvminiAPI",
                        "lvmini_admin",
                        "roles"
                    },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new [] { "http://localhost:49649/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:49649/signout-callback-oidc" },
                    RequireConsent = false,
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };
        }
    }
}
