using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Service.Persistance.Repositories
{
    public class ProductGroupRepository : BaseRepository<ProductGroup>, IProductGroupRepository
    {
        public ProductGroupRepository(LvMiniDbContext context) : base(context)
        {
        }

        public IEnumerable<ProductGroup> GetAll()
        {
            return Entities
                .Include(p => p.Products)
                .ThenInclude(pg => pg.Product)
                .ToList();
        }

        public async Task<bool> ProductGroupExists(string name)
        {
            return await Entities
                .AsNoTracking()
                .AnyAsync(pg => pg.Name == name);
        }
    }
}
