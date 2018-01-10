using AutoMapper;
using Data.Service.Core;
using Data.Service.Core.Entities;
using LVMiniApi.Api.Service;
using LVMiniApi.Filters;
using LVMiniApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using static LVMiniApi.Api.Service.UserValidator;


namespace LVMiniApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            UserRepository = _unitOfWork.Users;
            Mapper = mapper;
        }

        // GET api/users
        [HttpGet]
        public IActionResult Get()
        {
            var users = UserRepository.GetAll();
            if (users == null)
                return NotFound();

            return Ok(Mapper.Map<IEnumerable<UserModel>>(users));
        }

        // GET api/users/[username]
        [HttpGet("{username}", Name = "UserGet")]
        public async Task<IActionResult> Get(string username)
        {
            using (_unitOfWork)
            {
                var user = await UserRepository.GetByUsername(username);
                if (user == null)
                    return NotFound();

                return Ok(Mapper.Map<UserModel>(user));
            }
        }

        // POST api/users
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Register([FromBody] UserModel model)
        {
            var user = Mapper.Map<User>(model);
            if (ValidateUserExists(user, UserRepository))
            {
                ModelState.AddModelError("Username", "A user with this information already exists!");
                return BadRequest(ModelState);
            }

            user.Password = Hasher.PasswordHash(user.Password);
            await UserRepository.Insert(user);
            await _unitOfWork.Commit();

            var newUri = Url.Link("UserGet", new { username = user.Username });
            return Created(newUri, Mapper.Map<UserModel>(user));
        }

        // PATCH/PUT api/users/[username]
        [HttpPatch("{username}")]
        [HttpPut("{username}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateUser(string username, [FromBody] EditUserModel model)
        {
            var user = await UserRepository.GetByUsername(username);
            if (user == null)
                return NotFound();

            ValidateUserUpdate(user, model);

            UserRepository.Update(user);
            //await InsertLog(user.Username, LogType.ProfileUpdate, LogRepository);

            await _unitOfWork.Commit();

            return Ok(Mapper.Map<UserModel>(user));
        }
    }
}