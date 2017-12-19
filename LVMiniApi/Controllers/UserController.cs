using System;
using LVMiniApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Data.Service.Core;
using Data.Service.Core.Entities;
using Data.Service.Core.Enums;
using LVMiniApi.Api.Service;

namespace LVMiniApi.Controllers
{
    [Produces("application/json")]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<IUser> _hasher;
        private readonly ILogRepository _logRepository;

        public UserController(IUserRepository userRepository, IPasswordHasher<IUser> hasher, ILogRepository logRepository)
        {
            _userRepository = userRepository;
            _hasher = hasher;
            _logRepository = logRepository;
        }

        // GET api/User/GetById/id
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userRepository.GetById(id);
            return Json(user);
        }

        // POST api/User/Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (ApiHelper.UserExists(user, _userRepository))
            {
                ModelState.AddModelError("Username", "A user with this information already exists!");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            user.Password = _hasher.HashPassword(user, user.Password);
            await _userRepository.Insert(user);

            return Json(user);
        }


        // POST api/User/Login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUserModel user)
        {
            if (!ApiHelper.UserExists(user, _userRepository))
            {
                ModelState.AddModelError("Username", "No such user!");
                return NotFound(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var getUser = _userRepository.GetAll(u => u.Username == user.Username).ToList();

            if (_hasher.VerifyHashedPassword(user, getUser[0].Password, user.Password) == PasswordVerificationResult.Success)
            {
                ApiHelper.InsertLog(getUser[0].Id, LogAction.Logout, DateTime.Now, _logRepository);
                return Json(getUser[0]);
            }

            return BadRequest("Login failed!");
        }
    }
}