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
    public class AdminController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IModifiedUserHandler _userHandler;

        public AdminController(IUnitOfWork unitOfWork, IModifiedUserHandler handler)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.UserRepository;
            _userHandler = handler;
        }

        // GET: api/admin/users
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repository.GetAll());
        }

        // PUT: api/admin/users
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]ModifiedUserModel user)
        {
            if (ModelState.IsValid)
            {
                var storedUser = await _repository.GetByUsername(user.Username);
                storedUser = _userHandler.SetChangesToStoredUser(storedUser, user);
                _repository.Update(storedUser);
                var resultCommits = await _unitOfWork.Commit();
                var storedUserWithTheChanges = await _repository.GetByUsername(user.Username);
                if (_userHandler.CheckTheChanges(storedUserWithTheChanges, user))
                {
                    return Ok(storedUser);
                }

            }
            return BadRequest("Invalid Model State.");
        }
    }
}
