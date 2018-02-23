using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Data.Service.Migrations
{
    public partial class InitialProjectModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "IbClue");

            migrationBuilder.EnsureSchema(
                name: "admin");

            migrationBuilder.CreateTable(
                name: "Logs",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Action = table.Column<string>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

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
                name: "Users",
                schema: "admin",
                columns: table => new
                {
                    SubjectId = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 1000, nullable: false),
                    Username = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                schema: "IbClue",
                columns: table => new
                {
                    IDAccount = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountCategoryCode = table.Column<string>(maxLength: 10, nullable: false),
                    AccountNumber = table.Column<string>(maxLength: 100, nullable: true),
                    AccountStatusCode = table.Column<string>(maxLength: 10, nullable: true),
                    IdAccountSource = table.Column<int>(nullable: true),
                    ProductCode = table.Column<string>(maxLength: 15, nullable: true)
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
                    DateLoanRequestReceived = table.Column<DateTime>(nullable: true),
                    DecisionDate = table.Column<DateTime>(nullable: true),
                    ExpectedFundingAtClosing = table.Column<decimal>(nullable: true),
                    IDAccount = table.Column<int>(nullable: false),
                    IdLoanSource = table.Column<int>(nullable: true),
                    IsLoanRequest = table.Column<bool>(nullable: true),
                    LoanDate = table.Column<DateTime>(nullable: true),
                    NewMoney = table.Column<decimal>(nullable: true),
                    ProposedCloseDate = table.Column<DateTime>(nullable: true)
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
                    IsActive = table.Column<bool>(nullable: true),
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
                name: "UserClaims",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(maxLength: 250, nullable: false),
                    ClaimValue = table.Column<string>(maxLength: 250, nullable: false),
                    SubjectId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_SubjectId",
                        column: x => x.SubjectId,
                        principalSchema: "admin",
                        principalTable: "Users",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 250, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 250, nullable: false),
                    SubjectId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_SubjectId",
                        column: x => x.SubjectId,
                        principalSchema: "admin",
                        principalTable: "Users",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersTeams_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "admin",
                        principalTable: "Users",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_ProductGroup_Name",
                schema: "admin",
                table: "ProductGroup",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TeamName",
                schema: "admin",
                table: "Teams",
                column: "TeamName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_SubjectId",
                schema: "admin",
                table: "UserClaims",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_SubjectId",
                schema: "admin",
                table: "UserLogins",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username_Email",
                schema: "admin",
                table: "Users",
                columns: new[] { "Username", "Email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersTeams_UserId",
                schema: "admin",
                table: "UsersTeams",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductCode",
                schema: "IbClue",
                table: "Product",
                column: "ProductCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductGroupProduct");

            migrationBuilder.DropTable(
                name: "Logs",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "admin");

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

            migrationBuilder.DropTable(
                name: "Users",
                schema: "admin");
        }
    }
}
