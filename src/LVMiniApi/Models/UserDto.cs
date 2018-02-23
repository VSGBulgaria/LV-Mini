namespace LVMiniApi.Models
{
    /// <summary>
    /// A model for representing the information for a user.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// The url at which you can access the user.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The user's username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The user's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The user's first name.
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// The user's last name.
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// Weather the user is still active or not.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
