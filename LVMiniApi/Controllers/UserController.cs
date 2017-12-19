using System.IdentityModel.Tokens.Jwt;
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
    [Route("api/user")]
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
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (UserExists(user))
            {
                ModelState.AddModelError("Username", "A user with this information already exists!");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            user.Password = _hasher.HashPassword(user, user.Password);           
            await _repository.Insert(user);
            return Ok("You have registered successfully!");
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody]string firstname)
        {
            var user = _repository.GetAll(u => u.FirstName == firstname).ToList();
            var currentUser = user[0];
            currentUser.FirstName = "TestUpdateMethod";
            await _repository.Update(currentUser);
            return Ok();
        }
        // POST api/User/Login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUserModel user)
        {
            if (!UserExists(user))
            {
                ModelState.AddModelError("Username", "No such user!");
                return NotFound(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var getUser = _repository.GetAll(u => u.Username == user.Username).ToList();
            var passwordCheck = _hasher.VerifyHashedPassword(user, getUser[0].Password, user.Password);
            if (passwordCheck == PasswordVerificationResult.Success)
                return Ok(getUser[0]);

            return BadRequest("Login failed!");
        }

        private bool UserExists(IUser user)
        {
            if (_repository.GetAll(u => u.Username == user.Username).ToList().Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}