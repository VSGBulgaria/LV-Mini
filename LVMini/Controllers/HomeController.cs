using AutoMapper;
using IdentityModel.Client;
using LVMini.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

namespace LVMini.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        public HomeController(IMapper mapper)
        {
            _mapper = mapper;
        }
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
            var discoveryClient = new DiscoveryClient("http://localhost:55817/");
            var doc = await discoveryClient.GetAsync();

            //var client = new TokenClient(
            //    address: doc.TokenEndpoint,
            //    clientId: "lvmini_code",
            //    clientSecret: "interns");

            //var response = await client.RequestClientCredentialsAsync("lvminiAPI lvmini_admin");
            //var accessToken = response.AccessToken;

            //Response.Cookies.Append("mvchybrid", accessToken);

            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, User);

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("http://localhost:55817/account/login/");
            }

            return RedirectToAction("Index", "Home");
        }



        [Authorize]
        public IActionResult Login()
        {
            return RedirectToAction("Index", "Home");
        }


        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        //[HttpPost]
        //public IActionResult Register(RegisterViewModel model)
        //{
        //    //if (ModelState.IsValid)
        //    //{
        //    //    using (HttpClient client = new HttpClient())
        //    //    {
        //    //        var user = _mapper.Map<UserModel>(model);
        //    //        var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

        //    //        var httpResponseMessage = await client.PostAsync($"http://localhost:53920/api/users", content);

        //    //        if (httpResponseMessage.StatusCode == HttpStatusCode.Created)
        //    //        {
        //    //            return RedirectToAction("Index", "Home");
        //    //        }
        //    //    }
        //    //}
        //    return Redirect("http://localhost:55817/account/register");
        //}

        [HttpGet]
        public IActionResult Register()
        {
            return Redirect("http://localhost:55817/account/register?returnUrl=" + "");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //MyProfile
        [HttpGet]
        public IActionResult MyProfile()
        {
            return View();
        }
    }
}
