namespace LVMiniApi.Models
{
    /// <summary>
    /// Model for editing user information.
    /// </summary>
    public class EditUserDto
    {
        public string Email { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }
    }
}
