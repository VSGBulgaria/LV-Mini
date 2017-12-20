using System;
using System.Collections.Generic;
using LVMiniApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Service.Core;
using Data.Service.Core.Entities;
using Data.Service.Core.Enums;
using LVMiniApi.Api.Service;
using LVMiniApi.Filters;

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
            IEnumerable<User> users = UserRepository.GetAll();
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
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (ApiHelper.UserExists(user, UserRepository))
            {
                ModelState.AddModelError("Username", "A user with this information already exists!");
                return BadRequest(ModelState);
            }

            user.Password = Hasher.HashPassword(user, user.Password);
            await UserRepository.Insert(user);

            return Ok(Mapper.Map<UserModel>(user));
        }
    }
}