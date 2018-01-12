using LVMini.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

                    var httpResponseMessage = await client.PostAsync($"http://localhost:53920/api/users", content);

                    if (httpResponseMessage.StatusCode == HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View(model);
        }

        //MyProfile
        [HttpGet]
        public IActionResult MyProfile()
        {
            return View();
        }

        //AdminPage
        [HttpGet]
        public IActionResult AdminPage()
        {
            return View();
        }
    }
}