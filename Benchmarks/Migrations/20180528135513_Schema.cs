using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Benchmarks.Migrations
{
    public partial class Schema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_EntityA_EntityCId",
                table: "EntityA",
                column: "EntityCId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityA");

            migrationBuilder.DropTable(
                name: "EntityB");

            migrationBuilder.DropTable(
                name: "EntityC");
        }
    }
}
