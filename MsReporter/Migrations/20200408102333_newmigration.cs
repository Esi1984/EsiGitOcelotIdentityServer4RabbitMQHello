using Microsoft.EntityFrameworkCore.Migrations;

namespace MsReporter.Migrations
{
    public partial class newmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportsHello",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CounterMs1 = table.Column<decimal>(nullable: false),
                    CounterMs2 = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportsHello", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportsHello");
        }
    }
}
