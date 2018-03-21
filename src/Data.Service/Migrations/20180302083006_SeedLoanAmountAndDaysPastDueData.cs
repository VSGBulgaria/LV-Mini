using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Data.Service.Migrations
{
    public partial class SeedLoanAmountAndDaysPastDueData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().FullName)
                , @"Service\SeedData\LoanMissingColums.sql");
            migrationBuilder.Sql(File.ReadAllText(path));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE LV_Mini.IbClue.Loan SET LV_Mini.IbClue.Loan.DaysPastDue = NULL, " +
                "LV_Mini.IbClue.Loan.LoanAmount = NULL");
        }
    }
}
