using LVMiniAdminApi.Attributes;

namespace LVMiniAdminApi.Models
{
    public class ModifiedUserModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        [Changeable]
        public string FirstName { get; set; }
        [Changeable]
        public string LastName { get; set; }
    }
}
