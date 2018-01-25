namespace LVMini.Models
{
    public class MyProfileModel
    {
        public MyProfileModel(string username, string email, string firstname, string lastname)
        {
            Username = username;
            Email = email;
            FirstName = firstname;
            LastName = lastname;
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
