using LVMini.Models;
using LVMini.Service.Constants;
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
                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                    var httpResponseMessage = await client.PostAsync(Resources.MainApiUsersUrl, content);

                    if (httpResponseMessage.StatusCode == HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View(model);
        }

        //MyProfile
        [AuthorizeFilter]
        public ActionResult MyProfile()
        {

            return View();

        }

        [HttpPost]
        public IActionResult MyProfile(MyProfileViewModel model)
        {
            return View(model);
        }

        //AdminPage

        [HttpPost]
        [Authorize(Roles = "admin")]

        [HttpGet]
        [Authorize(Roles = Role.Admin)]

        public async Task<IActionResult> Admin()
        {
            var accessToken = await this.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            using (var client = new HttpClient())
            {
                client.SetBearerToken(accessToken);
                var httpResponse = await client.GetAsync("http://localhost:53990/api/Admin/users");
                if (httpResponse.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<List<UserModel>>(content);

                    return View(users);
                }
            }
            return View();
        }

        //Validate Username
        public async Task<IActionResult> CheckUser(string name)
        {
            using (var client = new HttpClient())
            {
                if (string.IsNullOrEmpty(name))
                {
                    return NotFound();
                }
                var httpResponse = await client.GetAsync($"http://localhost:53920/api/users/" + name);
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserModel>(content);

                    return Json(Ok());
                }

                return Json(httpResponse.StatusCode);
            }
        }

        //Validate Email
        public async Task<IActionResult> CheckEmail(string email)
        {
            using (var client = new HttpClient())
            {
                if (string.IsNullOrEmpty(email))
                {
                    return NotFound();
                }

                var httpResponse = await client.GetAsync($"http://localhost:53920/api/users/");
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<IEnumerable<UserModel>>(content);

                    foreach (var user in users)
                    {
                        if (user.Email == email)
                        {
                            return Json(Ok());
                        }
                    }
                }
                return Json(NotFound());
            }
        }
        //Edit User

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DisplayUserInfo(string username)
        {
            using (var client = new HttpClient())
            {
                if (string.IsNullOrEmpty(username))
                {
                    // return bad request view
                    return View();
                }
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
                //return bad request view
                return View();
            }
        }

        internal class HttpStatusCodeResult : ActionResult
        {
            private HttpStatusCode badRequest;

            public HttpStatusCodeResult(HttpStatusCode badRequest)
            {
                this.badRequest = badRequest;
            }
        }

        internal class AuthorizeFilterAttribute : Attribute
        {
        }
    }
}