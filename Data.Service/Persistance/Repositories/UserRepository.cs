using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<User> GetByUsernameAndPassword(string username, string password)
        {
            return await Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public bool UserIsOwner(string ownerId, string dbUserId)
        {
            var user = GetAll(u => u.Id == Convert.ToInt32(dbUserId));

            return user != null && user.Any(u => u.Id == Convert.ToInt32(ownerId));
        }
    }
}
