using Data.Service.Core;
using Data.Service.Core.Entities;

namespace Data.Service.Persistance.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(LvMiniDbContext context)
            : base(context)
        {
        }
    }
}
