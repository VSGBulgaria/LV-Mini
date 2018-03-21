using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Service.Migrations
{
    public partial class AddRequiredColumnsInLoanTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DaysPastDue",
                schema: "IbClue",
                table: "Loan",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LoanAmount",
                schema: "IbClue",
                table: "Loan",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysPastDue",
                schema: "IbClue",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "LoanAmount",
                schema: "IbClue",
                table: "Loan");
        }
    }
}
