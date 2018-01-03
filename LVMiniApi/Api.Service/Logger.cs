using Data.Service.Core;
using Data.Service.Core.Entities;
using Data.Service.Core.Enums;
using System;
using System.Threading.Tasks;

namespace LVMiniApi.Api.Service
{
    /// <summary>
    /// Provides a database logger.
    /// </summary>
    internal class Logger
    {
        /// <summary>
        ///Inserts a log of the current user and action in the database.
        /// </summary>
        public static async Task InsertLog(string username, LogType type, ILogRepository logRepository)
        {
            Log log = new Log
            {
                ActionId = (int)type,
                Username = username,
                Time = DateTime.Now
            };

            await logRepository.Insert(log);
        }
    }
}
