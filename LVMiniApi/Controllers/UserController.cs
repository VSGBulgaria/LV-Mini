using LVMiniApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Data.Service.Core;
using Data.Service.Core.Entities;

namespace LVMiniApi.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher<IUser> _hasher;

        public UserController(IUserRepository repository, IPasswordHasher<IUser> hasher)
        {
            _repository = repository;
            _hasher = hasher;
        }

        // GET api/User/GetById/id
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _repository.GetById(id);
            return Json(user);
        }

        // POST api/User/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            bool userCheck = _repository.GetAll(u => u.Username == user.Username).ToList().Count > 0;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (userCheck)
            {
                ModelState.AddModelError("Username", "A user with this information already exists!");
                return BadRequest(ModelState);
            }

            var passwordHash = _hasher.HashPassword(user, user.Password);
            user.Password = passwordHash;

            await _repository.Insert(user);

            return Ok("You have registered successfully!");
        }

        // POST api/User/Login
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginUserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userCheck = _repository.GetAll(u => u.Username == user.Username).ToList();
            if (userCheck.Count == 0)
            {
                ModelState.AddModelError("Username", "No such user!");
                return BadRequest(ModelState);
            }

            var passwordCheck = _hasher.VerifyHashedPassword(user, userCheck[0].Password, user.Password);
            if (passwordCheck == PasswordVerificationResult.Success)
            {
                return Ok(userCheck[0]);
            }

            return BadRequest("Login failed!");
        }
    }
}