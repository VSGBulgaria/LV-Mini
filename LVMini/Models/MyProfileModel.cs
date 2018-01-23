namespace LVMini.Models
{
    public class MyProfileModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public MyProfileModel(string username, string email, string firstName, string lastName)
        {
            Username = username;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
