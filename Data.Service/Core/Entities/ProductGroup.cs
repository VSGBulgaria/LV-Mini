using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Service.Core.Entities
{
    [Table("ProductGroup", Schema = "admin")]
    public class ProductGroup
    {
        [Key]
        public int IDProductGroup { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public ICollection<ProductGroupProduct> Products { get; set; } = new Collection<ProductGroupProduct>();
    }
}
