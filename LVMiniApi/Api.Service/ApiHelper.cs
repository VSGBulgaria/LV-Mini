using System;
using System.Linq;
using Data.Service.Core;
using Data.Service.Core.Entities;
using Data.Service.Core.Enums;

namespace LVMiniApi.Api.Service
{
    public static class ApiHelper
    {
        //Checks if such a user exists in the database.
        public static bool UserExists(IUser user, IUserRepository userRepository)
        {
            return userRepository.GetAll(u => u.Username == user.Username).ToList().Count > 0;
        }

        //Inserts a log in the database.
        public static void InsertLog(int userId, LogAction action, DateTime time, ILogRepository logRepository)
        {
            Log log = new Log
            {
                Action = action,
                UserId = userId,
                Time = time
            };

            logRepository.Insert(log);
        }
    }
}
