using Data.Service.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Persistance
{
    public class LvMiniDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Loan> Loan { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }

        public LvMiniDbContext(DbContextOptions<LvMiniDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.Username, u.Email })
                .IsUnique();

            modelBuilder.Entity<UserTeam>()
                .HasKey(userTeam => new { userTeam.TeamId, userTeam.UserId });

            modelBuilder.Entity<UserTeam>()
                .HasOne(userTeam => userTeam.Team)
                .WithMany(team => team.UsersTeams)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(userTeam => userTeam.TeamId);

            modelBuilder.Entity<UserTeam>()
                .HasOne(userTeam => userTeam.User)
                .WithMany(user => user.UsersTeams)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(userTeam => userTeam.UserId);

            modelBuilder.Entity<ProductGroupProduct>()
                .HasKey(pgp => new { pgp.IDProduct, pgp.IDProductGroup });

            modelBuilder.Entity<Team>()
                .HasIndex(team => team.TeamName)
                .IsUnique();

        }
    }
}