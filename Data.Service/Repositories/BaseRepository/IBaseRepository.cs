using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Service.Entities;

namespace Data.Service.Repositories.BaseRepository
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filterExpression = null);
        Task<T> GetById(int id);

        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(int id);
    }
}
