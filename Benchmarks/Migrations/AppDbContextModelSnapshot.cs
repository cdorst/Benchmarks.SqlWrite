﻿// <auto-generated />
using Benchmarks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Benchmarks.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Benchmarks.Model.EntityA", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<short>("EntityBId");

                    b.Property<long>("EntityCId");

                    b.HasKey("Id");

                    b.HasAlternateKey("EntityBId", "EntityCId");

                    b.HasIndex("EntityCId");

                    b.ToTable("EntityA");
                });

            modelBuilder.Entity("Benchmarks.Model.EntityB", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MyProperty")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAlternateKey("MyProperty");

                    b.ToTable("EntityB");
                });

            modelBuilder.Entity("Benchmarks.Model.EntityC", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MyProperty")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAlternateKey("MyProperty");

                    b.ToTable("EntityC");
                });

            modelBuilder.Entity("Benchmarks.Model.MemoryOptimizedEntityA", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<short>("EntityBId");

                    b.Property<long>("EntityCId");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasAlternateKey("EntityBId", "EntityCId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("EntityBId")
                        .IsUnique()
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("EntityCId")
                        .IsUnique()
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.ToTable("MemoryOptimizedEntityA");

                    b.HasAnnotation("SqlServer:MemoryOptimized", true);
                });

            modelBuilder.Entity("Benchmarks.Model.MemoryOptimizedEntityB", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MyProperty")
                        .IsRequired();

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasAlternateKey("MyProperty")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.ToTable("MemoryOptimizedEntityB");

                    b.HasAnnotation("SqlServer:MemoryOptimized", true);
                });

            modelBuilder.Entity("Benchmarks.Model.MemoryOptimizedEntityC", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MyProperty")
                        .IsRequired();

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasAlternateKey("MyProperty")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.ToTable("MemoryOptimizedEntityC");

                    b.HasAnnotation("SqlServer:MemoryOptimized", true);
                });

            modelBuilder.Entity("Benchmarks.Model.EntityA", b =>
                {
                    b.HasOne("Benchmarks.Model.EntityB", "EntityB")
                        .WithMany()
                        .HasForeignKey("EntityBId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Benchmarks.Model.EntityC", "EntityC")
                        .WithMany()
                        .HasForeignKey("EntityCId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Benchmarks.Model.MemoryOptimizedEntityA", b =>
                {
                    b.HasOne("Benchmarks.Model.MemoryOptimizedEntityB", "EntityB")
                        .WithOne()
                        .HasForeignKey("Benchmarks.Model.MemoryOptimizedEntityA", "EntityBId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Benchmarks.Model.MemoryOptimizedEntityC", "EntityC")
                        .WithOne()
                        .HasForeignKey("Benchmarks.Model.MemoryOptimizedEntityA", "EntityCId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
