using IdentityModel;
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
                new ApiResource("lvminiAPI", "LV Mini API")
                {
                    ApiSecrets = {new Secret("mainAPIsecret".Sha256())}
                },
                new ApiResource("lvmini_admin", "LV Mini Admin")
                {
                    UserClaims =
                    {
                        JwtClaimTypes.Role
                    },
                    Scopes =
                    {
                        new Scope
                        {
                            Name = "lvmini_admin.full_access",
                            Emphasize = true,

                            // include additional claim for that scope
                            UserClaims =
                            {
                                "role"
                            }
                        }
                    },
                    ApiSecrets = {new Secret("adminAPIsecret".Sha256())}
                }
            };
        }

        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("roles", "Your role(s)", new []{"role"}),
                new IdentityResource("usernames", "Your role(s)", new []{"username"}),

            };
        }

        public static IEnumerable<Client> Clients()
        {
            return new Client[]
            {
                new Client
                {
                    ClientName = "LV Mini",
                    ClientId = "lvmini",

                    ClientSecrets = { new Secret("interns".Sha256())},
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AccessTokenType = AccessTokenType.Reference,

                    AllowedScopes = new []
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "lvminiAPI",
                        "lvmini_admin",
                        "roles",
                        "usernames"
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
