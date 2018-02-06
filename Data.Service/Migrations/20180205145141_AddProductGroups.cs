using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Service.Migrations
{
    public partial class AddProductGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdProduct",
                schema: "IbClue",
                table: "Product",
                newName: "IDProduct");

            migrationBuilder.CreateTable(
                name: "ProductGroup",
                schema: "admin",
                columns: table => new
                {
                    IDProductGroup = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsActive = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGroup", x => x.IDProductGroup);
                });

            migrationBuilder.CreateTable(
                name: "ProductGroupProducts",
                columns: table => new
                {
                    IDProduct = table.Column<int>(nullable: false),
                    IDProductGroup = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGroupProducts", x => new { x.IDProduct, x.IDProductGroup });
                    table.ForeignKey(
                        name: "FK_ProductGroupProducts_Product_IDProduct",
                        column: x => x.IDProduct,
                        principalSchema: "IbClue",
                        principalTable: "Product",
                        principalColumn: "IDProduct",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductGroupProducts_ProductGroup_IDProductGroup",
                        column: x => x.IDProductGroup,
                        principalSchema: "admin",
                        principalTable: "ProductGroup",
                        principalColumn: "IDProductGroup",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductGroupProducts_IDProductGroup",
                table: "ProductGroupProducts",
                column: "IDProductGroup");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductGroupProducts");

            migrationBuilder.DropTable(
                name: "ProductGroup",
                schema: "admin");

            migrationBuilder.RenameColumn(
                name: "IDProduct",
                schema: "IbClue",
                table: "Product",
                newName: "IdProduct");
        }
    }
}
