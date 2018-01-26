namespace LVMiniApi.Models
{
    /// <summary>
    /// Model for displaying an existing user from the database.
    /// </summary>
    public class UserModel
    {
        public string Url { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }
    }
}
