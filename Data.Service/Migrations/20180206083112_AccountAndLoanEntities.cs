using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Data.Service.Migrations
{
    public partial class AccountAndLoanEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                schema: "IbClue",
                columns: table => new
                {
                    IDAccount = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountCategoryCode = table.Column<string>(maxLength: 10, nullable: false),
                    AccountStatusCode = table.Column<string>(maxLength: 10, nullable: false),
                    IDProduct = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.IDAccount);
                });

            migrationBuilder.CreateTable(
                name: "Loan",
                schema: "IbClue",
                columns: table => new
                {
                    IDLoan = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateLoanRequestReceived = table.Column<DateTime>(nullable: false),
                    DecisionDate = table.Column<DateTime>(nullable: false),
                    ExpectedFundingAtClosing = table.Column<decimal>(nullable: false),
                    IDAccount = table.Column<int>(nullable: false),
                    IsLoanRequest = table.Column<bool>(nullable: false),
                    LoanDate = table.Column<DateTime>(nullable: false),
                    NewMoney = table.Column<decimal>(nullable: false),
                    ProposedCloseDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loan", x => x.IDLoan);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account",
                schema: "IbClue");

            migrationBuilder.DropTable(
                name: "Loan",
                schema: "IbClue");
        }
    }
}
