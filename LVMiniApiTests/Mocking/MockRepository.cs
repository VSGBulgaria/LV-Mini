using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data.Service.Core;
using Data.Service.Core.Entities;
using NUnit.Framework;

namespace LVMiniApiTests.Mocking
{
    class MockRepository : IUserRepository
    {
        public IEnumerable<User> GetAll(Expression<Func<User, bool>> filterExpression = null)
        {
            return new List<User>();
        }

        public Task<User> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Insert(User entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(User entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
