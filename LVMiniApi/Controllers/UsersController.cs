using System.Collections.Generic;
using LVMiniApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Data.Service.Core;
using Data.Service.Core.Entities;
using LVMiniApi.Api.Service;
using LVMiniApi.Filters;
using Microsoft.AspNetCore.Http;

namespace LVMiniApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        public UsersController(IUserRepository userRepository, IPasswordHasher<IUser> hasher, IMapper mapper)
        {
            UserRepository = userRepository;
            Hasher = hasher;
            Mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var users = UserRepository.GetAll();
            if (users == null)
                return NotFound();

            return Ok(Mapper.Map<IEnumerable<UserModel>>(users));
        }

        // GET api/User/GetById/id
        [HttpGet("{username}", Name = "UserGet")]
        public async Task<IActionResult> Get(string username)
        {
            var user = await UserRepository.GetByUsername(username);
            if (user == null)
                return NotFound();

            return Ok(Mapper.Map<UserModel>(user));
        }

        // POST api/User/Register
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Register([FromBody] UserModel model)
        {
            var user = Mapper.Map<User>(model);
            if (ApiHelper.UserExists(user, UserRepository))
            {
                ModelState.AddModelError("Username", "A user with this information already exists!");
                return BadRequest(ModelState);
            }

            user.Password = Hasher.HashPassword(user, user.Password);
            await UserRepository.Insert(user);

            var newUri = Url.Link("UserGet", new {username = user.Username});
            return Created(newUri, Mapper.Map<UserModel>(user));
        }

        [HttpPatch("{username}")]
        [HttpPut("{username}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateUser(string username, [FromBody] EditUserModel model)
        {
            var oldUser = await UserRepository.GetByUsername(username);
            if (oldUser == null)
                return NotFound();

            oldUser = ApiHelper.ValidateUpdate(oldUser, model);
            await UserRepository.Update(oldUser);

            return Ok(Mapper.Map<UserModel>(oldUser));
        }
    }
}