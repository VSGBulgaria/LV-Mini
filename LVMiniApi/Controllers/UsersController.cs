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
    /// <summary>
    /// Provides non-admin actions for manipulating users.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        // inject the UnitOfWork
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Mapper = mapper;
            // get the repository from the UnitOfWork
            UserRepository = _unitOfWork.UserRepository;
        }

        /// <summary>
        /// Gets all the existing users in the database.
        /// </summary>
        /// <returns>Http 200 and a collection of users. If there are none returns Http 404.</returns>
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = UserRepository.GetAll();
            if (users == null)
                return NotFound();

            return Ok(Mapper.Map<IEnumerable<UserModel>>(users));
        }

        /// <summary>
        /// Gets a specific user from the database by a provided unique username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Http 200 and the user's information. Returns Http 404 if no such user exists.</returns>
        [HttpGet("{username}", Name = "UserGet")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await UserRepository.GetByUsername(username);
            if (user == null)
                return NotFound();

            return Ok(Mapper.Map<UserModel>(user));
        }


        /// <summary>
        /// Registers a new user in the database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Http 201 and the created user's information. Http 400 if the parameters are not valid.</returns>
        [HttpPost]
        [ValidateModel]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserModel model)
        {
            using (_unitOfWork)
            {
                // map the model to a user entity and check if there is such a user
                var user = Mapper.Map<User>(model);
                if (ValidateUserExists(user, UserRepository))
                {
                    ModelState.AddModelError("Username", "A user with this information already exists!");
                    return BadRequest(ModelState);
                }

                user.Password = Hasher.PasswordHash(user.Password);
                await UserRepository.Insert(user);
                await _unitOfWork.Commit();

                // get the username surrogate key to return
                var newUri = Url.Link("UserGet", new { username = user.Username });
                return Created(newUri, Mapper.Map<UserModel>(user));
            }
        }

        [HttpPost("{object}")]
        public IActionResult Post()
        {
            return BadRequest("The post request doesn't take any parameters!");
        }


        /// <summary>
        /// Updates the current logged in user's information.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="model"></param>
        /// <returns>Http 200 and the updated user information if there is such a user and he is the current logged in user.</returns>
        [HttpPatch("{username}")]
        [HttpPut("{username}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateUser(string username, [FromBody] EditUserModel model)
        {
            var user = await UserRepository.GetByUsername(username);
            if (user == null)
                return NotFound();

            // checks if the client logged in user is the same as the user from the databse
            // so someone can't change another user's information
            var clientUserId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (clientUserId != null && user.SubjectId != clientUserId)
                return Forbid();

            ValidateUserUpdate(user, model);
            UserRepository.Update(user);

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