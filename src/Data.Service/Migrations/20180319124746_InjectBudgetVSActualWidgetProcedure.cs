using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Service.Migrations
{
    public partial class InjectBudgetVSActualWidgetProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    USE LV_Mini
                    GO
                    CREATE  PROCEDURE  usp_GetBudgetVSActualWidgetInfo 
                    AS
                    BEGIN 
                    
                     SELECT pg.Name AS ProductGroup, pg.YearlyBudget, SUM(l.LoanAmount) AS SumOfAllLoansForGrop FROM LV_Mini.admin.ProductGroup AS pg
                    LEFT OUTER JOIN LV_Mini.dbo.ProductGroupProduct AS pgp ON pg.IDProductGroup = pgp.IDProductGroup
                    LEFT OUTER JOIN LV_Mini.IbClue.Product AS p ON pgp.IDProduct = p.IDProduct
                    LEFT OUTER JOIN LV_Mini.IbClue.Account AS c ON p.ProductCode = c.ProductCode
                    LEFT OUTER JOIN LV_Mini.IbClue.Loan AS l ON c.IDAccount = l.IDAccount
                    WHERE YEAR(l.LoanDate) = Year(GETDATE())
                    GROUP BY pg.Name, pg.YearlyBudget
                    
                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    USE LV_Mini
                    GO
                    DROP PROCEDURE usp_GetBudgetVSActualWidgetInfo");
        }
    }
}