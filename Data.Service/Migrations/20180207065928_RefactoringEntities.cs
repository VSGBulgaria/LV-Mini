using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Data.Service.Migrations
{
    public partial class RefactoringEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                schema: "admin",
                table: "Teams");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                schema: "admin",
                table: "Teams",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "admin",
                table: "Teams",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "admin",
                table: "ProductGroup",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TeamName",
                schema: "admin",
                table: "Teams",
                column: "TeamName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGroupProduct_Product_IDProduct",
                table: "ProductGroupProduct",
                column: "IDProduct",
                principalSchema: "IbClue",
                principalTable: "Product",
                principalColumn: "IDProduct",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGroupProduct_ProductGroup_IDProductGroup",
                table: "ProductGroupProduct",
                column: "IDProductGroup",
                principalSchema: "admin",
                principalTable: "ProductGroup",
                principalColumn: "IDProductGroup",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductGroupProduct_Product_IDProduct",
                table: "ProductGroupProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductGroupProduct_ProductGroup_IDProductGroup",
                table: "ProductGroupProduct");

            migrationBuilder.DropIndex(
                name: "IX_Teams_TeamName",
                schema: "admin",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "admin",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "admin",
                table: "ProductGroup");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                schema: "admin",
                table: "Teams",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "admin",
                table: "Teams",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
