﻿using Data.Service.Core.Entities;
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
                .Include(p => p.Products)
                .ThenInclude(pg => pg.Product)
                .ToList();
        }

        public override Task<ProductGroup> GetById(int id)
        {
            return Entities
                .Include(p => p.Products)
                .ThenInclude(pg => pg.Product)
                .FirstOrDefaultAsync(x => x.IDProductGroup == id);
        }

        public async Task<bool> ProductGroupExists(string name)
        {
            return await Entities
                .AsNoTracking()
                .AnyAsync(pg => pg.Name == name);
        }
    }
}
