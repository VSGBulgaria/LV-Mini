using IdentityModel.Client;
using LVMini.Models;
using LVMini.Service.Classes;
using LVMini.Service.Constants;
using LVMini.Service.Interfaces;
using LVMini.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Resources = LVMini.Properties.Resources;

namespace LVMini.Controllers
{
    public class AccountsController : Controller
    {
        private readonly HttpClient client;

        public AccountsController(IHttpClientProvider httpClient)
        {
            client = httpClient.Client();
        }

        [Authorize]
        public IActionResult Login()
        {
            return RedirectToAction("Index", "Home");
        }

        public async Task Logout()
        {
            DiscoveryResponse authServer = await DiscoveryClient.GetAsync("http://localhost:55817");

            TokenRevocationClient revocationClient = new TokenRevocationClient(authServer.RevocationEndpoint, "lvmini", "interns");

            string accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                var revokeAccessTokenResponse = await revocationClient.RevokeAccessTokenAsync(accessToken);
                if (revokeAccessTokenResponse.IsError)
                    throw new Exception("Problem encountered while revoking the access token.", revokeAccessTokenResponse.Exception);
            }

            string refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var revokeRefreshTokenResponse = await revocationClient.RevokeRefreshTokenAsync(refreshToken);
                if (revokeRefreshTokenResponse.IsError)
                    throw new Exception("Problem encountered while revoking the access token.", revokeRefreshTokenResponse.Exception);
            }

            var content = new StringContent(JsonConvert.SerializeObject(User.Identity.Name), encoding: Encoding.UTF8, mediaType: "application/json");
            await client.PostAsync("http://localhost:53920/api/accounts/logout", content);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var httpResponseMessage = await client.PostAsync(Resources.MainApiUsersUrl, content);

                if (httpResponseMessage.StatusCode == HttpStatusCode.Created)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        //MyProfile
        [Authorize]
        public IActionResult MyProfile()
        {
            MyProfileModel model = HelperInitializer.ConstructMyProfileModel(User.Claims);
            return View(model);
        }

        //AdminPage
        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Admin()
        {
            var httpResponse = await client.GetAsync("http://localhost:53990/api/Admin/users");
            if (httpResponse.StatusCode.Equals(HttpStatusCode.OK))
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<UserModel>>(content);

                return View(users);
            }
            return RedirectToAction("AccessDenied", "Authorization");
        }

        //Validate Username
        public async Task<bool> CheckUser(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            var httpResponse = await client.GetAsync($"http://localhost:53920/api/users/" + name);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserModel>(content);
                return true;
            }
            return false;
        }

        //Validate Email
        public async Task<bool> CheckEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var httpResponse = await client.GetAsync($"http://localhost:53920/api/users/");
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<IEnumerable<UserModel>>(content);

                    foreach (var user in users)
                    {
                        if (user.Email == email)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        //Edit User
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DisplayUserInfo(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                var uri = "http://localhost:53920/api/users/" + username;

                var httpResponse = await client.GetAsync(uri);
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    var currentUser = JsonConvert.DeserializeObject<UserModel>(content);

                    if (currentUser != null)
                    {
                        //return view with user data 
                        return View(currentUser);
                    }

                }
            }
            //return bad request view
            return View();
        }
    }
}