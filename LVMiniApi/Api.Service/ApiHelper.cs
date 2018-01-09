using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Service.Core;
using Data.Service.Core.Entities;
using Data.Service.Core.Enums;
using LVMiniApi.Models;

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
        public static async Task InsertLog(string username, LogType type, DateTime time, ILogRepository logRepository)
        {
            Log log = new Log
            {
                ActionId = (int)type,
                Username = username,
                Time = time
            };

            await logRepository.Insert(log);
        }

        public static User ValidateUpdate(User user, EditUserModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Email))
                user.Email = model.Email;

            if (!string.IsNullOrWhiteSpace(model.Firstname))
                user.FirstName = model.Firstname;
            if (model.Firstname == "null")
                user.FirstName = null;

            if (!string.IsNullOrWhiteSpace(model.Lastname))
                user.LastName = model.Lastname;
            if (model.Lastname == "null")
                user.LastName = null;

            return user;
        }
    }
}
