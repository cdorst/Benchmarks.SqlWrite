using Benchmarks.Model;
using Microsoft.EntityFrameworkCore;

namespace Benchmarks
{
    public class AppDbContext : DbContext
    {
        public DbSet<EntityA> EntityA { get; set; }
        public DbSet<EntityB> EntityB { get; set; }
        public DbSet<EntityC> EntityC { get; set; }
        public DbSet<MemoryOptimizedEntityA> MemoryOptimizedEntityA { get; set; }
        public DbSet<MemoryOptimizedEntityB> MemoryOptimizedEntityB { get; set; }
        public DbSet<MemoryOptimizedEntityC> MemoryOptimizedEntityC { get; set; }

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

            modelBuilder.Entity<MemoryOptimizedEntityA>()
                .ForSqlServerIsMemoryOptimized()
                .HasAlternateKey(
                    nameof(Benchmarks.Model.MemoryOptimizedEntityA.EntityBId),
                    nameof(Benchmarks.Model.MemoryOptimizedEntityA.EntityCId));
            modelBuilder.Entity<MemoryOptimizedEntityA>()
                .HasOne<MemoryOptimizedEntityB>(nameof(Benchmarks.Model.MemoryOptimizedEntityA.EntityB)).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MemoryOptimizedEntityA>()
                .HasOne<MemoryOptimizedEntityC>(nameof(Benchmarks.Model.MemoryOptimizedEntityA.EntityC)).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MemoryOptimizedEntityB>()
                .ForSqlServerIsMemoryOptimized()
                .HasAlternateKey(nameof(Benchmarks.Model.MemoryOptimizedEntityB.MyProperty));
            modelBuilder.Entity<MemoryOptimizedEntityC>()
                .ForSqlServerIsMemoryOptimized()
                .HasAlternateKey(nameof(Benchmarks.Model.MemoryOptimizedEntityC.MyProperty));
        }
    }
}
