using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Data.Service.Migrations
{
    public partial class ResolvingSmallProblems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "IbClue");

            migrationBuilder.EnsureSchema(
                name: "admin");

            migrationBuilder.RenameTable(
                name: "Users",
                newSchema: "admin");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                newSchema: "admin");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                newSchema: "admin");

            migrationBuilder.RenameTable(
                name: "Logs",
                newSchema: "admin");

            migrationBuilder.CreateTable(
                name: "ProductGroup",
                schema: "admin",
                columns: table => new
                {
                    IDProductGroup = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGroup", x => x.IDProductGroup);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                schema: "admin",
                columns: table => new
                {
                    TeamId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsActive = table.Column<bool>(nullable: false),
                    TeamName = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                });

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

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "IbClue",
                columns: table => new
                {
                    IDProduct = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsActive = table.Column<bool>(nullable: false),
                    IsHidden = table.Column<bool>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 15, nullable: false),
                    ProductDescription = table.Column<string>(maxLength: 150, nullable: false),
                    ProductType = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.IDProduct);
                });

            migrationBuilder.CreateTable(
                name: "UsersTeams",
                schema: "admin",
                columns: table => new
                {
                    TeamId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTeams", x => new { x.TeamId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UsersTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalSchema: "admin",
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersTeams_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "admin",
                        principalTable: "Users",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductGroupProduct",
                columns: table => new
                {
                    IDProduct = table.Column<int>(nullable: false),
                    IDProductGroup = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGroupProduct", x => new { x.IDProduct, x.IDProductGroup });
                    table.ForeignKey(
                        name: "FK_ProductGroupProduct_Product_IDProduct",
                        column: x => x.IDProduct,
                        principalSchema: "IbClue",
                        principalTable: "Product",
                        principalColumn: "IDProduct",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductGroupProduct_ProductGroup_IDProductGroup",
                        column: x => x.IDProductGroup,
                        principalSchema: "admin",
                        principalTable: "ProductGroup",
                        principalColumn: "IDProductGroup",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductGroupProduct_IDProductGroup",
                table: "ProductGroupProduct",
                column: "IDProductGroup");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TeamName",
                schema: "admin",
                table: "Teams",
                column: "TeamName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersTeams_UserId",
                schema: "admin",
                table: "UsersTeams",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductGroupProduct");

            migrationBuilder.DropTable(
                name: "UsersTeams",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "Account",
                schema: "IbClue");

            migrationBuilder.DropTable(
                name: "Loan",
                schema: "IbClue");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "IbClue");

            migrationBuilder.DropTable(
                name: "ProductGroup",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "Teams",
                schema: "admin");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "admin");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                schema: "admin");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                schema: "admin");

            migrationBuilder.RenameTable(
                name: "Logs",
                schema: "admin");
        }
    }
}
