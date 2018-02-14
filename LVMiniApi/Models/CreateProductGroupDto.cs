using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LVMiniApi.Models
{
    /// <summary>
    /// Model for creating a ProductGroup.
    /// </summary>
    public class CreateProductGroupDto
    {
        /// <summary>
        /// The name of the group. This is required.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// An optional collection of initial product ids.
        /// </summary>
        public ICollection<int> Products { get; set; }
            = new Collection<int>();
    }
}
