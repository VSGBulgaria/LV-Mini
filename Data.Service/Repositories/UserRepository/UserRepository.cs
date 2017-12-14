using Data.Service.Entities;
using Data.Service.Persistance;
using Data.Service.Repositories.BaseRepository;

namespace Data.Service.Repositories.UserRepository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(LvMiniDbContext context)
            : base(context)
        {
        }
    }
}
