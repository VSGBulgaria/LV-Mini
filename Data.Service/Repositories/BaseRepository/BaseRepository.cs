using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Service.Entities;
using Data.Service.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Repositories.BaseRepository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly LvMiniDbContext _context;
        private readonly DbSet<T> _entities;

        protected BaseRepository(LvMiniDbContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filterExpression = null)
        {
            IQueryable<T> query = _entities;

            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }

            return query.AsNoTracking();
        }

        public async Task<T> GetById(int id)
        {
            return await _entities
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Insert(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
