using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Data.Service.Migrations
{
    public partial class SummingAndSelectingAllClosedLoans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE LV_Mini.admin.ProductGroup SET YearlyBudget = IDProductGroup * 100000");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE LV_Mini.admin.ProductGroup SET YearlyBudget = 0");
        }
    }
}