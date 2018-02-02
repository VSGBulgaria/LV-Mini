using System.ComponentModel.DataAnnotations;

namespace LVMiniAdminApi.Models
{
    public abstract class BaseUser
    {
        [Required]
        [StringLength(20)]
        public string Username { get; set; }
    }
}
