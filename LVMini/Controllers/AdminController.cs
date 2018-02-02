﻿using LVMini.Models;
using LVMini.Properties;
using LVMini.Service.Constants;
using LVMini.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LVMini.Controllers
{
    [Authorize(Roles = Role.Admin)]
    public class AdminController : Controller
    {
        private readonly HttpClient _client;

        public AdminController(IHttpClientProvider httpClient)
        {
            _client = httpClient.Client();
        }

        [Authorize(Policy = "CanGetUsers")]
        public async Task<IActionResult> Users()
        {
            var samplePaging = "?pageNumber=1&pageSize=3";
            var httpResponseMessage = await _client.GetAsync(Resources.AdminApi + samplePaging);
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            var data = await httpResponseMessage.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<IEnumerable<ModifiedUserModel>>(data);

            return View(users);
        }

        [HttpPost]
        public bool ModifyUserInfo([FromBody]ModifiedUserModel model)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = _client.PutAsync("http://localhost:53990/api/admin/users", stringContent).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}