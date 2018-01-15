using IdentityModel;
using IdentityModel.Client;
using LVMini.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TokenResponse = IdentityModel.Client.TokenResponse;

namespace LVMini.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Users()
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var httpResponseMessage = await client.GetAsync($"http://localhost:53920/api/users");
                if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await RefreshTokensAsync();
                }

                var data = httpResponseMessage.Content.ReadAsStringAsync().Result;

                var users = JsonConvert.DeserializeObject<IEnumerable<UserModel>>(data);

                return View(users);
            }
        }

        private async Task RefreshTokensAsync()
        {
            DiscoveryResponse authServerInfo = await DiscoveryClient.GetAsync($"http://localhost:55817");

            var tokenClient = new TokenClient(authServerInfo.TokenEndpoint, "lvmini_code", "interns");

            string refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            TokenResponse tokenResponse = await tokenClient.RequestRefreshTokenAsync(refreshToken);
            if (tokenResponse.HttpStatusCode == HttpStatusCode.OK)
            {

                string identityToken = await HttpContext.GetTokenAsync("id_token");

                var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);

                AuthenticationToken[] tokens = new[]
                {
                    new AuthenticationToken()
                    {
                        Name = OpenIdConnectParameterNames.IdToken,
                        Value = identityToken
                    },
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

                AuthenticateResult authenticationInformation = await HttpContext.AuthenticateAsync("Cookies");
                authenticationInformation.Properties.StoreTokens(tokens);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationInformation.Principal, authenticationInformation.Properties);
            }
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }


        public async Task<IActionResult> Contact()
        {
            DiscoveryResponse authenticationServer = await DiscoveryClient.GetAsync($"http://localhost:55817/");

            var request = new AuthorizeRequest(authenticationServer.AuthorizeEndpoint);
            var url = request.CreateAuthorizeUrl(
                clientId: "lvmini_code",
                scope: "lvminiAPI lvmini_admin",
                responseType: OidcConstants.ResponseTypes.CodeIdToken,
                redirectUri: "http://localhost:49649/signin-oidc",
                state: CryptoRandom.CreateUniqueId(),
                nonce: CryptoRandom.CreateUniqueId());

            var response = new AuthorizeResponse(url);

            var code = response.Code;

            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }



        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
