using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class nursetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_patients_DoctorID",
                table: "patients",
                column: "DoctorID");

            migrationBuilder.AddForeignKey(
                name: "FK_patients_hMUsers_DoctorID",
                table: "patients",
                column: "DoctorID",
                principalTable: "hMUsers",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_patients_hMUsers_DoctorID",
                table: "patients");

            migrationBuilder.DropIndex(
                name: "IX_patients_DoctorID",
                table: "patients");
        }
    }
}
