﻿// <auto-generated />
using Benchmarks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Benchmarks.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20180528135513_Schema")]
    partial class Schema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rc1-32029")
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
#pragma warning restore 612, 618
        }
    }
}
