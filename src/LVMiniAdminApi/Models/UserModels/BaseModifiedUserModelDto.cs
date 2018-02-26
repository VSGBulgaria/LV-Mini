using LVMiniAdminApi.Attributes;

namespace LVMiniAdminApi.Models.UserModels
{
    public abstract class BaseModifiedUserModelDto : BaseUserDto
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
