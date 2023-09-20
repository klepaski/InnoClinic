using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfilesAPI.Migrations
{
    /// <inheritdoc />
    public partial class DoctorWithoutNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_DoctorSpecializations_SpecializationId",
                table: "Doctors");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_DoctorSpecializations_SpecializationId",
                table: "Doctors",
                column: "SpecializationId",
                principalTable: "DoctorSpecializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_DoctorSpecializations_SpecializationId",
                table: "Doctors");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_DoctorSpecializations_SpecializationId",
                table: "Doctors",
                column: "SpecializationId",
                principalTable: "DoctorSpecializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
