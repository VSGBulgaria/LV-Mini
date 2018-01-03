using IdentityModel.Client;
using LVMini.Models;
using LVMini.ViewModels;
using LVMiniApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
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
using System.Text;
using System.Threading.Tasks;

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

                await HttpContext.SignInAsync("Cookies", authenticationInformation.Principal, authenticationInformation.Properties);
            }
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Authorize]
        public IActionResult Login()
        {
            return RedirectToAction("Index", "Home");
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var client = new HttpClient();
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://localhost:53920/api/login", stringContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login", "Home");
        }

        public IActionResult Register()
        {

            ViewData["Message"] = "Register Page";
            return View();

        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
