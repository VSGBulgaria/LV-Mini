using Data.Service.Core.Entities;
using System.Collections.Generic;

namespace LVMiniApi.Models
{
    public class DispalyProductGroupDto
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
