using LVMini.Models;
using LVMini.Properties;
using LVMini.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace LVMini.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient client;

        public HomeController(IHttpClientProvider httpClient)
        {
            client = httpClient.Client();
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "CanGetUsers")]
        public async Task<IActionResult> Users()
        {
            var httpResponseMessage = await client.GetAsync(Resources.MainApiUsersUrl);
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                return RedirectToAction("AccessDenied", "Authorization");

            var data = httpResponseMessage.Content.ReadAsStringAsync().Result;

            var users = JsonConvert.DeserializeObject<IEnumerable<UserModel>>(data);

            return View(users);
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }


        public IActionResult Contact()
        {
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
