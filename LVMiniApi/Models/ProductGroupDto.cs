using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LVMiniApi.Models
{
    public class ProductGroupDto
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public ICollection<ProductDto> Products { get; set; } = new Collection<ProductDto>();
    }
}
