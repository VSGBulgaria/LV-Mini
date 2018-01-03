using System.ComponentModel.DataAnnotations;
using Data.Service.Core;

namespace LVMiniAdminApi.Models
{
    public class LoginUserModel : IUser
    {
        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
