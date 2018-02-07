using Data.Service.Core.Entities;
using System.Threading.Tasks;

namespace Data.Service.Core.Interfaces
{
    public interface IProductGroupRepository : IBaseRepository<ProductGroup>
    {
        Task<bool> ProductGroupExists(string name);
    }
}
