using Microsoft.EntityFrameworkCore;
using PCStoreApi.Domain.Entities;

namespace PCStoreApi.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {                
        }

        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<PCBuild> PCBuilds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>().ToTable(nameof(UserInfo));
            modelBuilder.Entity<UserInfo>()
                .HasOne(u => u.PCBuild)
                .WithOne(p => p.User)
                .HasForeignKey<PCBuild>(p => p.UserID);
        }
    }
}
