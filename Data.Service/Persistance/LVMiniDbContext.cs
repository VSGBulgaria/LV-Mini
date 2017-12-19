using Data.Service.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Persistance
{
    public class LvMiniDbContext : DbContext
    {
        public LvMiniDbContext(DbContextOptions<LvMiniDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
