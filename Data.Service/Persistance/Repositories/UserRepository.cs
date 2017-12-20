using System.Threading.Tasks;
using Data.Service.Core;
using Data.Service.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Persistance.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(LvMiniDbContext context)
            : base(context)
        {
        }

        public async Task<User> GetByUsername(string username)
        {
            return await Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
