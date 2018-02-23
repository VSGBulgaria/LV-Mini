using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Service.Persistance.Repositories
{
    public class ProductGroupRepository : BaseRepository<ProductGroup>, IProductGroupRepository
    {
        public ProductGroupRepository(LvMiniDbContext context) : base(context)
        {
        }

        public override IEnumerable<ProductGroup> GetAll(Expression<Func<ProductGroup, bool>> filterExpression = null)
        {
            return Entities
                .AsNoTracking()
                .Include(pg => pg.Products)
                .ThenInclude(pgp => pgp.Product)
                .ToList();
        }

        public override Task<ProductGroup> GetById(int id)
        {
            return Entities
                .Include(pg => pg.Products)
                .ThenInclude(pgp => pgp.Product)
                .FirstOrDefaultAsync(x => x.IDProductGroup == id);
        }

        public async Task<bool> ProductGroupExists(string name)
        {
            return await Entities
                .AsNoTracking()
                .AnyAsync(pg => pg.Name == name);
        }

        public async Task<ProductGroup> GetProductGroupByName(string name)
        {
            return await Entities
                .Include(pg => pg.Products)
                .ThenInclude(pgp => pgp.Product)
                .FirstOrDefaultAsync(pg => pg.Name == name);
        }

        public async Task<Product> GetProductByCode(string code)
        {
            return await Context.Set<Product>().FirstOrDefaultAsync(p => p.ProductCode == code);
        }

        public async Task<bool> ProductGroupContainsProduct(string productGroupName, string productCode)
        {
            var productGroup = await Entities
                .Where(pg => pg.Name == productGroupName)
                .Include(pg => pg.Products)
                .ThenInclude(pgp => pgp.Product)
                .FirstOrDefaultAsync();

            if (productGroup == null)
                return false;

            foreach (var item in productGroup.Products)
            {
                if (item.Product.ProductCode == productCode)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
