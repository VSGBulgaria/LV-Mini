using System.ComponentModel.DataAnnotations;

namespace LVMiniAdminApi.Models
{
    public class LoginUserModel : BaseUser
    {
        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
