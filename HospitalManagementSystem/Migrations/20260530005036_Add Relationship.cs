using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RoleID",
                table: "hMUserRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_hMUserRoles_RoleID",
                table: "hMUserRoles",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_hMUserRoles_UserID",
                table: "hMUserRoles",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_hMUserRoles_hMRoles_RoleID",
                table: "hMUserRoles",
                column: "RoleID",
                principalTable: "hMRoles",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_hMUserRoles_hMUsers_UserID",
                table: "hMUserRoles",
                column: "UserID",
                principalTable: "hMUsers",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hMUserRoles_hMRoles_RoleID",
                table: "hMUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_hMUserRoles_hMUsers_UserID",
                table: "hMUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_hMUserRoles_RoleID",
                table: "hMUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_hMUserRoles_UserID",
                table: "hMUserRoles");

            migrationBuilder.AlterColumn<string>(
                name: "RoleID",
                table: "hMUserRoles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
