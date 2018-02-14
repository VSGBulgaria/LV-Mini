using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LVMiniApi.Models
{
    /// <summary>
    /// Model for representing the information of a product group.
    /// </summary>
    public class ProductGroupDto
    {
        /// <summary>
        /// Shows the url you can use to access the product group.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The name of the product group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Shows if the group is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Displays all the products the group contains.
        /// </summary>
        public ICollection<ProductDto> Products { get; set; } = new Collection<ProductDto>();
    }
}
