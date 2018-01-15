﻿using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace AuthorizationServer.Configuration
{
    public static class InMemoryConfiguration
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

                    ClientSecrets = { new Secret("interns".Sha256())},
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

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
                    PostLogoutRedirectUris = { "http://localhost:49649/Home/Login" },
                    RequireConsent = false
                }
            };
        }
    }
}
