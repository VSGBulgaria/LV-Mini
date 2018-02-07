using Data.Service.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Data.Service.Persistance.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DbContext Context;
        protected readonly DbSet<T> Entities;

        protected BaseRepository(LvMiniDbContext context)
        {
            Context = context;
            Entities = Context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>> filterExpression = null)
        {
            IQueryable<T> query = Entities;

            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }

            return query;
        }

        public virtual async Task<T> GetById(int id)
        {
            return await Entities.FindAsync(id);
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
