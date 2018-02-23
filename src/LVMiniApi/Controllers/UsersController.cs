using AutoMapper;
using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using Data.Service.Services;
using LVMiniApi.Helpers;
using LVMiniApi.Models;
using LVMiniApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace LVMiniApi.Controllers
{
    /// <summary>
    /// Provides non-admin actions for manipulating users. You have to be an authenticated user for most of it.
    /// </summary>
    [Route("api/users")]
    [Produces("application/json")]
    public class UsersController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITypeHelperService _typeHelperService;

        /// <summary>
        /// Injects the services needed through constructor injection.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        /// <param name="mapper">AutoMapper's Mapper class.</param>
        /// <param name="typeHelperService"></param>
        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, ITypeHelperService typeHelperService)
        {
            _unitOfWork = unitOfWork;
            _typeHelperService = typeHelperService;

            // inject and set the mapper from the BaseController
            Mapper = mapper;
        }

        /// <summary>
        /// Gets a specific user from the database by a provided unique username.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="fields"></param>
        /// <returns>Http 200 and the user's information. Returns Http 404 if no such user exists.</returns>
        [HttpGet("{username}", Name = "UserGet")]
        [ProducesResponseType(typeof(UserDto), 200, StatusCode = StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserByUsername(string username, string fields)
        {
            if (!_typeHelperService.TypeHasProperties<UserDto>(fields))
            {
                return BadRequest();
            }

            var user = await _unitOfWork.UserRepository.GetByUsername(username);
            if (user == null)
            {
                return NotFound();
            }
            var userToReturn = Mapper.Map<UserDto>(user);
            return Ok(userToReturn.ShapeData(fields));
        }

        /// <summary>
        /// Blocks a GetAll request because that is an admin privilege.
        /// </summary>
        /// <returns> HTTP 403: Forbidden </returns>
        [HttpGet]
        [ProducesResponseType(400, StatusCode = StatusCodes.Status400BadRequest)]
        public IActionResult BlockGetAll()
        {
            return BadRequest("You have to be an admin to obtain a list of users!");
        }


        /// <summary>
        ///     Registers a new user in the database. 
        /// </summary>
        /// <param name="user">The information for registering a user.</param>
        /// <returns>
        ///     Http 201 and the created user's information. 
        ///     Http 400 if the parameters are not valid.
        /// </returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), 201, StatusCode = StatusCodes.Status201Created)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            using (_unitOfWork)
            {
                if (await _unitOfWork.UserRepository.UserExists(user.Username))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }

                var userEntity = Mapper.Map<User>(user);
                userEntity.Password = Hasher.PasswordHash(userEntity, userEntity.Password);

                await _unitOfWork.UserRepository.Insert(userEntity);

                if (!await _unitOfWork.Commit())
                {
                    throw new Exception("Creating a user failed on save.");
                }

                // return 201 with the username surrogate key
                return CreatedAtRoute("UserGet", new { username = userEntity.Username }, Mapper.Map<UserDto>(userEntity));
            }
        }

        /// <summary>
        /// Blocks POST with a parameter to this controller.
        /// </summary>
        [HttpPost("{username}")]
        [ProducesResponseType(400, StatusCode = StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BlockUserRegister(string username)
        {
            if (await _unitOfWork.UserRepository.UserExists(username))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return BadRequest("You can't post to a specific user!");
        }


        /// <summary>
        /// Updates the current logged in user's information.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="model">The new information to update the user with. Everything is optional.</param>
        /// <returns>Http 200 and the updated user information if there is such a user and he is the current logged in user.</returns>
        [HttpPatch("{username}")]
        [HttpPut("{username}")]
        [ProducesResponseType(typeof(UserDto), 200, StatusCode = StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUser(string username, [FromBody] EditUserDto model)
        {
            if (!await _unitOfWork.UserRepository.UserExists(username))
            {
                return NotFound();
            }

            var user = await _unitOfWork.UserRepository.GetByUsername(username);
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
            if (!await _unitOfWork.Commit())
            {
                throw new Exception("Updating a user failed on save.");
            }

            return Ok(Mapper.Map<UserDto>(user));
        }

        /// <summary>
        /// Blocks a PATCH request without specific user parameters.
        /// </summary>
        [HttpPatch]
        [ProducesResponseType(400, StatusCode = StatusCodes.Status400BadRequest)]
        public IActionResult BlockPatchWithoutParameters()
        {
            return BadRequest("You have to provide a specific existing user in order to PATCH!");
        }

        /// <summary>
        /// Blocks a PUT request without specific user parameters.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(400, StatusCode = StatusCodes.Status400BadRequest)]
        public IActionResult BlockPutWithoutParameters()
        {
            return BadRequest("You have to provide a specific existing user in order to PUT!");
        }

        /// <summary>
        /// Blocks all DELETE requests to this controller because deleting users is not allowed.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [HttpDelete("{username}")]
        [ProducesResponseType(400, StatusCode = StatusCodes.Status400BadRequest)]
        public IActionResult BlockDeletingUsers()
        {
            return BadRequest("Deleting a user is not possible!");
        }
    }
}