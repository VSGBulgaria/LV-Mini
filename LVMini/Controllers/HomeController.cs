using IdentityModel.Client;
using LVMini.Models;
using LVMini.Service.Classes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
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
        public async Task<IActionResult> Index()
        {
            await WriteOutIdentityInformation();
            return View();
        }

        [Authorize(Policy = "CanGetUsers")]
        public async Task<IActionResult> Users()
        {
            using (HttpClient client = new HttpClient())
            {
                var httpResponseMessage = await client.GetAsync(client.BaseAddress + "users");
                if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await TokenService.RefreshTokensAsync();
                }

                var data = httpResponseMessage.Content.ReadAsStringAsync().Result;

                var users = JsonConvert.DeserializeObject<IEnumerable<UserModel>>(data);

                return View(users);
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

        public async Task WriteOutIdentityInformation()
        {
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            Debug.WriteLine($"Identity toke: {identityToken}");

            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }
        }
    }
}
