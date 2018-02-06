using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Data.Service.Migrations
{
    public partial class EntityChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductGroupProducts_Product_IDProduct",
                table: "ProductGroupProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductGroupProducts_ProductGroup_IDProductGroup",
                table: "ProductGroupProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductGroupProducts",
                table: "ProductGroupProducts");

            migrationBuilder.RenameTable(
                name: "ProductGroupProducts",
                newName: "ProductGroupProduct");

            migrationBuilder.RenameIndex(
                name: "IX_ProductGroupProducts_IDProductGroup",
                table: "ProductGroupProduct",
                newName: "IX_ProductGroupProduct_IDProductGroup");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "admin",
                table: "ProductGroup",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "admin",
                table: "ProductGroup",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductGroupProduct",
                table: "ProductGroupProduct",
                columns: new[] { "IDProduct", "IDProductGroup" });

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductGroupProduct",
                table: "ProductGroupProduct");

            migrationBuilder.RenameTable(
                name: "ProductGroupProduct",
                newName: "ProductGroupProducts");

            migrationBuilder.RenameIndex(
                name: "IX_ProductGroupProduct_IDProductGroup",
                table: "ProductGroupProducts",
                newName: "IX_ProductGroupProducts_IDProductGroup");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "admin",
                table: "ProductGroup",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "IsActive",
                schema: "admin",
                table: "ProductGroup",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductGroupProducts",
                table: "ProductGroupProducts",
                columns: new[] { "IDProduct", "IDProductGroup" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGroupProducts_Product_IDProduct",
                table: "ProductGroupProducts",
                column: "IDProduct",
                principalSchema: "IbClue",
                principalTable: "Product",
                principalColumn: "IDProduct",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGroupProducts_ProductGroup_IDProductGroup",
                table: "ProductGroupProducts",
                column: "IDProductGroup",
                principalSchema: "admin",
                principalTable: "ProductGroup",
                principalColumn: "IDProductGroup",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
