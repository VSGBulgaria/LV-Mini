namespace LVMiniApi.Models
{
    /// <summary>
    /// The model for editing a user. All fields are optional.
    /// </summary>
    public class EditUserDto
    {
        /// <summary>
        /// The user's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The user's FirstName.
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// The user's LastName.
        /// </summary>
        public string Lastname { get; set; }
    }
}
