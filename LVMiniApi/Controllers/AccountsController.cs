﻿using Data.Service.Core.Enums;
using Data.Service.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LVMiniApi.Controllers
{
    /// <summary>
    /// A helper controller for logging user actions on their execution.
    /// </summary>
    [Route("api/accounts")]
    public class AccountsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            LogRepository = unitOfWork.LogRepository;
        }

        [HttpPost("logout")]
        public async Task Logout([FromBody] string username)
        {
            await LogRepository.InsertLog(username, UserAction.Logout);
            await _unitOfWork.Commit();
        }

        [HttpPost("login")]
        public async Task Login([FromBody] string username)
        {
            await LogRepository.InsertLog(username, UserAction.Login);
            await _unitOfWork.Commit();
        }
    }
}