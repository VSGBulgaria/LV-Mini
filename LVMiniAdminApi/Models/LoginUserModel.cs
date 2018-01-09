using System.ComponentModel.DataAnnotations;

namespace LVMiniAdminApi.Models
{
    public class LoginUserModel
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
