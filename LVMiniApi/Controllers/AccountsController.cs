using Data.Service.Core.Enums;
using Data.Service.Core.Interfaces;
using LVMiniApi.Api.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LVMiniApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Logger _logger;

        public AccountsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = new Logger(_unitOfWork.Logs);
        }

        [Route("logout")]
        [HttpPost]
        public async Task Logout([FromBody] string username)
        {
            await _logger.InsertLog(username, UserAction.Logout);
            await _unitOfWork.Commit();
        }

        [Route("login")]
        [HttpPost]
        public async Task Login([FromBody] string username)
        {
            await _logger.InsertLog(username, UserAction.Login);
            await _unitOfWork.Commit();
        }
    }
}