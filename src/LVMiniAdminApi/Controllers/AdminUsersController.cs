using Data.Service.Core.Interfaces;
using LVMiniAdminApi.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LVMiniAdminApi.Models.UserModels;

namespace LVMiniAdminApi.Controllers
{
    /// <summary>
    /// Controller creating, reading and updating users.
    /// </summary>
    [Produces("application/json")]
    [Route("api/admin/users")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminUsersController : BaseController
    {
        public AdminUsersController(IModifiedUserHandler userHandler, ITeamRepository teamRepository, IUserRepository userRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _userHandler = userHandler;
        }

        /// <summary>
        /// Returns all stored users.
        /// </summary>
        /// <returns>Returns "Http 200 ok" with all users in the request body.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var users = _userRepository.GetAll();
            return Ok(users);
        }

        /// <summary>
        /// Update user info.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns "Http 200 ok" with modified user in the request body.
        /// Returns "Http 400 Bad request" if the state of the model is invalid.</returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]ModifiedUserModelDto user)
        {
            if (ModelState.IsValid)
            {
                var storedUser = await _userRepository.GetByUsername(user.Username);
                storedUser = _userHandler.SetChangesToStoredUser(storedUser, user);
                _userRepository.Update(storedUser);
                var storedUserWithTheChanges = await _userRepository.GetByUsername(user.Username);
                if (_userHandler.CheckTheChanges(storedUserWithTheChanges, user))
                {
                    return Ok(storedUser);
                }
            }
            return BadRequest("Invalid Model State.");
        }
    }
}
