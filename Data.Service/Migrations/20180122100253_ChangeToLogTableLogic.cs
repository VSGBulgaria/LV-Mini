using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Data.Service.Migrations
{
    public partial class ChangeToLogTableLogic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Actions_ActionId",
                table: "Logs");

            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropIndex(
                name: "IX_Logs_ActionId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "ActionId",
                table: "Logs");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Logs",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "Logs");

            migrationBuilder.AddColumn<int>(
                name: "ActionId",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Action = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_ActionId",
                table: "Logs",
                column: "ActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Actions_ActionId",
                table: "Logs",
                column: "ActionId",
                principalTable: "Actions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
