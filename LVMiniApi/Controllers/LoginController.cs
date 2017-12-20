using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Service.Core;
using Data.Service.Core.Enums;
using LVMiniApi.Api.Service;
using LVMiniApi.Filters;
using LVMiniApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LVMiniApi.Controllers
{
    [Produces("application/json")]
    [Route("api/login")]
    public class LoginController : BaseController
    {
        public LoginController(ILogRepository logRepository, IUserRepository userRepository, IPasswordHasher<IUser> hasher)
        {
            LogRepository = logRepository;
            UserRepository = userRepository;
            Hasher = hasher;
        }

        // POST api/login
        [HttpPost]
        [ValidateModel]
        public IActionResult Post([FromBody] LoginUserModel user)
        {
            if (!ApiHelper.UserExists(user, UserRepository))
            {
                ModelState.AddModelError("Username", "No such user!");
                return NotFound(ModelState);
            }

            var getUser = UserRepository.GetAll(u => u.Username == user.Username).ToList();
            if (Hasher.VerifyHashedPassword(user, getUser[0].Password, user.Password) == PasswordVerificationResult.Success)
            {
                ApiHelper.InsertLog(getUser[0].Id, LogAction.Logout, DateTime.Now, LogRepository);
                return Ok();
            }

            return BadRequest("Login failed!");
        }
    }
}