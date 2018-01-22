using Data.Service.Core.Entities;
using Data.Service.Core.Enums;
using Data.Service.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LVMiniApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogRepository _logRepository;

        public AccountsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logRepository = _unitOfWork.Logs;
        }

        [Route("/logout")]
        [HttpPost("{username}")]
        [Authorize]
        public async Task Logout([FromBody] string username)
        {
            string logOutAction = UserAction.Logout.ToString();
            Log logoutLog = new Log()
            {
                Action = logOutAction,
                Username = username,
                Time = DateTime.Now
            };

            await _logRepository.Insert(logoutLog);
            await _unitOfWork.Commit();
        }
    }
}