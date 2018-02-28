using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Data.Service.Migrations
{
    public partial class AddNewColumnToProductGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearlyBudget",
                schema: "admin",
                table: "ProductGroup",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearlyBudget",
                schema: "admin",
                table: "ProductGroup");
        }
    }
}
