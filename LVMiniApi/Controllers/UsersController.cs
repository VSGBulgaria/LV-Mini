using AutoMapper;
using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using Data.Service.Services;
using LVMiniApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace LVMiniApi.Controllers
{
    /// <summary>
    /// Provides non-admin actions for manipulating users.
    /// </summary>
    [Route("api/users")]
    [Authorize]
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
        /// Gets a specific user from the database by a provided unique username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Http 200 and the user's information. Returns Http 404 if no such user exists.</returns>
        [HttpGet("{username}", Name = "UserGet")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await UserRepository.GetByUsername(username);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<UserDto>(user));
        }

        /// <summary>
        /// Blocks a GetAll request because that is an admin privelage.
        /// </summary>
        [HttpGet]
        public IActionResult BlockGetAll()
        {
            return Forbid("You have to be an admin to obtain a list of users!");
        }


        /// <summary>
        /// Registers a new user in the database. 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Http 201 and the created user's information. Http 400 if the parameters are not valid.</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            using (_unitOfWork)
            {
                if (await UserRepository.UserExists(user.Username))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }

                var userEntity = Mapper.Map<User>(user);
                userEntity.Password = Hasher.PasswordHash(userEntity, userEntity.Password);

                await UserRepository.Insert(userEntity);

                if (!await _unitOfWork.Commit())
                {
                    throw new Exception("Creating a user failed on save.");
                }

                // return 201 with the username surrogate key
                return CreatedAtRoute("UserGet", new { username = userEntity.Username }, Mapper.Map<UserDto>(userEntity));
            }
        }

        /// <summary>
        /// Blocks posting with a parameter to this controller.
        /// </summary>
        [HttpPost("{username}")]
        public async Task<IActionResult> BlockUserRegister(string username)
        {
            if (await UserRepository.UserExists(username))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return BadRequest("You can't post to a specific user!");
        }


        /// <summary>
        /// Updates the current logged in user's information.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="model"></param>
        /// <returns>Http 200 and the updated user information if there is such a user and he is the current logged in user.</returns>
        [HttpPatch("{username}")]
        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateUser(string username, [FromBody] EditUserDto model)
        {
            if (!await UserRepository.UserExists(username))
            {
                return NotFound();
            }

            var user = await UserRepository.GetByUsername(username);
            if (user == null)
            {
                return NotFound();
            }

            var subjectId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (subjectId != null && user.SubjectId != subjectId)
            {
                return Forbid();
            }

            Mapper.Map(model, user);
            UserRepository.Update(user);
            if (!await _unitOfWork.Commit())
            {
                throw new Exception("Updating a user failed on save.");
            }

            return Ok(Mapper.Map<UserDto>(user));
        }

        /// <summary>
        /// Blocks a generic patch request without specific user parameters.
        /// </summary>
        [HttpPatch]
        public IActionResult BlockPatchWithoutParameters()
        {
            return BadRequest("You have to provide a specific existing user in order to PATCH!");
        }

        /// <summary>
        /// Blocks all DELETE requests to this controller because deleting users is not allowed.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [HttpDelete("{object}")]
        public IActionResult BlockDeletingUsers()
        {
            return BadRequest("Deleting a user is not possible!");
        }
    }
}