using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AccountUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Offices",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "RegistryPhoneNumber" },
                values: new object[] { "Brest, Kolesnika street, 4 | Office: 54", "+375333186223" });

            migrationBuilder.UpdateData(
                table: "Offices",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "RegistryPhoneNumber" },
                values: new object[] { "Minsk, Sverdlova street, 13a | Office: 226", "+375333733839" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Offices",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "RegistryPhoneNumber" },
                values: new object[] { "Brest, Kolesnika street, 4. Office: 54", "+375 (33) 318-62-23" });

            migrationBuilder.UpdateData(
                table: "Offices",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "RegistryPhoneNumber" },
                values: new object[] { "Minsk, Sverdlova street, 13a. Office: 226", "+375 (33) 373-38-39" });
        }
    }
}
