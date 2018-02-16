using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.IO;

namespace Data.Service.Migrations
{
    public partial class SeedAccountLoanAndProductTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                @"PopulateAccountLoanAndProductTables.sql");
            migrationBuilder.Sql(File.ReadAllText(sqlFile));

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
