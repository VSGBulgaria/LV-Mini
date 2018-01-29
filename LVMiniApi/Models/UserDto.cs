namespace LVMiniApi.Models
{
    /// <summary>
    /// Model for displaying an existing user from the database.
    /// </summary>
    public class UserDto
    {
        public string Url { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
