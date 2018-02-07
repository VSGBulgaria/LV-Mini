using IdentityModel.Client;
using LVMini.Models;
using LVMini.Service.Classes;
using LVMini.Service.Constants;
using LVMini.Service.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LVMini.Controllers
{
    public class AccountsController : Controller
    {
        private readonly HttpClient _client;

        public AccountsController(IHttpClientProvider httpClient)
        {
            _client = httpClient.Client();
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
                {
                    throw new Exception("Problem encountered while revoking the access token.", revokeAccessTokenResponse.Exception);
                }
            }

            string refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var revokeRefreshTokenResponse = await revocationClient.RevokeRefreshTokenAsync(refreshToken);
                if (revokeRefreshTokenResponse.IsError)
                {
                    throw new Exception("Problem encountered while revoking the access token.", revokeRefreshTokenResponse.Exception);
                }
            }

            var content = new StringContent(JsonConvert.SerializeObject(User.Identity.Name), encoding: Encoding.UTF8, mediaType: "application/json");
            await _client.PostAsync("http://localhost:53920/api/accounts/logout", content);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        //MyProfile
        [Authorize]
        public async Task<IActionResult> MyProfile()
        {
            var name = User.Identity.Name;
            var httpResponse = await _client.GetAsync("http://localhost:53920/api/users/" + name);
            if (httpResponse.StatusCode.Equals(HttpStatusCode.OK))
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<MyProfileUserModel>(content);
                return View(users);
            }
            return View();
        }

        //AdminPage
        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public IActionResult Admin()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public bool ModifyMyProfileInfo([FromBody]MyProfileValuesChangedUserModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = User.Identity.Name;

                var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = _client.PatchAsync("http://localhost:53920/api/users/" + currentUser, stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var content = new StringContent(JsonConvert.SerializeObject(User.Identity.Name), encoding: Encoding.UTF8, mediaType: "application/json");
                    var result = _client.PostAsync("http://localhost:53920/api/accounts/profileupdate", content).Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}