using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Service.Migrations
{
    public partial class UpdateNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserClaim_Users_SubjectId",
                table: "UserClaim");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserClaim",
                table: "UserClaim");

            migrationBuilder.RenameTable(
                name: "UserClaim",
                newName: "UserClaims");

            migrationBuilder.RenameIndex(
                name: "IX_UserClaim_SubjectId",
                table: "UserClaims",
                newName: "IX_UserClaims_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserClaims",
                table: "UserClaims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaims_Users_SubjectId",
                table: "UserClaims",
                column: "SubjectId",
                principalTable: "Users",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserClaims_Users_SubjectId",
                table: "UserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserClaims",
                table: "UserClaims");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                newName: "UserClaim");

            migrationBuilder.RenameIndex(
                name: "IX_UserClaims_SubjectId",
                table: "UserClaim",
                newName: "IX_UserClaim_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserClaim",
                table: "UserClaim",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaim_Users_SubjectId",
                table: "UserClaim",
                column: "SubjectId",
                principalTable: "Users",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
