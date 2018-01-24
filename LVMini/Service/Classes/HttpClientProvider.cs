using IdentityModel.Client;
using LVMini.Service.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace LVMini.Service.Classes
{
    public sealed class HttpClientProvider : IHttpClientProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public HttpClientProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = new HttpClient();
        }

        public HttpClient Client()
        {
            HttpContext context = _httpContextAccessor.HttpContext;
            string accessToken;

            string expiresAt = context.GetTokenAsync("expires_at").Result;
            if (!string.IsNullOrWhiteSpace(expiresAt) && DateTime.Parse(expiresAt).AddSeconds(-30).ToUniversalTime() < DateTime.UtcNow)
            {
                accessToken = RenewTokens().Result;
            }
            else
            {
                accessToken = context.GetTokenAsync(OpenIdConnectParameterNames.AccessToken).Result;
            }

            if (!string.IsNullOrWhiteSpace(accessToken))
                _httpClient.SetBearerToken(accessToken);

            return _httpClient;
        }

        private async Task<string> RenewTokens()
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;

            DiscoveryResponse authServerInfo = await DiscoveryClient.GetAsync($"http://localhost:55817");

            TokenClient tokenClient = new TokenClient(authServerInfo.TokenEndpoint, "lvmini", "interns");

            string refreshToken = await httpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            TokenResponse tokenResponse = await tokenClient.RequestRefreshTokenAsync(refreshToken);

            if (!tokenResponse.IsError)
            {
                AuthenticateResult authenticationInformation = await httpContext.AuthenticateAsync("Cookies");
                var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);
                authenticationInformation.Properties.UpdateTokenValue("expires_at",
                    expiresAt.ToString("O", CultureInfo.InvariantCulture));

                authenticationInformation.Properties.UpdateTokenValue(OpenIdConnectParameterNames.AccessToken,
                    tokenResponse.AccessToken);
                authenticationInformation.Properties.UpdateTokenValue(OpenIdConnectParameterNames.RefreshToken,
                    tokenResponse.RefreshToken);

                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    authenticationInformation.Principal, authenticationInformation.Properties);
                return tokenResponse.AccessToken;
            }

            return null;
        }
    }
}