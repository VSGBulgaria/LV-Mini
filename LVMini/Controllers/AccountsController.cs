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
            //if (AuthenticationManager.LoggedUser.IsAdmin)
            //{
            //    return RedirectToAction("AdminProfile", "User");
            //}
            //MyProfileViewModel model = new MyProfileViewModel
            //{

            //    Email = HttpUtility.HtmlEncode(AuthenticationManager.LoggedUser.Email),
            //    FirstName = AuthenticationManager.LoggedUser.FirstName,
            //    LastName = AuthenticationManager.LoggedUser.LastName
            //};

            return View(/*model*/);
        }

        [HttpPost]
        public IActionResult MyProfile(MyProfileViewModel model)
        {
            // User user = new User()
            //{

            //    Email = model.Email,
            //    FirstName = model.FirstName,
            //    LastName = model.LastName,

            //};



            //model = new MyProfileViewModel()
            //{

            //    Email = user.Email,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName
            //};


            return View(model);
        }

        //Users Page
        //[HttpGet]
        //public IActionResult Users()
        //{
        //    return View();
        //}

        //AdminPage
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
    }

    internal class AuthorizeFilterAttribute : Attribute
    {
    }
}