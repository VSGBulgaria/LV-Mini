using Data.Service.Core.Interfaces;
using LVMiniAdminApi.Contracts;
using LVMiniAdminApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LVMiniAdminApi.Controllers
{
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

        // GET: api/admin/users
        [HttpGet]
        public IActionResult Get()
        {
            var users = _userRepository.GetAll();
            return Ok(users);
        }

        // PUT: api/admin/users
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]ModifiedUserModel user)
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
