using Data.Service.Core;
using System.ComponentModel.DataAnnotations;

namespace LVMiniApi.Models
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
