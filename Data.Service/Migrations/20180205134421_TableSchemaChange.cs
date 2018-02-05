using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Service.Migrations
{
    public partial class TableSchemaChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "admin");

            migrationBuilder.RenameTable(
                name: "Users",
                newSchema: "admin");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                newSchema: "admin");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                newSchema: "admin");

            migrationBuilder.RenameTable(
                name: "Logs",
                newSchema: "admin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Users",
                schema: "admin");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                schema: "admin");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                schema: "admin");

            migrationBuilder.RenameTable(
                name: "Logs",
                schema: "admin");
        }
    }
}
