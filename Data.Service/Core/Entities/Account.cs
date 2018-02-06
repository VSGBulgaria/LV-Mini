using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Service.Core.Entities
{
    [Table("Account", Schema = "IbClue")]
    public class Account
    {
        [Key]
        [Required]
        public int IDAccount { get; set; }

        [Required]
        public int IDProduct { get; set; }

        [Required]
        [MaxLength(15)]
        public string ProductCode { get; set; }

        [Required]
        [MaxLength(10)]
        public string AccountCategoryCode { get; set; }

        [Required]
        [MaxLength(10)]
        public string AccountStatusCode { get; set; }
    }
}
