using Data.Service.Core.Enums;
using Data.Service.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LVMiniApi.Controllers
{
    /// <summary>
    /// A helper controller for logging user actions on their execution.
    /// </summary>
    [Route("api/accounts")]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Inject the classes needed through constructor injection.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        public AccountsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Logs to the database that the user has logged out.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <returns>Void</returns>
        [HttpPost("logout")]
        public async Task Logout([FromBody] string username)
        {
            await _unitOfWork.LogRepository.InsertLog(username, UserAction.Logout);
            await _unitOfWork.Commit();
        }

        /// <summary>
        /// Logs to the database that the user has logged in.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <returns>Void</returns>
        [HttpPost("login")]
        public async Task Login([FromBody] string username)
        {
            await _unitOfWork.LogRepository.InsertLog(username, UserAction.Login);
            await _unitOfWork.Commit();
        }

        /// <summary>
        /// Logs to the database that the user has updated his/her profile.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <returns>Void</returns>
        [HttpPost("profileupdate")]
        public async Task ProfileUpdate([FromBody] string username)
        {
            await _unitOfWork.LogRepository.InsertLog(username, UserAction.ProfileUpdate);
            await _unitOfWork.Commit();
        }
    }
}