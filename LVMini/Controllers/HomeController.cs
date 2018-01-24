using LVMini.Models;
using LVMini.Properties;
using LVMini.Service.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace LVMini.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "CanGetUsers")]
        public async Task<IActionResult> Users()
        {
            var httpResponseMessage = await Client.GetAsync(Resources.MainApiUsersUrl);
            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                await TokenService.RefreshTokensAsync();
            }

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
