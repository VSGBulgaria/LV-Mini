using Data.Service.Core.Entities;
using Data.Service.Core.Enums;
using Data.Service.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace LVMiniApi.Api.Service
{
    /// <summary>
    /// Provides a database logger.
    /// </summary>
    internal class Logger
    {
        private ILogRepository _logRepository;

        public Logger(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        /// <summary>
        ///Inserts a log of the current user and action in the database.
        /// </summary>
        public async Task InsertLog(string username, UserAction action)
        {
            string actionName = action.ToString();

            Log log = new Log
            {
                Action = actionName,
                Username = username,
                Time = DateTime.Now
            };

            await _logRepository.Insert(log);
        }
    }
}
