namespace LVMiniApi.Models
{
    /// <summary>
    /// Model for the edit user action.
    /// </summary>
    public class EditUserDto
    {
        public string Email { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }
    }
}
