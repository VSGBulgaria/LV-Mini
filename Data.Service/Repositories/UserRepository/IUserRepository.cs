using System;
using System.Collections.Generic;
using System.Text;
using Data.Service.Entities;
using Data.Service.Repositories.BaseRepository;

namespace Data.Service.Repositories.UserRepository
{
    public interface IUserRepository : IBaseRepository<User>
    {
    }
}
