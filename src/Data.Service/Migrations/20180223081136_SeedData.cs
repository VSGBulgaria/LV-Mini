using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;
using System.Reflection;

namespace Data.Service.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().FullName), @"Service\SeedData\PopulateAccountLoanAndProductTables.sql");
            migrationBuilder.Sql(File.ReadAllText(path));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM IbClue.Account");
            migrationBuilder.Sql("DELETE FROM IbClue.Loan");
            migrationBuilder.Sql("DELETE FROM IbClue.Product");
        }
    }
}
