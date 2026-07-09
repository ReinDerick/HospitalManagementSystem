using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class addnurse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_patients_hMUsers_DoctorID",
                table: "patients");

            migrationBuilder.DropIndex(
                name: "IX_patients_DoctorID",
                table: "patients");

            migrationBuilder.AddColumn<Guid>(
                name: "NurseID",
                table: "patients",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_patients_NurseID",
                table: "patients",
                column: "NurseID");

            migrationBuilder.AddForeignKey(
                name: "FK_patients_hMUsers_NurseID",
                table: "patients",
                column: "NurseID",
                principalTable: "hMUsers",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_patients_hMUsers_NurseID",
                table: "patients");

            migrationBuilder.DropIndex(
                name: "IX_patients_NurseID",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "NurseID",
                table: "patients");

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
    }
}
