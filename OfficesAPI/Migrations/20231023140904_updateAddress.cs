using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficesAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Receptionists");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Offices",
                newName: "Street");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Offices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "Offices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OfficeNumber",
                table: "Offices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Offices",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "City", "HouseNumber", "OfficeNumber", "Street" },
                values: new object[] { "Brest", "4", "54", "Kolesnika" });

            migrationBuilder.UpdateData(
                table: "Offices",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "City", "HouseNumber", "OfficeNumber", "Street" },
                values: new object[] { "Minsk", "13a", "226", "Sverdlova" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "OfficeNumber",
                table: "Offices");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Offices",
                newName: "Address");

            migrationBuilder.CreateTable(
                name: "Receptionists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receptionists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receptionists_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Offices",
                keyColumn: "Id",
                keyValue: 1,
                column: "Address",
                value: "Brest, Kolesnika street, 4 | Office: 54");

            migrationBuilder.UpdateData(
                table: "Offices",
                keyColumn: "Id",
                keyValue: 2,
                column: "Address",
                value: "Minsk, Sverdlova street, 13a | Office: 226");

            migrationBuilder.CreateIndex(
                name: "IX_Receptionists_OfficeId",
                table: "Receptionists",
                column: "OfficeId",
                unique: true);
        }
    }
}
