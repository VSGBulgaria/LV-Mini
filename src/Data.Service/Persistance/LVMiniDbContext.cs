using Data.Service.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Persistance
{
    public class LvMiniDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Loan> Loan { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }

        public LvMiniDbContext(DbContextOptions<LvMiniDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.Username, u.Email })
                .IsUnique();

            modelBuilder.Entity<UserTeam>()
                .HasKey(userTeam => new { userTeam.TeamId, userTeam.UserId });

            modelBuilder.Entity<ProductGroupProduct>()
                .HasKey(pgp => new { pgp.IDProduct, pgp.IDProductGroup });

            modelBuilder.Entity<Team>()
                .HasIndex(team => team.TeamName)
                .IsUnique();

            modelBuilder.Entity<ProductGroup>()
                .HasIndex(pg => pg.Name)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.ProductCode)
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.;Database=LV_Mini;Trusted_Connection=True;");
            }
        }
    }
}