using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LVMiniApi.Models
{
    public class CreateProductGroupDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<int> Products { get; set; }
            = new Collection<int>();
    }
}
