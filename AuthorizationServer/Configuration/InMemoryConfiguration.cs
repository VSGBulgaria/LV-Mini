using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace AuthorizationServer.Configuration
{
    public class InMemoryConfiguration
    {
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new[]
            {
                new ApiResource("lvmini", "LV Mini")
                {
                    UserClaims = new List<string>(){"email"}
                },
                new ApiResource("lvmini_admin", "LV Mini Admin")
                {
                UserClaims = new List<string>(){"email"}
                }
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
                    ClientId = "lvmini",
                    ClientSecrets = new [] { new Secret("interns".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = new [] { "lvmini" }
                },

                new Client
                {
                    ClientId = "lvmini_implicit",
                    ClientSecrets = new [] { new Secret("interns".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new []
                    {
                        "lvmini",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new [] { "http://localhost:49649/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:49649/signout-callback-oidc" }
                },

                new Client
                {
                    ClientId = "lvmini_code",
                    ClientSecrets = new [] { new Secret("interns".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowedScopes = new []
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "lvmini"
                    },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new [] { "http://localhost:49649/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:49649/signout-callback-oidc" }
                }
            };
        }


        public static IEnumerable<TestUser> Users()
        {
            return new[]
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "simo",
                    Password = "123456789",
                    Claims = new List<Claim>()
                    {
                        new Claim("email", "simo@abv.bg")
                    }
                }
            };
        }
    }
}
