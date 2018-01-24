using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace LVMini.Service.Classes
{
    public static class TokenService
    {
        private static readonly HttpContextAccessor HttpContextAccessor;

        static TokenService()
        {
            HttpContextAccessor = new HttpContextAccessor();
        }

        public static async Task RefreshTokensAsync()
        {
            var httpContext = HttpContextAccessor.HttpContext;

            DiscoveryResponse authServerInfo = await DiscoveryClient.GetAsync($"http://localhost:55817");

            var tokenClient = new TokenClient(authServerInfo.TokenEndpoint, "lvmini_code", "interns");

            string refreshToken = await httpContext.GetTokenAsync("refresh_token");

            TokenResponse tokenResponse = await tokenClient.RequestRefreshTokenAsync(refreshToken);

            if (!tokenResponse.IsError)
            {
                AuthenticateResult authenticationInformation = await httpContext.AuthenticateAsync("Cookies");

                var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);

                AuthenticationToken[] tokens = new[]
                {
                        new AuthenticationToken()
                        {
                            Name = OpenIdConnectParameterNames.AccessToken,
                            Value = tokenResponse.AccessToken
                        },
                        new AuthenticationToken()
                        {
                            Name = OpenIdConnectParameterNames.RefreshToken,
                            Value = tokenResponse.RefreshToken
                        },
                        new AuthenticationToken()
                        {
                            Name = "expires_at",
                            Value = expiresAt.ToString("O", CultureInfo.InvariantCulture)
                        }
                    };

                authenticationInformation.Properties.StoreTokens(tokens);

                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationInformation.Principal, authenticationInformation.Properties);
            }
        }
    }
}
