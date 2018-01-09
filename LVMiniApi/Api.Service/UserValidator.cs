using Data.Service.Core;
using Data.Service.Core.Entities;
using LVMiniApi.Models;
using System.Linq;

namespace LVMiniApi.Api.Service
{
    /// <summary>
    /// Provides user-related helper methods for the main API.
    /// </summary>
    internal class UserValidator
    {
        /// <summary>
        ///Checks if such a user exists in the database.
        /// </summary>
        /// <returns>
        /// True or False.
        /// </returns>
        public static bool ValidateUserExists(User user, IUserRepository userRepository)
        {
            return userRepository.GetAll(u => u.Username == user.Username).ToList().Count > 0;
        }

        /// <summary>
        ///Validates the updated UserModel information and mapps it to the existing user in the database.
        /// </summary>
        /// <returns>
        /// The new, updated user.
        /// </returns>
        public static void ValidateUserUpdate(User user, EditUserModel model)
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
        }
    }
}
