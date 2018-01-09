using Data.Service.Core;
using LVMiniApi.Api.Service;
using LVMiniApi.Filters;
using LVMiniApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static LVMiniApi.Api.Service.UserValidator;

namespace LVMiniApi.Controllers
{
    [Produces("application/json")]
    [Route("api/login")]
    public class LoginController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            LogRepository = _unitOfWork.Logs;
            UserRepository = _unitOfWork.Users;
        }

        // POST api/login
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] LoginUserModel user)
        {
            if (!ValidateUserExists(user, UserRepository))
            {
                ModelState.AddModelError("Username", "No such user!");
                return NotFound(ModelState);
            }

            var getUser = await UserRepository.GetByUsername(user.Username);
            user.Password = Hasher.PasswordHash(user.Password);

            if (user.Password == getUser.Password && user.Username == getUser.Username)
            {
                //await InsertLog(getUser.Username, LogType.Login, LogRepository);
                //await _unitOfWork.Commit();
            }

            return BadRequest("Login failed!");
        }
    }
}