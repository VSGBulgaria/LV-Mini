using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using LVMini.Models;
using LVMini.Properties;
using LVMini.Service.Constants;
using LVMini.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        public async Task<IActionResult> PostTeam([FromBody]Team team)
        {
            if (team.TeamName.IsNullOrEmpty())
            {
                return BadRequest("TeamName can not be null or empty.");
            }
            var stringContent = new StringContent(JsonConvert.SerializeObject(team), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(Resources.AdminTeamsApi, stringContent);
            var resultMessage = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest(resultMessage);
            }
            return Ok(resultMessage);
        }

        public async Task<IActionResult> MakeTeamInActive([FromBody] string teamName)
        {
            if (teamName.IsNullOrEmpty())
            {
                return BadRequest("TeamName can not be null or empty.");
            }
            var stringContent = new StringContent(JsonConvert.SerializeObject(teamName), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(Resources.AdminTeamsApi + "/inactive", stringContent);
            var resultMessage = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest(resultMessage);
            }
            return Ok(resultMessage);
        }

        public async Task<IActionResult> MakeTeamActive([FromBody] string teamName)
        {
            if (teamName.IsNullOrEmpty())
            {
                return BadRequest("TeamName can not be null or empty.");
            }
            var stringContent = new StringContent(JsonConvert.SerializeObject(teamName), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(Resources.AdminTeamsApi + "/active", stringContent);
            var resultMessage = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest(resultMessage);
            }
            return Ok(resultMessage);
        }

        public async Task<IActionResult> AddUserToTeam([FromBody]TeamUserDto teamUserDto )
        {
            if (teamUserDto.TeamName.IsNullOrEmpty())
            {
                return BadRequest($"Team name can not be null or empty.");
            }
            if (teamUserDto.UserName.IsNullOrEmpty())
            {
                return BadRequest($"User name can not be null or empty.");
            }
            var stringContent = new StringContent(JsonConvert.SerializeObject(teamUserDto), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(Resources.AdminTeamsApi + "/add/user", stringContent);
            var resultMessage = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest(resultMessage);
            }
            
            return Ok(resultMessage);
        }

        public async Task<IActionResult> RemoveUserFromTeam([FromBody] TeamUserDto teamUserDto)
        {
            if (teamUserDto.TeamName.IsNullOrEmpty())
            {
                return BadRequest($"Team name can not be null or empty.");
            }
            if (teamUserDto.UserName.IsNullOrEmpty())
            {
                return BadRequest($"User name can not be null or empty.");
            }
            var stringContent = new StringContent(JsonConvert.SerializeObject(teamUserDto), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(Resources.AdminTeamsApi + "/remove/user", stringContent);
            var resultMessage = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest(resultMessage);
            }
            return Ok(resultMessage);
        }

    }
}