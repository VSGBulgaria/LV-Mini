﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Service.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Data.Service.Core
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filterExpression = null);
        Task<T> GetById(int id);

        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(int id);
    }
}
