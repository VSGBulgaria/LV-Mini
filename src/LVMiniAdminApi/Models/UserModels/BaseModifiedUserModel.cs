using LVMiniAdminApi.Attributes;

namespace LVMiniAdminApi.Models
{
    public abstract class BaseModifiedUserModel : BaseUser
    {
        public string Email { get; set; }
        [Changeable]
        public string FirstName { get; set; }
        [Changeable]
        public string LastName { get; set; }
        [Changeable]
        public bool IsActive { get; set; }
    }
}
