using AutoMapper;
using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using LVMiniApi.Api.Service;
using LVMiniApi.Filters;
using LVMiniApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LVMiniApi.Api.Service.UserValidator;


namespace LVMiniApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
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
        [Authorize]
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
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] RegisterUserModel model)
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

        [HttpPost("{object}")]
        public IActionResult Post()
        {
            return BadRequest("The post request doesn't take any parameters!");
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

            var clientUserId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (clientUserId != null && user.Id.ToString() != clientUserId)
                return Forbid();

            ValidateUserUpdate(user, model);
            UserRepository.Update(user);
            //await InsertLog(user.Username, UserAction.ProfileUpdate);

            await _unitOfWork.Commit();

            return Ok(Mapper.Map<UserModel>(user));
        }

        [HttpPatch]
        [HttpPut]
        public IActionResult PatchPut()
        {
            return BadRequest("You have to pass a user to the patch request!");
        }

        [HttpDelete]
        [HttpDelete("{object}")]
        public IActionResult Delete()
        {
            return BadRequest("You can't delete users!");
        }
    }
}