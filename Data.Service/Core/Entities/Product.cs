using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Service.Core.Entities
{
    [Table("Product", Schema = "IbClue")]
    public class Product
    {
        [Key]
        public int IDProduct { get; set; }

        [Required]
        [MaxLength(15)]
        public string ProductCode { get; set; }

        [Required]
        [MaxLength(150)]
        public string ProductDescription { get; set; }

        [Required]
        public byte ProductType { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public bool IsHidden { get; set; }
    }
}
