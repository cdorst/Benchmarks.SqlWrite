using Benchmarks.Model;
using Microsoft.EntityFrameworkCore;

namespace Benchmarks
{
    public class AppDbContext : DbContext
    {
        public DbSet<EntityA> EntityA { get; set; }
        public DbSet<EntityB> EntityB { get; set; }
        public DbSet<EntityC> EntityC { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(Constants.ConnectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntityA>().HasAlternateKey(
                nameof(Benchmarks.Model.EntityA.EntityBId),
                nameof(Benchmarks.Model.EntityA.EntityCId));
            modelBuilder.Entity<EntityB>().HasAlternateKey(
                nameof(Benchmarks.Model.EntityB.MyProperty));
            modelBuilder.Entity<EntityC>().HasAlternateKey(
                nameof(Benchmarks.Model.EntityC.MyProperty));
        }
    }
}
