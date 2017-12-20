using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Service.Core;
using Data.Service.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Persistance.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected DbContext Context;
        protected DbSet<T> Entities;

        protected BaseRepository(DbContext context)
        {
            Context = context;
            Entities = Context.Set<T>();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filterExpression = null)
        {
            IQueryable<T> query = Entities;

            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }

            return query.AsNoTracking();
        }

        public async Task<T> GetById(int id)
        {
            return await Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Insert(T entity)
        {
            await Entities.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            Entities.Update(entity);
            await Context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            Entities.Remove(entity);
            await Context.SaveChangesAsync();
        }
    }
}
