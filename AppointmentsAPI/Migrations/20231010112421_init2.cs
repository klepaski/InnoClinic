using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentsAPI.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpecializationName",
                table: "Appointments",
                newName: "DoctorSpecialization");

            migrationBuilder.AlterColumn<string>(
                name: "PatientPhoneNumber",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DoctorSpecialization",
                table: "Appointments",
                newName: "SpecializationName");

            migrationBuilder.AlterColumn<string>(
                name: "PatientPhoneNumber",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
