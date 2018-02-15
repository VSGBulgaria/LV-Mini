using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LVMini.Properties;
using LVMini.Service.Constants;
using LVMini.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LVMini.Controllers
{
    [Authorize(Roles = Role.Admin)]
    public class AdminTeamsController : Controller
    {
        private readonly HttpClient _client;

        public AdminTeamsController(IHttpClientProvider httpClient)
        {
            _client = httpClient.Client();
        }


        public async Task<IActionResult> GetAllTeams()
        {
            var httpResponseMessage = await _client.GetAsync(Resources.AdminTeamsApi + "/all");
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                return Json("Something went wrong.");
            }
            var data = await httpResponseMessage.Content.ReadAsStringAsync();
            return Ok(data);
        }

        public async Task<IActionResult> GetCurrentTeam([FromBody]string username)
        {
            var httpResponseMessage = await _client.GetAsync(Resources.AdminTeamsApi + "/" + username);
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                return Json("Something went wrong.");
            }
            var data = await httpResponseMessage.Content.ReadAsStringAsync();
            return Ok(data);
        }

    }
}