using Data.Service.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Service.Core.Interfaces
{
    public interface IProductGroupRepository : IBaseRepository<ProductGroup>
    {
        Task<bool> ProductGroupExists(string name);
        IEnumerable<ProductGroup> GetAll();
    }
}
