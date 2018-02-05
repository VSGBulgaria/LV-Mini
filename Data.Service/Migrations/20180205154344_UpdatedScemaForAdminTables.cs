using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Data.Service.Migrations
{
    public partial class UpdatedScemaForAdminTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UsersTeams",
                newSchema: "admin");

            migrationBuilder.RenameTable(
                name: "Teams",
                newSchema: "admin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UsersTeams",
                schema: "admin");

            migrationBuilder.RenameTable(
                name: "Teams",
                schema: "admin");
        }
    }
}
