using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Benchmarks.Migrations
{
    public partial class Schema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("SqlServer:MemoryOptimized", true);

            migrationBuilder.CreateTable(
                name: "EntityB",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MyProperty = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityB", x => x.Id);
                    table.UniqueConstraint("AK_EntityB_MyProperty", x => x.MyProperty);
                });

            migrationBuilder.CreateTable(
                name: "EntityC",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MyProperty = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityC", x => x.Id);
                    table.UniqueConstraint("AK_EntityC_MyProperty", x => x.MyProperty);
                });

            migrationBuilder.CreateTable(
                name: "MemoryOptimizedEntityB",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MyProperty = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryOptimizedEntityB", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.UniqueConstraint("AK_MemoryOptimizedEntityB_MyProperty", x => x.MyProperty)
                        .Annotation("SqlServer:Clustered", false);
                })
                .Annotation("SqlServer:MemoryOptimized", true);

            migrationBuilder.CreateTable(
                name: "MemoryOptimizedEntityC",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MyProperty = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryOptimizedEntityC", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.UniqueConstraint("AK_MemoryOptimizedEntityC_MyProperty", x => x.MyProperty)
                        .Annotation("SqlServer:Clustered", false);
                })
                .Annotation("SqlServer:MemoryOptimized", true);

            migrationBuilder.CreateTable(
                name: "EntityA",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EntityBId = table.Column<short>(nullable: false),
                    EntityCId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityA", x => x.Id);
                    table.UniqueConstraint("AK_EntityA_EntityBId_EntityCId", x => new { x.EntityBId, x.EntityCId });
                    table.ForeignKey(
                        name: "FK_EntityA_EntityB_EntityBId",
                        column: x => x.EntityBId,
                        principalTable: "EntityB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityA_EntityC_EntityCId",
                        column: x => x.EntityCId,
                        principalTable: "EntityC",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemoryOptimizedEntityA",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EntityBId = table.Column<short>(nullable: false),
                    EntityCId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryOptimizedEntityA", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.UniqueConstraint("AK_MemoryOptimizedEntityA_EntityBId_EntityCId", x => new { x.EntityBId, x.EntityCId })
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_MemoryOptimizedEntityA_MemoryOptimizedEntityB_EntityBId",
                        column: x => x.EntityBId,
                        principalTable: "MemoryOptimizedEntityB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemoryOptimizedEntityA_MemoryOptimizedEntityC_EntityCId",
                        column: x => x.EntityCId,
                        principalTable: "MemoryOptimizedEntityC",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("SqlServer:MemoryOptimized", true);

            migrationBuilder.CreateIndex(
                name: "IX_EntityA_EntityCId",
                table: "EntityA",
                column: "EntityCId");

            migrationBuilder.CreateIndex(
                name: "IX_MemoryOptimizedEntityA_EntityCId",
                table: "MemoryOptimizedEntityA",
                column: "EntityCId")
                .Annotation("SqlServer:Clustered", false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityA");

            migrationBuilder.DropTable(
                name: "MemoryOptimizedEntityA")
                .Annotation("SqlServer:MemoryOptimized", true);

            migrationBuilder.DropTable(
                name: "EntityB");

            migrationBuilder.DropTable(
                name: "EntityC");

            migrationBuilder.DropTable(
                name: "MemoryOptimizedEntityB")
                .Annotation("SqlServer:MemoryOptimized", true);

            migrationBuilder.DropTable(
                name: "MemoryOptimizedEntityC")
                .Annotation("SqlServer:MemoryOptimized", true);

            migrationBuilder.AlterDatabase()
                .OldAnnotation("SqlServer:MemoryOptimized", true);
        }
    }
}
