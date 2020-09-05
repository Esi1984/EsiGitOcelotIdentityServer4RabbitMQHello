using Microsoft.EntityFrameworkCore.Migrations;

namespace MsReporter3.Migrations
{
    public partial class newmigrationR02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportHello",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    message = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportHello", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportHello");
        }
    }
}
