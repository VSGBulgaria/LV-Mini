using System.ComponentModel.DataAnnotations;
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
        public DbSet<Log> Logs { get; set; }
        public DbSet<LogAction> Actions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => new {u.Username, u.Email})
                .IsUnique();
        }
    }
}
