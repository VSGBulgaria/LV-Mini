using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Service.Migrations
{
    public partial class LoanAmountPerYearStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE IbClue.GetLoanAmountPerYear
                                   AS
                                   BEGIN
                                       SELECT DATEPART(year, loan.DateLoanRequestReceived) AS [Year], SUM(loan.NewMoney) AS [Money]
                                       FROM [IbClue].[Loan] AS loan
                                       WHERE (loan.IsLoanRequest = 1) AND loan.DateLoanRequestReceived IS NOT NULL AND YEAR(loan.DateLoanRequestReceived) > YEAR(GETDATE()) - 3
                                       GROUP BY YEAR(loan.DateLoanRequestReceived)
                                       ORDER BY DATEPART(year, loan.DateLoanRequestReceived) DESC
                                   END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IbClue.GetLoanAmountPerYear;");
        }
    }
}
