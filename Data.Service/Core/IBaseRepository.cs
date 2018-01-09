using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Service.Core
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filterExpression = null);
        Task<T> GetById(int id);

        Task Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
