using Data.Service.Core;
using Data.Service.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Service.Persistance.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly DbContext Context;
        protected readonly DbSet<T> Entities;

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
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task Insert(T entity)
        {
            await Entities.AddAsync(entity);
        }

        public void Update(T entity)
        {
            Entities.Update(entity);
        }

        public void Delete(T entity)
        {
            Entities.Remove(entity);
        }
    }
}
