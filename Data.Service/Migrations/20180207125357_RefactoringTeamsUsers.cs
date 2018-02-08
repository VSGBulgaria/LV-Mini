using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Data.Service.Migrations
{
    public partial class RefactoringTeamsUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersTeams_Teams_TeamId",
                schema: "admin",
                table: "UsersTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersTeams_Users_UserId",
                schema: "admin",
                table: "UsersTeams");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersTeams_Teams_TeamId",
                schema: "admin",
                table: "UsersTeams",
                column: "TeamId",
                principalSchema: "admin",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersTeams_Users_UserId",
                schema: "admin",
                table: "UsersTeams",
                column: "UserId",
                principalSchema: "admin",
                principalTable: "Users",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersTeams_Teams_TeamId",
                schema: "admin",
                table: "UsersTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersTeams_Users_UserId",
                schema: "admin",
                table: "UsersTeams");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersTeams_Teams_TeamId",
                schema: "admin",
                table: "UsersTeams",
                column: "TeamId",
                principalSchema: "admin",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersTeams_Users_UserId",
                schema: "admin",
                table: "UsersTeams",
                column: "UserId",
                principalSchema: "admin",
                principalTable: "Users",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
